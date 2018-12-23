using System.Text.RegularExpressions;
using TinyPinyin.Core;

namespace CZJ.Common
{
    /// <summary>
    /// 字符处理拓展类
    /// </summary>
    public static class StringUtil
    {

        /// <summary>
        /// 将单个单词首字母转换成大写
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>转换后的</returns>
        public static string ToTitleCase(this string s)
        {
            return s.Substring(0, 1).ToUpper() + s.Substring(1).ToLower();
        }

        /// <summary>
        /// 判断字符串中是否包含中文
        /// </summary>
        /// <param name="str">需要判断的字符串</param>
        /// <returns>判断结果</returns>
        public static bool HasChinese(this string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        /// <summary>
        /// 判断字符是否为中文
        /// </summary>
        /// <param name="c">需要判断的字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsChinese(this char c)
        {
            return PinyinHelper.IsChinese(c);
        }

        /// <summary> 
        /// 汉字转化为拼音
        /// </summary> 
        /// <param name="str">汉字</param> 
        /// <returns>全拼</returns> 
        public static string GetPinyin(this string str)
        {
            if (!str.HasChinese())
            {
                return str;
            }
            return PinyinHelper.GetPinyin(str, "");
        }

        /// <summary> 
        /// 汉字转化为拼音首字母
        /// </summary> 
        /// <param name="str">汉字</param> 
        /// <returns>首字母</returns> 
        public static string GetFirstPinyin(this string str)
        {
            if (!str.HasChinese())
            {
                return str;
            }
            return PinyinHelper.GetPinyinInitials(str);
        }

        /// <summary>
        /// 获取汉字对应的拼音首字母 和全拼组合中间空格隔开
        /// </summary>
        /// <example> 测试路口  cclk ceshilukou</example>
        /// <param name="str">汉字</param>
        /// <returns>拼音首字母 和全拼组合中间空格隔开</returns>
        /// <remarks>
        /// 2016-06-24 杜冬军修改计算全拼速度慢的问题，慢的原因由于字符串不包含中文或者中英文混合 这种情况速度很慢
        /// </remarks>
        public static string GetBopomofo(this string str)
        {
            return GetBopomofo(str, 0);
        }

        /// <summary>
        /// 获取汉字对应的拼音首字母 和全拼组合中间空格隔开
        /// </summary>
        /// <example> 测试路口  cclk ceshilukou</example>
        /// <param name="str">汉字</param>
        /// <param name="MaxLength">保留最大长度</param>
        /// <returns>拼音首字母 和全拼组合中间空格隔开</returns>
        /// <remarks>
        /// 2016-06-24 杜冬军修改计算全拼速度慢的问题，慢的原因由于字符串不包含中文或者中英文混合 这种情况速度很慢
        /// </remarks>
        public static string GetBopomofo(this string str, int MaxLength)
        {

            //参数为空 或者不包含中文则直接返回
            if (string.IsNullOrEmpty(str) || !str.HasChinese())
            {
                return str;
            }
            string py = PinyinHelper.GetPinyin(str, "") + " " + PinyinHelper.GetPinyinInitials(str);
            if (MaxLength > 0)
            {
                return py.Substring(0, py.Length < MaxLength ? py.Length : MaxLength);
            }
            else
            {
                return py;
            }
        }

        /// <summary>
        /// 判断字符串是否为中文
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>bool</returns>
        public static bool IsChinese(this string str)
        {
            return Regex.IsMatch(str, @"^[\u4e00-\u9fa5]+$");
        }
    }
}
