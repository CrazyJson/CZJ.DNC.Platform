namespace CZJ.Events.Bus
{
    /// <summary>
    ///  定义所有事件数据类的接口
    /// </summary>
    public interface IEventData
    {
        /// <summary>
        /// 事件发生时间
        /// </summary>
        string ReportedTime { get; set; }

        /// <summary>
        /// 事件源
        /// </summary>
        object EventSource { get; set; }
    }
}
