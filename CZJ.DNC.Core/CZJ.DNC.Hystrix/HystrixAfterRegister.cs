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
        public IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> Register(IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> builderRegister, Type type, bool bClassRegister)
        {
            var needHystrixInterceptor = type.GetMethods().Any(t => t.GetCustomAttribute<HystrixCommandAttribute>() != null);
            if (!needHystrixInterceptor)
            {
                return builderRegister;
            }
            if (bClassRegister)
            {
                return builderRegister.EnableClassInterceptors().InterceptedBy(typeof(HystrixInterceptor));
            }
            else
            {
                return builderRegister.EnableInterfaceInterceptors().InterceptedBy(typeof(HystrixInterceptor));
            }
        }
    }
}
