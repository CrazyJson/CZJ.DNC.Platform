using System.Data;

namespace CZJ.DNC.Excel
{
    /// <summary>
    /// 校验参数信息
    /// </summary>
    public class ImportVerifyParam
    {
        /// <summary>
        /// Excel数据源
        /// </summary>
        public DataTable DTExcel { get; set; }

        /// <summary>
        /// 行索引
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// 列索引
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColName { get; set; }

        /// <summary>
        /// 列值
        /// </summary>
        public object CellValue { get; set; }
    }
}
