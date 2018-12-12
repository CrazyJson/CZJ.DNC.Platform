using CZJ.DNC.Feign;
using WebApiClient;
using WebApiClient.Attributes;

namespace CZJ.DNC.Web.Sample
{
    /// <summary>
    /// 
    /// </summary>
    [HttpHost("http://localhost:58706")]
    [SignFilter()]
    public interface IPaymentWebApi : IHttpApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        [HttpGet("/api/values")]
        [Timeout(1 * 1000)]
        ITask<string> GetPaymentHistoryByAccountAsync(string x);
    }
}
