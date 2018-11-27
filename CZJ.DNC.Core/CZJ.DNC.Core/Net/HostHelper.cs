using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace CZJ.DNC.Net
{
    /// <summary>
    /// 服务器信息读取
    /// </summary>
    public class HostHelper
    {
        /// <summary>
        /// IP正则表达式
        /// </summary>
        private static Regex IPRegex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");

        /// <summary>
        /// 验证字符串是否正确IP地址
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns></returns>
        public static bool VaildIP(string ip)
        {
            return IPRegex.IsMatch(ip);
        }

        /// <summary>
        /// 获取本机的IPV4地址
        /// </summary>
        /// <returns>本机的IPV4地址</returns>
        public static string GetIpAddressV4()
        {
            return GetAllIpAddressV4()[0];
        }

        /// <summary>
        /// 获取本机所有的IPV4地址
        /// </summary>
        /// <returns>本机所有的IPV4地址</returns>
        public static string[] GetAllIpAddressV4()
        {
            var list = GetRealNetworkInterfaceMessage();
            if (list.Count == 0)
            {
                list.Add("127.0.0.1");
            }
            return list.ToArray();
        }

        /// <summary>
        /// 判断服务节点是否激活
        /// </summary>
        /// <param name="hostwithport">主机ip:port</param>
        /// <returns>bool</returns>
        public static bool EnsureActiveNode(string hostwithport)
        {
            var arr = hostwithport.Split(':');
            string ipAddress = arr[0];
            int port = Convert.ToInt32(arr[1]);
            return EnsureActiveNode(ipAddress, port);
        }

        /// <summary>
        /// 判断服务节点是否激活
        /// </summary>
        /// <param name="host">主机ip</param>
        /// <param name="port">端口</param>
        /// <returns>bool</returns>
        public static bool EnsureActiveNode(string host, int port)
        {
            try
            {
                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Bind(new IPEndPoint(IPAddress.Any, 0));
                    socket.Connect(new IPEndPoint(IPAddress.Parse(host), port));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary></summary>   
        /// 获取本机无线网卡，物理网卡ipv4
        /// <summary></summary>   
        public static List<string> GetRealNetworkInterfaceMessage()
        {
            List<string> list = new List<string>();
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var network in networkInterfaces)
            {
                if (network.OperationalStatus != OperationalStatus.Up)
                    continue;
                var properties = network.GetIPProperties();
                if (properties.GatewayAddresses.Count == 0)
                    continue;

                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;
                    if (IPAddress.IsLoopback(address.Address))
                        continue;
                    list.Add(address.Address.ToString());
                    break;
                }
            }
            return list;
        }

        public static bool IsPortInUsed(int port)
        {
            IPGlobalProperties ipGlobalProps = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipsTCP = ipGlobalProps.GetActiveTcpListeners();

            if (ipsTCP.Any(p => p.Port == port))
            {
                return true;
            }

            IPEndPoint[] ipsUDP = ipGlobalProps.GetActiveUdpListeners();
            if (ipsUDP.Any(p => p.Port == port))
            {
                return true;
            }

            TcpConnectionInformation[] tcpConnInfos = ipGlobalProps.GetActiveTcpConnections();
            if (tcpConnInfos.Any(conn => conn.LocalEndPoint.Port == port))
            {
                return true;
            }

            return false;
        }
    }
}
