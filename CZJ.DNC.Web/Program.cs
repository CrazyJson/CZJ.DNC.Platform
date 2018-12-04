using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;
using Microsoft.AspNetCore;
using System.Linq;
using CZJ.DNC.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using System.Diagnostics;

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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //增加commandline支持
            var config = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(AppContext.BaseDirectory, "Config"))
                    .AddXmlFile("Config.config", optional: true)
                    .AddXmlFile("Config_Local.config", optional: true)
                    .AddCommandLine(args).Build();
            string serverUrls = config.GetValue<string>("server.urls");
            int port = GetPort(serverUrls);
            //window下程序参数中带有service=true表示以Windows服务运行方式
            bool isService = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
                !(args.Contains("console=true") || Debugger.IsAttached);
            if (!isService && HostHelper.IsPortInUsed(port))
            {
                //如果站点端口已经被占用
                Console.WriteLine("端口已被占用，将关闭之前已启动程序");
                CloseApp(port);
            }
            var host = WebHost.CreateDefaultBuilder(args).UseKestrel()
                        .UseStartup<Startup>()
                        .UseUrls(serverUrls)
                        .Build();

            if (isService)
            {
                host.RunAsService();
            }
            else
            {
                host.Run();
            }
        }

        /// <summary>
        /// 获取站点端口
        /// </summary>
        /// <param name="serverUrls">启动url</param>
        /// <returns>站点端口</returns>
        private static int GetPort(string serverUrls)
        {
            var arrUrl = serverUrls.Split(";");
            var url = new Uri(arrUrl[0].Replace("*", "127.0.0.1"));
            return url.Port;
        }

        /// <summary>
        /// 关闭当前端口程序
        /// </summary>
        /// <param name="port"></param>
        private static void CloseApp(int port)
        {
            string url = $"http://127.0.0.1:{port}/api/App/Stop";
            using (var httpclient = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.RequestUri = new Uri(url);
                requestMessage.Method = HttpMethod.Head;
                requestMessage.Headers.TryAddWithoutValidation("Stop-Application", "yes");
                try
                {
                    httpclient.SendAsync(requestMessage).GetAwaiter().GetResult();
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}