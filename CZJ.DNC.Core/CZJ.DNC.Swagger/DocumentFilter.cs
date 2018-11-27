using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using CZJ.Common;

namespace CZJ.DNC.SwaggerExtend
{
    /// <summary>
    /// Document过滤
    /// </summary>
    public class DocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            var hiddenType = typeof(SwaggerHiddenAttribute);
            foreach (var apiDescription in context.ApiDescriptions)
            {
                if (apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) && methodInfo != null)
                {
                    if (methodInfo.GetCustomAttributes(true).FirstOrDefault(e => e.GetType() == hiddenType) == null)
                    {
                        continue;
                    }
                    var key = "/" + apiDescription.RelativePath.TrimEnd('/');
                    if (swaggerDoc.Paths.ContainsKey(key))
                        swaggerDoc.Paths.Remove(key);
                }
            }
        }
    }
}
