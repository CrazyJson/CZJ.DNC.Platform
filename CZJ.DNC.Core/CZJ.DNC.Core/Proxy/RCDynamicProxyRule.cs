//using CZJ.Common.Core;
//using CZJ.Common.Micro;
//using CZJ.DNC.AsyncLock;
//using CZJ.DNC.Net;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace CZJ.Common.Proxy
//{
//    /// <summary>
//    /// 从注册中心获取接口节点的动态代理规则
//    /// </summary>
//    public class RCDynamicProxyRule : IDynamicProxyRule
//    {
//        private ConcurrentDictionary<string, List<MicroPathNode>> dict = null;

//        /// <summary>
//        /// 异步锁对象
//        /// </summary>
//        private AsyncLock _lock;

//        private readonly ILogger logger;

//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        public RCDynamicProxyRule(ILogger<RCDynamicProxyRule> logger)
//        {
//            dict = new ConcurrentDictionary<string, List<MicroPathNode>>(StringComparer.OrdinalIgnoreCase);
//            this.logger = logger;
//            _lock = new AsyncLock();
//        }

//        /// <summary>
//        /// 解析转发路径对应实际接口路径
//        /// </summary>
//        /// <param name="path">转发路径</param>
//        /// <returns>实际路径</returns>
//        public async Task<string> ParsePath(string path)
//        {
//            return await ParsePath(path, null);
//        }

//        /// <summary>
//        /// 解析转发路径对应实际接口路径
//        /// </summary>
//        /// <param name="path">转发路径</param>
//        /// <param name="method">请求Method get,post 用于精确匹配</param>
//        /// <returns>实际路径</returns>
//        public async Task<string> ParsePath(string path, HttpMethod method)
//        {
//            if (string.IsNullOrWhiteSpace(path))
//            {
//                throw new ArgumentNullException("参数path空异常");
//            }
//            if (dict.Keys.Count == 0)
//            {
//                using (await _lock.LockAsync())
//                {
//                    if (dict.Keys.Count == 0)
//                    {
//                        await QueryAllPath();
//                    }
//                }
//            }
//            string pPath = path;
//            //解析路径对应节点
//            if (!path.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) &&
//                !path.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
//            {
//                pPath = "http://127.0.0.1" + path;
//            }
//            Uri uri = new Uri(pPath, UriKind.RelativeOrAbsolute);
//            List<MicroNode> allNodes = new List<MicroNode>();
//            string activeNodeHost = string.Empty;
//            string key = uri.AbsolutePath;
//            //从缓存里面获取节点信息
//            if (dict.TryGetValue(key, out List<MicroPathNode> nodes))
//            {
//                IEnumerable<List<MicroNode>> allNodesList = null;
//                if (method != null)
//                {
//                    allNodesList = nodes.Where(e => e.Path.method == method.ToString()).Select(e => e.Nodes);
//                }
//                if (allNodesList == null || allNodesList.Count() == 0)
//                {
//                    allNodesList = nodes.Select(e => e.Nodes);
//                }
//                foreach (var item in allNodesList)
//                {
//                    allNodes.AddRange(item);
//                }
//                activeNodeHost = GetHost(allNodes);
//                if (string.IsNullOrEmpty(activeNodeHost))
//                {
//                    dict.TryRemove(key, out List<MicroPathNode> t);
//                }
//            }
//            //从注册中心获取节点信息
//            if (string.IsNullOrEmpty(activeNodeHost))
//            {
//                var pn = await GetPathFromRegisterCenter(uri.AbsolutePath);
//                if (pn != null)
//                {
//                    activeNodeHost = GetHost(pn.Nodes);
//                    if (!string.IsNullOrEmpty(activeNodeHost))
//                    {
//                        dict.TryAdd(key, new List<MicroPathNode> { pn });
//                    }
//                }
//            }
//            if (string.IsNullOrEmpty(activeNodeHost))
//            {
//                return string.Empty;
//            }
//            else
//            {
//                return activeNodeHost;
//            }
//        }

//        #region "私有方法"

//        /// <summary>
//        /// 获取激活节点
//        /// </summary>
//        /// <param name="nodes">节点列表</param>
//        /// <returns>激活节点信息</returns>
//        private string GetHost(List<MicroNode> nodes)
//        {
//            if (nodes == null || nodes.Count == 0)
//            {
//                return string.Empty;
//            }
//            Random r = new Random();
//            var node = nodes[r.Next(nodes.Count)];
//            if (HostHelper.EnsureActiveNode(node.Host.host))
//            {
//                return $"{node.Host.scheme}://{node.Host.host}";
//            }
//            else
//            {
//                nodes.Remove(node);
//                return GetHost(nodes);
//            }
//        }

//        /// <summary>
//        /// 从注册中心中获取所有路径信息
//        /// </summary>
//        private async Task QueryAllPath()
//        {
//            CitmsHttpRequest request = new CitmsHttpRequest
//            {
//                AddressUrl = SysConfig.MicroServiceOption.Cloud.RegistCenterUri + "/micro/UDDI/QueryPaths",
//                Method = HttpMethod.Post,
//                ProxyRequest = false,
//                Body = new
//                {
//                    pathPattern = "",
//                    scheme = "http"
//                }
//            };
//            var result = await request.SendAsync<List<MicroPathNode>>();
//            if (result != null)
//            {
//                logger.LogInformation($"从注册中心({SysConfig.MicroServiceOption.Cloud.RegistCenterUri})查询所有Http路径成功，共获取{result.Count}条路径信息");
//                var resultDict = result.GroupBy(e => e.Path.path).ToDictionary(e => e.Key, e => e.ToList());
//                foreach (var key in resultDict.Keys)
//                {
//                    dict.TryAdd(key, resultDict[key]);
//                }
//            }
//        }


//        /// <summary>
//        ///从注册中心中,获取实际接口路径地址
//        /// </summary>
//        /// <param name="path">转发路径</param>
//        private async Task<MicroPathNode> GetPathFromRegisterCenter(string path)
//        {
//            CitmsHttpRequest request = new CitmsHttpRequest
//            {
//                AddressUrl = SysConfig.MicroServiceOption.Cloud.RegistCenterUri + "/micro/UDDI/QueryPaths",
//                Method = HttpMethod.Post,
//                ProxyRequest = false,
//                Body = new
//                {
//                    pathPattern = path,
//                    scheme = "http"
//                }
//            };
//            var result = await request.SendAsync<List<MicroPathNode>>();
//            if (result != null && result.Count > 0)
//            {
//                logger.LogInformation($"从注册中心({SysConfig.MicroServiceOption.Cloud.RegistCenterUri})查询{path}路径信息成功");
//                return result[0];
//            }
//            else
//            {
//                return null;
//            }
//        }
//        #endregion
//    }
//}
