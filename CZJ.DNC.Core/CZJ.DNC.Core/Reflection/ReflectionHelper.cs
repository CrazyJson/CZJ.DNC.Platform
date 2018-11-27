using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CZJ.Reflection
{
    /// <summary>
    /// Defines helper methods for reflection.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Checks whether <paramref name="givenType"/> implements/inherits <paramref name="genericType"/>.
        /// </summary>
        /// <param name="givenType">Type to check</param>
        /// <param name="genericType">Generic type</param>
        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            foreach (var interfaceType in givenType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }

            if (givenType.BaseType == null)
            {
                return false;
            }

            return IsAssignableToGenericType(givenType.BaseType, genericType);
        }

        /// <summary>
        /// Gets a list of attributes defined for a class member and it's declaring type including inherited attributes.
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute</typeparam>
        /// <param name="memberInfo">MemberInfo</param>
        public static List<TAttribute> GetAttributesOfMemberAndDeclaringType<TAttribute>(MemberInfo memberInfo)
            where TAttribute : Attribute
        {
            var attributeList = new List<TAttribute>();

            //Add attributes on the member
            if (memberInfo.IsDefined(typeof(TAttribute), true))
            {
                attributeList.AddRange(memberInfo.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>());
            }

            //Add attributes on the class
            if (memberInfo.DeclaringType != null && memberInfo.DeclaringType.IsDefined(typeof(TAttribute), true))
            {
                attributeList.AddRange(memberInfo.DeclaringType.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>());
            }

            return attributeList;
        }

        /// <summary>
        /// 获取一个类型的真实类型 int? 返回int类型
        /// </summary>
        /// <param name="propertyType">属性类型</param>
        /// <returns></returns>
        public static Type GetRealPropertyType(this Type propertyType)
        {
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return propertyType.GetGenericArguments()[0];
            }
            else
            {
                return propertyType;
            }
        }

        /// <summary>
        /// 判断一个类型是否为数值类型 int int?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumberType(this Type type)
        {
            var realType = GetRealPropertyType(type);
            switch (realType.ToString())
            {
                case "System.Single":
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.Decimal":
                case "System.Double":
                case "System.SByte":
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断类型是否为值类型 string ,int...
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumericType(this Type type)
        {
            var o = GetRealPropertyType(type);
            if(o == typeof(string))
            {
                return true;
            }
            return !o.IsClass && !o.IsInterface && o.GetInterfaces().Any(q => q == typeof(IFormattable));
        }
    }
}