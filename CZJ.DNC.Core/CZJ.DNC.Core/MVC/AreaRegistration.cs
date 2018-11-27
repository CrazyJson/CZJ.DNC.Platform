namespace CZJ.Common
{
    /// <summary>
    /// 区域注册
    /// </summary>
    public abstract class AreaRegistration
    {
        /// <summary>
        /// 区域名称，用于接口第二级名称，如果为默认会采用
        /// appsettings.json的AppNo节点值
        /// </summary>
        public abstract string AreaName { get; }
    }
}
