namespace CZJ.DNC.Excel
{
    /// <summary>
    /// excel导入结果
    /// </summary>
    public class ImportResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 额外数据信息
        /// </summary>
        public object ExtraInfo { get; set; }
    }
}
