using CZJ.Common.Serializer;
using CZJ.Dependency;
using System;
using System.Data;

namespace CZJ.DNC.Excel
{
    /// <summary>
    ///默认接口获取数据处理器
    /// </summary>
    public class DefaultApiResultHandler : IApiResultHandler, ISingletonDependency
    {
        private readonly IObjectSerializer serializer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer">序列化对象</param>
        public DefaultApiResultHandler(IObjectSerializer serializer)
        {
            this.serializer = serializer;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; } = "default";

        /// <summary>
        /// 从请求响应体中获取数据转换成DataTable
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="info">导出信息</param>
        /// <returns></returns>
        public DataTable Parse(string text, ExcelInfo info)
        {
            var responseJson = serializer.Deserialize<ExcelApiResult>(text);
            if (responseJson.Code == 0)
            {
                return info.ConvertDataEx2Data(responseJson.Data);
            }
            else
            {
                throw new Exception(responseJson.Message);
            }
        }
    }
}
