using CZJ.DNC.Hystrix;
using WebApiClient;
using WebApiClient.Attributes;

namespace CZJ.DNC.Web.Sample
{
    /// <summary>
    /// 
    /// </summary>
    //[HttpHost("http://192.168.0.133:6001")]
    //[SignFilter]
    public interface IHealthApi : IHttpApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/SysConfig/UAC/GetUserInfo")]       
        //[Timeout(1 * 1000)]
        ITask<string> Get([Header("Authorization")]string token);
    }
}
