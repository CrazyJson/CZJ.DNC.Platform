using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;
using WebApiClient.Contexts;

namespace CZJ.DNC.Feign
{
    public class TokenActionFilter : IApiActionFilter
    {
        public IHttpContextAccessor httpContextAccessor { get; set; }

        public Task OnBeginRequestAsync(ApiActionContext context)
        {
            //context.HttpApiConfig.
            throw new NotImplementedException();
        }

        public Task OnEndRequestAsync(ApiActionContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class SignFilterAttribute : ApiActionFilterAttribute
    {
        public IHttpContextAccessor httpContextAccessor { get; set; }

        //public SignFilterAttribute(IHttpContextAccessor httpContextAccessor)
        //{

        //}

        public override Task OnBeginRequestAsync(ApiActionContext context)
        {
            var sign = DateTime.Now.Ticks.ToString();
            context.RequestMessage.AddUrlQuery("sign", sign);
            return base.OnBeginRequestAsync(context);
        }
    }
}
