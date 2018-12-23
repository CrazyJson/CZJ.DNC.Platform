using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace CZJ.DNC.Excel
{
    /// <summary>
    /// Execl相关信息
    /// </summary>
    public class ExcelInfo
    {
        /// <summary>
        /// Execl列信息
        /// </summary>
        public List<ColumnInfo> ColumnInfoList { get; set; }

        /// <summary>
        /// Execl文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///获取数据源的方法信息
        /// </summary>
        public string Api { get; set; }

        /// <summary>
        /// 获取数据请求类型 post get
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public object Filter { get; set; }

        private List<Dictionary<string, object>> _dataEx { get; set; }

        /// <summary>
        /// 解决复杂列表序列化报错问题
        /// </summary>
        [JsonProperty(PropertyName = "Data")]
        public List<Dictionary<string, object>> DataEx
        {
            get
            {
                return _dataEx;
            }
            set
            {
                _dataEx = value;
                Data = ConvertDataEx2Data(value);
            }
        }

        /// <summary>
        /// 需要导出的数据
        /// </summary>
        [JsonIgnore]
        public DataTable Data { get; set; }

        private bool isExportSelectData = false;
        /// <summary>
        /// 是否为导出当前选中数据
        /// 如果wei true 则不进行远程查询
        /// </summary>
        public bool IsExportSelectData
        {
            get { return isExportSelectData; }
            set { isExportSelectData = value; }
        }

        /// <summary>
        /// 备注信息-不为空将放置在第一行
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 获取文件拓展名
        /// </summary>
        /// <returns></returns>
        public string GetFileExt()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                return Path.GetExtension(FileName);
            }
            return ".xlsx";
        }

        /// <summary>
        /// 是否按照前台传的列来序列化DataTable
        /// </summary>
        public bool ColAsSerialize { get; set; }

        /// <summary>
        /// 将DataEx转换成Data
        /// </summary>
        public DataTable ConvertDataEx2Data(List<Dictionary<string, object>> data)
        {
            DataTable dt = new DataTable();
            if (data == null)
            {
                return dt;
            }
            if (ColumnInfoList == null || ColAsSerialize == false)
            {
                return JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(data));
            }

            List<string> columns = new List<string>(ColumnInfoList.Count);
            Dictionary<string, object> firstRow = null;
            object value = null;
            if (data != null && data.Count > 0)
            {
                firstRow = data[0];
            }
            else
            {
                firstRow = new Dictionary<string, object>();
            }
            var dictColumnType = new Dictionary<string, bool>();
            foreach (var item in ColumnInfoList)
            {
                if (!string.IsNullOrEmpty(item.Field))
                {
                    dt.Columns.Add(item.Field);
                    columns.Add(item.Field);

                    if (firstRow.TryGetValue(item.Field, out value))
                    {
                        if (value is double || value is float ||
                            value is int || value is long)
                        {
                            dt.Columns[item.Field].DataType = value.GetType();
                            dictColumnType[item.Field] = true;
                        }
                        else
                        {
                            dictColumnType[item.Field] = false;
                        }
                    }
                    else
                    {
                        dictColumnType[item.Field] = false;
                    }
                }
            }

            if (data != null)
            {
                DataRow dr = null;
                int i = 0;
                foreach (var item in data)
                {
                    dr = dt.NewRow();
                    foreach (string column in columns)
                    {
                        if (!item.TryGetValue(column, out value) || value == null)
                        {
                            if (dictColumnType[column])
                            {
                                value = DBNull.Value;
                            }
                            else
                            {
                                value = null;
                            }
                        }
                        dr[column] = value;
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        /// <summary>
        /// 固定列数量
        /// </summary>
        public int FixColumns { get; set; }

        /// <summary>
        /// 合并表头信息
        /// </summary>
        public List<MoreHeader> GroupHeader { get; set; }
    }

    /// <summary>
    /// 多表头信息
    /// </summary>
    public class MoreHeader
    {
        /// <summary>
        ///  开始列列名
        /// </summary>
        public string StartColumnName { get; set; }

        /// <summary>
        ///  合并列数量
        /// </summary>
        public int NumberOfColumns { get; set; }

        /// <summary>
        ///  合并表头名称
        /// </summary>
        public string TitleText { get; set; }
    }

    /// <summary>
    /// Excel多标签页导出Sheet相关信息
    /// </summary>
    public class ExportSheetInfo
    {
        /// <summary>
        /// 标签页名称
        /// </summary>
        public string SheetName { get; set; }
        /// <summary>
        /// Sheet列信息
        /// </summary>
        public List<ColumnInfo> ColumnInfoList { get; set; }

        /// <summary>
        ///Sheet对应的数据
        /// </summary>
        public DataTable Data { get; set; }

        /// <summary>
        /// 备注信息-不为空将放置在第一行
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 固定列数量
        /// </summary>
        public int FixColumns { get; set; }

        /// <summary>
        /// 合并表头信息
        /// </summary>
        public List<MoreHeader> GroupHeader { get; set; }
    }

    /// <summary>
    /// 多标签页导出信息
    /// </summary>
    public class ExportMultiSheet
    {
        /// <summary>
        /// Execl文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 多标签页信息
        /// </summary>
        public List<ExportSheetInfo> ListSheet { get; set; }
    }
}
