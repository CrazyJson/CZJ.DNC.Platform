using Consul;
using CZJ.Auditing;
using CZJ.Common.Core;
using CZJ.Common.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;
using CZJ.DNC.Core;

namespace CZJ.Discovery.Consul
{
    /// <summary>
    /// 微服务注册
    /// </summary>
    public class ConsulRegister : IServiceModule
    {
        public int Order => 20;

        //是否需要向注册中心注册信息
        private bool needRegister;

        //服务是否启动了https协议
        private string enableHttps;

        private string consulHost;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            needRegister = (env.IsDevelopment() && needRegister) || !env.IsDevelopment();
            if (!needRegister)
            {
                return;
            }
            //请求注册的 Consul 地址
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{consulHost}"));
            var appInfo = app.ApplicationServices.GetRequiredService<IAppInfoProvider>();
            var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                HTTP = $"http://{appInfo.IpAddress}:{appInfo.Ports[0]}/api/health",//健康检查地址
                Timeout = TimeSpan.FromSeconds(5)
            };

            // Register service with consul
            var meta = new Dictionary<string, string>();
            meta["EnableHttps"] = enableHttps;
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = DESEncrypt.Md5Hash($"{appInfo.IpAddress}:{appInfo.Ports[0]}"),
                Name = SysConfig.MicroServiceOption.Name,
                Address = appInfo.IpAddress,
                Port = Convert.ToInt32(appInfo.Ports[0]),
                Tags = SysConfig.MicroServiceOption.Tags,
                Meta = meta
            };
            try
            {
                //服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
                consulClient.Agent.ServiceRegister(registration).Wait();
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<ConsulRegister>();
                logger.LogError(e, $"向consul【{consulClient.Config.Address}】注册失败！");
            }
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            if (services.FirstOrDefault(e => e.ServiceType == typeof(IServiceDiscoveryProvider)) == null)
            {
                services.AddSingleton<IServiceDiscoveryProvider, ConsulServiceDiscoveryProvider>();
            }
            needRegister = configuration.GetValue<string>("needRegister") == "1";
            consulHost = configuration["MicroService:consul:host"];
            enableHttps = configuration.GetValue<bool>("EnableHttps") ? "1" : "0";
        }
    }
}
