namespace CZJ.Common.Serializer
{
    /// <summary>
    /// 对象序列化接口
    /// </summary>
    public interface IObjectSerializer 
    {
        /// <summary>
        /// 反序列序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        T Deserialize<T>(string value);

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string Serialize(object value);
    }
}