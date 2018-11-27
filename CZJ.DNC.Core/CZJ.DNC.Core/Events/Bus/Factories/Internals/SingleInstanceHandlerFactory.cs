using CZJ.Events.Bus.Handlers;

namespace CZJ.Events.Bus.Factories.Internals
{
    /// <summary>
    /// 单例对象事件处理工厂 This <see cref="IEventHandlerFactory"/>
    /// </summary>
    internal class SingleInstanceHandlerFactory : IEventHandlerFactory
    {
        /// <summary>
        /// 事件处理实例
        /// </summary>
        public IEventHandler HandlerInstance { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        public SingleInstanceHandlerFactory(IEventHandler handler)
        {
            HandlerInstance = handler;
        }

        public IEventHandler GetHandler()
        {
            return HandlerInstance;
        }

        public void ReleaseHandler(IEventHandler handler)
        {
            
        }
    }
}