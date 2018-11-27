namespace CZJ.Auditing
{
    /// <summary>
    /// 客户端信息接口
    /// </summary>
    public interface IClientInfoProvider
    {
        /// <summary>
        /// 浏览器信息
        /// </summary>
        UserAgentInfo BrowserInfo { get; }

        /// <summary>
        /// 客户端Ip地址
        /// </summary>
        string ClientIpAddress { get; }
    }
}