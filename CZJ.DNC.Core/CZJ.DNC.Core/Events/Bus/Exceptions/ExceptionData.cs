using System;

namespace CZJ.Events.Bus.Exceptions
{
    /// <summary>
    /// 事件通知异常数据
    /// </summary>
    public class ExceptionData : EventData
    {
        /// <summary>
        /// 异常对象
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="exception">异常对象</param>
        public ExceptionData(Exception exception)
        {
            Exception = exception;
        }
    }
}
