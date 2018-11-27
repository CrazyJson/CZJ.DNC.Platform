using System.IO;

namespace CZJ.IO.Extensions
{
    /// <summary>
    /// IO流拓展
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// 将IO流转换成Byte数组
        /// </summary>
        /// <param name="stream">IO流</param>
        /// <returns>Byte数组</returns>
        public static byte[] GetAllBytes(this Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
