/*
 * Model: EF初始化模块
 * Desctiption: EF初始化服务
 * Author: 杜冬军
 * Created: 2016/06/20 14:05:50 
 * Copyright：武汉中科通达高新技术股份有限公司
 */
using CZJ.Common.Entity;
using CZJ.Common.Mapping;
using CZJ.Dependency;
using CZJ.Reflection;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CZJ.Common
{
    /// <summary>
    /// EF初始化-进行实体读取
    /// </summary>
    public class EFInitializer
    {
        private static bool s_inited = false;
        /// <summary>
		/// 读取配置,寻找符合规则的实体
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
        public static void UnSafeInit()
        {
            if (s_inited)
            {
                return;
            }
            var typeFinder = IocManager.Instance.Resolve<ITypeFinder>();

            var entityTypes = typeFinder.FindAllEntity();
            foreach (var type in entityTypes)
            {
 				//缓存相关信息
                MappingResolver.CreateEntityMap(type);
            }
            s_inited = true;
        }

        public static string BinDirectory
        {
            get
            {
                return AppContext.BaseDirectory;
            }
        }
    }
    /// <summary>
    /// EF实体相关属性缓存
    /// </summary>
    public class EFTypeDescriptionCache
    {
        private static BindingFlags BindFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public;

        /// <summary>
        /// 获取实体的主键字段
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>主键字段</returns>
        public static string GetPrimaryKeyField(Type entityType)
        {
            var map = MappingResolver.GetEntityMap(entityType);
            if (map != null)
            {
                var colinfo = map.KeyMaps.First();
                if (colinfo == null)
                {
                    throw new ArgumentException(string.Format("实体{0}不存在主键字段", entityType.FullName));
                }
                return colinfo.PropertyName;
            }
            else
            {
                throw new ArgumentException(string.Format("实体{0}不存在", entityType.FullName));
            }
        }

        public static Type GetPropType<T>(string propName)
        {
            Type DefaultType = typeof(string);
            Type result = typeof(string);
            Type entityType = typeof(T);
            var map = MappingResolver.GetEntityMap(entityType);
            if (map != null)
            {
                var cmi = map.PropertyMaps.FirstOrDefault(e => e.PropertyName == propName);
                if (cmi != null)
                {
                    result = cmi.PropertyType;
                }
            }
            else
            {
                var pInfo = entityType.GetProperty(propName, BindFlags);
                result = pInfo == null ? DefaultType : pInfo.PropertyType;
            }
            if (result.IsEnum)
            {
                result = Enum.GetUnderlyingType(result);
            }
            return result;
        }
    }
}
