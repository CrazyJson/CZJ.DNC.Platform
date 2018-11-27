using System;

namespace CZJ.Common.Mapping
{
    /// <summary>
    /// An <see langword="interface"/> defining a provider to get entity mapping data.
    /// </summary>
    public interface IMappingProvider
    {
        /// <summary>
        /// Gets the <see cref="EntityMap"/> for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of the entity.</param>
        /// <returns>An <see cref="EntityMap"/> with the mapping data.</returns>
        EntityMap GetEntityMap(Type type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        EntityMap CreateEntityMap(Type type);
    }
}
