using Autofac;
using Autofac.Extras.DynamicProxy;
using CZJ.Dependency;
using CZJ.DNC.Hystrix;
using CZJ.Reflection;
using System;
using System.Linq;
using System.Reflection;

namespace CZJ.DNC.Hystrix
{
    /// <summary>
    /// 
    /// </summary>
    public class HystrixRegister : IDependencyRegistrar
    {
        /// <summary>
        /// 
        /// </summary>
        public int Order
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="typeFinder"></param>
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //注册熔断拦截器
            builder.RegisterType<HystrixInterceptor>();
        }
    }
}
