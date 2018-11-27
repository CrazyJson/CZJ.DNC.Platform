using System;
using AutoMapper;
using CZJ.Collections.Extensions;

namespace CZJ.AutoMapper
{
    /// <summary>
    /// MapsTo属性用来标记ViewModel类目标类型 eg Person PersonViewModel
    /// </summary>
    /// <example>
    ///     [MapsTo(typeof(PersonViewModel))]
    ///     public class Person
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MapsToAttribute : AutoMapAttributeBase
    {
        public MemberList MemberList { get; set; } = MemberList.Source;

        public MapsToAttribute(params Type[] targetTypes)
            : base(targetTypes)
        {

        }

        public MapsToAttribute(MemberList memberList, params Type[] targetTypes)
            : this(targetTypes)
        {
            MemberList = memberList;
        }

        public override void CreateMap(IMapperConfigurationExpression configuration, Type type)
        {
            if (TargetTypes.IsNullOrEmpty())
            {
                return;
            }

            foreach (var targetType in TargetTypes)
            {
                configuration.CreateMap(type, targetType, MemberList);
                configuration.CreateMap(targetType, type, MemberList);
            }
        }
    }
}