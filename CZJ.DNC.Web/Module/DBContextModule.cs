using CZJ.Common.Core;
using CZJ.Common.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CZJ.DNC.Web.Module
{
    /// <summary>
    /// EF模块注册
    /// </summary>
    public class DBContextModule : IServiceModule
    {
        /// <summary>
        /// 
        /// </summary>
        public int Order => 12;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            string conStr = string.Empty;
            if (!SysConfig.ConnectionStrings.TryGetValue("MySqlConnection", out conStr))
            {
                throw new System.Exception("请在appsettings.json ConnectionStrings节点下配置MySqlConnection连接字符串");
            }
            //services.AddDbContextPool<MySqlDBContext>(options =>
            //{
            //    options.UseMySQL(conStr);
            //});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //EFInitializer.UnSafeInit();
            //XmlCommandManager.UnSafeInit();
            //var db = app.ApplicationServices.GetService<MySqlDBContext>();
            //EFCoreLocator.RegisterDefaults();
            //EFCoreLocator.RegisterProviderRelate(db.Database.ProviderName, DBType.MySQL);
        }
    }
}
