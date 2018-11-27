using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using CZJ.Common;

namespace CZJ.DNC.SwaggerExtend
{
    /// <summary>
    /// Swagger添加自定义头
    /// </summary>
    public class CustomHeaderFilter : IOperationFilter
    {
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
            var customHeaderType = typeof(CustomHeaderAttribute);
            //Method Attribute
            var customHeaders = context.MethodInfo.GetCustomAttributes(true)
                .Where(e => e.GetType() == customHeaderType).ToList();
            //Controller Attribute
            var controllHeaders = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                          .Where(e => e.GetType() == customHeaderType).ToList();
            customHeaders.AddRange(controllHeaders);
            if (customHeaders != null && customHeaders.Count > 0)
            {
                foreach(CustomHeaderAttribute customHeader in customHeaders)
                {
                    if (string.IsNullOrEmpty(customHeader.Name))
                    {
                        continue;
                    }
                    var bodyParameter = new NonBodyParameter
                    {
                        Name = customHeader.Name,
                        In = "header",
                        Type = "string",
                        Required = customHeader.Required,
                        Description = string.IsNullOrEmpty(customHeader.Description) ? customHeader.Name : customHeader.Description
                    };
                    operation.Parameters.Add(bodyParameter);
                }
            }
        }
    }
}
