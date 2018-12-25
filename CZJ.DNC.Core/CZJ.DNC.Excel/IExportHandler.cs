using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace CZJ.DNC.Excel
{
    /// <summary>
    /// 导出数据处理器接口
    /// </summary>
    public interface IExportHandler
    {
        /// <summary>
        /// 获取导出生成的内存流
        /// </summary>
        /// <param name="info">EXECL相关信息</param>
        /// <param name="headers">请求头</param>    
        /// <returns>内存流</returns>
        Task<MemoryStream> GetExportStream(ExcelInfo info, IHeaderDictionary headers);
    }
}
