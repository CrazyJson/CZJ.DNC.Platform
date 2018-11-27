using System.ComponentModel;

namespace CZJ.Common
{
    /// <summary>
    /// 接口调用返回码
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 0,

        /// <summary>
        /// Token错误
        /// </summary>
        [Description("Token错误")]
        TokenError = 5000,

        /// <summary>
        /// 系统错误
        /// </summary>
        [Description("系统错误")]
        SystemError = 10001,

        /// <summary>
        /// 服务端资源不可用
        /// </summary>
        [Description("服务端资源不可用")]
        Server_Resources_Unavailable = 10002,

        /// <summary>
        /// 远程服务出错
        /// </summary>
        [Description("远程服务出错")]
        Remote_Service_Error = 10003,

        /// <summary>
        /// 参数错误，请参考API文档
        /// </summary>
        [Description("参数错误，请参考API文档")]
        Parameter_Error = 10008,

        /// <summary>
        /// 非法请求
        /// </summary>
        [Description("非法请求")]
        Bad_Request = 10012,

        /// <summary>
        /// 请求的HTTP METHOD不支持
        /// </summary>
        [Description("请求的HTTP METHOD不支持")]
        HttpMethd_Nonsupport = 10021,

        /// <summary>
        /// 该接口已经废弃
        /// </summary>
        [Description("该接口已经废弃")]
        Api_Abandoned = 10026,

        /// <summary>
        /// 接口返回值格式异常
        /// </summary>
        [Description("接口返回值格式异常")]
        ApiResult_FomatterError = 10027,

        /// <summary>
        /// 接口未授权
        /// </summary>
        [Description("接口未授权")]
        Api_Unauthorized = 10028,

        /// <summary>
        /// 接口调用异常
        /// </summary>
        [Description("接口调用异常")]
        Api_CallError = 10029,

        /// <summary>
        /// 新增出错
        /// </summary>
        [Description("新增出错")]
        Inser_Error = 10030,

        /// <summary>
        /// 更新出错
        /// </summary>
        [Description("更新出错")]
        Update_Error = 10031,

        /// <summary>
        /// 删除出错
        /// </summary>
        [Description("删除出错")]
        Delete_Error = 10032,

        /// <summary>
        /// 查询出错
        /// </summary>
        [Description("查询出错")]
        Query_Error = 10033,

        /// <summary>
        /// 其它错误
        /// </summary>
        [Description("其它错误")]
        Other_Error = 11000,

        /// <summary>
        /// License授权错误
        /// </summary>
        [Description("License授权错误")]
        License_Error = 12000
    }

    public static class ResultCodeExtend
    {
        /// <summary>
        /// 是否成功请求
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsSuccessCode(this ResultCode code)
        {
            return code == ResultCode.Success;
        }
    }

    /// <summary>
    /// 定义调用 WebApi 返回结果的格式
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class ApiResult<T>
    {
        /// <summary>
        /// 错误码，不同的 api 接口自己定义，调用方需要根据具体接口的 来进行处理，后期会定义一系列标准错误码.
        /// 
        /// 通用错误码
        /// 错误码         错误信息
        /// 
        /// </summary>
        public ResultCode Code { get; set; }

        /// <summary>
        /// 接口返回码 说明
        /// </summary>
        public string CodeRemark
        {
            get
            {
                return EnumHelper.GetDescription(Code);
            }
        }

        private string _message;

        /// <summary>
        /// 执行返回消息
        /// </summary>
        /// <remarks></remarks>
        public string Msg
        {
            get
            {
                if (Code != ResultCode.Success && string.IsNullOrWhiteSpace(_message))
                {
                    return EnumHelper.GetDescription(Code);
                }
                else
                {
                    return _message;
                }
            }
            set
            {
                _message = value;
            }
        }

        /// <summary>
        /// 返回的主要内容.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 数据总条数
        /// </summary>
        public int? TotalCount { get; set; }

        public ApiResult()
        {
            Data = default(T);
            Code = ResultCode.Success;
        }
    }
}
