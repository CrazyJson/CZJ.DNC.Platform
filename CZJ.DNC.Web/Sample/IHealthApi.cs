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
        [HttpGet("/api/SysConfig/UAC/GetUserInfo?a=12")]       
        //[Timeout(1 * 1000)]
        [Tags("AppNo","SysConfig")]
        ITask<string> Get([Header("Authorization")]string token);
    }
}
