using System;

namespace CZJ.Reflection
{
    /// <summary>
    /// 类型查找接口
    /// </summary>
    public interface ITypeFinder
    {
        /// <summary>
        /// 查找指定类型
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        /// <returns></returns>
        Type[] Find(Func<Type, bool> predicate);

        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <returns></returns>
        Type[] FindAll();
    }
}