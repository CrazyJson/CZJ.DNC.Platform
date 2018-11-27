using CZJ.Common.Core;
using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace CZJ.DNC.License
{
    /// <summary>
    /// 校验授权
    /// </summary>
    public class LicenseHelper
    {
        private static string path = FileHelper.GetAbsolutePath("license.lic");

        private static string NO_LICENSE_FILE = "没有检测到License授权文件。请使用机器码：{0}联系管理员获取授权文件！";

        private static string ERROR_LICENSE = "License信息错误。请使用机器码：{0}联系管理员重新获取授权文件！";

        private static string Machine_NO_MATCH = "授权文件的注册机器与本机不一致。请使用机器码：{0}联系管理员重新获取授权文件！";

        private static string SOFTWARE_TRIAL_EXPIRED = "软件试用已到期，请使用机器码：{0}联系管理员重新获取授权文件！";

        private static readonly CustomMD5 customMD5 = CustomMD5.MD5;

        /// <summary>
        /// 校验License是否合法
        /// </summary>
        /// <returns></returns>
        public unsafe static string Verify()
        {
            if (!File.Exists(path))
            {
                return string.Format(NO_LICENSE_FILE, RegisterClass.GetMachineCode());
            }
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                try
                {
                    string text = sr.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        return string.Format(ERROR_LICENSE, RegisterClass.GetMachineCode());
                    }

                    byte[] snBase64 = Convert.FromBase64String(text);
                    // 获取固化ID
                    string sid = RegisterClass.GetMachineCode();
                    // 计算固化ID的MD5
                    byte* pb = stackalloc byte[16];
                    fixed (char* ps = sid)
                    {
                        customMD5.ComputeMD5(ps, (ulong)(sid.Length << 1), pb);
                    }
                    // 前4位是随机数
                    int index = 0;
                    uint rand = BitHelper.ToUInt32(snBase64, ref index);
                    int x = snBase64[0] ^ snBase64[1] ^ snBase64[2] ^ snBase64[3];
                    for (int i = 4; i < snBase64.Length; i++)
                    {
                        snBase64[i] = (byte)(snBase64[i] ^ x);
                    }
                    uint sum = 47 ^ rand;
                    // 验证ID是否相等
                    for (int i = 0; i < 16; ++i)
                    {
                        byte b = pb[i];
                        if (snBase64[i + index] != b)
                        {
                            return string.Format(Machine_NO_MATCH, RegisterClass.GetMachineCode());
                        }
                        sum ^= b;
                    }
                    index += 16;
                    long ticks = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        byte b = snBase64[i + index];
                        ticks = (ticks << 8) | b;
                        sum ^= b;
                    }
                    index += 8;
                    int end = snBase64.Length - 4;
                    string additional = StringUtil.UTF8NoBOM.GetString(snBase64, index, end - index);
                    while (index < end)
                    {
                        sum ^= snBase64[index++];
                    }
                    if (BitHelper.ToUInt32(snBase64, ref index) == sum)
                    {
                        //存在有效期，校验试用是否结束
                        if (DateTime.Now > new DateTime(ticks))
                        {
                            return string.Format(SOFTWARE_TRIAL_EXPIRED, RegisterClass.GetMachineCode());
                        }
                    }
                }
                catch
                {
                    return string.Format(ERROR_LICENSE, RegisterClass.GetMachineCode());
                }
            }
            return string.Empty;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RegisterClass
    {
        private static readonly CustomMD5 md5 = new CustomMD5("ChenFei@Citms381");

        /// <summary>
        /// 获取cpu信息
        /// </summary>
        /// <returns></returns>
        private static string getCpu()
        {
            string strCpu = Environment.ProcessorCount.ToString();
            //if (Platform.IsWindow)
            //{
            //    ManagementClass myCpu = new ManagementClass("win32_Processor");
            //    ManagementObjectCollection myCpuConnection = myCpu.GetInstances();
            //    foreach (ManagementObject myObject in myCpuConnection)
            //    {
            //        strCpu = myObject.Properties["Processorid"].Value.ToString();
            //        break;
            //    }
            //}
            //else
            //{
            //    strCpu = Environment.ProcessorCount.ToString();
            //}
            return strCpu;
        }

        //取得设备硬盘的卷标号
        private static string GetDiskVolumeSerialNumber()
        {
            //if (Platform.IsWindow)
            //{
            //    try
            //    {
            //        string strDiskID = string.Empty;
            //        ManagementClass mc = new ManagementClass("Win32_DiskDrive");
            //        ManagementObjectCollection moc = mc.GetInstances();
            //        foreach (ManagementObject mo in moc)
            //        {
            //            strDiskID = mo.Properties["Model"].Value.ToString();
            //        }
            //        moc = null;
            //        mc = null;
            //        return strDiskID;
            //    }
            //    catch
            //    {
            //        return "unknown";
            //    }
            //}
            return "unknown";
        }

        /// <summary>
        /// 机器码
        /// </summary>
        private static string MachineCode = string.Empty;

        /// <summary>
        /// 收集硬件信息生成机器码,生成机器码
        /// </summary>
        /// <returns>机器码</returns>
        public static string GetMachineCode()
        {
            if (string.IsNullOrEmpty(MachineCode))
            {
                string temp = getCpu() + GetDiskVolumeSerialNumber() + GetMacAddress();//获得Cpu+硬盘序列号+mac地址
                string[] strid = new string[temp.Length];//
                for (int i = 0; i < temp.Length; i++)//把字符赋给数组
                {
                    strid[i] = temp.Substring(i, 1);
                }
                Array.Sort(strid);
                MachineCode = md5.GetMD5String(string.Join("", strid), true);
            }
            return MachineCode;
        }

        /// <summary>  
        /// 获取本机MAC地址  
        /// </summary>  
        /// <returns>本机MAC地址</returns>  
        public static string GetMacAddress()
        {
            var networks = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var network in networks)
            {
                if (network.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    var physicalAddress = network.GetPhysicalAddress();
                    return string.Join(":", physicalAddress.GetAddressBytes().Select(b => b.ToString("X2")));
                }
            }
            return "unknown";
            //try
            //{
            //    if (!Platform.IsWindow)
            //    {
            //        var networks = NetworkInterface.GetAllNetworkInterfaces();
            //        foreach (var network in networks)
            //        {
            //            if (network.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            //            {
            //                var physicalAddress = network.GetPhysicalAddress();
            //                return string.Join(":", physicalAddress.GetAddressBytes().Select(b => b.ToString("X2")));
            //            }
            //        }
            //    }
            //    string strMac = string.Empty;
            //    ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            //    ManagementObjectCollection moc = mc.GetInstances();
            //    foreach (ManagementObject mo in moc)
            //    {
            //        if ((bool)mo["IPEnabled"] == true)
            //        {
            //            strMac = mo["MacAddress"].ToString();
            //        }
            //    }
            //    moc = null;
            //    mc = null;
            //    return strMac;
            //}
            //catch
            //{
            //    return "unknown";
            //}
        }
    }

    /// <summary>
    /// 授权信息
    /// </summary>
    internal class LicenseInfo
    {
        /// <summary>
        /// 机器码，为空则不限制机器可多台部署
        /// </summary>
        public string MachineCode { get; set; }

        /// <summary>
        /// 注册码生成日期
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 试用期 为0则永不过期
        /// </summary>
        public int ProbationPeriod { get; set; }

        /// <summary>
        /// 额外混淆信息
        /// </summary>
        public string OtherInfo { get; set; }
    }
}
