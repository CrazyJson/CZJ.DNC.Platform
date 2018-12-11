using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;

namespace CZJ.DNC.Web.Sample
{
    /// <summary>
    /// 
    /// </summary>
    [HttpHost("http://localhost:58706")]
    public interface IPaymentWebApi : IHttpApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        [WebApiClient.Attributes.HttpGet("/api/values")]
        ITask<string> GetPaymentHistoryByAccountAsync(string x);
    }
}
