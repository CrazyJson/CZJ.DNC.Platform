using CZJ.Dependency;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiClient.Attributes;
using WebApiClient.Contexts;

namespace CZJ.DNC.Feign
{
    /// <summary>
    /// 请求传递过滤器
    /// </summary>
    public class TraceHeaderFilterAttribute : ApiActionFilterAttribute
    {
        private string[] headers;

        /// <summary>
        /// 将请求响应内容写入统一日志的过滤器
        /// </summary>
        public TraceHeaderFilterAttribute(string[] headers)
        {
            this.OrderIndex = int.MinValue + 1;
            this.Enable = true;
            this.headers = headers;
        }

        /// <summary>
        /// 准备请求之前
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public override Task OnBeginRequestAsync(ApiActionContext context)
        {
            if (headers != null && headers.Length > 0)
            {
                var list = new List<string>(headers.Length);
                foreach (string header in headers)
                {
                    var accessor = IocManager.Instance.Resolve<IHttpContextAccessor>();
                    StringValues values;
                    if (accessor.HttpContext?.Request?.Headers?.TryGetValue(header, out values) ?? false)
                    {
                        context.RequestMessage.Headers.TryAddWithoutValidation(header, values.ToArray());
                        list.Add(header);
                    }
                }
            }
            return base.OnBeginRequestAsync(context);
        }
    }
}
