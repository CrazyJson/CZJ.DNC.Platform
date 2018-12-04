using Autofac.Builder;
using Autofac.Features.Scanning;
using System;
using System.Collections.Generic;
using System.Text;

namespace CZJ.Dependency
{
    /// <summary>
    /// Ioc类型注册处理接口，可用于拦截器注册
    /// </summary>
    public interface IAfterRegister
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builderRegister"></param>
        /// <param name="type">当前注册类型</param>
        /// <param name="bClassRegister">是否类注册</param>
        /// <returns></returns>
        IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> Register(IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> builderRegister, Type type, bool bClassRegister);
    }
}
