using Autofac;
using CZJ.Auditing;
using CZJ.Common.Core;
using CZJ.Common.Module;
using CZJ.Dependency;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace CZJ.DNC.Web.Module
{
    /// <summary>
    /// MVC模块注册
    /// </summary>
    public class MvcModule : IServiceModule
    {
        /// <summary>
        /// 
        /// </summary>
        public int Order => 10;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddCors();

            //添加GZip响应压缩插件
            services.AddResponseCompression();

           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //设置默认启动页
            app.UseDefaultFiles();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseResponseCompression();
            if (SysConfig.StaticDirectory != null)
            {
                foreach (var item in SysConfig.StaticDirectory)
                {
                    var path = FileHelper.GetDirectoryPath(item.PhysicalRelativePath, true);
                    app.UseFileServer(new FileServerOptions()
                    {
                        FileProvider = new PhysicalFileProvider(path),
                        RequestPath = new PathString(item.RequestPath),
                        EnableDirectoryBrowsing = item.EnableDirectoryBrowsing
                    });
                }
            }
            app.UseMvc();
            //放在UseMvc之后,静态资源目录配置
            app.UseStaticFiles(new StaticFileOptions
            {
                //设置不限制content-type
                //ServeUnknownFileTypes = true
            });
            var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
            var appParam = new TypedParameter(typeof(IServerAddressesFeature), serverAddressesFeature);
            IocManager.Instance.Resolve<IAppInfoProvider>(appParam);
        }
    }
}
