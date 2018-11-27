using CZJ.Common.Core;
using System;

namespace CZJ.Common.Module.Common
{
    public class ConsoleConfigure : IAfterRunConfigure
    {
        public int Order
        {
            get
            {
                return 100;
            }
        }

        public void Configure()
        {
            Console.Title = $"{SysConfig.MicroServiceOption.Title}({SysConfig.MicroServiceOption.Name}) {SysConfig.MicroServiceOption.Version}";
        }
    }
}
