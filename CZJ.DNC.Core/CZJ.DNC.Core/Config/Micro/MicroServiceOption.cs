namespace CZJ.Common.Core
{
    /// <summary>
    /// 微服务的配置信息
    /// </summary>
    public class MicroServiceOption
    {
        /// <summary>
        /// 服务的唯一标识 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 服务的标签
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// 服务的名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 服务的版本号
        /// </summary>
        public string Version { get; set; }
    }
}
