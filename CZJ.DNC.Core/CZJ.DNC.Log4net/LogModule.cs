using CZJ.Common.Core;
using CZJ.Common.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CZJ.DNC.Log4net
{
    /// <summary>
    /// 日志清理任务
    /// </summary>
    public class LogModule : IServiceModule
    {
        /// <summary>
        /// 
        /// </summary>
        public int Order => 130;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.RemoveAll<ILoggerProvider>();
            services.AddSingleton(typeof(ILoggerProvider), new Log4NetProvider());
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        int day = configuration.GetValue<int>("ClearLog");
                        day = day <= 0 ? 3 : day;
                        var dir = FileHelper.GetDirectoryPath("/logs");
                        var now = DateTime.Now;
                        long seconds = day * 24 * 60 * 60;
                        FileInfo fi = null;
                        if (Directory.Exists(dir))
                        {
                            var files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
                            foreach (var file in files)
                            {
                                fi = new FileInfo(file);
                                if ((now - fi.CreationTime).TotalSeconds > seconds)
                                {
                                    try
                                    {
                                        fi.Delete();
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                    catch { }
                    Task.Delay(6 * 60 * 60 * 1000).Wait();
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

        }
    }
}
