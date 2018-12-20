using CZJ.DNC.Feign;
using WebApiClient;
using WebApiClient.Attributes;

namespace CZJ.DNC.Web.Sample
{
    /// <summary>
    /// 
    /// </summary>
    [TraceHeaderFilter(new string[] { "Authorization"})]
    [HttpHost("http://192.168.0.133:6001")]
    public interface IAccountApi : IHttpApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/SysConfig/Account/FindAll")]

        //[Timeout(1 * 1000)]
        ITask<string> Get();
    }
}
