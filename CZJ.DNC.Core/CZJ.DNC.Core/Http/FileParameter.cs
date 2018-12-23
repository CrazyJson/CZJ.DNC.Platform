using CZJ.DNC.Net;
using System.IO;

namespace System.Net.Http
{
    public struct FileParameter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="bytes">文件字节流</param>
        public FileParameter(byte[] bytes, string filename)
        {
            Bytes = bytes;
            FileName = filename;
            ContentType = MimeHelper.GetMineType(filename);
            Name = "file";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileStream">调用方会自动释放</param>
        /// <param name="filename">文件名</param>
        public FileParameter(Stream fileStream, string filename)
        {
            byte[] buffer = null;
            using (fileStream)
            {
                buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
            }
            Bytes = buffer;
            FileName = filename;
            ContentType = MimeHelper.GetMineType(filename);
            Name = "file";
        }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 文件字节流
        /// </summary>
        public byte[] Bytes;

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName;
        /// <summary>
        /// 文件类型
        /// </summary>
        public string ContentType;
    }
}
