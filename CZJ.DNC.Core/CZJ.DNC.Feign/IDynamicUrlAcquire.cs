using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebApiClient;

namespace CZJ.DNC.Feign
{
    /// <summary>
    /// 动态url获取器
    /// </summary>
    public interface IDynamicUrlAcquire
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="httpMethod"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        string[] GetHost(Uri uri, HttpMethod httpMethod, Tags tags);
    }
}
