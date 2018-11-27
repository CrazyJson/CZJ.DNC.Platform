namespace CZJ.Common.Query
{
    /// <summary>
    /// 过滤条件接口定义
    /// </summary>
    public interface IBaseFilter
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        string SortField { get; set; }

        /// <summary>
        /// 升序或者降序 desc asc
        /// </summary>
        string SortOrder { get; set; }
    }

    /// <summary>
    /// 分页查询过滤条件
    /// </summary>
    public class PageFilter : IBaseFilter
    {
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 第几页,从1开始
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// 升序或者降序 desc asc
        /// </summary>
        public string SortOrder { get; set; }
    }
}
