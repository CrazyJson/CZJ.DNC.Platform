using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using CZJ.Common;

namespace CZJ.DNC.Excel
{
    /// <summary>
    /// NPOI操作EXECL帮助类
    /// </summary>
    public class NPOIHelper
    {
        /// <summary>
        /// EXECL最大列宽
        /// </summary>
        private const int maxColumnWidth = 100 * 256;

        /// <summary>
        /// 默认行高
        /// </summary>
        private const int rowHeight = 20;

        /// <summary>
        /// 生成EXECL文件，通过读取DataTable和列头映射信息
        /// </summary>
        /// <param name="excelInfo">Excel导出信息</param>
        /// <returns>内存流</returns>
        public static MemoryStream Export(ExcelInfo excelInfo)
        {
            if (excelInfo == null || excelInfo.Data == null || excelInfo.ColumnInfoList == null)
            {
                throw new ArgumentNullException();
            }
            IWorkbook workbook = GetWorkbook(excelInfo.FileName);
            var sheetInfo = new ExportSheetInfo
            {
                Data = excelInfo.Data,
                ColumnInfoList = excelInfo.ColumnInfoList,
                Remark = excelInfo.Remark,
                FixColumns = excelInfo.FixColumns,
                GroupHeader = excelInfo.GroupHeader
            };
            WriteSheetInfo(workbook, sheetInfo);
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            return ms;
        }

        /// <summary>
        /// 导出数据到Excel的多个标签页中
        /// </summary>
        /// <param name="multiSheet">多标签页信息</param>
        /// <returns>内存流</returns>
        public static MemoryStream ExportToMutilSheet(ExportMultiSheet multiSheet)
        {
            if (multiSheet == null || multiSheet.ListSheet == null || multiSheet.ListSheet.Count == 0)
            {
                throw new ArgumentNullException();
            }
            string fileExt = ".xlsx";
            if (string.IsNullOrEmpty(multiSheet.FileName))
            {
                multiSheet.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;
            }
            else if (string.IsNullOrEmpty(Path.GetExtension(multiSheet.FileName)))
            {
                multiSheet.FileName += fileExt;
            }
            IWorkbook workbook = GetWorkbook(multiSheet.FileName);
            foreach (ExportSheetInfo sheetInfo in multiSheet.ListSheet)
            {
                WriteSheetInfo(workbook, sheetInfo);
            }
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// 从EXECL中读取数据 转换成DataTable
        /// 每个sheet页对应一个DataTable
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static List<DataTable> GetDataTablesFrom(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("文件不存在");

            var result = new List<DataTable>();
            using (var stream = new MemoryStream(File.ReadAllBytes(filePath)))
            {
                var workbook = GetWorkbook(filePath, stream);
                DataTable dt = null;
                DataColumn column = null;
                ISheet sheet = null;
                IRow row = null;
                DataRow dr = null;
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    dt = new DataTable();
                    sheet = workbook.GetSheetAt(i);
                    row = sheet.GetRow(0);
                    int cellCount = row.LastCellNum;
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        column = new DataColumn(row.GetCell(j).StringCellValue);
                        dt.Columns.Add(column);
                    }

                    int rowCount = sheet.LastRowNum;
                    for (int a = (sheet.FirstRowNum + 1); a < rowCount; a++)
                    {
                        row = sheet.GetRow(a);
                        if (row == null) continue;
                        dr = dt.NewRow();
                        for (int b = row.FirstCellNum; b < cellCount; b++)
                        {
                            if (row.GetCell(b) == null) continue;
                            dr[b] = row.GetCell(b).ToString();
                        }
                        dt.Rows.Add(dr);
                    }
                    result.Add(dt);
                }
                return result;
            }
        }

        /// <summary>
        /// 获取第一个Sheet
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>Sheet</returns>
        public static ISheet GetFirstSheet(string filePath)
        {
            using (Stream stream = new MemoryStream(File.ReadAllBytes(filePath)))
            {
                var workbook = GetWorkbook(filePath, stream);
                if (workbook.NumberOfSheets > 0)
                {
                    return workbook.GetSheetAt(0);
                }
            }
            return null;
        }

