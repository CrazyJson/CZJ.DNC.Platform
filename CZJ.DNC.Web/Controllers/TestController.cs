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
        /// 检测程序Http服务是否正常
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Get()
        {
            return await testService.Say();
        }

        /// <summary>
        /// 检测程序Http服务是否正常
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public string GetXX()
        {
            return testService.SayXX();
        }

        /// <summary>
        /// 检测程序Http服务是否正常
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [CustomHeader(Description = "token", Name = "Authorization", Required = true)]
        public IActionResult GetUserInfo()
        {
            return Redirect("http://192.168.0.133:6001/api/SysConfig/Account/PostQuery");
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
        Task<string> Say();

        string SayXX();
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
        public IHealthApi healthApi { get; set; }

        public IAccountApi accountApi { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HystrixCommand(nameof(FallBackMethod),
IsEnableCircuitBreaker = true,
ExceptionsAllowedBeforeBreaking = 3,
MillisecondsOfBreak = 1000 * 10)]
        public async Task<string> Say()
        {
            var p = await healthApi.Get("Basic F13CECD23F92526B9B9FECA81F973D32A56C6129168A058F04D977EBFBA55BF152DFDFF288D7C190D027896E6075451E");
            return $"TestService-{p}-{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}";
        }

        [HystrixCommand(nameof(FallBackSayXX),
IsEnableCircuitBreaker = true,
ExceptionsAllowedBeforeBreaking = 3,
MillisecondsOfBreak = 1000 * 10)]
        public string SayXX()
        {
            //return "123";
            throw new Exception("123");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> FallBackMethod()
        {
            var list = await accountApi.Get("Basic F13CECD23F92526B9B9FECA81F973D32A56C6129168A058F04D977EBFBA55BF152DFDFF288D7C190D027896E6075451E");
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string FallBackSayXX()
        {
            return "FallBackSayXX";
        }
    }

}
