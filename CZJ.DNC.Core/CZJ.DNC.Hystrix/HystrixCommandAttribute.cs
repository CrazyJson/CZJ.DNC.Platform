using Autofac.Extras.DynamicProxy;
using System;

namespace CZJ.DNC.Hystrix
{
    /// <summary>
    /// 熔断降级自定义属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class HystrixCommandAttribute : InterceptAttribute
    {
        public HystrixCommandAttribute() : base(typeof(HystrixInterceptor))
        {
        }

        /// <summary>
        /// 最多重试几次，如果为0则不重试
        /// </summary>
        public int MaxRetryTimes { get; set; } = 0;

        /// <summary>
        /// 重试间隔的毫秒数
        /// </summary>
        public int RetryIntervalMilliseconds { get; set; } = 100;

        /// <summary>
        /// 是否启用熔断
        /// </summary>
        public bool IsEnableCircuitBreaker { get; set; } = false;

        /// <summary>
        /// 熔断前出现允许错误几次
        /// </summary>
        public int ExceptionsAllowedBeforeBreaking { get; set; } = 3;

        /// <summary>
        /// 熔断多长时间（毫秒）
        /// </summary>
        public int MillisecondsOfBreak { get; set; } = 1000;

        /// <summary>
        /// 执行超过多少毫秒则认为超时（0表示不检测超时）
        /// </summary>
        public int TimeOutMilliseconds { get; set; } = 0;

        /// <summary>
        /// 缓存多少毫秒（0表示不缓存），用“类名+方法名+所有参数ToString拼接”做缓存Key
        /// </summary>

        public int CacheTTLMilliseconds { get; set; } = 0;

        /// <summary>
        /// 降级的方法名
        /// </summary>
        /// <example>nameof(GetAllProductsFallBackAsync)</example>
        public string FallBackMethod { get; set; }
    }
}
