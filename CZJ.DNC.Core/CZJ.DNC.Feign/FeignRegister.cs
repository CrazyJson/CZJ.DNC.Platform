using Autofac;
using CZJ.Dependency;
using CZJ.Reflection;
using System;
using System.Linq;
using System.Reflection;
using WebApiClient;
using WebApiClient.Attributes;

namespace CZJ.DNC.Feign
{
    /// <summary>
    /// 
    /// </summary>
    public class FeignRegister : IDependencyRegistrar
    {
        /// <summary>
        /// 
        /// </summary>
        public int Order
        {
            get
            {
                return 20;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="typeFinder"></param>
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            var arrHttpApiType = typeFinder.FindAll().Where(t => typeof(IHttpApi).IsAssignableFrom(t)
                && t != typeof(IHttpApi)).ToArray();
            HttpHostAttribute httpHostAttribute = null;
            HttpApiConfig httpApiConfig = null;
            HttpApiClient apiClient = null;
            Uri httpHost;
            var dynamicUrlFilter = new DynamicUrlFilterAttribute();
            var LoggerFactoryFilter = new LoggerFactoryFilterAttribute();
            bool needAddUrlFilter = false;
            foreach (var type in arrHttpApiType)
            {
                httpHostAttribute = type.GetCustomAttribute<HttpHostAttribute>();
                if (httpHostAttribute == null || httpHostAttribute.Host == null)
                {
                    httpHost = new Uri("http://127.0.0.1", UriKind.Absolute);
                    needAddUrlFilter = true;
                }
                else
                {
                    httpHost = httpHostAttribute.Host;
                    needAddUrlFilter = false;
                }
                httpApiConfig = new HttpApiConfig { HttpHost = httpHost };
                httpApiConfig.GlobalFilters.Add(LoggerFactoryFilter);
                if (needAddUrlFilter)
                {
                    httpApiConfig.GlobalFilters.Add(dynamicUrlFilter);
                }
                apiClient = HttpApiClient.Create(type, httpApiConfig);
                builder.RegisterInstance(apiClient).AsImplementedInterfaces()
                    .SingleInstance().PropertiesAutowired();
            }
        }
    }
}
