using Autofac;
using CZJ.Dependency;
using CZJ.Reflection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;
using WebApiClient.Contexts;

namespace CZJ.DNC.Feign
{
    /// <summary>
    /// 
    /// </summary>
    public class FeignRegister : IDependencyRegistrar
    {
        /// <summary>
        /// 
        /// </summary>
        public int Order
        {
            get
            {
                return 20;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="typeFinder"></param>
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            var arrHttpApiType = typeFinder.FindAll().Where(t => typeof(IHttpApi).IsAssignableFrom(t)
                && t != typeof(IHttpApi)).ToArray();
            HttpHostAttribute httpHostAttribute = null;
            HttpApiConfig httpApiConfig = null;
            HttpApiClient apiClient = null;
            Uri httpHost;
            UrlFilterAttribute urlFilter = new UrlFilterAttribute();
            foreach (var type in arrHttpApiType)
            {
                httpHostAttribute = type.GetCustomAttribute<HttpHostAttribute>();
                if (httpHostAttribute == null || httpHostAttribute.Host == null)
                {
                    httpHost = new Uri("", UriKind.Absolute);
                }
                else
                {
                    httpHost = httpHostAttribute.Host;
                }


                httpApiConfig = new HttpApiConfig { HttpHost = httpHost };

                httpApiConfig.GlobalFilters.Add(urlFilter);
                apiClient = HttpApiClient.Create(type, httpApiConfig);



                builder.RegisterInstance(apiClient).AsImplementedInterfaces()
                    .SingleInstance().PropertiesAutowired();
            }
        }
    }

    /// <summary>
    /// 表示将请求响应内容写入统一日志的过滤器
    /// </summary>
    public class UrlFilterAttribute : ApiActionFilterAttribute
    {
        /// <summary>
        /// tag的key
        /// </summary>
        private static readonly string tagKey = "$TraceFilter";

        /// <summary>
        /// 获取或设置日志的EventId
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// 获取是否输出请求内容
        /// </summary>
        public bool TraceRequest { get; set; } = true;

        /// <summary>
        /// 获取是否输出响应内容
        /// </summary>
        public bool TraceResponse { get; set; } = true;

        /// <summary>
        /// 将请求响应内容写入统一日志的过滤器
        /// </summary>
        public UrlFilterAttribute()
        {
            this.OrderIndex = int.MaxValue;
        }

        /// <summary>
        /// 准备请求之前
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async override Task OnBeginRequestAsync(ApiActionContext context)
        {
            //if (context.HttpApiConfig.LoggerFactory == null)
            //{
            //    return;
            //}
            context.RequestMessage.RequestUri = new Uri("");
            var request = new Request
            {
                Time = DateTime.Now,
                Message = await context.RequestMessage.ToStringAsync().ConfigureAwait(false)
            };
            context.Tags.Set(tagKey, request);
        }

        /// <summary>
        /// 请求完成之后
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public async override Task OnEndRequestAsync(ApiActionContext context)
        {
            var logging = context.HttpApiConfig.LoggerFactory;
            //if (logging == null)
            //{
            //    return;
            //}

            var builder = new StringBuilder();
            const string format = "yyyy-MM-dd HH:mm:ss.fff";
            var request = context.Tags.Get(tagKey).As<Request>();

            if (this.TraceRequest == true)
            {
                builder.AppendLine($"[REQUEST] {request.Time.ToString(format)}")
                    .AppendLine($"{request.Message.TrimEnd()}");
            }

            var response = context.ResponseMessage;
            if (this.TraceResponse && response != null && response.Content != null)
            {
                if (this.TraceRequest == true)
                {
                    builder.AppendLine();
                }

                builder.AppendLine($"[RESPONSE] {DateTime.Now.ToString(format)}")
                    .AppendLine($"{await response.Content.ReadAsStringAsync().ConfigureAwait(false)}");
            }

            var message = builder
                .AppendLine()
                .Append($"[TIMESPAN] {DateTime.Now.Subtract(request.Time)}")
                .ToString();

            var method = context.ApiActionDescriptor.Member;
            var categoryName = $"{method.DeclaringType.Name}.{method.Name}";
            var logger = logging.CreateLogger(categoryName);

            if (context.Exception == null)
            {
                Console.WriteLine("{0}{1}", this.EventId, message);
                //logger.LogInformation(this.EventId, message);
            }
            else
            {
                Console.WriteLine("{0}{1}{2}", this.EventId, context.Exception, message);
                //logger.LogError(this.EventId, context.Exception, message);
            }
        }

        /// <summary>
        /// 请求信息
        /// </summary>
        private class Request
        {
            /// <summary>
            /// 请求时间
            /// </summary>
            public DateTime Time { get; set; }

            /// <summary>
            /// 请求消息
            /// </summary>
            public string Message { get; set; }
        }
    }
}
