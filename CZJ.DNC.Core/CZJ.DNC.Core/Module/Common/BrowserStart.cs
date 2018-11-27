using CZJ.Common.Core.Uri;
using CZJ.Auditing;

namespace CZJ.Common.Module.Common
{
    /// <summary>
    /// 端口监听成功后打开浏览器
    /// </summary>
    public class BrowserStart : IAfterRunConfigure
    {
        private IAppInfoProvider app;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="app"></param>
        public BrowserStart(IAppInfoProvider app)
        {
            this.app = app;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Order => 1;

        /// <summary>
        /// 
        /// </summary>
        public void Configure()
        {
            string url = $"http://{app.IpAddress}:{app.Ports[0]}";
            url.OpenBrowserUrl();
        }
    }
}
