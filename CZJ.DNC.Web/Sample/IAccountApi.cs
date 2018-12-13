using CZJ.DNC.Hystrix;
using WebApiClient;
using WebApiClient.Attributes;

namespace CZJ.DNC.Web.Sample
{
    /// <summary>
    /// 
    /// </summary>
    [HttpHost("http://192.168.0.133:6001")]
    //[SignFilter]
    public interface IAccountApi : IHttpApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/SysConfig/Account/FindAll")]       
        //[Timeout(1 * 1000)]
        ITask<string> Get([Header("Authorization")]string token);
    }
}
