using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace CZJ.Reflection
{
    /// <summary>
    /// 默认的程序集查找实现
    /// </summary>
    public class CurrentDomainAssemblyFinder : IAssemblyFinder
    {
        public static CurrentDomainAssemblyFinder Instance { get; } = new CurrentDomainAssemblyFinder();

        /// <summary>
        /// 获取所有程序集
        /// </summary>
        /// <returns></returns>
        public List<Assembly> GetAllAssemblies()
        {
            var deps = DependencyContext.Default;
            //排除所有的系统程序集、Nuget下载包(但包含自己上传的CZJ.开头的)
            var libs = deps.CompileLibraries.Where(lib => lib.Name.StartsWith("CZJ.", StringComparison.CurrentCultureIgnoreCase) 
                || (!lib.Serviceable && lib.Type != "package"));
            var list = new List<Assembly>();
            foreach (var lib in libs)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    list.Add(assembly);
                }
                catch (Exception)
                {
                }
            }
            return list;
        }
    }
}