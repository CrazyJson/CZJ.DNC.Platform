using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace CZJ.Common.Mapping
{
    /// <summary>
    /// A provider class to get entity mapping data.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class uses a lot of reflection to get access to internal mapping data that Entity Framework
    /// does not provide access to.  There maybe issues with this implementation as version of of Entity Framework change.
    /// </para>
    /// <para>
    /// This implementation can be overridden using the <see cref="Locator"/> container resolving 
    /// the <see cref="IMappingProvider"/> <see langword="interface"/>.
    /// </para>
    /// </remarks>
    public class ReflectionMappingProvider : IMappingProvider
    {
        private static BindingFlags s_flag = BindingFlags.Instance | BindingFlags.Public;

        private ConcurrentDictionary<Type, EntityMap> _cache;

        public ReflectionMappingProvider()
        {
            _cache = new ConcurrentDictionary<Type, EntityMap>();
        }

        public EntityMap GetEntityMap(Type type)
        {
            EntityMap map = null;
            _cache.TryGetValue(type, out map);
            return map;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public EntityMap CreateEntityMap(Type entityType)
        {
            var map = GetEntityMap(entityType);
            if (map != null)
            {
                return map;
            }
            map = new EntityMap(entityType);
            var tableAttr = entityType.GetCustomAttribute<TableAttribute>();
            map.TableName = string.IsNullOrEmpty(tableAttr.Name) ? entityType.Name : tableAttr.Name;


            PropertyInfo[] properties = entityType.GetProperties(s_flag);

            foreach (PropertyInfo prop in properties)
            {
                KeyAttribute attrKey = prop.GetCustomAttribute<KeyAttribute>();
                ColumnAttribute attrColumn = prop.GetCustomAttribute<ColumnAttribute>();
                NotMappedAttribute attrIgnore = prop.GetCustomAttribute<NotMappedAttribute>();
                if (attrIgnore != null)
                {
                    continue;
                }
                bool iskey = attrKey != null;
                string dbColName = attrIgnore != null ? string.Empty : ((attrColumn != null && !string.IsNullOrEmpty(attrColumn.Name)) ? attrColumn.Name : prop.Name);
                var pm = new PropertyMap
                {
                    PropertyName = prop.Name,
                    ColumnName = dbColName,
                    PropertyType = prop.PropertyType
                };
                if (iskey)
                {
                    map.KeyMaps.Add(pm);
                }
                map.PropertyMaps.Add(pm);
            }
            _cache.TryAdd(entityType, map);
            return map;
        }
    }
}
