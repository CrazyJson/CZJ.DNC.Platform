using CZJ.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace CZJ.Common.Entity
{
    public static class EntityFind
    {
        /// <summary>
        /// 获取所有实体类型
        /// </summary>
        /// <param name="typeFinder"></param>
        /// <returns></returns>
        public static IEnumerable<Type> FindAllEntity(this ITypeFinder typeFinder)
        {
            return typeFinder.FindAll().Where(e => e.BaseType == typeof(object) &&
                e.GetCustomAttribute<TableAttribute>() != null);
        }
    }
}
