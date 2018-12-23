using System;

namespace CZJ.DNC.Excel
{
    /// <summary>
    /// 字段校验及映射信息
    /// </summary>
    public class ImportVerify
    {
        /// <summary>
        /// Excel列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 数据库字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 定义列的数据类型 typeof(System.DateTime)
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// 字段校验函数
        /// </summary>
        public Func<ImportVerifyParam, object, string> VerifyFunc { get; set; }
    }
}
