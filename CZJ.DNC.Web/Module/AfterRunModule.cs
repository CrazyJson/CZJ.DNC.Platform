using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using CZJ.Common.Module;
using CZJ.Dependency;

namespace CZJ.DNC.Web.Module
{
    /// <summary>
    /// 运行后配置模块
    /// </summary>
    public class AfterRunModule : IServiceModule
    {
        /// <summary>
        /// 
        /// </summary>
        public int Order => int.MaxValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var completeModule = IocManager.Instance.Resolve<AfterRunConfigureModule>();
            completeModule.Configure();         
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
