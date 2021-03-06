﻿using CZJ.Auditing;
using CZJ.Common;
using CZJ.Dependency;
using CZJ.Common.Serializer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    /// <summary>
    /// Represents the extensions for <see cref="HttpClient"/>.
    /// </summary>
    public static class HttpClientExtensions
    {
        private static HttpClient _client = new HttpClient(new HttpClientHandler
        {
            MaxConnectionsPerServer = 100000,
            UseDefaultCredentials = false,
            AllowAutoRedirect = false,
            UseCookies = false,
            Proxy = null,
            UseProxy = false,
            AutomaticDecompression = DecompressionMethods.GZip
        });

        #region "私有方法"

        private static HttpRequestMessage GetHttpRequestMessage(CitmsHttpRequest request, Uri uri)
        {
            if (request.PathQuery != null)
            {
                uri = uri.UsePathQuery(KeyValueSerializer.Serialize(request.PathQuery));
            }
            var requestMessage = new HttpRequestMessage();
            var requestMethod = request.Method;
            if (requestMethod != HttpMethod.Get &&
                requestMethod != HttpMethod.Head &&
                requestMethod != HttpMethod.Trace
                )
            {
                HttpContent content = null;
                if (request.HasFile)
                {
                    //文件上传
                    var form = new MultipartFormDataContent();
                    foreach (var file in request.FileParameters)
                    {
                        var fileContent = new ByteArrayContent(file.Bytes);
                        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            FileName = file.FileName,
                            Name = file.Name
                        };
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                        form.Add(fileContent);
                    }
                    content = form;
                }
                else if (request.MediaType == "application/json" && request.Body != null)
                {
                    content = new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.UTF8, request.MediaType);
                }
                else if (request.MediaType == "application/x-www-form-urlencoded" && request.Body != null)
                {
                    content = new FormUrlEncodedContent(request.Body as IEnumerable<KeyValuePair<string, string>>);
                }
                requestMessage.Content = content;
            }
            requestMessage.Headers.Host = uri.Authority;
            requestMessage.RequestUri = uri;
            requestMessage.Method = requestMethod;
            if (!string.IsNullOrEmpty(request.Token))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", request.Token);
            }
            request.RequestSet?.Invoke(requestMessage);
            return requestMessage;
        }

        /// <summary>
        /// 创建HttpClient
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static HttpClient CreateClient(CitmsHttpRequest request)
        {
            //_client.Timeout = TimeSpan.FromMilliseconds(request.TimeOutMilSeconds);
            //_client.DefaultRequestHeaders.Connection.Add("keep-alive");
            return _client;
        }

        /// <summary>
        ///  使用json格式化内容方法
        /// </summary>
        /// <typeparam name="TResp"></typeparam>
        /// <param name="resp"></param>
        /// <returns></returns>
        public static async Task<T> JsonFormat<T>(this HttpResponseMessage resp)
        {
            var contentStr = await resp.Content.ReadAsStringAsync();
            if (typeof(T) == typeof(System.String))
            {
                return (T)Convert.ChangeType(contentStr, typeof(T));
            }
            try
            {
                return JsonConvert.DeserializeObject<T>(contentStr);
            }
            catch (Exception e)
            {
                Exception ex = new Exception($"接口返回值：{contentStr}，反序列化成类型{typeof(T)}出现异常", e);
                throw new ApiException(ex, ResultCode.ApiResult_FomatterError);
            }
        }
        #endregion


        /// <summary>
        /// url添加query或替换segment
        /// </summary>
        /// <param name="uri">url</param>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public static Uri UsePathQuery(this Uri uri, IEnumerable<KeyValuePair<string, string>> keyValues)
        {
            var editor = new UriEditor(uri);
            foreach (var keyValue in keyValues)
            {
                if (editor.Replace(keyValue.Key, keyValue.Value) == false)
                {
                    editor.AddQuery(keyValue.Key, keyValue.Value);
                }
            }
            return editor.Uri;
        }

        /// <summary>
        /// 发送Http请求
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns></returns>
        public async static Task<HttpResponseMessage> SendAsync(this CitmsHttpRequest request)
        {
            bool isAboluteUrl = true;//是否绝对url地址
            if (!request.AddressUrl.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) &&
                !request.AddressUrl.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
            {
                var app = IocManager.Instance.Resolve<IAppInfoProvider>();
                request.AddressUrl = $"http://{app.IpAddress}:{app.Ports[0]}" + request.AddressUrl;
                isAboluteUrl = false;
            }
            Uri uri = new Uri(request.AddressUrl, UriKind.RelativeOrAbsolute);
            if (request.ProxyRequest)
            {
                var ruleList = await uri.PathAndQuery.MatchProxyRule();
                if (ruleList != null && ruleList.Count > 0)
                {
                    int i = 1;
                    foreach (var proxyOption in ruleList)
                    {
                        uri = new Uri(UriHelper.BuildAbsolute(
                            proxyOption.Uri.Scheme,
                            new HostString(proxyOption.Uri.Authority),
                            proxyOption.Uri.AbsolutePath,
                            uri.AbsolutePath,
                            new QueryString(uri.Query).Add(new QueryString(proxyOption.Uri.Query)))
                        );
                        using (var requestMessage = GetHttpRequestMessage(request, uri))
                        {
                            try
                            {
                                var result = await GetHttpResponseMessage(request, requestMessage);
                                if (i == ruleList.Count || result.StatusCode != HttpStatusCode.NotFound)
                                {
                                    IocManager.Instance.Resolve<ILogger<CitmsHttpRequest>>()
                                        .LogInformation("代理插件--请求【{0} {1}】，响应码：{2}",
                                        request.Method.ToString(), requestMessage.RequestUri.ToString(), result.StatusCode.ToInt());
                                    return result;
                                }
                            }
                            catch (Exception e)
                            {
                                if (i == ruleList.Count)
                                {
                                    throw e;
                                }
                                else
                                {
                                    IocManager.Instance.Resolve<ILogger<CitmsHttpRequest>>().LogError(e, "调用接口异常");
                                }
                            }
                            i++;
                        }
                    }
                }
                else if (!isAboluteUrl && uri.PathAndQuery.MatchNeedProxy())
                {
                    throw new Exception($"请求地址{uri.AbsolutePath}未能找到代理请求节点，无法进行请求转发");
                }
            }
            using (var requestMessageNew = GetHttpRequestMessage(request, uri))
            {
                return await GetHttpResponseMessage(request, requestMessageNew);
            }
        }

        private static async Task<HttpResponseMessage> GetHttpResponseMessage(CitmsHttpRequest request, HttpRequestMessage requestMessage)
        {
            var maxTimeout = _client.Timeout;
            var timeSpan = TimeSpan.FromMilliseconds(request.TimeOutMilliseconds);
            if (maxTimeout >= TimeSpan.Zero && timeSpan > maxTimeout)
            {
                throw new Exception($"Timeout值{timeSpan}不能超时HttpClient.Timeout[{maxTimeout}]");
            }
            try
            {
                var client = CreateClient(request);
                if (request.TimeOutMilliseconds <= 0)
                {
                    return await client.SendAsync(requestMessage);
                }
                else
                {
                    var cancellation = new CancellationTokenSource(timeSpan);
                    return await client.SendAsync(requestMessage, cancellation.Token);
                }
            }
            catch (HttpRequestException e)
            {
                Exception ex = new Exception($"{request.Method.ToString()}请求{request.AddressUrl},实际地址：{requestMessage.RequestUri.ToString()},参数{JsonConvert.SerializeObject(request.Body)}发生异常,{e.Message}", e);
                if (e.Message == "操作超时")
                {
                    throw new ApiException(ex, ResultCode.Server_Resources_Unavailable);
                }
                throw new ApiException(ex, ResultCode.Api_CallError);
            }
            catch (Exception e)
            {
                Exception ex = new Exception($"{request.Method.ToString()}请求{request.AddressUrl},实际地址：{requestMessage.RequestUri.ToString()},参数{JsonConvert.SerializeObject(request.Body)}发生异常,{e.Message}", e);
                throw new ApiException(ex, ResultCode.Api_CallError);
            }
        }

        /// <summary>
        /// 发送Http请求
        /// </summary>
        /// <typeparam name="T">序列化类型</typeparam>
        /// <param name="request">请求对象</param>
        /// <returns></returns>
        public async static Task<T> SendAsync<T>(this CitmsHttpRequest request)
        {
            using (var r = await SendAsync(request))
            {
                if (r.IsSuccessStatusCode)
                {
                    return await JsonFormat<T>(r);
                }
                else
                {
                    Exception ex = new Exception($"{request.Method.ToString()}请求{request.AddressUrl},参数{JsonConvert.SerializeObject(request.Body)}，服务器响应码{Convert.ToInt32(r.StatusCode)}({r.ReasonPhrase}){r.Content.ReadAsStringAsync().Result}");
                    ResultCode code = ResultCode.Api_CallError;
                    switch (r.StatusCode)
                    {
                        case HttpStatusCode.BadRequest:
                            code = ResultCode.Bad_Request;
                            break;
                        case HttpStatusCode.Unauthorized:
                            code = ResultCode.Api_Unauthorized;
                            break;
                        case HttpStatusCode.NotFound:
                            code = ResultCode.Server_Resources_Unavailable;
                            break;
                        default:
                            code = ResultCode.Api_CallError;
                            break;
                    }
                    throw new ApiException(ex, code);
                }
            }
        }
    }
}