// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using CZJ.Common.Proxy;
using CZJ.Dependency;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System.Net.Http
{
    /// <summary>
    /// 代理规则拓展
    /// </summary>
    public static class ProxyExtend
    {
        private static ProxyOptionGroup options = null;

        /// <summary>
        /// 初始化代理转发规则
        /// </summary>
        /// <param name="_options"></param>
        public static void InitProxyRule(ProxyOptionGroup _options)
        {
            options = _options;
        }

        /// <summary>
        /// 添加代理转发规则
        /// </summary>
        /// <param name="rule"></param>
        public static void AddProxyRule(ProxyOptions rule)
        {
            if (options == null)
            {
                options = new ProxyOptionGroup { Excludes = new List<string>(), Options = new List<ProxyOptions>() };
            }
            options.Options.Add(rule);
        }

        /// <summary>
        /// 找到转发匹配规则
        /// </summary>
        /// <param name="options">规则组</param>
        /// <param name="path">请求路径</param>
        /// <returns>规则组</returns>
        public async static Task<List<ProxyOptions>> MatchProxyRule(this string path)
        {
            return await MatchProxyRule(path, options);
        }

        /// <summary>
        /// 匹配请求路径是否需要代理请求
        /// 如果为否则请求本机
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>bool</returns>
        public static bool MatchNeedProxy(this string path)
        {
            //静态代理规则
            if (options == null || (options.Excludes != null && options.Excludes.Exists(e => path.StartsWith(e, StringComparison.CurrentCultureIgnoreCase))))
            {
                return false;
            }
            List<ProxyOptions> opList = new List<ProxyOptions>();
            foreach (var item in options.Options)
            {
                if (item.Excludes != null && item.Excludes.Exists(e => path.StartsWith(e, StringComparison.CurrentCultureIgnoreCase)))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 找到转发匹配规则
        /// </summary>
        /// <param name="options">规则组</param>
        /// <param name="path">请求路径</param>
        /// <returns>规则组</returns>
        public async static Task<List<ProxyOptions>> MatchProxyRule(this string path, ProxyOptionGroup options)
        {
            //静态代理规则
            if (options == null || (options.Excludes != null && options.Excludes.Exists(e => path.StartsWith(e, StringComparison.CurrentCultureIgnoreCase))))
            {
                return null;
            }
            List<ProxyOptions> opList = new List<ProxyOptions>();
            foreach (var item in options.Options)
            {
                if (item.Excludes != null && item.Excludes.Exists(e => path.StartsWith(e, StringComparison.CurrentCultureIgnoreCase)))
                {
                    continue;
                }
                if (item.MatchReg.IsMatch(path))
                {
                    opList.Add(item);
                }
            }
            //静态代理规则没有匹配中
            if (opList.Count == 0)
            {
                //动态代理规则
                var dynamicProxyList = IocManager.Instance.Resolve<IEnumerable<IDynamicProxyRule>>();
                if (dynamicProxyList != null && dynamicProxyList.Count() > 0)
                {
                    foreach (var pathParse in dynamicProxyList)
                    {
                        string uri = await pathParse.ParsePath(path);
                        if (!string.IsNullOrWhiteSpace(uri) && !uri.Equals(path))
                        {
                            opList.Add(new ProxyOptions
                            {
                                MatchReg = new Regex(path, RegexOptions.IgnorePatternWhitespace),
                                Uri = new Uri(uri)
                            });
                        }
                    }
                }
            }
            return opList;
        }
    }
}
