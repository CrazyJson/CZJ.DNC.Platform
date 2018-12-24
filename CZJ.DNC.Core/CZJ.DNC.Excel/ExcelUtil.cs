using Microsoft.AspNetCore.Http;
using System;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CZJ.DNC.Excel
{
    /// <summary>
    /// Execl操作帮助类
    /// </summary>
    public static class ExcelUtil
    {
        /// <summary>
        /// 拓展方法,生成EXECL
        /// </summary>
        /// <param name="info">EXECL相关信息</param>
        /// <param name="headers">请求头</param>    
        /// <returns>Execl路径</returns>
        public async static Task<MemoryStream> ExportExeclStream(this ExcelInfo info, IHeaderDictionary headers)
        {
            //1.获取列表对应数据
            info.Data = await GetGirdData(info, headers);
            //2.创建文档
            MemoryStream ms = NPOIHelper.Export1(info);
            return ms;
        }


        /// <summary>
        /// 从WebAPI中获取列表数据
        /// </summary>
        /// <param name="info">EXECL相关信息</param>
        /// <param name="headers">请求头</param>    
        /// <returns></returns>
        private async static Task<DataTable> GetGirdData(ExcelInfo info, IHeaderDictionary headers)
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
                HttpMethod method = HttpMethod.Get;
                if (info.Type.Equals(HttpMethod.Post.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    method = HttpMethod.Post;
                }
                var request = new CitmsHttpRequest
                {
                    Method = method,
                    AddressUrl = info.Api,
                    Body = info.Filter,
                    RequestSet = (requestMessage) =>
                    {
                        foreach (var key in headers)
                        {
                            requestMessage.Headers.TryAddWithoutValidation(key.Key, key.Value.ToArray());
                        }
                    }
                };
                var responseJson = await request.SendAsync<ExcelApiResult>();
                if (responseJson.Code == 0)
                {
                    return info.ConvertDataEx2Data(responseJson.Data);
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add(info.ColumnInfoList[0].Field);
                    DataRow dr = dt.NewRow();
                    dr[0] = responseJson.Message;
                    dt.Rows.Add(dr);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
