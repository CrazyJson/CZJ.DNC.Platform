using System;

namespace CZJ.Events.Bus
{
    /// <summary>
    /// IEventData实现 
    /// <see cref="IEventData"/>
    /// </summary>
    [Serializable]
    public abstract class EventData : IEventData
    {
        /// <summary>
        /// 时间发生时间
        /// </summary>
        public string ReportedTime { get; set; }

        /// <summary>
        /// 事件源 (可选项).
        /// </summary>
        public object EventSource { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected EventData()
        {
            ReportedTime = DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}