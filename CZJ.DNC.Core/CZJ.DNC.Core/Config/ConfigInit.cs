using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace CZJ.Common.Core
{
    /// <summary>
    /// 配置文件信息初始化,config.json会覆盖appsettings.json
    /// 下次获取最新又覆盖了本地修改的相关配置问题，通过添加本地配置文件格式为app_Local.config开解决此问题
    /// </summary>
    public class ConfigInit
    {
        /// <summary>
        /// 本地配置文件地址
        /// </summary>
        private static readonly string ConfigLocalPath = FileHelper.GetAbsolutePath("Config/Config_Local.config");

        /// <summary>
        /// 获取本地化配置文件地址
        /// </summary>
        /// <returns>本地化配置文件地址</returns>
        public static string GetLocalConfigPath()
        {
            return ConfigLocalPath;
        }
        /// <summary>
        /// 初始化配置信息
        /// </summary>
        public static void InitConfig(IConfiguration Configuration)
        {
            //数据库连接字符串信息
            var dict = Configuration.GetSection("ConnectionStrings").Get<Dictionary<string, string>>();
            var dictCon = new Dictionary<string, string>();
            if (dict != null)
            {
                foreach (string key in dict.Keys)
                {
                    string value = dict[key];
                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }
                    dictCon[key] = value;
                }
            }
            SysConfig.ConnectionStrings = dictCon;

            SysConfig.MicroServiceOption = Configuration.GetSection("MicroService").Get<MicroServiceOption>();
            if (SysConfig.MicroServiceOption == null)
            {
                SysConfig.MicroServiceOption = new MicroServiceOption();
            }
            SysConfig.StaticDirectory = Configuration.GetSection("StaticDirectory").Get<List<StaticFilesOptions>>();
        }
    }
}