        /// <summary>
        /// 某一列自适应内容宽度
        /// </summary>
        /// <param name="sheet">sheet标签</param>
        /// <param name="columnIndex">列索引</param>
        public static void AutoSizeColumn(ISheet sheet,int columnIndex)
        {
            sheet.AutoSizeColumn(columnIndex);
            int width = sheet.GetColumnWidth(columnIndex) + 2560;
            sheet.SetColumnWidth(columnIndex, width > maxColumnWidth ? maxColumnWidth : width);
        }

        #region "Excel模版数据读取相关"

        /// <summary>
        /// 从excel第一个sheet中读取数据
        /// </summary>
        /// <param name="ins">输入流</param>
        /// <param name="headRowIndex">标题行索引 默认为第6行</param>
        /// <param name="fSheet">第一个sheet</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataFromExcel(Stream ins, out ISheet fSheet, int headRowIndex = 5)
        {
            IWorkbook workbook = GetWorkbook(ins);
            fSheet = null;
            DataTable dt = new DataTable();
            if (workbook.NumberOfSheets > 0)
            {
                fSheet = workbook.GetSheetAt(0);
                if (fSheet.LastRowNum < headRowIndex)
                {
                    throw new ArgumentException("Excel模版错误,标题行索引大于总行数");
                }

                //读取标题行
                IRow row = null;
                ICell cell = null;

                row = fSheet.GetRow(headRowIndex);
                object objColumnName = null;
                for (int i = 0, length = row.LastCellNum; i < length; i++)
                {
                    cell = row.GetCell(i);
                    if (cell == null)
                    {
                        continue;
                    }
                    objColumnName = GetCellVale(cell);
                    if (objColumnName != null)
                    {
                        try
                        {
                            dt.Columns.Add(objColumnName.ToString().Trim());
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                    }
                    else
                    {
                        dt.Columns.Add("");
                    }
                }

                //读取数据行
                object[] entityValues = null;
                int columnCount = dt.Columns.Count;

                for (int i = headRowIndex + 1, length = fSheet.LastRowNum; i < length; i++)
                {
                    row = fSheet.GetRow(i);
                    if (row == null)
                    {
                        continue;
                    }
                    entityValues = new object[columnCount];
                    //用于判断是否为空行
                    bool isHasData = false;
                    int dataColumnLength = row.LastCellNum < columnCount ? row.LastCellNum : columnCount;
                    for (int j = 0; j < dataColumnLength; j++)
                    {
                        cell = row.GetCell(j);
                        if (cell == null)
                        {
                            continue;
                        }
                        entityValues[j] = GetCellVale(cell);
                        if (!isHasData && j < columnCount && entityValues[j] != null)
                        {
                            isHasData = true;
                        }
                    }
                    if (isHasData)
                    {
                        dt.Rows.Add(entityValues);
                    }
                }
            }
            return dt;
        }


        /// <summary>
        /// 从excel中所有sheet中读取数据
        /// </summary>
        /// <param name="ins">输入流</param>
        /// <param name="headRowIndex">标题行索引 默认为第6行</param>
        /// <param name="listSheet">所有有数据的Sheet</param>
        /// <returns>DataTable</returns>
        public static DataSet GetDataSetFromExcel(Stream ins, List<string> sheetNames, int headRowIndex, out List<ISheet> listSheet)
        {
            IWorkbook workbook = GetWorkbook(ins);
            DataSet ds = new DataSet();
            List<ISheet> sheets = new List<ISheet>();
            if (workbook.NumberOfSheets > 0)
            {
                //读取标题行
                IRow row = null;
                ICell cell = null;
                ISheet fSheet = null;
                DataTable dt = null;
                foreach (string sheetName in sheetNames)
                {
                    fSheet = workbook.GetSheet(sheetName);
                    if (fSheet == null || fSheet.LastRowNum < headRowIndex)
                    {
                        continue;
                    }
                    dt = new DataTable
                    {
                        TableName = sheetName
                    };
                    row = fSheet.GetRow(headRowIndex);
                    object objColumnName = null;
                    for (int i = 0, length = row.LastCellNum; i < length; i++)
                    {
                        cell = row.GetCell(i);
                        if (cell == null)
                        {
                            continue;
                        }
                        objColumnName = GetCellVale(cell);
                        if (objColumnName != null)
                        {
                            dt.Columns.Add(objColumnName.ToString().Trim());
                        }
                        else
                        {
                            dt.Columns.Add("");
                        }
                    }

                    //读取数据行
                    object[] entityValues = null;
                    int columnCount = dt.Columns.Count;

                    for (int i = headRowIndex + 1, length = fSheet.LastRowNum; i < length; i++)
                    {
                        row = fSheet.GetRow(i);
                        if (row == null)
                        {
                            continue;
                        }
                        entityValues = new object[columnCount];
                        //用于判断是否为空行
                        bool isHasData = false;
                        int dataColumnLength = row.LastCellNum < columnCount ? row.LastCellNum : columnCount;
                        for (int j = 0; j < dataColumnLength; j++)
                        {
                            cell = row.GetCell(j);
                            if (cell == null)
                            {
                                continue;
                            }
                            entityValues[j] = GetCellVale(cell);
                            if (!isHasData && j < columnCount && entityValues[j] != null)
                            {
                                isHasData = true;
                            }
                        }
                        if (isHasData)
                        {
                            dt.Rows.Add(entityValues);
                        }
                    }
                    ds.Tables.Add(dt);
                    sheets.Add(fSheet);
                }
            }
            listSheet = sheets;
            return ds;
        }

        /// <summary>
        /// 设置excel模版错误信息
        /// </summary>
        /// <param name="sheet">数据标签</param>
        /// <param name="rowindex">错误信息显示行</param>
        /// <param name="msg">错误信息</param>
        public static void SetTemplateErrorMsg(ISheet sheet, int rowindex, string msg)
        {
            IRow row = sheet.GetRow(rowindex);
            row = sheet.CreateRow(rowindex);
            if (row != null && !string.IsNullOrEmpty(msg))
            {
                sheet.AddMergedRegion(new CellRangeAddress(rowindex, rowindex, 0, row.LastCellNum));

                ICell cell = row.GetCell(0);
                if (cell == null)
                {
                    cell = row.CreateCell(0);
                }
                ICellStyle cellStyle = sheet.Workbook.CreateCellStyle();
                cellStyle.VerticalAlignment = VerticalAlignment.Center;
                cellStyle.Alignment = HorizontalAlignment.Left;
                IFont font = sheet.Workbook.CreateFont();
                font.FontHeightInPoints = 12;
                font.Color = HSSFColor.Red.Index;
                cellStyle.SetFont(font);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(msg);
            }
        }

        /// <summary>
        /// 获取数据行的错误信息提示样式
        /// </summary>
        /// <returns>错误数据行样式</returns>
        public static ICellStyle GetErrorCellStyle(IWorkbook wb)
        {
            ICellStyle cellStyle = wb.CreateCellStyle();
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.Alignment = HorizontalAlignment.Left;
            IFont font = wb.CreateFont();
            //font.FontHeightInPoints = 12;
            font.Color = HSSFColor.Red.Index;
            cellStyle.SetFont(font);
            return cellStyle;
        }

        /// <summary>
        /// 获取标题行的错误信息提示样式
        /// </summary>
        /// <returns>错误标题行样式</returns>
        public static ICellStyle GetErrorHeadCellStyle(IWorkbook wb)
        {
            ICellStyle cellStyle = wb.CreateCellStyle();
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.Alignment = HorizontalAlignment.Center;
            IFont font = wb.CreateFont();
            font.Boldweight = short.MaxValue;
            font.Color = HSSFColor.Red.Index;
            cellStyle.SetFont(font);
            cellStyle.FillPattern = FillPattern.SolidForeground;
            return cellStyle;
        }

        /// <summary>
        /// 获取单元格值
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns>单元格值</returns>
        private static object GetCellVale(ICell cell)
        {
            object obj = null;
            switch (cell.CellType)
            {
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        obj = cell.DateCellValue;
                    }
                    else
                    {
                        obj = cell.NumericCellValue;
                    }
                    break;
                case CellType.String:
                    if (string.IsNullOrEmpty(cell.StringCellValue))
                        obj = string.Empty;
                    else
                        obj = cell.StringCellValue.Trim();
                    break;
                case CellType.Boolean:
                    obj = cell.BooleanCellValue;
                    break;
                case CellType.Formula:
                    obj = cell.CellFormula;
                    break;

            }
            return obj;
        }
        #endregion

