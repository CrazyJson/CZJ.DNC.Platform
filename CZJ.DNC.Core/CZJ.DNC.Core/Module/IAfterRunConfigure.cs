namespace CZJ.Common.Module
{
    /// <summary>
    /// 服务启动后注册的服务
    /// </summary>
    public interface IAfterRunConfigure
    {
        int Order { get; }

        void Configure();
    }
}
