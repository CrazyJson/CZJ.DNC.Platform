namespace CZJ.Common.Core
{
    /// <summary>
    /// 静态资源浏览配置
    /// </summary>
    public class StaticFilesOptions
    {
        /// <summary>
        /// 以程序目录开始  磁盘相对路径
        /// </summary>
        public string PhysicalRelativePath { get; set; }

        /// <summary>
        /// url起始路径
        /// </summary>
        public string RequestPath { get; set; }

        /// <summary>
        /// 是否开启目录浏览
        /// </summary>
        public bool EnableDirectoryBrowsing { get; set; } = false;
    }
}
