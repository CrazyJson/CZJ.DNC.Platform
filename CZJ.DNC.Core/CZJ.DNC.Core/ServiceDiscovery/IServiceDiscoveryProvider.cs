using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CZJ.DNC.Core
{
    /// <summary>
    /// 服务获取接口定义
    /// </summary>
    public interface IServiceDiscoveryProvider
    {
        /// <summary>
        /// 获取动态url的主机地址
        /// </summary>
        /// <param name="uri">动态url</param>
        /// <param name="httpMethod">请求方式</param>
        /// <param name="tags">tags可从中去除appno</param>
        /// <returns>http://192.168.0.1:1234</returns>
        Task<string[]> GetHost(Uri uri, HttpMethod httpMethod, Dictionary<string, object> tags);
    }
}
