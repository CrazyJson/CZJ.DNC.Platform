using System;
using AutoMapper;
using CZJ.Collections.Extensions;

namespace CZJ.AutoMapper
{
    /// <summary>
    /// MapsFrom属性用来标记ViewModel类来源类型 eg Person PersonViewModel
    /// </summary>
    /// <example>
    ///     [MapsFrom(typeof(Person))]
    ///     public class PersonViewModel : Person
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MapsFromAttribute : AutoMapAttributeBase
    {
        public MemberList MemberList { get; set; } = MemberList.Destination;

        /// <summary>
        /// 来源类型
        /// </summary>
        /// <param name="sourceTypes"></param>
        public MapsFromAttribute(params Type[] sourceTypes)
            : base(sourceTypes)
        {

        }

        public MapsFromAttribute(MemberList memberList, params Type[] sourceTypes)
            : this(sourceTypes)
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
                configuration.CreateMap(targetType, type, MemberList);
                configuration.CreateMap(type, targetType, MemberList);
            }
        }
    }
}