using Autofac;
using AutoMapper;
using CZJ.Common.Entity;
using CZJ.Dependency;
using CZJ.Reflection;
using System;
using System.Reflection;

namespace CZJ.AutoMapper
{
    /// <summary>
    /// 注册AutoMapper实例
    /// </summary>
    public class AutoMapperRegistrar : IDependencyRegistrar
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
            Action<IMapperConfigurationExpression> configurer = configuration =>
            {
                //初始化注册所有类型
                var types = typeFinder.Find(type =>
                {
                    var typeInfo = type.GetTypeInfo();
                    return typeInfo.IsDefined(typeof(MapsFromAttribute)) ||
                           typeInfo.IsDefined(typeof(MapsToAttribute));
                });
                foreach (var type in types)
                {
                    foreach (var autoMapAttribute in type.GetTypeInfo().GetCustomAttributes<AutoMapAttributeBase>())
                    {
                        autoMapAttribute.CreateMap(configuration, type);
                    }
                }
                var entityTypes = typeFinder.FindAllEntity();
                foreach (var type in entityTypes)
                {
                    configuration.CreateMap(type, type);
                }
            };
            Mapper.Reset();
            Mapper.Initialize(configurer);
            builder.RegisterInstance(Mapper.Instance).As<IMapper>().SingleInstance();
        }
    }
}
