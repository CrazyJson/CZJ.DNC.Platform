namespace CZJ.DNC.Excel
{
    /// <summary>
    /// DataGrid信息
    /// </summary>
    public class ColumnInfo
    {
        /// <summary>
        /// 文本对齐方式
        /// </summary>
        public string Align { get; set; } = "left";

        /// <summary>
        /// 标题头
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 绑定列
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 是否超链接列
        /// </summary>
        public bool IsLink { get; set; }
    }
}
