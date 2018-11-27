using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore;

namespace CZJ.DNC.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //增加commandline支持
            var config = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(AppContext.BaseDirectory, "Config"))
                    .AddXmlFile("Config.config", optional: true)
                    .AddXmlFile("Config_Local.config", optional: true)
                    .AddCommandLine(args).Build();
            string serverUrls = config.GetValue<string>("server.urls");
            var host = WebHost.CreateDefaultBuilder(args).UseKestrel()
                   .UseStartup<Startup>()
                   .UseUrls(serverUrls)
                   .Build();
            host.Run();
        }
    }
}
