using System;

namespace CZJ.Common.Query
{
    /// <summary>
    /// 过滤字段描述，包含匹配符和对应的匹配字段
    /// </summary>
    /// <example>
    ///     [Filter(Operator = OperatorEnum.EQ,FieldName="RoleName")]
    ///     public class RoleName{get; set;} 
    /// </example>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PropFilterAttribute : Attribute
    {

        /// <summary>
        /// 对应字段名称，为空默认为属性名称。
        /// 如果为多个字段逗号分隔，被解析成 a=1 or b=1
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// SQL操作符 like =  in...
        /// </summary>
        public OperatorEnum Operator { get; set; }

        /// <summary>
        /// 字段查询时是否忽略大小写比较
        /// </summary>
        public bool IgnoreCase { get; set; }
    }
}
