using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CZJ.Common;

namespace CZJ.DNC.Web.Controllers
{
    /// <summary>
    /// 健康检测接口
    /// </summary>
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        /// <summary>
        /// 检测程序Http服务是否正常
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<string> Get()
        {
            return new ApiResult<string>();
        }
    }
}
