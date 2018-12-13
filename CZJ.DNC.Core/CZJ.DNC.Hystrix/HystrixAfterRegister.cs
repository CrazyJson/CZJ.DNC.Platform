using Autofac.Builder;
using Autofac.Extras.DynamicProxy;
using Autofac.Features.Scanning;
using CZJ.Dependency;
using System;
using System.Linq;
using System.Reflection;

namespace CZJ.DNC.Hystrix
{
    /// <summary>
    /// 熔断类型注册，判断方法里面有HystrixCommandAttribute属性的
    /// </summary>
    public class HystrixAfterRegister : IAfterRegister
    {
        public IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> Register(IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> builderRegister, Type type)
        {
            var needHystrixInterceptor = type.GetMethods().Any(t => t.GetCustomAttribute<HystrixCommandAttribute>() != null);
            if (!needHystrixInterceptor)
            {
                return builderRegister;
            }
            bool bClassRegister = type.IsClass && !type.IsAbstract && !type.BaseType.IsInterface && type.BaseType != typeof(object);
            if (bClassRegister)
            {
                return builderRegister.EnableClassInterceptors().InterceptedBy(typeof(HystrixInterceptor));
            }
            else
            {
                return builderRegister.EnableInterfaceInterceptors().InterceptedBy(typeof(HystrixInterceptor));
            }
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> Register<TLimit, TActivatorData, TRegistrationStyle>(IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration, Type type)
        {
            var needHystrixInterceptor = type.GetMethods().Any(t => t.GetCustomAttribute<HystrixCommandAttribute>() != null);
            if (!needHystrixInterceptor)
            {
                return registration;
            }
            bool bClassRegister = type.IsClass && !type.IsAbstract && !type.BaseType.IsInterface && type.BaseType != typeof(object);
            if (bClassRegister)
            {
                return registration.InterceptedBy(typeof(HystrixInterceptor));
            }
            else
            {
                return registration.EnableInterfaceInterceptors().InterceptedBy(typeof(HystrixInterceptor));
            }
        }
    }
}
