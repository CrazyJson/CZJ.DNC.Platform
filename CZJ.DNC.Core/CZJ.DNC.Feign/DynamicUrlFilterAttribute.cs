using CZJ.Dependency;
using CZJ.DNC.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        public override async Task OnBeginRequestAsync(ApiActionContext context)
        {
            context.Tags.Set(tagKey, DateTime.Now);
            //动态获取host
            string[] hosts = null;
            try
            {
                var provider = IocManager.Instance.Resolve<IServiceDiscoveryProvider>();
                var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                object objAppNo = context.Tags["AppNo"];
                dict["AppNo"] = objAppNo;
                hosts = await provider.GetHost(context.RequestMessage.RequestUri, context.RequestMessage.Method, dict);
                if (hosts == null || hosts.Length == 0)
                {
                    throw new Exception($"未找到{(objAppNo != null && !string.IsNullOrEmpty(objAppNo.ToString()) ? objAppNo.ToString() : "")}" +
                        $"【{context.RequestMessage.Method.ToString()}{context.RequestMessage.RequestUri.PathAndQuery}】的节点信息");
                }
            }
            catch
            {
                throw new Exception("未注册IServiceDiscoveryProvider服务的实现类");
            }
            Random rand = new Random();
            var index = rand.Next(hosts.Length);
            var host = hosts[index];
            string url = host + context.RequestMessage.RequestUri.PathAndQuery;
            Uri uri = new Uri(url);
            if (context.HttpApiConfig.LoggerFactory != null)
            {
                var method = context.ApiActionDescriptor.Member;
                var categoryName = $"{method.DeclaringType.Name}.{method.Name}";
                var logger = context.HttpApiConfig.LoggerFactory.CreateLogger<DynamicUrlFilterAttribute>();
                logger.LogInformation("服务间调用插件Feign【{0}】--获取到请求【{1} {2}】的实际节点：【{3}】",
                      categoryName, context.RequestMessage.Method.ToString(),
                        context.RequestMessage.RequestUri.PathAndQuery, string.Join("，", hosts));
            }
            context.RequestMessage.RequestUri = uri;
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
