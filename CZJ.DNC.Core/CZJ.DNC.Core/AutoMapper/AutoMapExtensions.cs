using AutoMapper;

namespace CZJ.AutoMapper
{
    public static class AutoMapExtensions
    {
        /// <summary>
        /// 将一个动态对象转换成目标对象
        /// </summary>
        /// <typeparam name="TDestination">目标对象类型</typeparam>
        /// <param name="source">动态对象</param>
        /// <returns>目标对象</returns>
        /// <example>
        ///    dynamic dynamicObj = new ExpandoObject();
        ///    dynamicObj.Age = 103;
        ///    dynamicObj.Name = "tkb至简";
        ///    dynamicObj.SchoolCode = 456;
        ///    var pv = AutoMapExtensions.DynamicMap<PersonViewModel>(dynamicObj);
        /// </example>
        public static TDestination DynamicMap<TDestination>(dynamic source)
        {
            return Mapper.Map<TDestination>(source);
        }
        /// <summary>
        /// 使用AutoMapper library 创建一个新对象<typeparamref name="TDestination"/>.
        /// 对象必须在调用之前被映射
        /// </summary>
        /// <typeparam name="TDestination">目标对象类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>目标对象</returns>
        public static TDestination MapTo<TDestination>(this object source)
        {
            return Mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// 使用AutoMapper library 创建一个新对象<typeparamref name="TDestination"/>.
        /// 对象必须在调用之前被映射
        /// </summary>
        /// <typeparam name="TDestination">目标对象类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>目标对象</returns>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }
    }
}
