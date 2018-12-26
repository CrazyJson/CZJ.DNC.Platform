using CZJ.Common.Core;
using CZJ.DNC.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZJ.DNC.Web
{
    /// <summary>
    /// 列表数据字典批量导入
    /// </summary>
    public class DictExcelImport : ExcelImport
    {
        #region "私有属性 方法"

        /// <summary>
        /// 字典显示类型 grid tree
        /// </summary>
        private string showType;

        /// <summary>
        /// 字典分类
        /// </summary>
        private int kind;

        /// <summary>
        /// 获取展示类型
        /// </summary>
        /// <returns></returns>
        private string GetShowType()
        {
            if (string.IsNullOrWhiteSpace(showType))
            {
                kind = 1000;
                showType = "grid";
            }
            return showType;
        }

        /// <summary>
        /// 字典编号唯一性校验
        /// </summary>
        /// <param name="e">校验参数</param>
        /// <param name="extra">额外参数</param>
        /// <returns>错误信息</returns>
        private static string UniqueVerify(ImportVerifyParam e, object extra)
        {
            string result = "";
            result = ExcelImportHelper.GetCellMsg(e.CellValue, e.ColName, 50, true);
            if (string.IsNullOrEmpty(result))
            {
                var NoDict = extra as Dictionary<string, int>;
                //校验是否唯一
                if (NoDict.TryGetValue(e.CellValue.ToString(), out int total))
                {
                    if (total >= 1)
                    {
                        result += string.Format("{0}:“{1}”已经存在", e.ColName, e.CellValue);
                    }
                    NoDict[e.CellValue.ToString()] = total++;
                }
                else
                {
                    NoDict[e.CellValue.ToString()] = 1;
                }
            }
            return result;
        }


        /// <summary>
        ///下拉选项校验
        /// </summary>
        /// <param name="e">校验参数</param>
        /// <param name="extra">额外参数</param>
        /// <returns>错误信息</returns>
        private static string SelectVerify(ImportVerifyParam e, object extra)
        {
            string result = "";
            result = ExcelImportHelper.GetCellMsg(e.CellValue, e.ColName, 0, true);
            if (string.IsNullOrEmpty(result))
            {
                //校验是否唯一
                if (extra is Dictionary<string, string> dict)
                {
                    if (!dict.ContainsKey(e.CellValue.ToString()))
                    {
                        result += e.ColName + "下拉选项" + e.CellValue + "不存在";
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 是否系统级缓存
        /// </summary>
        private static Dictionary<string, string> GetSystemDict()
        {
            return new Dictionary<string, string>{
                { "否","false"},
                { "是" ,"true" }
            };
        }

        /// <summary>
        /// 数据字典编号
        /// </summary>
        private Dictionary<string, List<SysDictionary>> GetOrginDict(DataTable dt)
        {
            var dtNew = dt.Copy();
            foreach (DataRow dr in dtNew.Rows)
            {
                dr["IsSystem"] = "false";
            }
            var list = dtNew.ToList<SysDictionary>();
            GetShowType();
            var dbList = new List<SysDictionary>();
            return new Dictionary<string, List<SysDictionary>>() {
                { "all", dbList }
            };
        }
        #endregion

        /// <summary>
        /// 获取分类
        /// </summary>
        public override string Type => "dict";

        /// <summary>
        /// 模板文件地址
        /// </summary>
        public override string TemplatePath
        {
            get
            {
                if (GetShowType() == "grid")
                {
                    return FileHelper.GetAbsolutePath("/Template/Excel/列表数据字典批量添加.xls");
                }
                else
                {
                    return FileHelper.GetAbsolutePath("/Template/Excel/树形数据字典批量添加.xls");
                }
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        public override Dictionary<string, ImportVerify> DictFields
        {
            get
            {
                var list = new List<ImportVerify> {
                    new ImportVerify{ ColumnName="编号",FieldName="DictionaryNo",VerifyFunc =(e,extra) => ExcelImportHelper.GetCellMsg(e.CellValue, e.ColName, 100, true) },
                    new ImportVerify { ColumnName = "值", FieldName = "DictionaryValue", VerifyFunc = (e, extra) => ExcelImportHelper.GetCellMsg(e.CellValue, e.ColName, 100, false, false) },
                    new ImportVerify { ColumnName = "描述", FieldName = "Remark", VerifyFunc = (e, extra) => ExcelImportHelper.GetCellMsg(e.CellValue, e.ColName, 500) },
                    new ImportVerify { ColumnName = "是否系统级", FieldName = "IsSystem", VerifyFunc = SelectVerify }
                };
                if (GetShowType() == "tree")
                {
                    list.Insert(3, new ImportVerify { ColumnName = "父级编号", FieldName = "ParentId", VerifyFunc = (e, extra) => ExcelImportHelper.GetCellMsg(e.CellValue, e.ColName, 100, false) });
                }
                return list.ToDictionary(e => e.ColumnName, e => e);
            }
        }

        /// <summary>
        /// 获取额外的校验所需信息
        /// </summary>
        /// <param name="listColumn">所有列名集合</param>
        /// <param name="dt">dt</param>
        /// <returns>额外信息</returns>
        /// <remarks>
        /// 例如导入excel中含有下拉框 导入时需要判断选项值是否还存在，可以通过该方法查询选项值
        /// </remarks>
        public override Dictionary<string, object> GetExtraInfo(List<string> listColumn, DataTable dt)
        {
            Dictionary<string, object> extraInfo = new Dictionary<string, object>();
            foreach (string name in listColumn)
            {
                switch (name)
                {
                    case "IsSystem":
                        extraInfo[name] = GetSystemDict();
                        break;
                    case "DictionaryNo":
                        extraInfo[name] = GetOrginDict(dt);
                        break;
                    default:
                        break;
                }
            }
            return extraInfo;
        }

        /// <summary>
        /// 行数据校验
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="extraInfo"></param>
        /// <returns></returns>
        public override string RowVerify(DataRow dr, Dictionary<string, object> extraInfo)
        {
            string result = "";
            string no = dr["DictionaryNo"].ToString();
            var dictExtra = extraInfo["DictionaryNo"] as Dictionary<string, List<SysDictionary>>;
            var list = dictExtra["all"];
            if (GetShowType() == "grid")
            {
                //列表型，校验编号是否重复
                if (list.Count(e => e.DictionaryNo == no) > 1)
                {
                    result = $"字典编号【{no}】已经存在";
                }
            }
            else
            {
                //树形校验编号 和父级Id
                string pNo = dr["ParentId"]?.ToString();
                //编号是否重复
                if (list.Count(e => e.DictionaryNo == no && e.ParentId == pNo) > 1)
                {
                    result = $"同层级下字典编号【{no}】已经存在";
                }
                //父级编号是否存在
                if (!string.IsNullOrEmpty(pNo) && list.Count(e => e.DictionaryNo == pNo) < 1)
                {
                    result += (string.IsNullOrEmpty(result) ? "" : "，") + $"父级编号【{pNo}】不存在";
                }
            }
            return result;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="extraInfo"></param>
        /// <returns></returns>
        public override object SaveImportData(DataTable dt, Dictionary<string, object> extraInfo)
        {
            string columnName = string.Empty;
            object objExtra = null;
            Dictionary<string, string> dict = null;
            object objCellValue = null;
            dt.Columns.Add("Kind", typeof(System.Int32));
            dt.Columns.Add("SortNo", typeof(System.Int32));

            int? MaxSortNo = null;
            var treeMaxSortNo = new Dictionary<string, int>();
            var listData = (extraInfo["DictionaryNo"] as Dictionary<string, List<SysDictionary>>)["all"];
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    columnName = dc.ColumnName;
                    if (extraInfo.TryGetValue(columnName, out objExtra))
                    {
                        dict = objExtra as Dictionary<string, string>;
                        if (dict != null)
                        {
                            objCellValue = dr[columnName];
                            if (!ExcelImportHelper.ObjectIsNullOrEmpty(objCellValue))
                            {
                                dr[columnName] = dict[objCellValue.ToString()];
                            }
                        }
                    }
                }
                dr["Kind"] = kind;
                string no = dr["DictionaryNo"].ToString();
                //排序
                if (showType == "grid")
                {
                    if (!MaxSortNo.HasValue)
                    {
                        MaxSortNo = listData.Max(e => e.SortNo);
                        MaxSortNo = MaxSortNo.HasValue ? MaxSortNo : -1;
                    }
                    MaxSortNo += 2;
                    dr["SortNo"] = MaxSortNo;
                }
                else
                {
                    string pNo = dr["ParentId"]?.ToString();
                    string key = string.IsNullOrEmpty(pNo) ? "empty" : pNo;
                    if (!treeMaxSortNo.TryGetValue(key, out int sortNo))
                    {
                        MaxSortNo = listData.Where(e => e.ParentId == pNo).Max(e => e.SortNo);
                        sortNo = MaxSortNo.HasValue ? MaxSortNo.Value : -1;
                    }
                    sortNo += 2;
                    treeMaxSortNo[key] = sortNo;
                    dr["SortNo"] = sortNo;
                }
            }
            int failCount = 0;
            StringBuilder sb = new StringBuilder();
            var list = dt.ToList<SysDictionary>();
            foreach (var item in list)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    failCount++; ;
                    sb.AppendFormat("{0};", ex.Message);
                }
            }
            return string.Format("成功导入{0}条字典数据，失败{1}条！{2}{3}", list.Count - failCount, failCount, failCount > 0 ? "失败原因：" : "", sb.ToString());
        }

        /// <summary>
        /// 导出模板
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public override Task GetExportTemplate(Stream s)
        {
            //写入下拉框值
            var sheet = NPOIHelper.GetFirstSheet(TemplatePath);
            int dataRowIndex = StartRowIndex + 1;
            NPOIHelper.SetHSSFValidation(sheet, GetSystemDict().Keys.ToArray(), dataRowIndex, GetShowType() == "tree" ? 4 : 3);
            sheet.Workbook.Write(s);
            return Task.FromResult(0);
        }
    }
}