        #region "设置下拉选项"
        /// <summary>
        /// 设置某些列的值只能输入预制的数据,显示下拉框
        /// </summary>
        /// <param name="sheet">要设置的sheet</param>
        /// <param name="textlist">下拉框显示的内容</param>
        /// <param name="firstRow">开始行</param>
        /// <param name="firstCol">开始列</param>
        /// <param name="ValidationData">是否验证数据只能从下拉框中选择 默认为true   为false时既可以选择也可以输入</param>
        /// <returns>设置好的sheet</returns>
        public static ISheet SetHSSFValidation(ISheet sheet,
                string[] textlist, int firstRow, int firstCol, bool ValidationData = true)
        {
            return SetHSSFValidation(sheet, textlist, firstRow, sheet.LastRowNum, firstCol, firstCol, ValidationData);
        }

        /// <summary>
        /// 设置某些列的值只能输入预制的数据,显示下拉框
        /// </summary>
        /// <param name="sheet">要设置的sheet</param>
        /// <param name="textlist">下拉框显示的内容</param>
        /// <param name="firstRow">开始行</param>
        /// <param name="endRow">结束行</param>
        /// <param name="firstCol">开始列</param>
        /// <param name="endCol">结束列</param>
        /// <param name="ValidationData">是否验证数据只能从下拉框中选择 默认为true   为false时既可以选择也可以输入</param>
        /// <returns>设置好的sheet</returns>
        public static ISheet SetHSSFValidation(ISheet sheet,
                string[] textlist, int firstRow, int endRow, int firstCol,
                int endCol, bool ValidationData = true)
        {
            IWorkbook workbook = sheet.Workbook;
            if (endRow > sheet.LastRowNum)
            {
                endRow = sheet.LastRowNum;
            }
            ISheet hidden = null;
            string hiddenSheetName = "hidden" + sheet.SheetName.GetFirstPinyin();
            int hIndex = workbook.GetSheetIndex(hiddenSheetName);
            if (hIndex < 0)
            {
                hidden = workbook.CreateSheet(hiddenSheetName);
                workbook.SetSheetHidden(sheet.Workbook.NumberOfSheets - 1, SheetState.Hidden);
            }
            else
            {
                hidden = workbook.GetSheetAt(hIndex);
            }
            if (textlist == null || textlist.Length == 0)
            {
                textlist = new string[] { "" };
            }
            IRow row = null;
            ICell cell = null;
            for (int i = 0, length = textlist.Length; i < length; i++)
            {
                row = hidden.GetRow(i);
                if (row == null)
                {
                    row = hidden.CreateRow(i);
                }
                cell = row.GetCell(firstCol);
                if (cell == null)
                {
                    cell = row.CreateCell(firstCol);
                }
                cell.SetCellValue(textlist[i]);
            }

            // 加载下拉列表内容  
            string nameCellKey = hiddenSheetName + firstCol;
            IName namedCell = workbook.GetName(nameCellKey);
            if (namedCell == null)
            {
                namedCell = workbook.CreateName();
                namedCell.NameName = nameCellKey;
                namedCell.RefersToFormula = string.Format("{0}!${1}$1:${1}${2}", hiddenSheetName, NumberToChar(firstCol + 1), textlist.Length);
            }
            DVConstraint constraint = DVConstraint.CreateFormulaListConstraint(nameCellKey);

            // 设置数据有效性加载在哪个单元格上,四个参数分别是：起始行、终止行、起始列、终止列  
            CellRangeAddressList regions = new CellRangeAddressList(firstRow, endRow, firstCol, endCol);

            // 数据有效性对象  
            HSSFDataValidation validation = new HSSFDataValidation(regions, constraint)
            {
                //// 取消弹出错误框
                ShowErrorBox = ValidationData
            };
            sheet.AddValidationData(validation);
            return sheet;
        }
        #endregion

