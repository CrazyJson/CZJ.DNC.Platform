using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZJ.Common.Query
{
    /// <summary>
    /// 过滤条件SQL
    /// </summary>
    public class FilterSQL
    {
        public FilterSQL()
        {
            WhereBuilder = new StringBuilder();
            OrderBuilder = new StringBuilder();
        }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// 第几页
        /// </summary>
        public int? PageIndex { get; set; }

        /// <summary>
        /// 过滤条件
        /// </summary>
        public StringBuilder WhereBuilder { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public StringBuilder OrderBuilder { get; set; }

        /// <summary>
        ///  获取Where条件SQL
        /// </summary>
        public string GetWhereSql()
        {
            if (string.IsNullOrWhiteSpace(WhereBuilder.ToString().Trim()))
            {
                WhereBuilder.Append(" 1=1");
            }
            return WhereBuilder.ToString().Trim();
        }

        /// <summary>
        ///  获取排序SQL
        /// </summary>
        public string GetOrderSql()
        {
            return OrderBuilder.ToString().Trim();
        }

        /// <summary>
        ///  参数
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> Param { get; private set; }

        /// <summary>
        ///     Set alternative param
        /// </summary>
        /// <param name="param">The param.</param>
        public void SetParam(IEnumerable<KeyValuePair<string, object>> param)
        {
            if (Param != null)
            {
                Param = Param.Concat(param);
            }
            else
            {
                Param = param;
            }
        }
    }
}
