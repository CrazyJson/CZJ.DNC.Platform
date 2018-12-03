using Autofac;
using CZJ.Dependency;
using CZJ.Reflection;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Citms.Common.MVC
{
    /// <summary>
    /// 
    /// </summary>
    public class ControllerRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// 
        /// </summary>
        public int Order
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="typeFinder"></param>
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //注册Controller,实现属性注入
            var IControllerType = typeof(ControllerBase);
            var arrControllerType = typeFinder.FindAll().Where(t => IControllerType.IsAssignableFrom(t) && t != IControllerType).ToArray();
            builder.RegisterTypes(arrControllerType).PropertiesAutowired();
        }
    }
}
