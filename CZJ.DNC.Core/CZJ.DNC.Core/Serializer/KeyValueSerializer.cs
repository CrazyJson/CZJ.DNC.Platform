using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace CZJ.Common.Serializer
{
    /// <summary>
    /// KeyValueSerializer
    /// </summary>
    public class KeyValueSerializer
    {
        private static JsonSerializer serializer;

        static KeyValueSerializer()
        {
            var setting = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            serializer = JsonSerializer.Create(setting);
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> Serialize(object value)
        {
            var keyValuesWriter = new KeyValuesWriter();
            serializer.Serialize(keyValuesWriter, value);
            return keyValuesWriter;
        }
    }
}