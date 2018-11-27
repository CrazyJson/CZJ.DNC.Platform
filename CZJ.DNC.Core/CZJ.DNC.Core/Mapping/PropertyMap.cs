using System;

namespace CZJ.Common.Mapping
{
    /// <summary>
    /// A class representing a property map
    /// </summary>
    public class PropertyMap
    {
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        ///  Ù–‘¿‡–Õ
        /// </summary>
        public Type PropertyType { get; set; }
    }
}