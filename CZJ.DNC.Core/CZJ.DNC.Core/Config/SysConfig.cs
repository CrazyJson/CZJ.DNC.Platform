using System.Collections.Generic;

namespace CZJ.Common.Core
{
    /// <summary>
    /// 缓存系统所有配置信息，以键值对形式存在
    /// </summary>
    /// <example>
    /// 获取连接字符串 SysConfig.DefaultConnection
    /// </example>
    public class SysConfig
    {
        /// <summary>
        /// 数据库连接字符串信息
        /// </summary>
        public static Dictionary<string, string> ConnectionStrings { get; set; }

        /// <summary>
        /// 微服务配置信息
        /// </summary>
        public static MicroServiceOption MicroServiceOption { get; set; }

        /// <summary>
        /// 静态目录配置
        /// </summary>
        public static List<StaticFilesOptions> StaticDirectory { get; set; }
    }
}
