using System.IO;

namespace CZJ.DNC.Excel
{
    /// <summary>
    ///  重写Npoi流方法,解决07会关闭流的问题
    /// </summary>
    public class NpoiMemoryStream : MemoryStream
    {
        /// <summary>
        /// 
        /// </summary>
        public NpoiMemoryStream()
        {
            AllowClose = true;
        }

        /// <summary>
        /// 是否允许流关闭
        /// </summary>
        public bool AllowClose { get; set; }

        /// <summary>
        /// 重写流关闭方法
        /// </summary>
        public override void Close()
        {
            if (AllowClose)
            {
                base.Close();
            }
        }
    }
}
