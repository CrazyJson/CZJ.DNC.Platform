using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CZJ.DNC.SwaggerExtend
{
    /// <summary>
    /// Swagger添加Token认证头
    /// </summary>
    public class AddAuthTokenHeaderParameter : IOperationFilter
    {
        private IHostingEnvironment env;

        private string token;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        public AddAuthTokenHeaderParameter(IHostingEnvironment env, IConfiguration configuration)
        {
            this.env = env;
            token = configuration.GetValue<string>("Token");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }
            var anonymousType = typeof(AllowAnonymousAttribute);
            if (context.MethodInfo.GetCustomAttributes(true)
                .FirstOrDefault(e => e.GetType() == anonymousType) == null &&
                context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                           .FirstOrDefault(e => e.GetType() == anonymousType) == null)
            {
                var bodyParameter = new NonBodyParameter
                {
                    Name = "Authorization",
                    In = "header",
                    Type = "string",
                    Required = true,
                    Description = "认证Token(Basic +Token)"
                };
                if (env.IsDevelopment() && !string.IsNullOrWhiteSpace(token))
                {
                    bodyParameter.Default = $"Basic {token}";
                }
                operation.Parameters.Add(bodyParameter);
            }
        }
    }
}
