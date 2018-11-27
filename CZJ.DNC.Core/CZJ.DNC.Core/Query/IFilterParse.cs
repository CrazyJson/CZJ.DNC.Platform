namespace CZJ.Common.Query
{
    /// <summary>
    /// 过滤条件解析
    /// </summary>
    public interface IFilterParse
    {
        /// <summary>
        /// 过滤条件解析
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>解析结果</returns>
        FilterSQL Parse(IBaseFilter filter);
    }
}
