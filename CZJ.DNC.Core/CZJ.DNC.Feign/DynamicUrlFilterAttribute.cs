using CZJ.Dependency;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
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
        /// tag的key
        /// </summary>
        private const string tagKey = "DynamicUrlFilter";

        /// <summary>
        /// 
        /// </summary>
        public DynamicUrlFilterAttribute()
        {
            this.OrderIndex = 0;
        }

        /// <summary>
        /// 准备请求之前
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public override Task OnBeginRequestAsync(ApiActionContext context)
        {
            context.Tags.Set(tagKey, DateTime.Now);
            //动态获取host
            //IocManager.Instance.Resolve<IDy>()
            string url = "http://192.168.0.133:6001" + context.RequestMessage.RequestUri.PathAndQuery;
            Uri uri = new Uri(url);
            if (context.HttpApiConfig.LoggerFactory != null)
            {
                var method = context.ApiActionDescriptor.Member;
                var categoryName = $"{method.DeclaringType.Name}.{method.Name}";
                var logger = context.HttpApiConfig.LoggerFactory.CreateLogger<DynamicUrlFilterAttribute>();
                logger.LogInformation("服务间调用插件Feign【{0}】--获取到请求【{1} {2}】的实际节点：【{3}:{4}】",
                      categoryName, context.RequestMessage.Method.ToString(),
                        context.RequestMessage.RequestUri.PathAndQuery, uri.Host, uri.Port);
            }
            context.RequestMessage.RequestUri = uri;
            return base.OnBeginRequestAsync(context);
        }

        public override Task OnEndRequestAsync(ApiActionContext context)
        {
            var beginTime = context.Tags.Get(tagKey).As<DateTime>();
            var method = context.ApiActionDescriptor.Member;
            var categoryName = $"{method.DeclaringType.Name}.{method.Name}";
            var logger = context.HttpApiConfig.LoggerFactory.CreateLogger<DynamicUrlFilterAttribute>();
            string message = string.Format("服务间调用插件Feign【{0}】--【{1} {2}】响应码：{3}，耗时：{4}ms",
                  categoryName, context.RequestMessage.Method.ToString(), context.RequestMessage.RequestUri,
                  context.ResponseMessage.StatusCode, DateTime.Now.Subtract(beginTime).TotalMilliseconds);
            if (context.Exception == null)
            {
                logger.LogInformation(message);
            }
            else
            {
                logger.LogError(context.Exception, message);
            }
            return base.OnEndRequestAsync(context);
        }
    }
}
