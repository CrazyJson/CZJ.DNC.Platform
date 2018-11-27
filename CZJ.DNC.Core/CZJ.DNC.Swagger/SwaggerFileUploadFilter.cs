using CZJ.Common;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace CZJ.DNC.SwaggerExtend
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public class SwaggerFileUploadFilter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (!context.ApiDescription.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase) &&
                !context.ApiDescription.HttpMethod.Equals("PUT", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            var parameters = context.ApiDescription.ActionDescriptor.Parameters;
            var fileParameters = parameters.Where(n => n.ParameterType == typeof(SwaggerFile)).ToList();
            if (fileParameters.Count <= 0)
            {
                return;
            }
            operation.Consumes.Add("multipart/form-data");

            foreach (var fileParameter in fileParameters)
            {
                var parameter = operation.Parameters.SingleOrDefault(n => n.Name == fileParameter.Name);
                operation.Parameters.Remove(parameter);
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = parameter.Name,
                    In = "formData",
                    Description = "上传文件",
                    Required = true,
                    Type = "file"
                });
            }
        }
    }
}
