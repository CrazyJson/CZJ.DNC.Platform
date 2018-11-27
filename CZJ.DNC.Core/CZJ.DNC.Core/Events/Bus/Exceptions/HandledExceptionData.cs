using System;

namespace CZJ.Events.Bus.Exceptions
{
    /// <summary>
    ///事件异常处理对象
    /// </summary>
    public class HandledExceptionData : ExceptionData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="exception">异常对象</param>
        public HandledExceptionData(Exception exception)
            : base(exception)
        {

        }
    }
}