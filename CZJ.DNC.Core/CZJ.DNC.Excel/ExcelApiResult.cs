using System;
using System.Collections.Generic;

namespace CZJ.DNC.Excel
{
    /// <summary>
    /// 定义调用各业务接口返回结果的格式
    /// </summary>
    public class ExcelApiResult
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        /// <remarks></remarks>
        public int Code { get; set; }


        /// <summary>
        /// 执行返回消息
        /// </summary>
        /// <remarks></remarks>
        public string Message { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 返回的主要内容.
        /// </summary>
        public List<Dictionary<string, object>> Data { get; set; }
    }
}
