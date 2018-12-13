using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebApiClient.Attributes;
using WebApiClient.Contexts;

namespace CZJ.DNC.Feign
{
    /// <summary>
    /// 动态host获取过滤器
    /// </summary>
    public class DynamicUrlFilterAttribute : ApiActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public DynamicUrlFilterAttribute()
        {
            this.OrderIndex = int.MinValue;
        }

        /// <summary>
        /// 准备请求之前
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public override Task OnBeginRequestAsync(ApiActionContext context)
        {
            //动态获取host
            context.RequestMessage.RequestUri = new Uri("");
            if (context.HttpApiConfig.LoggerFactory != null)
            {
                var method = context.ApiActionDescriptor.Member;
                var categoryName = $"{method.DeclaringType.Name}.{method.Name}";
                var logger = context.HttpApiConfig.LoggerFactory.CreateLogger(categoryName);
                logger.LogInformation(1, "1233");
            }
            return base.OnBeginRequestAsync(context);
        }
    }
}