        #region "私有方法"

        /// <summary>
        /// 写入数据到标签页中，自动根据office类型会分多个标签页
        /// </summary>
        /// <param name="workbook">workbook对象</param>
        /// <param name="sheetInfo">sheetInfo标签页信息</param>
        private static void WriteSheetInfo(IWorkbook workbook, ExportSheetInfo sheetInfo)
        {
            if (sheetInfo == null || sheetInfo.Data == null || sheetInfo.ColumnInfoList == null)
            {
                throw new ArgumentNullException();
            }
            List<ColumnInfo> ColumnInfoList = sheetInfo.ColumnInfoList;

            //每个标签页最多行数
            ExcelVersion version = workbook is XSSFWorkbook ? ExcelVersion.XLSX : ExcelVersion.XLS;
            int sheetRow = GetSheetMaxRow(version);

            //表头样式
            bool headerGroup = sheetInfo.GroupHeader != null && sheetInfo.GroupHeader.Count > 0;
            ICellStyle headerStyle = GetHeaderStyle(workbook, headerGroup);

            //寻找列头和DataTable之间映射关系
            var culumnStyle = GetCellStyle(workbook, sheetInfo.Data, ColumnInfoList);

            var dictGroupMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            List<int> listColumnIndex = new List<int>(ColumnInfoList.Count);
            if (headerGroup)
            {
                int index = 0;
                foreach (var item in ColumnInfoList)
                {
                    if (sheetInfo.GroupHeader.FirstOrDefault(e => e.StartColumnName.Equals(item.Field, StringComparison.OrdinalIgnoreCase)) != null)
                    {
                        dictGroupMap[item.Field] = index;
                    }
                    index++;
                }
                for (int i = 0; i < ColumnInfoList.Count; i++)
                {
                    listColumnIndex.Add(i);
                }
                foreach (var item in sheetInfo.GroupHeader)
                {
                    int startCol = dictGroupMap[item.StartColumnName];
                    int lastCol = startCol + item.NumberOfColumns - 1;
                    for (int j = startCol; j <= lastCol; j++)
                    {
                        listColumnIndex.Remove(j);
                    }
                }
            }

            int total = sheetInfo.Data.Rows.Count;
            int sheetNum = (int)Math.Ceiling(total * 1.0 / sheetRow);
            sheetNum = sheetNum == 0 ? 1 : sheetNum;
            //超链接字体颜色
            IFont blueFont = workbook.CreateFont();
            blueFont.Color = HSSFColor.Blue.Index;
            ICell cell = null;
            ICellStyle cellStyle = null;
            ISheet sheet = null;
            object cellValue = null;
            //标题头索引
            int headIndex = string.IsNullOrEmpty(sheetInfo.Remark) ? 0 : 1;
            for (int sheetIndex = 0; sheetIndex < sheetNum; sheetIndex++)
            {
                if (string.IsNullOrEmpty(sheetInfo.SheetName))
                {
                    sheet = workbook.CreateSheet();
                }
                else
                {
                    sheet = workbook.CreateSheet($"{sheetInfo.SheetName}{sheetIndex + 1}");
                }
                if (headIndex > 0)
                {
                    //输出备注行
                    IRow RemarkRow = sheet.CreateRow(0);
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, ColumnInfoList.Count - 1));
                    ICell rcell = RemarkRow.CreateCell(0);
                    ICellStyle remarkStyle = workbook.CreateCellStyle();
                    remarkStyle.WrapText = true;
                    remarkStyle.VerticalAlignment = VerticalAlignment.Top;
                    remarkStyle.Alignment = HorizontalAlignment.Left;
                    IFont rfont = workbook.CreateFont();
                    rfont.FontHeightInPoints = 12;
                    remarkStyle.SetFont(rfont);
                    rcell.CellStyle = remarkStyle;
                    RemarkRow.HeightInPoints = rowHeight * 5;
                    rcell.SetCellValue(sheetInfo.Remark);
                }
                //表头
                int iRow = 0;
                if (headerGroup)
                {
                    InitGroupHeader(sheet, headIndex, sheetInfo, headerStyle, dictGroupMap, listColumnIndex);
                    iRow = 2 + headIndex;
                }
                else
                {
                    InitSimpleHeader(sheet, headIndex, sheetInfo, headerStyle);
                    iRow = 1 + headIndex;
                }

