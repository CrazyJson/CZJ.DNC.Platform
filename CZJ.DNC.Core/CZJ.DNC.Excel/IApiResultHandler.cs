using System.Data;

namespace CZJ.DNC.Excel
{
    /// <summary>
    /// 调用接口获取数据
    /// </summary>
    public interface IApiResultHandler
    {
        /// <summary>
        /// 数据处理器名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 从请求响应体中获取数据转换成DataTable
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="info">导出信息</param>
        /// <returns></returns>
        DataTable Parse(string text, ExcelInfo info);
    }
}
