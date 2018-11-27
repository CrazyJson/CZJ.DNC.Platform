using CZJ.Dependency;
using CZJ.Reflection;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CZJ.Common.Query
{
    /// <summary>
    /// 过滤条件解析
    /// </summary>
    public class DefaultFilterParse : IFilterParse, ISingletonDependency
    {
        private static BindingFlags BindFlags = BindingFlags.Instance | BindingFlags.Public;

        private ConcurrentDictionary<Type, List<FilterMap>> _cache = new ConcurrentDictionary<Type, List<FilterMap>>();

        /// <summary>
        /// 过滤条件解析
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>解析结果</returns>
        public FilterSQL Parse(IBaseFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("参数filter空异常，解析过滤条件失败");
            }
            var type = filter.GetType();
            var pageType = typeof(PageFilter);
            var mapList = GetOrAddMap(type);

            FilterSQL sqlQuery = new FilterSQL();
            if (pageType.IsAssignableFrom(type) && type != pageType)
            {
                //分页查询
                var pl = filter as PageFilter;
                if (pl.PageIndex < 1 && pl.PageSize > 0)
                {
                    throw new Exception("分页查询页索引PageIndex从1开始，请检查参数是否正确");
                }
                sqlQuery.PageIndex = pl.PageIndex;
                sqlQuery.PageSize = pl.PageSize;
            }

            #region "获取查询条件"
            var dictParam = new Dictionary<string, object>();
            var dictParamCount = new Dictionary<string, int>();
            string paramName = string.Empty;
            List<string> listField = new List<string>();
            //操作符
            string strOperator = string.Empty;
            foreach (var map in mapList)
            {
                object obj = map.PInfo.FastGetValue(filter);
                if (map.Operator != OperatorEnum.Null && map.Operator != OperatorEnum.NotNull
                    && (obj == null || string.IsNullOrEmpty(obj.ToString())))
                {
                    continue;
                }
                listField = new List<string>();
                if (map.IgnoreCase && obj.GetType() == typeof(string))
                {
                    //忽略大小写比较
                    obj = obj.ToString().ToLower();
                    foreach (var name in map.FieldName)
                    {
                        listField.Add(string.Format("lower({0})", name));
                    }
                }
                else
                {
                    listField = map.FieldName;
                }
                strOperator = map.Operator.GetDescription();
                sqlQuery.WhereBuilder.AppendFormat(" AND (");
                for (int i = 0, len = listField.Count; i < len; i++)
                {
                    switch (map.Operator)
                    {
                        //等于,不等于，小于，大于，小于等于，大于等于
                        case OperatorEnum.EQ:
                        case OperatorEnum.NE:
                        case OperatorEnum.GT:
                        case OperatorEnum.GE:
                        case OperatorEnum.LT:
                        case OperatorEnum.LE:
                            paramName = GetSqlParamName(map.FieldName[i], dictParamCount);
                            sqlQuery.WhereBuilder.AppendFormat("  {0} {1} @{2}", listField[i], strOperator, paramName);
                            dictParam[paramName] = obj;
                            break;
                        case OperatorEnum.Like:
                        case OperatorEnum.NotLike:
                        case OperatorEnum.LLike:
                        case OperatorEnum.RLike:
                            bool isContainSpecialChar = false;
                            if (map.Operator == OperatorEnum.Like || map.Operator == OperatorEnum.NotLike)
                            {
                                sqlQuery.WhereBuilder.AppendFormat(" {0} {1} '%{2}%'", listField[i], strOperator, ReplaceReg(obj.ToString(), out isContainSpecialChar));
                            }
                            else if (map.Operator == OperatorEnum.LLike)
                            {
                                sqlQuery.WhereBuilder.AppendFormat(" {0} {1} '%{2}'", listField[i], strOperator, ReplaceReg(obj.ToString(), out isContainSpecialChar));
                            }
                            else if (map.Operator == OperatorEnum.RLike)
                            {
                                sqlQuery.WhereBuilder.AppendFormat(" {0} {1} '{2}%'", listField[i], strOperator, ReplaceReg(obj.ToString(), out isContainSpecialChar));
                            }
                            if (isContainSpecialChar)
                            {
                                sqlQuery.WhereBuilder.AppendFormat(" escape('\\\\')");
                            }
                            break;
                        case OperatorEnum.Null:
                        case OperatorEnum.NotNull:
                            sqlQuery.WhereBuilder.AppendFormat(" {0} {1}", listField[0], strOperator);
                            break;
                        case OperatorEnum.In:
                        case OperatorEnum.NotIn:
                            string rValue = string.Empty;
                            List<string> list = new List<string>();
                            if (obj.GetType() == typeof(string))
                            {
                                list.AddRange(InReplaceReg(obj.ToString()).Split(',').ToList());
                            }
                            else
                            {
                                foreach (var tv in obj as IEnumerable)
                                {
                                    list.Add(InReplaceReg(tv.ToString()));
                                }
                            }
                            if (obj.GetType().IsNumberType())
                            {
                                rValue = string.Join(",", list);
                            }
                            else
                            {
                                rValue = "'" + string.Join("','", list) + "'";
                            }
                            sqlQuery.WhereBuilder.AppendFormat(" {0} {1} ({2})", listField[i], strOperator, rValue);
                            break;
                    }
                    if ((i + 1) != len)
                    {
                        sqlQuery.WhereBuilder.AppendFormat(" OR ");
                    }
                }
                sqlQuery.WhereBuilder.AppendFormat(" ) ");
            }
            if (sqlQuery.WhereBuilder.ToString().Length > 0)
            {
                sqlQuery.WhereBuilder.Remove(0, 4);
            }
            sqlQuery.SetParam(dictParam);
            #endregion


            #region "获取排序"

            if (!string.IsNullOrWhiteSpace(filter.SortField))
            {
                string SortOrder = string.IsNullOrWhiteSpace(filter.SortOrder) ? "ASC" : filter.SortOrder;

                //修改支持多字段排序 逗号分隔
                string[] arrSortField = filter.SortField.Split(',');
                //判断排序方向
                string[] arrSortOrder = SortOrder.Split(',');
                sqlQuery.OrderBuilder.Append(" ORDER BY ");
                for (int i = 0, length = arrSortField.Length, OrderLength = arrSortOrder.Length - 1; i < length; i++)
                {
                    sqlQuery.OrderBuilder.AppendFormat(" {0} {1}{2}", arrSortField[i], i <= OrderLength ? arrSortOrder[i] : "asc", i < length - 1 ? "," : " ");
                }
            }
            #endregion

            return sqlQuery;
        }

        /// <summary>
        /// In查询时替换特殊字符
        /// </summary>
        /// <param name="strValue">值</param>
        /// <returns>替换后的字符串</returns>
        private static string InReplaceReg(string strValue)
        {
            if (strValue.Contains("'"))
            {
                strValue = strValue.Replace("'", "''");
            }
            return strValue;
        }

        /// <summary>
        /// like查询时替换特殊字符
        /// </summary>
        /// <param name="strValue">值</param>
        /// <returns>替换后的字符串</returns>
        private static string ReplaceReg(string strValue, out bool isContainSpecialChar)
        {
            isContainSpecialChar = false;
            if (strValue.Contains("'"))
            {
                strValue = strValue.Replace("'", "''");
            }
            if (strValue.Contains("_"))
            {
                isContainSpecialChar = true;
                strValue = strValue.Replace("_", @"\_");
            }
            if (strValue.Contains("%"))
            {
                isContainSpecialChar = true;
                strValue = strValue.Replace("%", @"\%");
            }
            return strValue;
        }

        /// <summary>
        /// 处理参数中的特殊字符 eg  a.Name 做为oracle参数 :a.Name 会报错，处理后为 aName
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>处理后的参数名</returns>
        private static string GetSqlParamName(string name, Dictionary<string, int> dictParamCount)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }
            name = name.Replace(".", "");
            dictParamCount.TryGetValue(name, out int paramCount);
            dictParamCount[name] = ++paramCount;
            return name + (paramCount - 1).ToString();
        }

        /// <summary>
        /// 获取或新增属性映射
        /// </summary>
        /// <param name="type">filter类型</param>
        /// <returns></returns>
        private List<FilterMap> GetOrAddMap(Type type)
        {
            if (!_cache.TryGetValue(type, out List<FilterMap> mapList))
            {
                PropertyInfo[] properties = type.GetProperties(BindFlags);
                FilterMap map = null;
                mapList = new List<FilterMap>();
                foreach (PropertyInfo prop in properties)
                {
                    PropFilterAttribute attr = prop.GetCustomAttribute<PropFilterAttribute>();
                    if (attr == null)
                    {
                        continue;
                    }
                    map = new FilterMap
                    {
                        FieldName = (string.IsNullOrWhiteSpace(attr.FieldName) ? prop.Name : attr.FieldName).Split(',').ToList(),
                        Operator = attr.Operator,
                        IgnoreCase = attr.IgnoreCase,
                        PInfo = prop
                    };
                    mapList.Add(map);
                }
                _cache.TryAdd(type, mapList);
            }
            return mapList;
        }

        /// <summary>
        /// 过滤条件映射
        /// </summary>
        internal class FilterMap
        {
            /// <summary>
            /// 过滤条件字段名集合
            /// </summary>
            public List<string> FieldName { get; set; }

            /// <summary>
            /// 过滤类型 Like = > ....
            /// </summary>
            public OperatorEnum Operator { get; set; }

            /// <summary>
            /// 字段查询时是否忽略大小写比较
            /// </summary>
            public bool IgnoreCase { get; set; }

            /// <summary>
            /// 属性信息
            /// </summary>
            public PropertyInfo PInfo { get; set; }
        }
    }
}
