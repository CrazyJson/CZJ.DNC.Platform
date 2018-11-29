using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using CZJ.Common.Module;
using CZJ.DNC.License;
using System;

namespace CZJ.DNC.Web.Module
{
    /// <summary>
    /// License模块初始化
    /// </summary>
    public class LicenseModule : IServiceModule
    {

        /// <summary>
        /// 
        /// </summary>
        public int Order => 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
#if DEBUG
#else
            string msg = LicenseHelper.Verify();
            if (!string.IsNullOrEmpty(msg))
            {
                var log = loggerFactory.CreateLogger<LicenseModule>();
                log.LogError(msg);
                Environment.Exit(-1);
            }
            app.UseMiddleware<LicenseMiddleware>();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}
