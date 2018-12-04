using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Autofac.Features.Scanning;
using CZJ.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CZJ.Dependency
{
    /// <summary>
    /// Container manager
    /// </summary>
    public class IocManager : IIocManager
    {
        private IContainer _container;

        public static IocManager Instance { get; } = new IocManager();

        /// <summary>
        /// Ioc容器初始化
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public IServiceProvider Initialize(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            var AssemblyFinder = CurrentDomainAssemblyFinder.Instance;
            var typeFinder = TypeFinder.Instance;

            builder.RegisterInstance(AssemblyFinder).As<IAssemblyFinder>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            builder.RegisterInstance(Instance).As<IIocManager>().SingleInstance();

            var listAllType = typeFinder.FindAll();

            //找到所有外部IDependencyRegistrar实现，调用注册
            var registrarType = typeof(IDependencyRegistrar);
            var arrRegistrarType = listAllType.Where(t => registrarType.IsAssignableFrom(t) && t != registrarType).ToArray();
            var listRegistrarInstances = new List<IDependencyRegistrar>();
            foreach (var drType in arrRegistrarType)
            {
                listRegistrarInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            }
            //排序
            listRegistrarInstances = listRegistrarInstances.OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in listRegistrarInstances)
            {
                dependencyRegistrar.Register(builder, typeFinder);
            }

            var arrAfterRegistrarType = listAllType.Where(t => typeof(IAfterRegister).IsAssignableFrom(t)
                && t != typeof(IAfterRegister)).ToArray();
            List<IAfterRegister> afterRegisters = new List<IAfterRegister>();
            foreach (var item in arrAfterRegistrarType)
            {
                afterRegisters.Add((IAfterRegister)Activator.CreateInstance(item));
            }

            //注册ITransientDependency实现类
            var dependencyType = typeof(ITransientDependency);
            //注册ISingletonDependency实现类
            var singletonDependencyType = typeof(ISingletonDependency);
            bool bClassRegister = false, bISingletonRegister = false, bTransientRegister = false;
            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> builderRegister = null;
            foreach (var type in listAllType)
            {
                bTransientRegister = dependencyType.IsAssignableFrom(type) && type != dependencyType;
                bISingletonRegister = singletonDependencyType.IsAssignableFrom(type) && type != singletonDependencyType;
                if (!bTransientRegister && !bISingletonRegister)
                {
                    continue;
                }
                bClassRegister = type.IsClass && !type.IsAbstract && !type.BaseType.IsInterface && type.BaseType != typeof(object);
                builderRegister = builder.RegisterTypes(type);
                if (!bClassRegister)
                {
                    builderRegister = builderRegister.AsImplementedInterfaces();
                }
                else
                {
                    builderRegister = builderRegister.As(type.BaseType);
                }
                if (bTransientRegister)
                {
                    builderRegister = builderRegister.InstancePerLifetimeScope();
                }
                else
                {
                    builderRegister = builderRegister.SingleInstance();
                }
                builderRegister = builderRegister.PropertiesAutowired();
                if (afterRegisters.Count > 0)
                {
                    builderRegister = AfterRegister(builderRegister, afterRegisters, type, bClassRegister);
                }  
            }
            builder.Populate(services);
            _container = builder.Build();
            return new AutofacServiceProvider(_container);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builderRegister"></param>
        /// <param name="afterRegisters"></param>
        /// <param name="type"></param>
        /// <param name="bClassRegister"></param>
        /// <returns></returns>
        private IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> AfterRegister(IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> builderRegister
            , List<IAfterRegister> afterRegisters, Type type, bool bClassRegister)
        {
            foreach (var item in afterRegisters)
            {
                builderRegister = item.Register(builderRegister, type, bClassRegister);
            }
            return builderRegister;
        }

        /// <summary>
        /// Gets a container
        /// </summary>
        public virtual IContainer Container
        {
            get
            {
                return _container;
            }
        }

        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<T>();
            }
            return scope.ResolveKeyed<T>(key);
        }

        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual T Resolve<T>(params Parameter[] parameters) where T : class
        {
            var scope = Scope();
            return scope.Resolve<T>(parameters);
        }

        /// <summary>
        /// Resolve
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual object Resolve(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return scope.Resolve(type);
        }

        /// <summary>
        /// Resolve all
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved services</returns>
        public virtual T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<IEnumerable<T>>().ToArray();
            }
            return scope.ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }

        /// <summary>
        /// Resolve unregistered service
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual T ResolveUnregistered<T>(ILifetimeScope scope = null) where T : class
        {
            return ResolveUnregistered(typeof(T), scope) as T;
        }

        /// <summary>
        /// Resolve unregistered service
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual object ResolveUnregistered(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = Resolve(parameter.ParameterType, scope);
                        if (service == null) throw new Exception("Unknown dependency");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (Exception)
                {

                }
            }
            throw new Exception("No constructor  was found that had all the dependencies satisfied.");
        }

        /// <summary>
        /// Try to resolve srevice
        /// </summary>
        /// <param name="serviceType">Type</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <param name="instance">Resolved service</param>
        /// <returns>Value indicating whether service has been successfully resolved</returns>
        public virtual bool TryResolve(Type serviceType, ILifetimeScope scope, out object instance)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return scope.TryResolve(serviceType, out instance);
        }

        /// <summary>
        /// Check whether some service is registered (can be resolved)
        /// </summary>
        /// <param name="serviceType">Type</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Result</returns>
        public virtual bool IsRegistered(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return scope.IsRegistered(serviceType);
        }

        /// <summary>
        /// Resolve optional
        /// </summary>
        /// <param name="serviceType">Type</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual object ResolveOptional(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return scope.ResolveOptional(serviceType);
        }

        /// <summary>
        /// Get current scope
        /// </summary>
        /// <returns>Scope</returns>
        public virtual ILifetimeScope Scope()
        {
            try
            {
                //when such lifetime scope is returned, you should be sure that it'll be disposed once used (e.g. in schedule tasks)
                return Container.BeginLifetimeScope();
            }
            catch (Exception)
            {
                //we can get an exception here if RequestLifetimeScope is already disposed
                //for example, requested in or after "Application_EndRequest" handler
                //but note that usually it should never happen

                //when such lifetime scope is returned, you should be sure that it'll be disposed once used (e.g. in schedule tasks)
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
        }
    }
}
