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

        private string consulIP;

        private string consulPort;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            needRegister = (env.IsDevelopment() && needRegister) || !env.IsDevelopment();
            //请求注册的 Consul 地址
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{consulIP}:{consulPort}"));
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
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = DESEncrypt.Md5Hash($"{appInfo.IpAddress}:{appInfo.Ports[0]}"),
                Name = SysConfig.MicroServiceOption.Name,
                Address = appInfo.IpAddress,
                Port = Convert.ToInt32(appInfo.Ports[0]),
                Tags = SysConfig.MicroServiceOption.Tags
            };

            consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
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
            needRegister = configuration.GetValue<string>("needRegister") == "1";
            consulIP = configuration["MicroService:consul:ip"];
            consulPort = configuration["MicroService:consul:port"];
        }
    }
}
