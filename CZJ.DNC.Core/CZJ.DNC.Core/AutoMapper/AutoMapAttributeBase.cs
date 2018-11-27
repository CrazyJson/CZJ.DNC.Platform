using System;
using AutoMapper;

namespace CZJ.AutoMapper
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public abstract class AutoMapAttributeBase : Attribute
    {
        public Type[] TargetTypes { get; private set; }

        protected AutoMapAttributeBase(params Type[] targetTypes)
        {
            TargetTypes = targetTypes;
        }

        public abstract void CreateMap(IMapperConfigurationExpression configuration, Type type);
    }
}