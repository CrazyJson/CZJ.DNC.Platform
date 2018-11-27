using System;

namespace CZJ.Common.Mapping
{
    /// <summary>
    /// Static class to resolve Entity Framework mapping.
    /// </summary>
    public static class MappingResolver
    {
        static ReflectionMappingProvider provider = null;
        static MappingResolver()
        {
            provider = new ReflectionMappingProvider();
        }

        /// <summary>
        /// Gets an <see cref="EntityMap"/> for the entity type used in the specified <paramref name="query"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query used to create the <see cref="EntityMap"/> from.</param>
        /// <returns>An <see cref="EntityMap"/> for the specified <paramref name="query"/>.</returns>
        public static EntityMap GetEntityMap(this Type type)
        {
            return provider.GetEntityMap(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static EntityMap CreateEntityMap(Type type)
        {
            return provider.CreateEntityMap(type);
        }
    }
}
