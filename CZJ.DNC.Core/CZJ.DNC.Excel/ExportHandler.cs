using CZJ.Common.Serializer;
using CZJ.Dependency;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CZJ.DNC.Excel
{
    /// <summary>
    /// 导出数据处理器接口
    /// </summary>
    public class ExportHandler : IExportHandler, ISingletonDependency
    {
        private readonly IEnumerable<IApiResultHandler> allHandler;
        private readonly IObjectSerializer serializer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allHandler">所有处理器</param>
        /// <param name="serializer">序列化对象</param>
        public ExportHandler(IEnumerable<IApiResultHandler> allHandler, IObjectSerializer serializer)
        {
            this.allHandler = allHandler;
            this.serializer = serializer;
        }

        /// <summary>
        /// 获取导出生成的内存流
        /// </summary>
        /// <param name="info">EXECL相关信息</param>
        /// <param name="headers">请求头</param>    
        /// <returns>内存流</returns>
        public async Task<MemoryStream> GetExportStream(ExcelInfo info, IHeaderDictionary headers)
        {
            //1.获取列表对应数据
            info.Data = await GetGirdData(info, headers);
            //2.创建文档
            MemoryStream ms = NPOIHelper.Export(info);
            return ms;
        }

        /// <summary>
        /// 从WebAPI中获取列表数据
        /// </summary>
        /// <param name="info">EXECL相关信息</param>
        /// <param name="headers">请求头</param>    
        /// <returns></returns>
        private async Task<DataTable> GetGirdData(ExcelInfo info, IHeaderDictionary headers)
        {
            if (info.IsExportSelectData)
            {
                if (info.Data == null)
                {
                    info.Data = new DataTable();
                }
                return info.Data;
            }
            try
            {
                if (string.IsNullOrEmpty(info.ApiResultHandler))
                {
                    throw new Exception("从接口获取数据，但是没有对应得数据处理器参数ApiResultHandler空异常");
                }
                HttpMethod method = HttpMethod.Get;
                if (info.Type.Equals(HttpMethod.Post.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    method = HttpMethod.Post;
                }
                bool proxyRequest = true;
                if (Uri.TryCreate(info.Api, UriKind.RelativeOrAbsolute, out Uri uri) && uri.IsAbsoluteUri)
                {
                    proxyRequest = false;
                }
                var request = new CitmsHttpRequest
                {
                    Method = method,
                    AddressUrl = info.Api,
                    Body = info.Filter,
                    ProxyRequest = proxyRequest,
                    RequestSet = (requestMessage) =>
                    {
                        foreach (var key in headers)
                        {
                            requestMessage.Headers.TryAddWithoutValidation(key.Key, key.Value.ToArray());
                        }
                    }
                };
                var r = await request.SendAsync();
                if (r.IsSuccessStatusCode)
                {
                    var handler = allHandler.FirstOrDefault(e =>
                        e.Name.Equals(info.ApiResultHandler, StringComparison.CurrentCultureIgnoreCase));
                    if (handler == null)
                    {
                        throw new Exception($"数据处理器{info.ApiResultHandler}没有找到对应IExcelApiResultHandler实现类");
                    }
                    string text = await r.Content.ReadAsStringAsync();
                    return handler.Parse(text, info);
                }
                else
                {
                    throw new Exception($"{request.Method.ToString()}请求{request.AddressUrl},参数{serializer.Serialize(request.Body)}，服务器响应码{Convert.ToInt32(r.StatusCode)}({r.ReasonPhrase}){r.Content.ReadAsStringAsync().Result}");
                }
            }
            catch (Exception ex)
            {
                return GetErrorDataTable(ex.ToString(), info.ColumnInfoList);
            }
        }

        /// <summary>
        /// 错误内容对应的DataTable
        /// </summary>
        /// <param name="msg">错误消息</param>
        /// <param name="columnInfoList">列</param>
        /// <returns></returns>
        private DataTable GetErrorDataTable(string msg, List<ColumnInfo> columnInfoList)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(columnInfoList[0].Field);
            DataRow dr = dt.NewRow();
            dr[0] = msg;
            dt.Rows.Add(dr);
            return dt;
        }
    }
}
