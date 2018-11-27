namespace CZJ.Events.Bus.Handlers
{
    /// <summary>
    /// 事件处理类的接口<see cref="TEventData"/>.
    /// </summary>
    /// <typeparam name="TEventData">事件处理数据</typeparam>
    public interface IEventHandler<in TEventData> : IEventHandler
    {
        /// <summary>
        /// 事件处理实现的方法
        /// </summary>
        /// <param name="eventData">事件数据</param>
        void HandleEvent(TEventData eventData);
    }
}
