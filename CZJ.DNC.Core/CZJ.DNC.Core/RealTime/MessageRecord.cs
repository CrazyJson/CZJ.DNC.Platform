using System;

namespace CZJ.RealTime
{
    /// <summary>
    /// 消息记录格式
    /// </summary>
    public class MessageRecord
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MessageRecord()
        {
            PushTime = DateTime.Now;
        }

        public DateTime PushTime { get; set; }
        public string MsgType { get; set; }
        public object Message { get; set; }
    }
}
