using CZJ.Dependency;
using System;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Runtime.InteropServices;
using CZJ.DNC.Net;

namespace CZJ.Auditing
{
    /// <summary>
    /// 应用程序和服务器信息
    /// </summary>
    public class DefaultAppInfoProvider : IAppInfoProvider, ISingletonDependency
    {
        private readonly IServerAddressesFeature serverAddressesFeature;

        public DefaultAppInfoProvider(IServerAddressesFeature _serverAddressesFeature)
        {
            serverAddressesFeature = _serverAddressesFeature;
        }

        /// <summary>
        /// 机器名称
        /// </summary>
        public string MachineName
        {
            get
            {
                return Environment.MachineName;
            }
        }

        public string FrameWorkVersion
        {
            get
            {
                return RuntimeInformation.FrameworkDescription;
            }
        }

        public string ApplicationBasePath
        {
            get
            {
                return AppContext.BaseDirectory;
            }
        }

        /// <summary>
        /// 操作系统类型
        /// </summary>
        public string OSVersion
        {
            get
            {
                return RuntimeInformation.OSDescription;
            }
        }

        /// <summary>
        /// 操作系统位数
        /// </summary>
        public string OSBit
        {
            get
            {
                return RuntimeInformation.OSArchitecture.ToString();
            }
        }

        private string host;

        public string IpAddress
        {
            get
            {
                if (string.IsNullOrEmpty(host))
                {
                    foreach (var item in serverAddressesFeature.Addresses)
                    {
                        var httpArr = item.Split(new String[] { "//" }, StringSplitOptions.None);
                        var arr = (httpArr.Length > 1 ? httpArr[1] : httpArr[0]).Split(':');
                        if (HostHelper.VaildIP(arr[0]))
                        {
                            host = arr[0];
                        }
                    }
                    if (string.IsNullOrEmpty(host))
                    {
                        host = HostHelper.GetIpAddressV4();
                    }
                }
                return host;
            }
        }

        private string[] _ports;

        public string[] Ports
        {
            get
            {
                if (_ports == null)
                {
                    var ports = new string[serverAddressesFeature.Addresses.Count];
                    int i = 0;
                    foreach (var item in serverAddressesFeature.Addresses)
                    {
                        var arr = item.Split(':');
                        ports[i] = arr[arr.Length - 1];
                        i++;
                    }
                    _ports = ports;
                }
                return _ports;
            }
        }
    }
}
