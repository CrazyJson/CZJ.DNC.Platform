using CZJ.Dependency;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApiClient.Attributes;
using WebApiClient.Contexts;

namespace CZJ.DNC.Feign
{
    /// <summary>
    /// 动态设置日志工厂过滤器
    /// </summary>
    public class LoggerFactoryFilterAttribute : ApiActionFilterAttribute
    {
        private static object lockObj = new object();
        /// <summary>
        /// 
        /// </summary>
        public LoggerFactoryFilterAttribute()
        {
            OrderIndex = int.MinValue;
        }

        /// <summary>
        /// 准备请求之前
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public override Task OnBeginRequestAsync(ApiActionContext context)
        {
            if (context.HttpApiConfig.LoggerFactory == null)
            {
                lock (lockObj)
                {
                    if (context.HttpApiConfig.LoggerFactory == null)
                    {
                        context.HttpApiConfig.LoggerFactory = IocManager.Instance.Resolve<ILoggerFactory>();
                    }
                }
            }
            return base.OnBeginRequestAsync(context);
        }
    }
}
