using System;
using CZJ.Dependency;

namespace CZJ.Events.Bus.Handlers.Internals
{
    /// <summary>
    /// 事件处理适配器允许使用Action作为处理源 <see cref="IEventHandler{TEventData}"/>
    /// </summary>
    /// <typeparam name="TEventData">事件类型</typeparam>
    internal class ActionEventHandler<TEventData> :
        IEventHandler<TEventData>,
        ITransientDependency
    {
        /// <summary>
        /// 处理事件的Action
        /// </summary>
        public Action<TEventData> Action { get; private set; }

        /// <summary>
        /// 创建一个新实例 <see cref="ActionEventHandler{TEventData}"/>.
        /// </summary>
        /// <param name="handler">处理事件的Action</param>
        public ActionEventHandler(Action<TEventData> handler)
        {
            Action = handler;
        }

        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="eventData"></param>
        public void HandleEvent(TEventData eventData)
        {
            Action(eventData);
        }
    }
}