using System.Runtime.InteropServices;

namespace CZJ.Common
{
    public static class Platform
    {
        public const string NET_FRAMEWORK = ".NET Framework";
        public const string NET_CORE = ".NET Core";

        public static bool IsFullFramework => RuntimeInformation.FrameworkDescription.StartsWith(NET_FRAMEWORK);

        public static bool IsNetCore => RuntimeInformation.FrameworkDescription.StartsWith(NET_CORE);

        /// <summary>
        /// 是否window系统
        /// </summary>
        public static bool IsWindow => RuntimeInformation.OSDescription.Contains("Windows");
    }
}
