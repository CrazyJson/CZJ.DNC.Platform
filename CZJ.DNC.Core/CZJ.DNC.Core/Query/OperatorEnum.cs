using System.ComponentModel;

namespace CZJ.Common.Query
{
    /// <summary>
    /// SQL查询匹配符
    /// </summary>
    public enum OperatorEnum
    {
        /// <summary>
        /// 等于
        /// </summary>
        [Description("=")]
        EQ = 0,

        /// <summary>
        /// 不等于
        /// </summary>
        [Description("<>")]
        NE = 1,

        /// <summary>
        /// 大于
        /// </summary>
        [Description(">")]
        GT = 2,

        /// <summary>
        /// 大于等于
        /// </summary>
        [Description(">=")]
        GE = 3,

        /// <summary>
        /// 小于
        /// </summary>
        [Description("<")]
        LT = 4,

        /// <summary>
        /// 小于等于
        /// </summary>
        [Description("<=")]
        LE = 5,

        /// <summary>
        /// 自动在值前后面加上%
        /// </summary>
        [Description("like")]
        Like = 6,

        /// <summary>
        /// 自动在值前面加上%
        /// </summary>
        [Description("like")]
        LLike = 7,

        /// <summary>
        /// 自动在值后面加上%
        /// </summary>
        [Description("like")]
        RLike = 8,


        /// <summary>
        /// 不类似
        /// </summary>
        [Description("not like")]
        NotLike = 9,

        /// <summary>
        /// 为空
        /// </summary>
        [Description("is null")]
        Null = 10,

        /// <summary>
        /// 不为空
        /// </summary>
        [Description("is not null")]
        NotNull = 11,

        /// <summary>
        /// 包含
        /// </summary>
        [Description("in")]
        In = 12,

        /// <summary>
        /// 不包含
        /// </summary>
        [Description("not in")]
        NotIn = 13
    }
}
