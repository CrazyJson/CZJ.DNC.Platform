using CZJ.Common;

namespace System.Net.Http
{
    /// <summary>
    /// Http请求异常对象
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// 异常码
        /// </summary>
        public ResultCode Code { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="code">状态码</param>
        public ApiException(Exception exception, ResultCode code) : base(exception.Message, exception)
        {
            Code = code;
        }
    }
}
