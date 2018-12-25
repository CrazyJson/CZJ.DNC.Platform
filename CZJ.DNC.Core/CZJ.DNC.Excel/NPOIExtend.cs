using NPOI.SS.UserModel;
using System;
using System.IO;

namespace CZJ.DNC.Excel
{
    /// <summary>
    /// NPOI拓展方法
    /// </summary>
    public static class NPOIExtend
    {
        /// <summary>
        /// 将workbook写入到Npoi内存流中，解决07Write会关闭流的问题
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns>NpoiMemoryStream</returns>
        public static NpoiMemoryStream WriteNpoiMemoryStream(this IWorkbook workbook)
        {
            NpoiMemoryStream ms = new NpoiMemoryStream();
            ms.AllowClose = false;
            workbook.Write(ms);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            ms.AllowClose = true;
            return ms;
        }

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
