using CZJ.Dependency;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using CZJ.DNC.Net;

namespace CZJ.Auditing
{
    /// <summary>
    /// 获取客户端信息
    /// </summary>
    public class DefaultClientInfoProvider : IClientInfoProvider, ITransientDependency
    {
        private readonly IHttpContextAccessor accessor;
        private readonly string[] arrProxyHeader;

        public DefaultClientInfoProvider(IHttpContextAccessor _accessor)
        {
            accessor = _accessor;
            arrProxyHeader = new string[] { "X-Forwarded-For", "X-Real-IP", "Proxy-Client-IP", "WL-Proxy-Client-IP", "REMOTE_ADDR" };
        }

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public UserAgentInfo BrowserInfo
        {
            get
            {
                return new UserAgentInfo(accessor.HttpContext.Request.Headers[HeaderNames.UserAgent].ToString());
            }
        }

        /// <summary>
        /// 客户端Ip
        /// </summary>
        public string ClientIpAddress
        {
            get
            {
                //nginx方向代理
                string clientIP = string.Empty;
                foreach (var header in arrProxyHeader)
                {
                    clientIP = GetHeaderValue(header);
                    if (IsEffectiveIP(clientIP))
                    {
                        return GetRealIp(clientIP);
                    }
                }
                if (accessor.HttpContext?.Connection?.RemoteIpAddress != null)
                {
                    clientIP = accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                    return GetRealIp(clientIP);
                }
                return clientIP;
            }
        }

        /// <summary>
        /// 从请求中获取指定头对应值
        /// </summary>
        /// <param name="headerName">请求头</param>
        /// <returns>值</returns>
        private string GetHeaderValue(string headerName)
        {
            StringValues values;
            string headValue = string.Empty;
            if (accessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();
                if (!string.IsNullOrWhiteSpace(rawValues))
                {
                    headValue = Convert.ToString(values);
                    return headValue.TrimEnd(',').Split(',').AsEnumerable<string>()
                     .Select(s => s.Trim())
                     .ToList()[0];
                }
            }
            return headValue;
        }

        /// <summary>
        /// 是否有效IP地址
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <returns>bool</returns>
        private bool IsEffectiveIP(string ipAddress)
        {
            return !(string.IsNullOrWhiteSpace(ipAddress) || "unknown".Equals(ipAddress, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 获取当前服务器的真实IP地址
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private string GetRealIp(string ipAddress)
        {
            if (ipAddress.Equals("127.0.0.1") || ipAddress.Equals("::1"))
            {
                return HostHelper.GetIpAddressV4();
            }
            else
            {
                return ipAddress;
            }
        }
    }
}
