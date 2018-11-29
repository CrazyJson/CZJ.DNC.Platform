using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Primitives;
using System;
using CZJ.Common;

namespace Citms.Common.Controllers
{
    /// <summary>
    /// 程序管理接口
    /// </summary>
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AppController : Controller
    {
        private readonly IApplicationLifetime lifetime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lifetime"></param>
        public AppController(IApplicationLifetime lifetime)
        {
            this.lifetime = lifetime;
        }

        /// <summary>
        /// 停止程序
        /// </summary>
        /// <returns></returns>
        [HttpHead]
        [Route("[action]")]
        [CustomHeader(Name = "Stop-Application", Description = "停止应用程序")]
        public void Stop()
        {
            if(Request.Headers.TryGetValue("Stop-Application", out StringValues values) 
                && values.ToString().Equals("yes",StringComparison.CurrentCultureIgnoreCase))
            {
                lifetime.StopApplication();
            }
        }
    }
}
