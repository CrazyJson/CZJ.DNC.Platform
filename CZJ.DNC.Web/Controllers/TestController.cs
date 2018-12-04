using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CZJ.Common;
using CZJ.DNC.Hystrix;
using System;
using System.Threading.Tasks;
using CZJ.Dependency;

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
        /// 
        /// </summary>
        public ITestService testService { get; set; }
        /// <summary>
        /// 检测程序Http服务是否正常
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            return testService.Say();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ITestService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string Say();
    }

    /// <summary>
    /// 
    /// </summary>
    public class TestService : ITestService, ITransientDependency
    {
        private static int i = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HystrixCommand(nameof(FallBackMethod),
IsEnableCircuitBreaker = true,
ExceptionsAllowedBeforeBreaking = 3,
MillisecondsOfBreak = 1000 * 10, MaxRetryTimes = 10, CacheTTLMilliseconds = 10 * 1000)]
        public string Say()
        {
            i++;
            if (i < 4)
            {
                throw new Exception("123");
            }

            return "TestService-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string FallBackMethod()
        {
            return "FallBackMethod";
        }
    }

}
