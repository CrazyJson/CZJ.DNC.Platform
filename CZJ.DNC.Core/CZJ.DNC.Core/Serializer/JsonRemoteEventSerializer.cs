using CZJ.Dependency;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CZJ.Common.Serializer
{
    /// <summary>
    /// 基于Newtonsoft.Json的序列化实现
    /// </summary>
    public class JsonRemoteEventSerializer : IObjectSerializer, ISingletonDependency
    {
        private readonly JsonSerializerSettings settings;

        public JsonRemoteEventSerializer()
        {
            settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, settings);
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, settings);
        }
    }
}