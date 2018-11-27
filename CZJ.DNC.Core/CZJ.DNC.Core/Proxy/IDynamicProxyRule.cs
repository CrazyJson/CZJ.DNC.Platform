using System.Net.Http;
using System.Threading.Tasks;

namespace CZJ.Common.Proxy
{
    /// <summary>
    /// 动态代理规则接口
    /// </summary>
    public interface IDynamicProxyRule
    {
        /// <summary>
        /// 解析转发路径对应实际接口路径
        /// </summary>
        /// <param name="path">转发路径</param>
        /// <returns>转发节点</returns>
        /// <remarks>
        ///  例如传入path 为 http://127.0.0.1/api/sysconfig/account/query
        ///  规则解析后返回  http://192.168.0.1 不需要返回后面的querypath
        ///  那么最终调用路径 http://192.168.0.1/api/sysconfig/account/query
        /// </remarks>
        Task<string> ParsePath(string path);

        /// <summary>
        /// 解析转发路径对应实际接口路径
        /// </summary>
        /// <param name="path">转发路径</param>
        /// <param name="method">请求Method get,post 用于精确匹配</param>
        /// <returns>实际路径</returns>
        /// <remarks>
        ///  例如传入path 为 http://127.0.0.1/api/sysconfig/account/query
        ///  规则解析后返回  http://192.168.0.1 不需要返回后面的querypath
        ///  那么最终调用路径 http://192.168.0.1/api/sysconfig/account/query
        /// </remarks>
        Task<string> ParsePath(string path, HttpMethod method);
    }
}
