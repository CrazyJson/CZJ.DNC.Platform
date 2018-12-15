using Consul;
using CZJ.DNC.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CZJ.Discovery.Consul
{
    /// <summary>
    /// 基于consul的服务发现实现
    /// </summary>
    public class ConsulServiceDiscoveryProvider : IServiceDiscoveryProvider
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        public IConfiguration configuration { get; set; }

        /// <summary>
        /// 获取服务实际节点
        /// </summary>
        /// <param name="uri">接口地址</param>
        /// <param name="httpMethod">请求method</param>
        /// <param name="tags">tags</param>
        /// <returns>实际节点数组</returns>
        public async Task<string[]> GetHost(Uri uri, HttpMethod httpMethod, Dictionary<string, object> tags)
        {
            object appNo = null;
            if (!tags.TryGetValue("AppNo", out appNo) || appNo == null || string.IsNullOrEmpty(appNo.ToString()))
            {
                throw new Exception("无法从tags中找到服务对应的key【AppNo】");
            }
            string serviceName = appNo.ToString();
            var list = new List<string>();
            using (var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{configuration["MicroService:consul:host"]}")))
            {
                var result = await consulClient.Agent.Services();
                //取出对应服务
                var services = result.Response.Values.Where(p => p.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase));
                string enableHttps;
                string scheme;
                foreach (var service in services)
                {
                    scheme = (service.Meta.TryGetValue("EnableHttps", out enableHttps) && enableHttps == "1") ? "https" : "http";
                    list.Add($"{scheme}://{service.Address}:{service.Port}");
                }
            }
            return list.ToArray();
        }
    }
}
