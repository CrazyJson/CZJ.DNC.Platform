namespace CZJ.Common.Core
{
    /// <summary>
    /// 代理规则
    /// </summary>
    public class ProxyOption
    {
        /// <summary>
        /// 代理url正则
        /// </summary>
        public string UrlReg { get; set; }

        /// <summary>
        /// 代理转发url
        /// </summary>
        public string ProxyPass { get; set; }

        /// <summary>
        /// 排除Url
        /// </summary>
        public string[] Excludes { get; set; }
    }
}
