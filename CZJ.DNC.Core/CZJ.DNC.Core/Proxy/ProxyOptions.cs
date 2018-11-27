// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace System.Net.Http
{
    public class ProxyOptionGroup
    {
        /// <summary>
        /// 匹配转发规则
        /// </summary>
        public List<ProxyOptions> Options { get; set; }

        /// <summary>
        /// 排除在外的Url地址，可为空。优先级高于Options里面设置的
        /// </summary>
        public List<string> Excludes { get; set; }
    }

    public class ProxyOptions
    {
        /// <summary>
        /// 代理转发URI
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// 需要代理的URL匹配规则 ，正则
        /// </summary>
        public Regex MatchReg { get; set; }

        /// <summary>
        /// 排除在外的Url地址，可为空
        /// </summary>
        public List<string> Excludes { get; set; }
    }
}