                //开始循环所有行
                int startRow = sheetIndex * (sheetRow - 1);
                int endRow = (sheetIndex + 1) * (sheetRow - 1);
                endRow = endRow <= sheetInfo.Data.Rows.Count ? endRow : total;
                int i = 0;
                for (int rowIndex = startRow; rowIndex < endRow; rowIndex++)
                {
                    IRow row = sheet.CreateRow(iRow);
                    row.HeightInPoints = rowHeight;
                    i = 0;
                    foreach (var item in ColumnInfoList)
                    {
                        cell = row.CreateCell(i);
                        if (culumnStyle.TryGetValue(item.Field, out cellStyle))
                        {
                            cellValue = sheetInfo.Data.Rows[rowIndex][item.Field];
                            Type columnType = sheetInfo.Data.Columns[item.Field].DataType;
                            cell.SetCellValue(cellValue, columnType);
                            cell.CellStyle = cellStyle;
                            if (item.IsLink)
                            {
                                cellValue = sheetInfo.Data.Rows[rowIndex][item.Field + "Link"];
                                if (cellValue != DBNull.Value && cellValue != null)
                                {
                                    //建一个HSSFHyperlink实体，指明链接类型为URL（这里是枚举，可以根据需求自行更改）  
                                    HSSFHyperlink link = new HSSFHyperlink(HyperlinkType.Url)
                                    {
                                        //给HSSFHyperlink的地址赋值 ，默认为该列加上Link
                                        Address = cellValue.ToString()
                                    };
                                    cell.Hyperlink = link;
                                    cell.CellStyle.SetFont(blueFont);
                                }
                            }
                        }
                        i++;
                    }
                    iRow++;
                }

