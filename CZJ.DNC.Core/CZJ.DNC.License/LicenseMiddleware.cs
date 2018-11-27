// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using CZJ.DNC.License;
using System.Net;
using CZJ.Common.Serializer;
using CZJ.Common;

namespace Microsoft.AspNetCore.Proxy
{
    /// <summary>
    /// License授权 Middleware
    /// </summary>
    public class LicenseMiddleware
    {
        private const int DefaultWebSocketBufferSize = 4096;

        private readonly RequestDelegate _next;
        private readonly IObjectSerializer serializer;
        private DateTime preverifyTime = DateTime.Now;//上次校验时间
        private bool verifySuccess = false;//是否验证成功

        public LicenseMiddleware(RequestDelegate next, IObjectSerializer serializer)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            this.serializer = serializer;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (!verifySuccess || (verifySuccess && (DateTime.Now - preverifyTime).TotalMinutes > 10))
            {
                string msg = LicenseHelper.Verify();
                if (!string.IsNullOrEmpty(msg))
                {
                    verifySuccess = false;
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    var result = new ApiResult<string>
                    {
                        Code = ResultCode.License_Error,
                        Msg = msg
                    };
                    var sc = new StringContent(serializer.Serialize(result));
                    await sc.CopyToAsync(context.Response.Body);
                }
                else
                {
                    verifySuccess = true;
                    preverifyTime = DateTime.Now;
                    await _next.Invoke(context);
                }
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
