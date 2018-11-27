using CZJ.Dependency;

namespace CZJ.Common
{
    /// <summary>
    /// 默认token信息
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// 默认用户Token，使用admin账号生成
        /// </summary>
        string UserToken { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DefaultToken : IToken, ISingletonDependency
    {
        /// <summary>
        /// 
        /// </summary>
        public string UserToken { get; set; } = "F13CECD23F92526B9B9FECA81F973D32A56C6129168A058F04D977EBFBA55BF152DFDFF288D7C190D027896E6075451E";
    }
}
