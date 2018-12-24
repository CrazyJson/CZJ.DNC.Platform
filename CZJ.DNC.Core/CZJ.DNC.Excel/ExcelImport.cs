using CZJ.Dependency;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CZJ.DNC.Excel
{
    /// <summary>
    /// excel导入处理抽象类
    /// </summary>
    public abstract class ExcelImport : ITransientDependency
    {
        /// <summary>
        /// 获取相应业务类型，系统会根据该类型寻找对应模块的处理类
        /// </summary>
        /// <returns>业务类型</returns>
        public abstract string Type { get; }

        /// <summary>
        /// 当前业务类型对应Excel模版路径
        /// </summary>
        /// <example>FileHelper.GetAbsolutePath("/Template/Excel/设备批量注册.xls")</example>
        /// <returns>模版文件路径</returns>
        public abstract string TemplatePath { get; }

        /// <summary>
        /// Excel字段映射及校验缓存
        /// </summary>
        /// <returns>字段映射</returns>
        public abstract Dictionary<string, ImportVerify> DictFields { get; }

        /// <summary>
        /// 其它参数，会从Url里面的extra中获取
        /// 用于解决导入接口只能传入type的问题
        /// </summary>
        public string ExtraParam { get; set; }

        /// <summary>
        /// 起始行索引-标题行
        /// </summary>
        /// <returns>起始行索引</returns>
        public virtual int StartRowIndex
        {
            get
            {
                return 5;
            }
        }

        /// <summary>
        ///返回对应的导出模版数据
        /// </summary>
        /// <param name="s">响应流</param>
        /// <returns>模版MemoryStream</returns>
        public async virtual Task GetExportTemplate(Stream s)
        {
            using (FileStream fs = File.OpenRead(TemplatePath))
            {
                await fs.CopyToAsync(s);
            }
        }

        /// <summary>
        ///从上传文件流中读取数据 保存为datatable
        /// </summary>
        /// <param name="ins">输入流</param>
        /// <param name="datasheet">数据得sheet表格</param>
        /// <returns>数据</returns>
        public virtual DataTable GetDataFromExcel(Stream ins, out ISheet datasheet)
        {
            return NPOIHelper.GetDataFromExcel(ins, out datasheet, StartRowIndex);
        }

        /// <summary>
        ///返回对应的导出模版数据
        /// </summary>
        /// <param name="ins">导入文件流</param>
        /// <param name="fileName">文件名</param>
        /// <returns>ImportResult</returns>
        public virtual Task<ImportResult> ImportTemplate(Stream ins, string fileName)
        {
            if (DictFields == null)
            {
                throw new ArgumentNullException("Excel字段映射及校验缓存字典DictFields空异常");
            }
            return Task.Factory.StartNew(() =>
            {
                //1.读取数据
                DataTable dt = GetDataFromExcel(ins, out ISheet datasheet);

                //2.校验列是否正确
                //相同列数
                int equalCount = (from p in GetColumnList(dt)
                                  join q in DictFields.Keys
                                  on p equals q
                                  select p).Count();
                if (equalCount < DictFields.Keys.Count)
                {
                    throw new Exception(string.Format("模版列和规定的不一致,正确的列为（{0}）", string.Join(",", DictFields.Keys)));
                }


                //2.改变列名为英文字段名
                ImportVerify objVerify = null;
                List<string> columns = new List<string>();
                List<string> removeColumns = new List<string>();
                foreach (DataColumn dc in dt.Columns)
                {
                    if (DictFields.TryGetValue(dc.ColumnName, out objVerify))
                    {
                        if (objVerify != null)
                        {
                            dc.ColumnName = objVerify.FieldName;
                            columns.Add(objVerify.FieldName);
                            continue;
                        }
                    }
                    removeColumns.Add(dc.ColumnName);
                }
                //3.删除无效列
                foreach (string remove in removeColumns)
                {
                    dt.Columns.Remove(remove);
                }
                //4.获取校验所需额外参数
                Dictionary<string, object> extraInfo = GetExtraInfo(columns, dt);
                // 英文字段名到中文列名映射关系
                Dictionary<string, ImportVerify> DictColumnFields = DictFields.Values.ToDictionary(e => e.FieldName, e => e);

                //5.开始校验
                ImportResult result = Verify(dt, datasheet, extraInfo, fileName, DictColumnFields);
                if (result.IsSuccess)
                {
                    //校验完成后进行数据类型转换
                    ImportVerify iv = null;
                    Type columnType = null;
                    DataTable dtNew = dt.Clone();
                    foreach (DataColumn dc in dtNew.Columns)
                    {
                        if (DictColumnFields != null && DictColumnFields.TryGetValue(dc.ColumnName, out iv))
                        {
                            if (iv.DataType != null)
                            {
                                columnType = iv.DataType;
                            }
                            else
                            {
                                columnType = dc.DataType;
                            }
                        }
                        else
                        {
                            columnType = typeof(string);
                        }
                        dc.DataType = columnType;
                    }
                    //复制数据到克隆的datatable里  
                    try
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            dtNew.ImportRow(dr);
                        }
                    }
                    catch { }

                    //3.保存数据
                    result.ExtraInfo = SaveImportData(dtNew, extraInfo);
                    if (result.ExtraInfo != null && result.ExtraInfo is string)
                    {
                        result.Message = result.ExtraInfo as string;
                        result.ExtraInfo = dtNew;
                    }
                    else
                    {
                        result.Message = string.Format("成功导入{0}条数据", dtNew.Rows.Count);
                    }

                }
                return result;
            });
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
        public virtual Dictionary<string, object> GetExtraInfo(List<string> listColumn, DataTable dt)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="extraInfo"></param>
        /// <returns></returns>
        public virtual string RowVerify(DataRow dr, Dictionary<string, object> extraInfo)
        {
            return string.Empty;
        }

        /// <summary>
        /// 校验数据是否正常
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="extraInfo"></param>
        /// <param name="sheet">数据sheet</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="DictColumnFields">英文字段名到中文列名映射关系</param>
        /// <returns>ImportResult</returns>
        public virtual ImportResult Verify(DataTable dt, ISheet sheet, Dictionary<string, object> extraInfo, string fileName, Dictionary<string, ImportVerify> DictColumnFields)
        {
            IWorkbook wb = sheet.Workbook;
            ImportResult result = new ImportResult();

            string[] arrErrorMsg = null;
            string errorMsg = string.Empty;
            int columnCount = dt.Columns.Count;
            string columnName = string.Empty;
            ImportVerify objVerify = null;
            ImportVerifyParam objVerifyParam = new ImportVerifyParam { DTExcel = dt, CellValue = null, ColName = columnName, ColumnIndex = 0, RowIndex = 0 };
            DataRow row = null;
            object objExtra = null;
            bool isCorrect = true;

            //错误数据行样式
            var cellErrorStyle = NPOIHelper.GetErrorCellStyle(wb);
            ICell errorCell = null;
            IRow sheetRow = null;

            for (int i = 0, rLength = dt.Rows.Count; i < rLength; i++)
            {
                row = dt.Rows[i];
                arrErrorMsg = new string[columnCount + 1];
                for (int j = 0; j < columnCount; j++)
                {
                    columnName = dt.Columns[j].ColumnName;
                    if (DictColumnFields.TryGetValue(columnName, out objVerify))
                    {
                        if (objVerify.VerifyFunc != null)
                        {
                            objVerifyParam.CellValue = row[j];
                            objVerifyParam.ColumnIndex = j;
                            objVerifyParam.RowIndex = i;
                            objVerifyParam.ColName = objVerify.ColumnName;
                            if (extraInfo != null)
                            {
                                extraInfo.TryGetValue(columnName, out objExtra);
                            }
                            arrErrorMsg[j] = objVerify.VerifyFunc(objVerifyParam, objExtra);
                        }
                    }
                }
                //行数据整体校验
                arrErrorMsg[columnCount] = RowVerify(row, extraInfo);

                errorMsg = string.Join("，", arrErrorMsg.Where(e => !string.IsNullOrEmpty(e)));
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    isCorrect = false;
                    //设置错误信息
                    sheetRow = sheet.GetRow(StartRowIndex + 1 + i);
                    errorCell = sheetRow.GetCell(columnCount);
                    if (errorCell == null)
                    {
                        errorCell = sheetRow.CreateCell(columnCount);
                    }
                    errorCell.CellStyle = cellErrorStyle;
                    errorCell.SetCellValue(errorMsg);
                }
            }

            //输出错误信息模版
            if (!isCorrect)
            {
                sheetRow = sheet.GetRow(StartRowIndex);
                errorCell = sheetRow.GetCell(columnCount);
                if (errorCell == null)
                {
                    errorCell = sheetRow.CreateCell(columnCount);
                }
                ICellStyle copyStyle = sheetRow.GetCell(columnCount - 1).CellStyle;
                ICellStyle style = NPOIHelper.GetErrorHeadCellStyle(wb);
                IFont font = style.GetFont(wb);
                IFont copyfont = copyStyle.GetFont(wb);
                font.FontHeight = copyfont.FontHeight;
                font.FontName = copyfont.FontName;
                style.FillForegroundColor = copyStyle.FillForegroundColor;
                style.BorderBottom = copyStyle.BorderBottom;
                style.BorderLeft = copyStyle.BorderLeft;
                style.BorderRight = copyStyle.BorderRight;
                style.BorderTop = copyStyle.BorderTop;
                errorCell.CellStyle = style;
                errorCell.SetCellValue("错误信息");

                //自适应列宽度
                NPOIHelper.AutoSizeColumn(sheet, columnCount);
                result.Message = ExcelImportHelper.GetErrorExcel(wb, fileName);
            }
            else
            {
                result.IsSuccess = true;
            }
            return result;
        }

        /// <summary>
        /// 批量保存数据
        /// </summary>
        /// <param name="dt">数据，可以调用CPQuery.MultiInsert(strSQL,dt)方法进行批量保存</param>
        /// <param name="extraInfo">额外参数</param>
        /// <returns>返回的额外数据信息，用于导入查询后台返回excel数据使用</returns>
        public abstract object SaveImportData(DataTable dt, Dictionary<string, object> extraInfo);

        /// <summary>
        /// 获取DateTable列名List集合
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>列名List集合</returns>
        public List<string> GetColumnList(DataTable dt)
        {
            List<string> columns = new List<string>();
            foreach (DataColumn column in dt.Columns)
            {
                columns.Add(column.ColumnName);
            }
            return columns;
        }
    }
}
