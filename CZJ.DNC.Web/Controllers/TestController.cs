using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using CZJ.Dependency;
using CZJ.Common;
using System.Text;
using System.IO;
using CZJ.DNC.Hystrix;
using CZJ.DNC.Web.Sample;
using System.Threading.Tasks;

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
        /// 
        /// </summary>
        public IPaymentWebApi paymentWebApi { get; set; }

        /// <summary>
        /// 检测程序Http服务是否正常
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Get()
        {
            var p = await paymentWebApi.GetPaymentHistoryByAccountAsync("my name is ");
            return testService.Say();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        [HttpPost]
        [Route("[action]")]
        public string Upload(SwaggerFile files)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("共接收到{0}文件", Request.Form.Files.Count);
            sb.AppendLine();
            foreach (var file in Request.Form.Files)
            {
                using (FileStream fs = new FileStream($@"C:\Users\ANGLE\Pictures\Screenshots\Temp\{file.FileName}", FileMode.Create))
                {
                    file.CopyTo(fs);
                }
                sb.AppendFormat("ContentDisposition:{0},ContentType:{1},FileName:{2},Name:{3}", file.ContentDisposition, file.ContentType, file.FileName, file.Name);
                sb.AppendLine();
            }
            return sb.ToString();
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
MillisecondsOfBreak = 1000 * 10, CacheTTLMilliseconds = 10 * 1000)]
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
