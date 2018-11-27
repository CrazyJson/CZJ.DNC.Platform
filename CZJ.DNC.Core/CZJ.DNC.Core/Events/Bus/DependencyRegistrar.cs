using Autofac;
using Autofac.Core;
using CZJ.Dependency;
using CZJ.Events.Bus.Factories;
using CZJ.Events.Bus.Handlers;
using CZJ.Reflection;

namespace CZJ.Events.Bus
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get
            {
                return 0;
            }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<EventBus>().As<IEventBus>().SingleInstance().OnActivated(InitEventHandler);
        }

        /// <summary>
        /// 注册所有实现IEventHandler<TEventData> 接口，获取所有事件处理器
        /// </summary>
        /// <param name="e"></param>
        public void InitEventHandler(IActivatedEventArgs<IEventBus> e)
        {
            var list = IocManager.Instance.ResolveAll<IEventHandler>();
            foreach (var component in list)
            {
                var interfaces = component.GetType().GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    if (!typeof(IEventHandler).IsAssignableFrom(@interface))
                    {
                        continue;
                    }
                    var genericArgs = @interface.GetGenericArguments();
                    if (genericArgs.Length == 1)
                    {
                        e.Instance.Register(genericArgs[0], new IocHandlerFactory(IocManager.Instance, component.GetType()));
                    }
                }
            }

        }
    }
}
