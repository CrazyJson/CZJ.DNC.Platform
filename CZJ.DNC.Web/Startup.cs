﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Collections.Generic;
using Rabbit.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using CZJ.Common.Module;
using CZJ.Common.Core;
using CZJ.Reflection;
using CZJ.Dependency;

namespace CZJ.DNC.Web
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 配置
        /// </summary>
        public IConfiguration Configuration { get; }

        private List<IServiceModule> configModules = new List<IServiceModule>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build().EnableTemplateSupport();
            Configuration.GetReloadToken().RegisterChangeCallback(OnSettingChanged, Configuration);
            ConfigInit.InitConfig(Configuration);
        }
        /// <summary>
        /// 配置文件改变
        /// </summary>
        /// <param name="state"></param>
        private void OnSettingChanged(object state)
        {
            IConfiguration configuration = (IConfiguration)state;
            ConfigInit.InitConfig(configuration);
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //替换IOC插件为Autofac
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            var moduleType = typeof(IServiceModule);
            var arrModuleType = TypeFinder.Instance.FindAll().Where(t => moduleType.IsAssignableFrom(t) && t != moduleType).ToArray();
            foreach (var drType in arrModuleType)
            {
                configModules.Add((IServiceModule)Activator.CreateInstance(drType));
            }
            foreach (var module in configModules)
            {
                module.ConfigureServices(services, Configuration);
            }
            return IocManager.Instance.Initialize(services);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
            }
            foreach (var module in configModules.OrderBy(e => e.Order))
            {
                module.Configure(app, env, loggerFactory);
            }       
        }
    }
}
