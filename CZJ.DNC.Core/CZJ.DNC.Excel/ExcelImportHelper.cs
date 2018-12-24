using CZJ.Common.Core;
using NPOI.SS.UserModel;
using System;
using System.IO;

namespace CZJ.DNC.Excel
{
    /// <summary>
    /// Excel导入公共类
    /// </summary>
    public class ExcelImportHelper
    {
        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="obj">待判断对象</param>
        /// <returns>bool</returns>
        public static bool ObjectIsNullOrEmpty(object obj)
        {
            return obj == null || string.IsNullOrEmpty(obj.ToString());
        }

        /// <summary>
        /// 获取错误信息Excel
        /// </summary>
        /// <param name="wb">excel对象</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public static string GetErrorExcel(IWorkbook wb, string fileName)
        {
            string ext = Path.GetExtension(fileName);
            string name = Path.GetFileNameWithoutExtension(fileName);
            string dirPath = FileHelper.GetDirectoryPath("/TempFiles/ErrorExcel", true);
            string relativePath = string.Format("/TempFiles/ErrorExcel/{0}{1}{2}", name, DateTime.Now.ToString("MMddHHmmss"), ext);
            string path = FileHelper.GetAbsolutePath(relativePath);
            using (FileStream fs = File.OpenWrite(path))
            {
                wb.Write(fs);
            }
            return relativePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellValue"></param>
        /// <param name="colName"></param>
        /// <param name="MaxLength"></param>
        /// <param name="Required"></param>
        /// <param name="isNChar"></param>
        /// <returns></returns>
        public static string GetCellMsg(object cellValue, string colName, int MaxLength = 0, bool Required = false, bool isNChar = false)
        {
            bool empty = ObjectIsNullOrEmpty(cellValue);
            if (Required && empty)
            {
                return colName + "必填";
            }
            if (MaxLength > 0 && !empty)
            {
                int length = isNChar ? cellValue.ToString().Trim().Length : GetLength(cellValue.ToString().Trim(), 3);
                if (length > MaxLength)
                {
                    return colName + "最大长度为" + MaxLength;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellValue"></param>
        /// <param name="colName"></param>
        /// <param name="MaxLength"></param>
        /// <param name="Required"></param>
        /// <param name="isNChar"></param>
        /// <returns></returns>
        public static string GetNumCellMsg(object cellValue, string colName, int MaxLength = 0, bool Required = false, bool isNChar = false)
        {
            bool empty = ObjectIsNullOrEmpty(cellValue);
            if (Required && empty)
            {
                return colName + "必填";
            }
            if (MaxLength > 0 && !empty)
            {
                int length = isNChar ? cellValue.ToString().Trim().Length : GetLength(cellValue.ToString().Trim(), 3);
                if (length > MaxLength)
                {
                    return colName + "最大长度为" + MaxLength;
                }
                if (!int.TryParse(cellValue.ToString().Trim(), out int value))
                {
                    return colName + "不是正确的数字";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取字符串长度。与string.Length不同的是，该方法将中文作 x 个字符计算。
        /// </summary>
        /// <param name="str">目标字符串</param>
        /// <param name="chinaLength">中文作x个字符 默认为2个</param>
        /// <returns>实际长度</returns>
        public static int GetLength(string str, int chinaLength = 2)
        {
            if (str == null || str.Length == 0) { return 0; }

            int len = str.Length;
            int realLen = len;

            #region 计算长度
            int clen = 0;//当前长度
            while (clen < len)
            {
                //每遇到一个中文，则将实际长度加一。
                if ((int)str[clen] > 128) { realLen = realLen + chinaLength - 1; }
                clen++;
            }
            #endregion

            return realLen;
        }

    }
}