                //自适应列宽度
                if (total < 5000)
                {
                    for (int j = 0; j < ColumnInfoList.Count; j++)
                    {
                        AutoSizeColumn(sheet, j);
                    }
                }
            }
        }

        /// <summary>
        /// 把1,2,3,...,35,36转换成A,B,C,...,Y,Z
        /// </summary>
        /// <param name="number">要转换成字母的数字（数字范围在闭区间[1,36]）</param>
        /// <returns></returns>

        private static string NumberToChar(int number)
        {
            if (1 <= number && 36 >= number)
            {
                int num = number + 64;
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] btNumber = new byte[] { (byte)num };
                return asciiEncoding.GetString(btNumber);
            }
            return "A";
        }

        /// <summary>
        /// 根据文件后缀获取IWorkbook
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns>IWorkbook</returns>
        private static IWorkbook GetWorkbook(ExcelVersion version)
        {
            if (version == ExcelVersion.XLSX)
            {
                return new XSSFWorkbook();
            }
            else
            {
                return new HSSFWorkbook();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static IWorkbook GetWorkbook(Stream stream)
        {
            try
            {
                return new XSSFWorkbook(stream);
            }
            catch
            {
                return new HSSFWorkbook(stream);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static IWorkbook GetWorkbook(string fileName, Stream stream)
        {
            ExcelVersion version = GetExcelVersion(fileName);
            if (version == ExcelVersion.XLSX)
            {
                return new XSSFWorkbook(stream);
            }
            else
            {
                return new HSSFWorkbook(stream);
            }
        }

        /// <summary>
        /// 根据文件后缀获取IWorkbook
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>IWorkbook</returns>
        private static IWorkbook GetWorkbook(string fileName)
        {
            ExcelVersion version = GetExcelVersion(fileName);
            return GetWorkbook(version);
        }

        /// <summary>
        /// 根据文件后缀获取excel版本
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>excel版本</returns>
        private static ExcelVersion GetExcelVersion(string fileName)
        {
            ExcelVersion version = ExcelVersion.XLSX;
            if (!string.IsNullOrEmpty(fileName) &&
                Path.GetExtension(fileName).Equals(".xls", StringComparison.CurrentCultureIgnoreCase))
            {
                version = ExcelVersion.XLS;
            }
            return version;
        }

        /// <summary>
        /// 获取sheet最大行数
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns>最大行数</returns>
        private static int GetSheetMaxRow(ExcelVersion version)
        {
            int row = 1048576;
            switch (version)
            {
                case ExcelVersion.XLS:
                    row = 65530;
                    break;
                case ExcelVersion.XLSX:
                    row = 1048576;
                    break;
                default:
                    break;
            }
            return row;
        }

        /// <summary>
        /// 获取导出表头样式
        /// </summary>
        /// <param name="workbook">workbook</param>
        /// <param name="headerGroup">表头是否需要分组合并</param>
        /// <returns>表头样式</returns>
        private static ICellStyle GetHeaderStyle(IWorkbook workbook, bool headerGroup)
        {
            //首行样式
            ICellStyle headerStyle = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont();
            font.Boldweight = short.MaxValue;
            headerStyle.SetFont(font);
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            headerStyle.Alignment = HorizontalAlignment.Center;
            if (!headerGroup)
            {
                headerStyle.FillPattern = FillPattern.SolidForeground;
                headerStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;
            }
            else
            {
                headerStyle.FillPattern = FillPattern.SolidForeground;
                headerStyle.FillForegroundColor = HSSFColor.LightCornflowerBlue.Index;
                headerStyle.BorderTop = BorderStyle.Thin;
                headerStyle.BorderLeft = BorderStyle.Thin;
                headerStyle.BorderRight = BorderStyle.Thin;
                headerStyle.BorderBottom = BorderStyle.Thin;
                headerStyle.TopBorderColor = HSSFColor.Black.Index;
                headerStyle.LeftBorderColor = HSSFColor.Black.Index;
                headerStyle.RightBorderColor = HSSFColor.Black.Index;
                headerStyle.BottomBorderColor = HSSFColor.Black.Index;
            }
            return headerStyle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="headIndex"></param>
        /// <param name="sheetInfo"></param>
        /// <param name="headerStyle"></param>
        private static void InitSimpleHeader(ISheet sheet, int headIndex, ExportSheetInfo sheetInfo, ICellStyle headerStyle)
        {
            ICell cell;
            //输出表头信息 并设置表头样式
            int i = 0;
            //输出表头
            IRow headerRow = sheet.CreateRow(headIndex);
            //设置行高
            headerRow.HeightInPoints = rowHeight;
            foreach (var data in sheetInfo.ColumnInfoList)
            {
                cell = headerRow.CreateCell(i);
                cell.SetCellValue(data.Header.Trim());
                cell.CellStyle = headerStyle;
                i++;
            }
            sheet.CreateFreezePane(sheetInfo.FixColumns, headIndex + 1, sheetInfo.FixColumns, headIndex + 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="headIndex"></param>
        /// <param name="sheetInfo"></param>
        /// <param name="headerStyle"></param>
        /// <param name="dictGroupMap"></param>
        /// <param name="listColumnIndex"></param>
        private static void InitGroupHeader(ISheet sheet, int headIndex, ExportSheetInfo sheetInfo, ICellStyle headerStyle
            , Dictionary<string, int> dictGroupMap, List<int> listColumnIndex)
        {
            //输出表头
            IRow firstHeaderRow = sheet.CreateRow(headIndex);
            IRow secondHeaderRow = sheet.CreateRow(headIndex + 1);

            //设置行高
            firstHeaderRow.HeightInPoints = rowHeight;
            secondHeaderRow.HeightInPoints = rowHeight;

            ICell cell;
            //输出表头信息 并设置表头样式
            int i = 0, groupIndex = 0;
            MoreHeader groupheader = null;
            foreach (var data in sheetInfo.ColumnInfoList)
            {
                cell = secondHeaderRow.CreateCell(i);
                cell.SetCellValue(data.Header.Trim());
                cell.CellStyle = headerStyle;

                cell = firstHeaderRow.CreateCell(i);
                cell.SetCellValue(data.Header.Trim());
                cell.CellStyle = headerStyle;

                groupheader = sheetInfo.GroupHeader[groupIndex];
                if (groupheader.StartColumnName.Equals(data.Field, StringComparison.CurrentCultureIgnoreCase))
                {
                    cell.SetCellValue(groupheader.TitleText.Trim());
                    groupIndex++;
                    if (groupIndex >= sheetInfo.GroupHeader.Count)
                    {
                        groupIndex--;
                    }
                }
                i++;
            }
            foreach (var item in listColumnIndex)
            {
                sheet.AddMergedRegion(new CellRangeAddress(headIndex, headIndex + 1, item, item));
            }
            int startCol, lastCol;
            foreach (var item in sheetInfo.GroupHeader)
            {
                startCol = dictGroupMap[item.StartColumnName];
                lastCol = startCol + item.NumberOfColumns - 1;
                sheet.AddMergedRegion(new CellRangeAddress(headIndex, headIndex, startCol, lastCol));
            }
            //冻结列 行
            sheet.CreateFreezePane(sheetInfo.FixColumns, headIndex + 2, sheetInfo.FixColumns, headIndex + 2);
        }

        /// <summary>
        /// 单元格样式
        /// </summary>
        /// <param name="workbook">workbook</param>
        /// <param name="dt">数据</param>
        /// <param name="columnInfoList">列信息</param>
        /// <returns></returns>
        private static Dictionary<string, ICellStyle> GetCellStyle(IWorkbook workbook, DataTable dt, List<ColumnInfo> columnInfoList)
        {
            //文本样式
            ICellStyle centerStyle = workbook.CreateCellStyle();
            centerStyle.VerticalAlignment = VerticalAlignment.Center;
            centerStyle.Alignment = HorizontalAlignment.Center;

            ICellStyle leftStyle = workbook.CreateCellStyle();
            leftStyle.VerticalAlignment = VerticalAlignment.Center;
            leftStyle.Alignment = HorizontalAlignment.Left;

            ICellStyle rightStyle = workbook.CreateCellStyle();
            rightStyle.VerticalAlignment = VerticalAlignment.Center;
            rightStyle.Alignment = HorizontalAlignment.Right;
            //寻找列头和DataTable之间映射关系
            var culumnStyle = new Dictionary<string, ICellStyle>(StringComparer.CurrentCultureIgnoreCase);
            foreach (DataColumn col in dt.Columns)
            {
                ColumnInfo info = columnInfoList.FirstOrDefault(e => e.Field.Equals(col.ColumnName, StringComparison.OrdinalIgnoreCase));
                if (info != null)
                {
                    switch (info.Align.ToLower())
                    {
                        case "left":
                            culumnStyle[col.ColumnName] = leftStyle;
                            break;
                        case "center":
                            culumnStyle[col.ColumnName] = centerStyle;
                            break;
                        case "right":
                            culumnStyle[col.ColumnName] = rightStyle;
                            break;
                    }
                }
            }
            return culumnStyle;
        }
        #endregion
    }

    /// <summary>
    /// NPOI拓展方法
    /// </summary>
    public static class NPOIExtend
    {
        /// <summary>
        /// 冻结表格
        /// </summary>
        /// <param name="sheet">sheet</param>
        /// <param name="colCount">冻结的列数</param>
        /// <param name="rowCount">冻结的行数</param>
        /// <param name="startCol">右边区域可见的首列序号，从1开始计算</param>
        /// <param name="startRow">下边区域可见的首行序号，也是从1开始计算</param>
        /// <example>
        /// sheet1.CreateFreezePane(0, 1, 0, 1); 冻结首行
        /// sheet1.CreateFreezePane(1, 0, 1, 0);冻结首列
        /// </example>
        public static void FreezePane(this ISheet sheet, int colCount, int rowCount, int startCol, int startRow)
        {
            sheet.CreateFreezePane(colCount, rowCount, startCol, startRow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cellValue"></param>
        /// <param name="columnType"></param>
        public static void SetCellValue(this ICell cell, object cellValue, Type columnType)
        {
            if (columnType == typeof(double) ||
                columnType == typeof(float) ||
                columnType == typeof(int) ||
                columnType == typeof(long))
            {
                cell.SetCellValue(cellValue != DBNull.Value ? Convert.ToDouble(cellValue) : 0);
            }
            else
            {
                cell.SetCellValue(cellValue != DBNull.Value ? cellValue.ToString() : string.Empty);
            }
        }
    }
}


