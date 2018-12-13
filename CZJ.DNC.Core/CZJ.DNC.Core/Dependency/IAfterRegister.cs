using Autofac.Builder;
using Autofac.Features.Scanning;
using System;
using System.Collections.Generic;

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
        /// <returns></returns>
        IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> Register(IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> builderRegister, Type type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builderRegister"></param>
        /// <param name="type">当前注册类型</param>
        /// <returns></returns>
        IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> Register<TLimit, TActivatorData, TRegistrationStyle>(IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration, Type type);
    }

    /// <summary>
    /// 
    /// </summary>
    public static class RegisterExtend
    {
        public static List<IAfterRegister> afterRegisters { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registration"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> OtherRegister(this IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration,Type type)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }
            if (afterRegisters != null && afterRegisters.Count > 0)
            {
                foreach (var item in afterRegisters)
                {
                    registration = item.Register(registration, type);
                }
            }
            return registration;
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> OtherRegister<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration, Type type)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }
            if (afterRegisters != null && afterRegisters.Count > 0)
            {
                foreach (var item in afterRegisters)
                {
                    registration = item.Register(registration, type);
                }
            }
            return registration;
        }
    }
}
