using System.Diagnostics;

namespace CZJ.Common.Core.Uri
{
    public static class UriExtend
    {
        /// <summary>
        /// 浏览器打开页面
        /// </summary>
        /// <param name="url">url地址</param>
        public static void OpenBrowserUrl(this string url)
        {
            if (!Platform.IsWindow)
            {
                return;
            }
            try
            {
                Process.Start(url);

                //RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
                //if (key != null)
                //{
                //    string s = key.GetValue("").ToString();
                //    var lastIndex = s.IndexOf(".exe", StringComparison.Ordinal);
                //    if (lastIndex == -1)
                //    {
                //        lastIndex = s.IndexOf(".EXE", StringComparison.Ordinal);
                //    }
                //    var path = s.Substring(1, lastIndex + 3);
                //    var result = Process.Start(path, url);
                //    if (result == null)
                //    {
                //        var result1 = Process.Start("explorer.exe", url);
                //        if (result1 == null)
                //        {
                //            Process.Start(url);
                //        }
                //    }
                //}
                //else
                //{
                //    var result1 = Process.Start("explorer.exe", url);
                //    if (result1 == null)
                //    {
                //        Process.Start(url);
                //    }
                //}
            }
            catch
            {
            }
        }

        ///// <summary>
        ///// 打开系统默认浏览器（用户自己设置了默认浏览器）
        ///// </summary>
        ///// <param name="url"></param>
        //private static void OpenDefaultBrowserUrl(string url)
        //{
        //    try
        //    {
        //        // 64位注册表路径
        //        var openKey = @"SOFTWARE\Wow6432Node\Google\Chrome";
        //        if (IntPtr.Size == 4)
        //        {
        //            // 32位注册表路径
        //            openKey = @"SOFTWARE\Google\Chrome";
        //        }
        //        RegistryKey appPath = Registry.LocalMachine.OpenSubKey(openKey);
        //        if (appPath != null)
        //        {
        //            var result = Process.Start("chrome.exe", url);
        //            if (result == null)
        //            {
        //                OpenDefaultBrowserUrl(url);
        //            }
        //        }
        //        else
        //        {
        //            var result = Process.Start("chrome.exe", url);
        //            if (result == null)
        //            {
        //                OpenDefaultBrowserUrl(url);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        // 出错调用用户默认设置的浏览器，还不行就调用IE
        //        OpenDefaultBrowserUrl(url);
        //    }
        //}
    }
}
