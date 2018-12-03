using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace CZJ.DNC.Hystrix
{
    /// <summary>
    /// 熔断拦截器
    /// </summary>
    public class HystrixInterceptor : IInterceptor
    {

        private static ConcurrentDictionary<MethodInfo, Policy> policies
            = new ConcurrentDictionary<MethodInfo, Policy>();

        private static readonly IMemoryCache memoryCache
            = new MemoryCache(new MemoryCacheOptions());

        public void Intercept(IInvocation invocation)
        {
            //一个HystrixCommand中保持一个policy对象即可
            //其实主要是CircuitBreaker要求对于同一段代码要共享一个policy对象
            //根据反射原理，同一个方法的MethodInfo是同一个对象，但是对象上取出来的HystrixCommandAttribute
            //每次获取的都是不同的对象，因此以MethodInfo为Key保存到policies中，确保一个方法对应一个policy实例
            HystrixCommandAttribute hystrixCommand = invocation.MethodInvocationTarget
                .GetCustomAttribute<HystrixCommandAttribute>();
            if (hystrixCommand == null)
            {
                invocation.Proceed();
            }
            else
            {
                policies.TryGetValue(invocation.MethodInvocationTarget, out Policy policy);
                lock (policies)//因为Invoke可能是并发调用，因此要确保policies赋值的线程安全
                {
                    if (policy == null)
                    {
                        policy = Policy.NoOp();//创建一个空的Policy
                        if (hystrixCommand.IsEnableCircuitBreaker)
                        {
                            policy = policy.Wrap(Policy.Handle<Exception>().CircuitBreaker(hystrixCommand.ExceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(hystrixCommand.MillisecondsOfBreak)));
                        }
                        if (hystrixCommand.TimeOutMilliseconds > 0)
                        {
                            policy = policy.Wrap(Policy.Timeout(() => TimeSpan.FromMilliseconds(hystrixCommand.TimeOutMilliseconds), Polly.Timeout.TimeoutStrategy.Pessimistic));
                        }
                        if (hystrixCommand.MaxRetryTimes > 0)
                        {
                            policy = policy.Wrap(Policy.Handle<Exception>().WaitAndRetry(hystrixCommand.MaxRetryTimes, i => TimeSpan.FromMilliseconds(hystrixCommand.RetryIntervalMilliseconds)));
                        }
                        Policy policyFallBack = Policy
                          .Handle<Exception>()
                          .Fallback((ctx, t) =>
                          {
                              IInvocation invocationContext = (IInvocation)ctx["invocationContext"];
                              var fallBackMethod = invocation.MethodInvocationTarget.DeclaringType
                                  .GetMethod(hystrixCommand.FallBackMethod);
                              Object fallBackResult = fallBackMethod.Invoke(invocation.InvocationTarget, invocation.Arguments);
                              //不能如下这样，因为这是闭包相关，如果这样写第二次调用Invoke的时候context指向的
                              //还是第一次的对象，所以要通过Polly的上下文来传递AspectContext
                              //context.ReturnValue = fallBackResult;
                              invocationContext.ReturnValue = fallBackResult;
                          }, (ex, t) => { throw ex; });
                        policy = policyFallBack.Wrap(policy);
                        //放入
                        policies.TryAdd(invocation.MethodInvocationTarget, policy);
                    }
                }

                //把本地调用的AspectContext传递给Polly，主要给FallbackAsync中使用，避免闭包的坑
                Context pollyCtx = new Context();
                pollyCtx["invocationContext"] = invocation;

                if (hystrixCommand.CacheTTLMilliseconds > 0)
                {
                    //用类名+方法名+参数的下划线连接起来作为缓存key
                    string cacheKey = "HystrixMethodCacheManager_Key_" + invocation.MethodInvocationTarget.DeclaringType
                                                                       + "." + invocation.MethodInvocationTarget + string.Join("_", invocation.Arguments);
                    //尝试去缓存中获取。如果找到了，则直接用缓存中的值做返回值
                    if (memoryCache.TryGetValue(cacheKey, out var cacheValue))
                    {
                        invocation.ReturnValue = cacheValue;
                    }
                    else
                    {
                        //如果缓存中没有，则执行实际被拦截的方法
                        policy.Execute(ctx => { invocation.Proceed(); }, pollyCtx);
                        //存入缓存中
                        using (var cacheEntry = memoryCache.CreateEntry(cacheKey))
                        {
                            cacheEntry.Value = invocation.ReturnValue;
                            cacheEntry.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMilliseconds(hystrixCommand.CacheTTLMilliseconds);
                        }
                    }
                }
                else//如果没有启用缓存，就直接执行业务方法
                {
                    policy.Execute(ctx => { invocation.Proceed(); }, pollyCtx);
                }
            }
        }
    }
}
