using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CZJ.Common;
using CZJ.DNC.Hystrix;

namespace CZJ.DNC.Web.Controllers
{
    /// <summary>
    /// 健康检测接口
    /// </summary>
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        /// <summary>
        /// 检测程序Http服务是否正常
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HystrixCommand(
            IsEnableCircuitBreaker = true,
            ExceptionsAllowedBeforeBreaking = 3,
            MillisecondsOfBreak = 1000 * 5)]
        public ApiResult<string> Get()
        {
            return new ApiResult<string>();
        }
    }
}
