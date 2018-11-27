using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CZJ.Common.Module
{
    /// <summary>
    /// 模块注册
    /// </summary>
    public interface IServiceModule
    {
        /// <summary>
        /// Configure调用顺序
        /// </summary>
        int Order { get; }

        void ConfigureServices(IServiceCollection services, IConfiguration configuration);


        void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory);
    }
}
