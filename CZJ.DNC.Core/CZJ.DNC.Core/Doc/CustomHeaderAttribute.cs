using System;

namespace CZJ.Common
{
    /// <summary>
    /// Swagger自定义头属性,允许定义多个
    /// </summary>
    /// <example>
    /// <code>
    ///     [CustomHeader(Description ="认证Token(Basic +Token)",Name ="Token")]
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CustomHeaderAttribute : Attribute
    {
        public CustomHeaderAttribute()
        {
            Required = true;
        }

        /// <summary>
        /// Header名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否必须
        /// </summary>
        public bool Required { get; set; }
    }
}
