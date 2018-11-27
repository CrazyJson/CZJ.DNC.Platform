using System;

namespace CZJ.Common
{
    /// <summary>
    /// 不在Swagger文档里面生成描述的标记属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class SwaggerHiddenAttribute : Attribute
    {

    }
}
