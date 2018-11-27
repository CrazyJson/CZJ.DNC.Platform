using System;
using System.Collections.Generic;

namespace CZJ.Common.Mapping
{
    /// <summary>
    /// 定义实体类型映射关系
    /// </summary>
    public class EntityMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMap"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        public EntityMap(Type entityType)
        {
            EntityType = entityType;
            KeyMaps = new List<PropertyMap>();
            PropertyMaps = new List<PropertyMap>();
        }

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        public Type EntityType { get; }
        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName { get; set; }

        /// <summary>
        /// Gets the property maps.
        /// </summary>
        public List<PropertyMap> PropertyMaps { get; }

        /// <summary>
        /// Gets the key maps.
        /// </summary>
        public List<PropertyMap> KeyMaps { get; }

    }
}