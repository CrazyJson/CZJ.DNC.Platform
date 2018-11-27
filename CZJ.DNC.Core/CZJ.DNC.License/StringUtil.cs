//******************************************************************************
//
// 文 件 名: StringUtil.cs
//
// 描    述: String 相关的工具类
//
// 作    者: 陈飞
//
// 创建时间: 2011-03-23
//
// 修改历史: 2011-03-23 陈飞创建
//******************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
//using Baiynui.Pool;

namespace CZJ.DNC.License
{
    /// <summary>
    /// 字符串工具类
    /// </summary>
    public static partial class StringUtil
    {
        /// <summary>
        /// 无 BOM 头的 UTF-8 编码
        /// </summary>
        public static readonly Encoding UTF8NoBOM = new UTF8Encoding(false, true);

        #region 删除的代码
        ///// <summary>
        ///// 单例对象池
        ///// </summary>
        //private static readonly IObjectPool _pool = new SingletonObjectPool();
        #endregion

        #region TryParse

        /// <summary>
        /// 将日期和时间的指定字符串表示形式转换为其等效的 System.DateTime。
        /// </summary>
        /// <param name="s">包含要转换的日期和时间的字符串。</param>
        /// <param name="time">
        /// 当此方法返回时，如果转换成功，则包含与 s 中包含的日期和时间等效的 System.DateTime 值；如果转换失败，则为 null。
        /// 如果 s 参数为 null，或者不包含日期和时间的有效字符串表示形式，则转换失败。该参数未经初始化即被传递。</param>
        /// <returns>如果 s 参数成功转换，则为 true；否则为 false。</returns>
        public static bool TryParse(string s, out DateTime? time)
        {
            // 用于保存结果的临时变量
            DateTime result;

            // 尝试进行转换
            if (DateTime.TryParse(s, out result))
            {
                // 转换成功
                time = result;
                return true;
            }

            // 转换失败
            time = null;
            return false;
        }

        /// <summary>
        /// 将数字的字符串表示形式转换为它的等效 32 位有符号整数。一个指示操作是否成功的返回值。
        /// </summary>
        /// <param name="s">包含要转换的数字的字符串。</param>
        /// <param name="n">
        /// 当此方法返回时，如果转换成功，则包含与 s 所包含的数字等效的 32 位有符号整数值；如果转换失败，则为 null。
        /// 如果 s 参数为 null，格式不正确，或者表示的数字小于 System.Int32.MinValue 或大于 System.Int32.MaxValue，则转换会失败。
        /// 该参数未经初始化即被传递。</param>
        /// <returns>如果 s 转换成功，则为 true；否则为 false。</returns>
        public static bool TryParse(string s, out int? n)
        {
            // 用于保存结果的临时变量
            int result;

            // 尝试进行转换
            if (int.TryParse(s, out result))
            {
                // 转换成功
                n = result;
                return true;
            }

            // 转换失败
            n = null;
            return false;
        }

        /// <summary>
        /// 将数字的字符串表示形式转换为它的等效 64 位有符号整数。一个指示操作是否成功的返回值。
        /// </summary>
        /// <param name="s">包含要转换的数字的字符串。</param>
        /// <param name="n">
        /// 当此方法返回时，如果转换成功，则包含与 s 所包含的数字等效的 64 位有符号整数值；如果转换失败，则为 null。
        /// 如果 s 参数为 null，格式不正确，或者表示的数字小于 System.Int64.MinValue 或大于 System.Int64.MaxValue，则转换会失败。
        /// 该参数未经初始化即被传递。</param>
        /// <returns>如果 s 转换成功，则为 true；否则为 false。</returns>
        public static bool TryParse(string s, out long? n)
        {
            // 用于保存结果的临时变量
            long result;

            // 尝试进行转换
            if (long.TryParse(s, out result))
            {
                // 转换成功
                n = result;
                return true;
            }

            // 转换失败
            n = null;
            return false;
        }

        /// <summary>
        /// 将数字的字符串表示形式转换为它的等效单精度浮点数字。一个指示转换是否成功的返回代码。
        /// </summary>
        /// <param name="s">表示要转换的数字的字符串。</param>
        /// <param name="n">
        /// 当此方法返回时，如果转换成功，则包含与 s 所包含的数值或符号等效的单精度浮点数字；如果转换失败，则为 null。
        /// 如果 s 参数为 null，不是有效格式的数字或者表示的数字小于 System.Single.MinValue 或大于 System.Single.MaxValue，
        /// 则转换将失败。该参数未经初始化即被传递。</param>
        /// <returns>如果 s 转换成功，则为 true；否则为 false。</returns>
        public static bool TryParse(string s, out float? n)
        {
            // 用于保存结果的临时变量
            float result;

            // 尝试进行转换
            if (float.TryParse(s, out result))
            {
                // 转换成功
                n = result;
                return true;
            }

            // 转换失败
            n = null;
            return false;
        }

        /// <summary>
        /// 将数字的字符串表示形式转换为它的等效双精度浮点数。一个指示转换是否成功的返回值。
        /// </summary>
        /// <param name="s">包含要转换的数字的字符串。</param>
        /// <param name="n">
        /// 当此方法返回时，如果转换成功，则包含与 s 参数等效的双精度浮点数；如果转换失败，则为 null。如果 s 参数为 null，
        /// 不是格式有效的数字或者表示的数字小于 System.Double.MinValue 或大于 System.Double.MaxValue，则转换将失败。
        /// 该参数未经初始化即被传递。</param>
        /// <returns>如果 s 转换成功，则为 true；否则为 false。</returns>
        public static bool TryParse(string s, out double? n)
        {
            // 用于保存结果的临时变量
            double result;

            // 尝试进行转换
            if (double.TryParse(s, out result))
            {
                // 转换成功
                n = result;
                return true;
            }

            // 转换失败
            n = null;
            return false;
        }

        /// <summary>
        /// 将数字的 System.String 表示形式转换为其等效的 System.Decimal 形式。一个指示转换是否成功的返回值。
        /// </summary>
        /// <param name="s">包含要转换的数字的 System.String 对象。</param>
        /// <param name="n">
        /// 当此方法返回时，如果转换成功，则返回值包含与 s 中包含的数值等效的 System.Decimal 数；如果转换失败，则返回值为 null。
        /// 如果 s 参数为 null，不是有效格式的数字或者表示的数字小于 System.Decimal.MinValue 或大于 System.Decimal.MaxValue，
        /// 则转换将失败。该参数未经初始化即被传递。</param>
        /// <returns>如果 s 转换成功，则为 true；否则为 false。</returns>
        public static bool TryParse(string s, out decimal? n)
        {
            // 用于保存结果的临时变量
            decimal result;

            // 尝试进行转换
            if (decimal.TryParse(s, out result))
            {
                // 转换成功
                n = result;
                return true;
            }

            // 转换失败
            n = null;
            return false;
        }

        ///// <summary>
        ///// 将一个值转换为指定类型(T) 的等效值。一个指示操作是否成功的返回值。
        ///// 注意： 此重载用到反射，会降低效率，请慎用！
        ///// </summary>
        ///// <typeparam name="T">一个包含有 TryParse 的类型</typeparam>
        ///// <param name="s">包含要转换的字符串。</param>
        ///// <param name="result">当此方法返回时，如果转换成功，则包含与 s 所包含的数字等效的 T 类型值；如果转换失败，则为 null。</param>
        ///// <returns>如果 s 转换成功，则为 true；否则为 false。</returns>
        //public static bool TryParse<T>(string s, out T? result) where T : struct
        //{
        //    Type type = typeof(T);

        //    MethodInfo method = _pool.GetObject(type) as MethodInfo;
        //    if (method == null)
        //    {
        //        method = type.GetMethod("TryParse", new Type[] { typeof(string), type.MakeByRefType() });
        //        if (method == null)
        //        {
        //            throw new ApplicationException("类型 " + type.Name + " 没有定义 TryParse(string, " + type.Name + ") 方法");
        //        }
        //        _pool.Create(type, method);
        //    }

        //    object[] parameters = { s, null };
        //    if ((bool)method.Invoke(null, parameters))
        //    {
        //        result = (T)parameters[1];
        //        return true;
        //    }

        //    result = null;
        //    return false;
        //}

        #endregion
    }

    // 
    public static partial class StringUtil
    {
        /// <summary>
        /// 从源字符串中找出目标字符串的个数（被查找到的子字符串部分不再参与查找）.
        /// </summary>
        /// <param name="src">源字符串</param>
        /// <param name="dest">目标字符串</param>
        /// <returns>目标字符串在源字符串中出现的次数</returns>
        public static int CountSubstring(string src, string dest)
        {
            if (src == null) throw new ArgumentNullException("src");
            if (dest == null) throw new ArgumentNullException("dest");
            if (dest.Length == 0) throw new ArgumentException("dest");

            int count = 0;
            int startIndex = 0;
            while (startIndex < src.Length)
            {
                int index = src.IndexOf(dest, startIndex);
                if (index == -1) return count;
                ++count;
                startIndex = index + dest.Length;
            }

            return count;
        }

        /// <summary>
        /// 从源字符串中找出目标字符串的个数（被查找到的子字符串部分仍然参与查找）.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="dest">The dest.</param>
        /// <returns></returns>
        public static int CountSubstring2(string src, string dest)
        {
            if (src == null) throw new ArgumentNullException("src");
            if (dest == null) throw new ArgumentNullException("dest");
            if (dest.Length == 0) throw new ArgumentException("dest");

            int count = 0;
            int startIndex = 0;
            while (startIndex < src.Length)
            {
                int index = src.IndexOf(dest, startIndex);
                if (index == -1) return count;
                ++count;
                startIndex = index + 1;
            }

            return count;
        }

        #region IsBlank
        /// <summary>
        /// 判断一个字符串是否是 null 或空白，如果是 null 或空白，返回 true, 否则返回 false
        /// </summary>
        /// <param name="str">要判断的目标字符串</param>
        /// <returns>如果是 null 或空白，返回 true, 否则返回 false</returns>
        public static bool IsBlank(string str)
        {
            if (null == str)
            {
                return true;
            }
            for (int i = 0; i < str.Length; ++i)
            {
                if (!char.IsWhiteSpace(str[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 判断对象是否是空白的，并对字符串进行特殊处理
        /// </summary>
        /// <param name="input"></param>
        /// <returns>当输入为 null, 或空白时，返回 true, 否则返回 false</returns>
        public static bool IsBlank(object input)
        {
            if (input == null)
            {
                return true;
            }
            string str = input.ToString();

            return IsBlank(str);
        }

        #endregion

        #region Fill

        /// <summary>
        /// Fills the specified input.
        /// </summary>
        /// <param name="input">输入字符串.</param>
        /// <param name="brush">画刷.</param>
        /// <param name="length">要填充到的长度.</param>
        /// <param name="fillRight">如果要填充在右侧，则为 true，否则为 false.</param>
        /// <returns></returns>
        public unsafe static string Fill(string input, string brush, int length, bool fillRight)
        {
            // 计算需要填充的长度
            int d = length - input.Length;

            if (d <= 0) return input;

            // 生成填充字符串
            //StringBuilder sb = new StringBuilder(width);
            //if (fillRight) sb.Append(input);
            //while (d > 0)
            //{
            //    if (d < brush.Length)
            //    {
            //        sb.Append(brush.Substring(0, d));
            //        break;
            //    }
            //    else
            //    {
            //        sb.Append(brush);
            //        d -= brush.Length;
            //    }
            //}
            //if (!fillRight) sb.Append(input);

            //return sb.ToString();

            char* pResult = stackalloc char[length + 1];
            int index = 0;
            if (fillRight && input != null && input.Length != 0)
                index += Copy(input, pResult + index, input.Length);
            while (d > 0)
            {
                int count = Copy(brush, pResult + index, d);
                d -= count;
                index += count;
            }
            if (!fillRight && input != null && input.Length != 0)
                index += Copy(input, pResult + index, input.Length);

            pResult[index] = '\0';
            return new string(pResult);
        }

        /// <summary>
        /// Copies the specified SRC.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="desc">The desc.</param>
        /// <param name="length">The width.</param>
        /// <returns></returns>
        private unsafe static int Copy(string src, char* desc, int length)
        {
            int n = System.Math.Min(length, src.Length);
            for (int i = 0; i < n; ++i)
            {
                desc[i] = src[i];
            }
            return n;
        }

        #endregion

        /// <summary>
        /// 获取子字符串，对于长度为零，或所选长度与原字符串相等的情况，不创建新字符串
        /// </summary>
        /// <param name="str">目标字符串</param>
        /// <param name="startIndex">开始下标</param>
        /// <param name="length">长度</param>
        /// <returns>子字符串</returns>
        public static string Substring(string str, int startIndex, int length)
        {
#if DEBUG
            if (str == null) throw new ArgumentNullException("str");
            if (startIndex < 0 || startIndex > str.Length) throw new ArgumentOutOfRangeException("startIndex");
            if (length < 0 || startIndex + length > str.Length) throw new ArgumentOutOfRangeException("length");
#endif

            if (length == 0) return string.Empty;
            if (startIndex == 0 && length == str.Length) return str;

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 获取子字符串，对于长度为零，或所选长度与原字符串相等的情况，不创建新字符串
        /// </summary>
        /// <param name="str">目标字符串</param>
        /// <param name="startIndex">开始下标</param>
        /// <returns>子字符串</returns>
        public static string Substring(string str, int startIndex)
        {
            return Substring(str, startIndex, str.Length - startIndex);
        }
    }

#if OLD_FORMAT
    // Format 2
    // 重新设计了一些东西
    public static partial class StringUtil
    {
        /// <summary>
        /// 验证输入是否是普通字符串（不包含 $）.
        /// </summary>
        /// <param name="format">要验证的字符串.</param>
        /// <returns>如果字符串为空，或不包含 $ 符号，则为 true，否则返回 false</returns>
        private static bool IsNormal(string format)
        {
            const char C_DOLLAR = '$';
            return (format == null || format.IndexOf(C_DOLLAR) == -1);
        }

        /// <summary>
        /// 参数格式化器
        /// </summary>
        /// <param name="format">格式化字符串，如 如 ${UserName}，$ 为特殊字符，如果需要输出 $，请使用 $$</param>
        /// <param name="target">目标对象.</param>
        /// <returns>格式化后的字符串</returns>
        public static string Format(string format, object target)
        {
#if DEBUG
            if (target == null) throw new ArgumentNullException("target");
#endif
            if (IsNormal(format)) return format;
            IFormattableMap formattableMap = new ObjectFormattableMap(target);
            return Format(format, formattableMap);
        }

        /// <summary>
        /// 参数格式化器
        /// </summary>
        /// <param name="format">格式化字符串，如 如 ${UserName}，$ 为特殊字符，如果需要输出 $，请使用 $$</param>
        /// <param name="target">目标对象.</param>
        /// <returns>格式化后的字符串</returns>
        public static string Format(string format, IDictionary target)
        {
#if DEBUG
            if (target == null) throw new ArgumentNullException("target");
#endif
            if (IsNormal(format)) return format;
            IFormattableMap formattableMap = new DictionaryFormattableMap(target);
            return Format(format, formattableMap);
        }

        /// <summary>
        /// 参数格式化器
        /// </summary>
        /// <param name="format">格式化字符串，如 如 ${UserName}，$ 为特殊字符，如果需要输出 $，请使用 $$</param>
        /// <param name="formattableMap">键值对映射.</param>
        /// <returns>格式化后的字符串</returns>
        public static string Format(string format, IFormattableMap formattableMap)
        {
#if DEBUG
            if (formattableMap == null) throw new ArgumentNullException("formattableMap");
#endif
            if (IsNormal(format)) return format;

            unsafe
            {
                fixed (char* pFormat = format)
                {
                    return Format(pFormat, format.Length, formattableMap);
                }
            }
        }

        /// <summary>
        /// 参数格式化器
        /// </summary>
        /// <param name="pFormat">指向格式化字符串的指针.</param>
        /// <param name="length">格式化字符串的长度.</param>
        /// <param name="formattableMap">键值对映射.</param>
        /// <returns>格式化后的字符串</returns>
        public static unsafe string Format(char* pFormat, int length, IFormattableMap formattableMap)
        {
    #region 常量定义
            // 特殊字符的定义
            const char C_MY = '$';
            const char C_ZKH = '{';
            const char C_YKH = '}';

            // 解析状态的定义
            const int STATE_NORMAL = 0x00000000;
            const int STATE_HAS_MY = 0x00000001;
            const int STATE_HAS_ZKH = 0x00000002;
    #endregion

            // index: 用于遍历字符串的下标，fix 用于记录开始的下标
            int index = 0, fix = 0;

            // 用于解析字符串的状态变量
            int state = STATE_NORMAL;

            char ch;

            // 用于保存结果的可变字符串
            StringBuilder sb = new StringBuilder();

            // 遍历字符串中的字符
            while (index < length)
            {
                // 获取当前要处理的字符
                ch = pFormat[index];
                switch (state)
                {
                    case STATE_NORMAL:
                        // 正常状态
                        if (C_MY == ch) { state = STATE_HAS_MY; fix = index; }  // 进入转义状态
                        else sb.Append(ch);                         // 保存当前字符
                        break;
                    case STATE_HAS_MY:
                        // 转义状态
                        if (C_ZKH == ch) { state = STATE_HAS_ZKH; } // 进入解析键状态
                        else if (C_MY == ch) { state = STATE_NORMAL; sb.Append(ch); }   // 输出转义字符 $
                        else goto __error;                                              // 出错
                        break;
                    case STATE_HAS_ZKH:
                        // 解析键状态
                        if (C_YKH == ch)
                        {
                            // 解析键完毕，进入正常状态
                            state = STATE_NORMAL;
                            sb.Append(formattableMap.Map(pFormat + fix + 2, index - fix - 2));
                        }
                        break;
                }
                ++index;
            }

            if (state == STATE_NORMAL) return sb.ToString();
            __error:
            throw new NormalException("格式字符串未正确关闭: " + new string(pFormat, fix, length - fix));
        }

        /// <summary>
        /// 参数格式化器
        /// </summary>
        /// <param name="format">格式化字符串，如 如 ${UserName}，$ 为特殊字符，如果需要输出 $，请使用 $$</param>
        /// <param name="formattableMap">键值对映射.</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatFast(string format, IFormattableMap formattableMap)
        {
#if DEBUG
            if (formattableMap == null) throw new ArgumentNullException("formattableMap");
#endif
            if (IsNormal(format)) return format;

            unsafe
            {
                fixed (char* pFormat = format)
                {
                    return FormatFast(pFormat, format.Length, formattableMap);
                }
            }
        }

        /// <summary>
        /// 参数格式化器
        /// </summary>
        /// <param name="pFormat">指向格式化字符串的指针.</param>
        /// <param name="length">格式化字符串的长度.</param>
        /// <param name="formattableMap">键值对映射.</param>
        /// <returns>格式化后的字符串</returns>
        public static unsafe string FormatFast(char* pFormat, int length, IFormattableMap formattableMap)
        {
    #region 常量定义
            // 特殊字符的定义
            const char C_MY = '$';
            const char C_ZKH = '{';
            const char C_YKH = '}';

            // 解析状态的定义
            const int STATE_NORMAL = 0x00000000;
            const int STATE_HAS_MY = 0x00000001;
            const int STATE_HAS_ZKH = 0x00000002;

            // 每次分配字符串的个数
            const int ALLOC_SIZE = 8;
    #endregion

            // index: 用于遍历字符串的下标，fix 用于记录开始的下标
            int index = 0, fix = 0;

            // 用于解析字符串的状态变量
            int state = STATE_NORMAL;

            char ch;

            // 保存结果相关的变量
            int resultLength = 0, resultIndex = 0;
            char* pResult = null;

            // 遍历字符串中的字符
            while (index < length)
            {
                // 获取当前要处理的字符
                ch = pFormat[index];
                switch (state)
                {
                    case STATE_NORMAL:
                        // 正常状态
                        if (C_MY == ch) { state = STATE_HAS_MY; fix = index; }  // 进入转义状态
                        else goto __append;                         // 保存当前字符
                        break;
                    case STATE_HAS_MY:
                        // 转义状态
                        if (C_ZKH == ch) { state = STATE_HAS_ZKH; } // 进入解析键状态
                        else if (C_MY == ch) { state = STATE_NORMAL; goto __append; }   // 输出转义字符 $
                        else goto __error;                                              // 出错
                        break;
                    case STATE_HAS_ZKH:
                        // 解析键状态
                        if (C_YKH == ch)
                        {
                            // 解析键完毕，进入正常状态
                            state = STATE_NORMAL;
                            string value = formattableMap.Map(pFormat + fix + 2, index - fix - 2);
                            //sb.Append(value);
                            if (value != null)
                                fixed (char* pt = value)
                                {
                                    for (int i = 0; i != value.Length; ++i)
                                    {
                                        if (resultIndex == 0)
                                        {
                                            char* p = stackalloc char[ALLOC_SIZE];
                                            pResult = p;
                                            resultIndex = ALLOC_SIZE;
                                        }
                                        pResult[--resultIndex] = pt[i];
                                        ++resultLength;
                                    }
                                }
                        }
                        break;
                }
                ++index;
                continue;

                // 将当前字符保存到结果中
                __append:
                if (resultIndex == 0)
                {
                    char* p = stackalloc char[ALLOC_SIZE];
                    pResult = p;
                    resultIndex = ALLOC_SIZE;
                }
                pResult[--resultIndex] = ch;
                ++resultLength;
                ++index;
            }

            if (state == STATE_NORMAL)
            {
                if (resultLength == 0) return string.Empty;

                // 输出结果
                pResult += resultIndex;
                for (int l = 0, r = resultLength - 1; l < r; ++l, --r)
                {
                    ch = pResult[l];
                    pResult[l] = pResult[r];
                    pResult[r] = ch;
                }
                return new string(pResult, 0, resultLength);
            }
            __error:
            throw new NormalException("格式字符串未正确关闭: " + new string(pFormat, fix, length - fix));
        }
    }
#endif

#if OLD_FORMAT
    // Format 1
    // 为提高格式化效率，不使用正则表达式替换的方法，改为自行分析格式化字符串
    // 经过测试，改进后的代码格式化效率有效大提高
    public static partial class StringUtil
    {
    #region Format

        /// <summary>
        /// 创建字符串.
        /// </summary>
        /// <param name="pc">The pc.</param>
        /// <param name="index">The index.</param>
        /// <param name="fcix">The fcix.</param>
        /// <param name="result">The result.</param>
        private static unsafe void NewString(char* pc, int index, ref int fcix, out string result)
        {
            int len = index - fcix - 1;

            if (len > 0) result = new string(pc, fcix + 1, len);
            else result = null;

            fcix = index;
        }

        ///// <summary>
        ///// 验证输入是否是普通字符串（不包含 $）.
        ///// </summary>
        ///// <param name="format">要验证的字符串.</param>
        ///// <returns>如果字符串为空，或不包含 $ 符号，则为 true，否则返回 false</returns>
        //private static bool IsNormal(string format)
        //{
        //    const char C_DOLLAR = '$';
        //    return (format == null || format.IndexOf(C_DOLLAR) == -1);
        //}

        /// <summary>
        /// 参数格式化器，此方法与 Format 方法相比，没有采用 StringBuilder 来构造字符串，而是采用在栈上分配内存的方法，以提高速度
        /// </summary>
        /// <param name="format">格式化字符串，如 如 ${Time,-10:yyyyMMddHHmmssfff}，其中 -10 为对齐</param>
        /// <param name="keyValueMap">键值对映射.</param>
        /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        /// <returns>
        /// 格式化后的字符串
        /// </returns>
        [Obsolete("此方法已经过时，请使用只有两个参数（IFormattableMap）的重载")]
        public static string FormatFast(string format, IKeyValueMap keyValueMap, bool ignore)
        {
            if (IsNormal(format)) return format;

            // 由于格式化方法中定义了大量的局部变量，故将格式化的实现单独放在一个方法中
            // 以避免 format 中不存在格式化字符 $ 时不必要的性能浪费
            // （如果具体实现放在一个方法体中，即使不执行实现代码也会在栈上分配内存）
            return InternalFormatFast(format, keyValueMap, ignore);
        }

        /// <summary>
        /// 参数格式化器，此方法与 Format 方法相比，没有采用 StringBuilder 来构造字符串，而是采用在栈上分配内存的方法，以提高速度
        /// </summary>
        /// <param name="format">格式化字符串，如 如 ${Time,-10:yyyyMMddHHmmssfff}，其中 -10 为对齐</param>
        /// <param name="keyValueMap">键值对映射.</param>
        /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        /// <returns>
        /// 格式化后的字符串
        /// </returns>
        internal static unsafe string InternalFormatFast(string format, IKeyValueMap keyValueMap, bool ignore)
        {
#if DEBUG
            if (keyValueMap == null) throw new ArgumentNullException("keyValueMap");
#endif
    #region 常量定义
            // 格式化字符串中用到的字符常量，如 ${Time,-10:yyyyMMddHHmmssfff}，其中 -10 为对齐
            const char C_DOLLAR = '$';
            const char C_LEFTBRACE = '{';
            const char C_COMMA = ',';
            const char C_COLON = ':';
            const char C_RIGHTBRACE = '}';
            const char C_SPACE = ' ';
            //const char C_BARS = '-';
    #endregion

    #region 变量定义
            // 格式化字符串长度
            int length = format.Length;

            // 解析格式化字符串
            int index = 0;

            // 标识当前处理什么处理状态 state 取值
            // 0: 初始
            // 1: 寻找 {
            // 2: 寻找 ,
            // 3: 寻找 :
            // 4: 寻找 }
            // 5: 已发现 }，不用处理此状态，因为发现 } 后， state 立即置为初始状态 0
            int state = 0;

            // 用于标识当前是处理属性格式字符串的哪一部分，part 取值
            // 0: propKey
            // 1: propAlign
            // 2: propFormat
            int part = 0;

            // 属性键名称，属性对齐，属性格式化字符串
            string propKey = null, propAlign = null, propFormat = null, temp;

            // 第一个字符下标，第二个字符下标
            int fcix = 0/*, scix = 0*/, ix = 0;

            // 保存长度的临时变量
            int len;

            // 用于保存结果的 StringBuilder
            //StringBuilder sb = new StringBuilder();
            const int SIZE = 8;
            int remain = 0, resultLength = 0;
            char* pResult = null;
    #endregion

            fixed (char* pc = format)
            {
                while (index < length)
                {
                    char ch = pc[index];
                    switch (state)
                    {
                        case 0: // 0: 初始
    #region
                            if (C_DOLLAR/* $ */ == ch)
                            {
                                state = 1;
                                ix = index;
                            }
                            else goto __append;
                            break;
    #endregion
                        case 1: // 1: 寻找 {
    #region
                            switch (ch)
                            {
                                case C_DOLLAR/* $ */:
                                    state = 0;
                                    goto __append;
                                case C_LEFTBRACE/* { */:
                                    state = 2;
                                    fcix = index;
                                    part = 0;
                                    break;
                            }
                            break;
    #endregion
                        case 2: // 2: 寻找 ,
    #region
                            switch (ch)
                            {
                                case C_COMMA/* , */:
                                    state = 3;

                                    // 创建字符串
                                    NewString(pc, index, ref fcix, out propKey);

                                    part = 1;
                                    break;

                                case C_COLON/* : */:
                                case C_RIGHTBRACE/* } */:
                                    state = 3;
                                    continue;
                            }
                            break;
    #endregion
                        case 3: // 3: 寻找 :
    #region
                            switch (ch)
                            {
                                case C_COLON/* : */:
                                    state = 4;

                                    // 创建字符串
                                    NewString(pc, index, ref fcix, out temp);

                                    if (part == 0) propKey = temp;
                                    else propAlign = temp;

                                    part = 2;
                                    break;

                                case C_RIGHTBRACE/* } */:
                                    state = 4;
                                    continue;
                            }
                            break;
    #endregion
                        case 4: // 4: 寻找 }
    #region
                            if (C_RIGHTBRACE == ch)
                            {
                                state = 0;

                                // 创建字符串
                                NewString(pc, index, ref fcix, out temp);

                                switch (part)
                                {
                                    case 0:
                                        propKey = temp;
                                        break;
                                    case 1:
                                        propAlign = temp;
                                        break;
                                    case 2:
                                        propFormat = temp;
                                        break;
                                }

                                // 获取属性值的字符串表示
                                if (null == (propKey))
                                {
                                    throw new NormalException("属性格式化时遇到问题: 属性名不能为空");
                                }

    #region 追加属性
                                object obj;
                                if (keyValueMap.TryGetValue(propKey, out obj))
                                {
                                    temp = null;
                                    // 格式化
                                    if (obj != null)
                                    {
                                        IFormattable formattable;
                                        if (null == (propFormat) || ((formattable = obj as IFormattable) == null))
                                        {
                                            temp = (obj.ToString());
                                        }
                                        else
                                        {
                                            temp = (formattable.ToString(propFormat, null));
                                        }
                                    }

                                    // 对齐
                                    if (null == (propAlign))
                                    {
                                        ////if (temp != null) sb.Append(temp);
                                        if (temp != null)
                                            fixed (char* pt = temp)
                                            {
                                                for (int i = 0; i < temp.Length; ++i)
                                                {
                                                    if (remain == 0)
                                                    {
                                                        char* pNewChars = stackalloc char[SIZE];
                                                        pResult = pNewChars + SIZE;
                                                        remain = SIZE;
                                                    }
                                                    *(--pResult) = pt[i];
                                                    --remain; ++resultLength;
                                                }
                                            }
                                    }
                                    else
                                    {
                                        len = (temp == null ? 0 : temp.Length);
                                        int align;
                                        if (int.TryParse(propAlign, out align))
                                        {
                                            if (align < 0)
                                            {
                                                ////if (len > 0) sb.Append(temp);
                                                fixed (char* pt = temp)
                                                {
                                                    for (int i = 0; i < temp.Length; ++i)
                                                    {
                                                        if (remain == 0)
                                                        {
                                                            char* pNewChars = stackalloc char[SIZE];
                                                            pResult = pNewChars + SIZE;
                                                            remain = SIZE;
                                                        }
                                                        *(--pResult) = pt[i];
                                                        --remain; ++resultLength;
                                                    }
                                                }

                                                align = -align - len;
                                                while (align-- > 0)
                                                {
                                                    ////sb.Append(C_SPACE);
                                                    if (remain == 0)
                                                    {
                                                        char* pNewChars = stackalloc char[SIZE];
                                                        pResult = pNewChars + SIZE;
                                                        remain = SIZE;
                                                    }
                                                    *(--pResult) = C_SPACE;
                                                    --remain; ++resultLength;
                                                }
                                            }
                                            else
                                            {
                                                align = align - len;
                                                while (align-- > 0)
                                                {
                                                    ////sb.Append(C_SPACE);
                                                    if (remain == 0)
                                                    {
                                                        char* pNewChars = stackalloc char[SIZE];
                                                        pResult = pNewChars + SIZE;
                                                        remain = SIZE;
                                                    }
                                                    *(--pResult) = C_SPACE;
                                                    --remain; ++resultLength;
                                                }
                                                ////if (len > 0) sb.Append(temp);
                                                fixed (char* pt = temp)
                                                {
                                                    for (int i = 0; i < temp.Length; ++i)
                                                    {
                                                        if (remain == 0)
                                                        {
                                                            char* pNewChars = stackalloc char[SIZE];
                                                            pResult = pNewChars + SIZE;
                                                            remain = SIZE;
                                                        }
                                                        *(--pResult) = pt[i];
                                                        --remain; ++resultLength;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            throw new NormalException("格式化对齐字符串不正确: " + propAlign);
                                        }
                                    }
                                }
                                else
                                {
                                    if (ignore)
                                    {
                                        while (ix <= index)
                                        {
                                            ////sb.Append(pc[ix++]);
                                            if (remain == 0)
                                            {
                                                char* pNewChars = stackalloc char[SIZE];
                                                pResult = pNewChars + SIZE;
                                                remain = SIZE;
                                            }
                                            *(--pResult) = pc[ix++];
                                            --remain; ++resultLength;
                                        }
                                    }
                                    else
                                    {
                                        throw new NormalException("找不到属性: " + propKey);
                                    }
                                }
    #endregion

                                // 重置参数
                                propKey = null; propAlign = null; propFormat = null;
                            }
                            break;
    #endregion
                    }
                    ++index;
                    continue;
                    __append:
                    if (remain == 0)
                    {
                        char* pNewChars = stackalloc char[SIZE];
                        pResult = pNewChars + SIZE;
                        remain = SIZE;
                    }
                    *(--pResult) = ch;
                    --remain; ++resultLength;

                    ++index;
                }
            }
            if (state != 0)
            {
                throw new NormalException("格式化字符串未正确关闭，位置: " + format.Substring(ix));
            }

            if (resultLength == 0) return string.Empty;
            for (int l = 0, r = resultLength - 1; l < r; ++l, --r)
            {
                char tmp = pResult[l];
                pResult[l] = pResult[r];
                pResult[r] = tmp;
            }
            return new string(pResult, 0, resultLength);
        }

        /// <summary>
        /// 参数格式化器
        /// </summary>
        /// <param name="format">格式化字符串，如 如 ${Time,-10:yyyyMMddHHmmssfff}，其中 -10 为对齐</param>
        /// <param name="keyValueMap">键值对映射.</param>
        /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        /// <returns>
        /// 格式化后的字符串
        /// </returns>
        [Obsolete("此方法已经过时，请使用只有两个参数（IFormattableMap）的重载")]
        public static string Format(string format, IKeyValueMap keyValueMap, bool ignore)
        {
            if (IsNormal(format)) return format;

            // 由于格式化方法中定义了大量的局部变量，故将格式化的实现单独放在一个方法中
            // 以避免 format 中不存在格式化字符 $ 时不必要的性能浪费
            // （如果具体实现放在一个方法体中，即使不执行实现代码也会在栈上分配内存）
            return InternalFormat(format, keyValueMap, ignore);
        }

        /// <summary>
        /// 参数格式化器
        /// </summary>
        /// <param name="format">格式化字符串，如 如 ${Time,-10:yyyyMMddHHmmssfff}，其中 -10 为对齐</param>
        /// <param name="keyValueMap">键值对映射.</param>
        /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        /// <returns>
        /// 格式化后的字符串
        /// </returns>
        internal static unsafe string InternalFormat(string format, IKeyValueMap keyValueMap, bool ignore)
        {
#if DEBUG
            if (keyValueMap == null) throw new ArgumentNullException("keyValueMap");
#endif
    #region 常量定义
            // 格式化字符串中用到的字符常量，如 ${Time,-10:yyyyMMddHHmmssfff}，其中 -10 为对齐
            const char C_DOLLAR = '$';
            const char C_LEFTBRACE = '{';
            const char C_COMMA = ',';
            const char C_COLON = ':';
            const char C_RIGHTBRACE = '}';
            const char C_SPACE = ' ';
            //const char C_BARS = '-';
    #endregion

    #region 变量定义
            // 格式化字符串长度
            int length = format.Length;

            // 解析格式化字符串
            int index = 0;

            // 标识当前处理什么处理状态 state 取值
            // 0: 初始
            // 1: 寻找 {
            // 2: 寻找 ,
            // 3: 寻找 :
            // 4: 寻找 }
            // 5: 已发现 }，不用处理此状态，因为发现 } 后， state 立即置为初始状态 0
            int state = 0;

            // 用于标识当前是处理属性格式字符串的哪一部分，part 取值
            // 0: propKey
            // 1: propAlign
            // 2: propFormat
            int part = 0;

            // 属性键名称，属性对齐，属性格式化字符串
            string propKey = null, propAlign = null, propFormat = null, temp;

            // 第一个字符下标，第二个字符下标
            int fcix = 0/*, scix = 0*/, ix = 0;

            // 保存长度的临时变量
            int len;

            // 用于保存结果的 StringBuilder
            StringBuilder sb = new StringBuilder();
    #endregion

            fixed (char* pc = format)
            {
                while (index < length)
                {
                    char ch = pc[index];
                    switch (state)
                    {
                        case 0: // 0: 初始
    #region
                            if (C_DOLLAR/* $ */ == ch)
                            {
                                state = 1;
                                ix = index;
                            }
                            else sb.Append(ch);
                            break;
    #endregion
                        case 1: // 1: 寻找 {
    #region
                            switch (ch)
                            {
                                case C_DOLLAR/* $ */:
                                    state = 0;
                                    sb.Append(ch);
                                    break;
                                case C_LEFTBRACE/* { */:
                                    state = 2;
                                    fcix = index;
                                    part = 0;
                                    break;
                            }
                            break;
    #endregion
                        case 2: // 2: 寻找 ,
    #region
                            switch (ch)
                            {
                                case C_COMMA/* , */:
                                    state = 3;

                                    // 创建字符串
                                    NewString(pc, index, ref fcix, out propKey);

                                    part = 1;
                                    break;

                                case C_COLON/* : */:
                                case C_RIGHTBRACE/* } */:
                                    state = 3;
                                    continue;
                            }
                            break;
    #endregion
                        case 3: // 3: 寻找 :
    #region
                            switch (ch)
                            {
                                case C_COLON/* : */:
                                    state = 4;

                                    // 创建字符串
                                    NewString(pc, index, ref fcix, out temp);

                                    if (part == 0) propKey = temp;
                                    else propAlign = temp;

                                    part = 2;
                                    break;

                                case C_RIGHTBRACE/* } */:
                                    state = 4;
                                    continue;
                            }
                            break;
    #endregion
                        case 4: // 4: 寻找 }
    #region
                            if (C_RIGHTBRACE == ch)
                            {
                                state = 0;

                                // 创建字符串
                                NewString(pc, index, ref fcix, out temp);

                                switch (part)
                                {
                                    case 0:
                                        propKey = temp;
                                        break;
                                    case 1:
                                        propAlign = temp;
                                        break;
                                    case 2:
                                        propFormat = temp;
                                        break;
                                }

                                // 获取属性值的字符串表示
                                if (null == (propKey))
                                {
                                    throw new NormalException("属性格式化时遇到问题: 属性名不能为空");
                                }

    #region 追加属性
                                object obj;
                                if (keyValueMap.TryGetValue(propKey, out obj))
                                {
                                    temp = null;
                                    // 格式化
                                    if (obj != null)
                                    {
                                        IFormattable formattable;
                                        if (null == (propFormat) || ((formattable = obj as IFormattable) == null))
                                        {
                                            temp = (obj.ToString());
                                        }
                                        else
                                        {
                                            temp = (formattable.ToString(propFormat, null));
                                        }
                                    }

                                    // 对齐
                                    if (null == (propAlign))
                                    {
                                        if (temp != null) sb.Append(temp);
                                    }
                                    else
                                    {
                                        len = (temp == null ? 0 : temp.Length);
                                        int align;
                                        if (int.TryParse(propAlign, out align))
                                        {
                                            if (align < 0)
                                            {
                                                if (len > 0) sb.Append(temp);
                                                align = -align - len;
                                                while (align-- > 0)
                                                {
                                                    sb.Append(C_SPACE);
                                                }
                                            }
                                            else
                                            {
                                                align = align - len;
                                                while (align-- > 0)
                                                {
                                                    sb.Append(C_SPACE);
                                                }
                                                if (len > 0) sb.Append(temp);
                                            }
                                        }
                                        else
                                        {
                                            throw new NormalException("格式化对齐字符串不正确: " + propAlign);
                                        }
                                    }
                                }
                                else
                                {
                                    if (ignore)
                                    {
                                        while (ix <= index)
                                        {
                                            sb.Append(pc[ix++]);
                                        }
                                    }
                                    else
                                    {
                                        throw new NormalException("找不到属性: " + propKey);
                                    }
                                }
    #endregion

                                // 重置参数
                                propKey = null; propAlign = null; propFormat = null;
                            }
                            break;
    #endregion
                    }
                    ++index;
                }
            }
            if (state != 0)
            {
                throw new NormalException("格式化字符串未正确关闭，位置: " + format.Substring(ix));
            }

            return sb.ToString();

    #region 删除的代码
            //// 格式化字符串中用到的字符常量，如 ${Time,-10:yyyyMMddHHmmssfff}，其中 -10 为对齐，对齐为保留功能，暂时不支持
            //const char C_DOLLAR = '$';
            //const char C_LEFTBRACE = '{';
            //const char C_COMMA = ',';
            //const char C_COLON = ':';
            //const char C_RIGHTBRACE = '}';

            //if (format == null || format.IndexOf(C_DOLLAR) == -1)
            //{
            //    return format;
            //}

            //// 格式化字符串长度
            //int width = format.Length;

            //// 解析格式化字符串
            //int index = 0;

            //// 标识当前处理什么处理状态 state 取值
            //// 0: 初始
            //// 1: 寻找 {
            //// 2: 寻找 ,
            //// 3: 寻找 :
            //// 4: 寻找 }
            //// 5: 已发现 }，不用处理此状态，因为发现 } 后， state 立即置为初始状态 0
            //int state = 0;

            //// 用于标识当前是处理属性格式字符串的哪一部分，part 取值
            //// 0: propKey
            //// 1: propAlign
            //// 2: propFormat
            //int part = 0;

            //// 属性键名称，属性对齐，属性格式化字符串
            //string propKey = null, propAlign = null, propFormat = null, temp;

            //// 第一个字符下标，第二个字符下标
            //int fcix = 0/*, scix = 0*/, ix = 0;

            //// 保存长度的临时变量
            //int len;

            //// 用于保存结果的 StringBuilder
            //StringBuilder sb = new StringBuilder();
            //fixed (char* pc = format)
            //{
            //    while (index < width)
            //    {
            //        char ch = pc[index];
            //        switch (state)
            //        {
            //            case 0:
            //                // 0: 初始
            //                if (C_DOLLAR/* $ */ == ch)
            //                {
            //                    state = 1;
            //                    ix = index;
            //                }
            //                else sb.Append(ch);
            //                break;
            //            case 1:
            //                // 1: 寻找 {
            //                switch (ch)
            //                {
            //                    case C_DOLLAR/* $ */:
            //                        state = 0;
            //                        sb.Append(ch);
            //                        break;
            //                    case C_LEFTBRACE/* { */:
            //                        state = 2;
            //                        fcix = index;
            //                        part = 0;
            //                        break;
            //                }
            //                break;
            //            case 2:
            //                // 2: 寻找 ,
            //                switch (ch)
            //                {
            //                    case C_COMMA/* , */:
            //                        state = 3;

            //                        len = index - fcix - 1;
            //                        propKey = new string(pc, fcix + 1, len);
            //                        fcix = index;
            //                        part = 1;
            //                        break;

            //                    case C_COLON/* : */:
            //                    case C_RIGHTBRACE/* } */:
            //                        state = 3;
            //                        continue;
            //                }
            //                break;
            //            case 3:
            //                // 3: 寻找 :
            //                switch (ch)
            //                {
            //                    case C_COLON/* : */:
            //                        state = 4;

            //                        len = index - fcix - 1;
            //                        temp = new string(pc, fcix + 1, len);
            //                        fcix = index;

            //                        if (part == 0) propKey = temp;
            //                        else propAlign = temp;

            //                        part = 2;
            //                        break;

            //                    case C_RIGHTBRACE/* } */:
            //                        state = 4;
            //                        continue;
            //                }
            //                break;
            //            case 4:
            //                // 4: 寻找 }
            //                if (C_RIGHTBRACE == ch)
            //                {
            //                    state = 0;

            //                    len = index - fcix - 1;
            //                    temp = new string(pc, fcix + 1, len);
            //                    fcix = index;

            //                    switch (part)
            //                    {
            //                        case 0:
            //                            propKey = temp;
            //                            break;
            //                        case 1:
            //                            propAlign = temp;
            //                            break;
            //                        case 2:
            //                            propFormat = temp;
            //                            break;
            //                    }

            //                    // 获取属性值的字符串表示
            //                    if (string.IsNullOrEmpty(propKey))
            //                    {
            //                        throw new NormalException("属性格式化时遇到问题: 属性名不能为空");
            //                    }
            //                    object obj;
            //                    if (keyValueMap.TryGetValue(propKey, out obj))
            //                    {
            //                        if (obj != null)
            //                        {
            //                            IFormattable formattable;
            //                            if (string.IsNullOrEmpty(propFormat) || ((formattable = obj as IFormattable) == null))
            //                            {
            //                                sb.Append(obj.ToString());
            //                            }
            //                            else
            //                            {
            //                                sb.Append(formattable.ToString(propFormat, null));
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (ignore)
            //                        {
            //                            while (ix <= index)
            //                            {
            //                                sb.Append(pc[ix++]);
            //                            }
            //                        }
            //                        else
            //                        {
            //                            throw new NormalException("找不到属性： " + propKey);
            //                        }
            //                    }

            //                    // 重置参数
            //                    propKey = null; propAlign = null; propFormat = null;
            //                }
            //                break;
            //        }
            //        ++index;
            //    }
            //}
            //if (state != 0)
            //{
            //    throw new NormalException("格式化字符串未正确关闭，位置: " + format.Substring(ix));
            //}

            //return sb.ToString();
    #endregion
        }

        /// <summary>
        /// 参数格式化器
        /// </summary>
        /// <param name="format">格式化字符串，如 如 ${Time,-10:yyyyMMddHHmmssfff}，其中 -10 为对齐</param>
        /// <param name="target">参数容器，可以是 IDictionary&lt;string, string&gt;, IDictionary&lt;string, object&gt;, IDictionary, 或是属性对象</param>
        /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true</param>
        /// <returns>
        /// 格式化后的字符串
        /// </returns>
        [Obsolete("此方法已经过时，请使用只有两个参数（IFormattableMap）的重载")]
        public static string Format(string format, object target, bool ignore)
        {
#if DEBUG
            if (target == null) throw new ArgumentNullException("target");
#endif
            if (IsNormal(format)) return format;
            IKeyValueMap keyValueMap = new ObjectKeyValueMap(target);
            return InternalFormat(format, keyValueMap, ignore);
        }

        /// <summary>
        /// 参数格式化器
        /// </summary>
        /// <param name="format">格式化字符串，如 如 ${Time,-10:yyyyMMddHHmmssfff}，其中 -10 为对齐</param>
        /// <param name="target">参数容器，可以是 IDictionary&lt;string, string&gt;, IDictionary&lt;string, object&gt;, IDictionary, 或是属性对象</param>
        /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true</param>
        /// <returns>
        /// 格式化后的字符串
        /// </returns>
        [Obsolete("此方法已经过时，请使用只有两个参数（IFormattableMap）的重载")]
        public static string Format(string format, IDictionary target, bool ignore)
        {
#if DEBUG
            if (target == null) throw new ArgumentNullException("target");
#endif
            if (IsNormal(format)) return format;
            IKeyValueMap keyValueMap = new DictionaryKeyValueMap(target);
            return InternalFormat(format, keyValueMap, ignore);
        }

    #region 删除的代码
        ///// <summary>
        ///// 参数格式化器
        ///// </summary>
        ///// <param name="format">格式化字符串，如 如 ${Time,-10:yyyyMMddHHmmssfff}，其中 -10 为对齐</param>
        ///// <param name="target">参数容器，可以是 IDictionary&lt;string, string&gt;, IDictionary&lt;string, object&gt;, IDictionary, 或是属性对象</param>
        ///// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true</param>
        ///// <returns>
        ///// 格式化后的字符串
        ///// </returns>
        //public static string Format<TValue>(string format, IDictionary<string, TValue> target, bool ignore)
        //{
        //    IKeyValueMap keyValueMap = new DictionaryKeyValueMap<TValue>(target);
        //    return Format(format, keyValueMap, ignore);
        //}
    #endregion

    #endregion

    #region 删除的代码
        // 采用正则表达式，支持属性格式化字符串的
        //private static Regex _formatRegex;

        ///// <summary>
        ///// 用于进行参数格式化的正则表达式
        ///// </summary>
        //private static Regex GetFormatRegex()
        //{
        //    if (_formatRegex == null)
        //    {
        //        _formatRegex = new Regex(@"(\$\$)|(\$\{(\w+)(:([^${}]+))?\})", RegexOptions.Compiled);
        //    }
        //    return _formatRegex;
        //}

        ///// <summary>
        ///// 参数格式化器
        ///// </summary>
        ///// <param name="format">格式化字符串</param>
        ///// <param name="replacement">属性替换方法接口，不能为空.</param>
        ///// <returns>
        ///// 格式化后的字符串
        ///// </returns>
        //public static string Format(string format, IReplacment replacement)
        //{
        //    if (format == null || format.IndexOf('$') == -1)
        //    {
        //        return format;
        //    }

        //    Regex formatRegex = GetFormatRegex();
        //    return formatRegex.Replace(format, new MatchEvaluator(replacement.ProcessMatch));
        //}

        ///// <summary>
        ///// 参数格式化器
        ///// </summary>
        ///// <param name="format">格式化字符串</param>
        ///// <param name="target">参数容器，可以是 IDictionary&lt;string, string&gt;, IDictionary&lt;string, object&gt;, IDictionary, 或是属性对象</param>
        ///// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true</param>
        ///// <returns>
        ///// 格式化后的字符串
        ///// </returns>
        //public static string Format(string format, object target, bool ignore)
        //{
        //    IReplacment replacement = new ObjectReplacement(target, ignore);
        //    return Format(format, replacement);
        //}

        ///// <summary>
        ///// 参数格式化器
        ///// </summary>
        ///// <param name="format">格式化字符串</param>
        ///// <param name="target">参数容器，可以是 IDictionary&lt;string, string&gt;, IDictionary&lt;string, object&gt;, IDictionary, 或是属性对象</param>
        ///// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true</param>
        ///// <returns>
        ///// 格式化后的字符串
        ///// </returns>
        //public static string Format(string format, IDictionary target, bool ignore)
        //{
        //    IReplacment replacement = new DictionaryReplacement(target, ignore);
        //    return Format(format, replacement);
        //}

        ///// <summary>
        ///// 参数格式化器
        ///// </summary>
        ///// <param name="format">格式化字符串</param>
        ///// <param name="target">参数容器，可以是 IDictionary&lt;string, string&gt;, IDictionary&lt;string, object&gt;, IDictionary, 或是属性对象</param>
        ///// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true</param>
        ///// <returns>
        ///// 格式化后的字符串
        ///// </returns>
        //public static string Format<TValue>(string format, IDictionary<string, TValue> target, bool ignore)
        //{
        //    IReplacment replacement = new DictionaryReplacement<TValue>(target, ignore);
        //    return Format(format, replacement);
        //}

        //#region 属性替换方法接口

        ///// <summary>
        ///// 属性替换方法接口
        ///// </summary>
        //public interface IReplacment
        //{
        //    /// <summary>
        //    /// 属性替换方法.
        //    /// </summary>
        //    /// <param name="match">正则表达式匹配项.</param>
        //    /// <returns></returns>
        //    string ProcessMatch(Match match);
        //}

        ///// <summary>
        ///// 属性替换方法的基本实现
        ///// </summary>
        //public abstract class Replacement : IReplacment
        //{
        //    #region Fields

        //    /// <summary>为 false, 则在没有找到匹配的属性后抛出异常，否则为 true</summary>
        //    private readonly bool _ignore;

        //    #endregion

        //    #region Ctors

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="Replacement"/> class.
        //    /// </summary>
        //    /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        //    public Replacement(bool ignore)
        //    {
        //        this._ignore = ignore;
        //    }

        //    #endregion

        //    /// <summary>
        //    /// 属性替换方法.
        //    /// </summary>
        //    /// <param name="match">正则表达式匹配项.</param>
        //    /// <returns></returns>
        //    public string ProcessMatch(Match match)
        //    {
        //        if (match.Value == "$$")
        //        {
        //            return "$";
        //        }

        //        object value;
        //        // 属性键
        //        string key = match.Groups[3].Value;
        //        // 属性格式化字符串
        //        string format = match.Groups[5].Value;
        //        if (TryGetValue(key, out value))
        //        {
        //            // 属性值是空的，返回 null
        //            if (value == null)
        //            {
        //                return null;
        //            }
        //            // 属性格式化字符串，返回属性值的字符串表示
        //            if (string.IsNullOrEmpty(format))
        //            {
        //                return value.ToString();
        //            }
        //            // 有格式化字符串，属性值是可以格式化的，进行格式化
        //            IFormattable formattable = value as IFormattable;
        //            if (formattable == null)
        //            {
        //                return value.ToString();
        //            }
        //            return formattable.ToString(format, null);
        //        }

        //        // 没有找到属性
        //        if (_ignore)
        //        {
        //            return match.Groups[2].Value;
        //        }
        //        throw new NormalException("找不到属性： " + key);
        //    }

        //    /// <summary>
        //    /// 偿试从KeyValueMap中获取属性值.
        //    /// </summary>
        //    /// <param name="key">属性名称.</param>
        //    /// <param name="value">如果成功获取，则以此返回获取到的属性值，否则为 null.</param>
        //    /// <returns>如果获取属性成功</returns>
        //    protected abstract bool TryGetValue(string key, out object value);
        //}

        ///// <summary>
        ///// 对象属性替换方法实现类
        ///// </summary>
        //public class ObjectReplacement : Replacement
        //{
        //    #region Fields

        //    /// <summary>目标对象</summary>
        //    private readonly object _target;

        //    #endregion

        //    #region Ctors

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="ObjectReplacement"/> class.
        //    /// </summary>
        //    /// <param name="target">目标对象.</param>
        //    /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        //    public ObjectReplacement(object target, bool ignore)
        //        : base(ignore)
        //    {
        //        this._target = target;
        //    }

        //    #endregion

        //    /// <summary>
        //    /// 偿试从KeyValueMap中获取属性值.
        //    /// </summary>
        //    /// <param name="key">属性名称.</param>
        //    /// <param name="value">如果成功获取，则以此返回获取到的属性值，否则为 null.</param>
        //    /// <returns>
        //    /// 如果获取属性成功
        //    /// </returns>
        //    protected override bool TryGetValue(string key, out object value)
        //    {
        //        Type t = _target.GetType();
        //        const BindingFlags BF = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        //        PropertyInfo property = t.GetProperty(key, BF);
        //        if (property == null)
        //        {
        //            value = null;
        //            return false;
        //        }

        //        value = property.GetValue(_target, null);
        //        return true;
        //    }
        //}

        ///// <summary>
        ///// 泛型字典属性替换方法实现类
        ///// </summary>
        ///// <typeparam name="TValue">The type of the value.</typeparam>
        //public class DictionaryReplacement<TValue> : Replacement
        //{
        //    #region Fields

        //    /// <summary>目标对象</summary>
        //    private readonly IDictionary<string, TValue> _target;

        //    #endregion

        //    #region Ctors

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="ObjectReplacement"/> class.
        //    /// </summary>
        //    /// <param name="target">目标对象.</param>
        //    /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        //    public DictionaryReplacement(IDictionary<string, TValue> target, bool ignore)
        //        : base(ignore)
        //    {
        //        this._target = target;
        //    }

        //    #endregion

        //    /// <summary>
        //    /// 偿试从KeyValueMap中获取属性值.
        //    /// </summary>
        //    /// <param name="key">属性名称.</param>
        //    /// <param name="value">如果成功获取，则以此返回获取到的属性值，否则为 null.</param>
        //    /// <returns>
        //    /// 如果获取属性成功
        //    /// </returns>
        //    protected override bool TryGetValue(string key, out object value)
        //    {
        //        TValue obj;
        //        if (_target.TryGetValue(key, out obj))
        //        {
        //            value = obj;
        //            return true;
        //        }

        //        value = null;
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 泛型字典属性替换方法实现类
        ///// </summary>
        ///// <typeparam name="TKey">The type of the key.</typeparam>
        ///// <typeparam name="TValue">The type of the value.</typeparam>
        //public class DictionaryReplacement<TKey, TValue> : Replacement
        //{
        //    #region Fields

        //    /// <summary>目标对象</summary>
        //    private readonly IDictionary<TKey, TValue> _target;

        //    #endregion

        //    #region Ctors

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="ObjectReplacement"/> class.
        //    /// </summary>
        //    /// <param name="target">目标对象.</param>
        //    /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        //    public DictionaryReplacement(IDictionary<TKey, TValue> target, bool ignore)
        //        : base(ignore)
        //    {
        //        this._target = target;
        //    }

        //    #endregion

        //    /// <summary>
        //    /// 偿试从KeyValueMap中获取属性值.
        //    /// </summary>
        //    /// <param name="key">属性名称.</param>
        //    /// <param name="value">如果成功获取，则以此返回获取到的属性值，否则为 null.</param>
        //    /// <returns>
        //    /// 如果获取属性成功
        //    /// </returns>
        //    protected override bool TryGetValue(string key, out object value)
        //    {
        //        TValue obj;
        //        TKey tkey = ChangeType(key);
        //        if (_target.TryGetValue(tkey, out obj))
        //        {
        //            value = obj;
        //            return true;
        //        }

        //        value = null;
        //        return false;
        //    }

        //    /// <summary>
        //    /// Changes the type.
        //    /// </summary>
        //    /// <param name="key">The key.</param>
        //    /// <returns></returns>
        //    private TKey ChangeType(string key)
        //    {
        //        if (typeof(TKey).IsAssignableFrom(typeof(string)))
        //        {
        //            return (TKey)(object)key;
        //        }
        //        return (TKey)(Convert.ChangeType(key, typeof(TKey)));
        //    }
        //}

        ///// <summary>
        ///// 字典属性替换方法实现类
        ///// </summary>
        //public class DictionaryReplacement : Replacement
        //{
        //    #region Fields

        //    /// <summary>目标对象</summary>
        //    private readonly IDictionary _target;

        //    #endregion

        //    #region Ctors

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="ObjectReplacement"/> class.
        //    /// </summary>
        //    /// <param name="target">目标对象.</param>
        //    /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        //    public DictionaryReplacement(IDictionary target, bool ignore)
        //        : base(ignore)
        //    {
        //        this._target = target;
        //    }

        //    #endregion

        //    /// <summary>
        //    /// 偿试从KeyValueMap中获取属性值.
        //    /// </summary>
        //    /// <param name="key">属性名称.</param>
        //    /// <param name="value">如果成功获取，则以此返回获取到的属性值，否则为 null.</param>
        //    /// <returns>
        //    /// 如果获取属性成功
        //    /// </returns>
        //    protected override bool TryGetValue(string key, out object value)
        //    {
        //        if (_target.Contains(key))
        //        {
        //            value = _target[key];
        //            return true;
        //        }

        //        value = null;
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 泛型集合属性替换方法实现类
        ///// </summary>
        ///// <typeparam name="TValue">The type of the value.</typeparam>
        //public class ListReplacement<TValue> : Replacement
        //{
        //    #region Fields

        //    /// <summary>目标对象</summary>
        //    private readonly IList<TValue> _target;

        //    #endregion

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="ObjectReplacement"/> class.
        //    /// </summary>
        //    /// <param name="target">目标对象.</param>
        //    /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        //    public ListReplacement(IList<TValue> target, bool ignore)
        //        : base(ignore)
        //    {
        //        this._target = target;
        //    }

        //    /// <summary>
        //    /// 偿试从KeyValueMap中获取属性值.
        //    /// </summary>
        //    /// <param name="key">属性名称.</param>
        //    /// <param name="value">如果成功获取，则以此返回获取到的属性值，否则为 null.</param>
        //    /// <returns>
        //    /// 如果获取属性成功
        //    /// </returns>
        //    protected override bool TryGetValue(string key, out object value)
        //    {
        //        int index;
        //        if (int.TryParse(key, out index))
        //        {
        //            if (index >= 0 && index < _target.Count)
        //            {
        //                value = _target[index];
        //                return true;
        //            }
        //        }

        //        value = null;
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 泛型集合属性替换方法实现类
        ///// </summary>
        //public class ListReplacement : Replacement
        //{
        //    #region Fields

        //    /// <summary>目标对象</summary>
        //    private readonly IList _target;

        //    #endregion

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="ObjectReplacement"/> class.
        //    /// </summary>
        //    /// <param name="target">目标对象.</param>
        //    /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        //    public ListReplacement(IList target, bool ignore)
        //        : base(ignore)
        //    {
        //        this._target = target;
        //    }

        //    /// <summary>
        //    /// 偿试从KeyValueMap中获取属性值.
        //    /// </summary>
        //    /// <param name="key">属性名称.</param>
        //    /// <param name="value">如果成功获取，则以此返回获取到的属性值，否则为 null.</param>
        //    /// <returns>
        //    /// 如果获取属性成功
        //    /// </returns>
        //    protected override bool TryGetValue(string key, out object value)
        //    {
        //        int index;
        //        if (int.TryParse(key, out index))
        //        {
        //            if (index >= 0 && index < _target.Count)
        //            {
        //                value = _target[index];
        //                return true;
        //            }
        //        }

        //        value = null;
        //        return false;
        //    }
        //}

        //#endregion
    #endregion

    #region 删除的代码
        // 采用正则表达式，不支持属性格式化字符串的
        //private static Regex _formatRegex;

        ///// <summary>
        ///// 用于进行参数格式化的正则表达式
        ///// </summary>
        //private static Regex GetFormatRegex()
        //{
        //    if (_formatRegex == null)
        //    {
        //        _formatRegex = new Regex(@"(\$\$)|(\$\{(\w+)\})", RegexOptions.Compiled);
        //    }
        //    return _formatRegex;
        //}

        ///// <summary>
        ///// 参数格式化器
        ///// </summary>
        ///// <param name="format">格式化字符串</param>
        ///// <param name="replacement">属性替换方法接口，不能为空.</param>
        ///// <returns>
        ///// 格式化后的字符串
        ///// </returns>
        //public static string Format(string format, IReplacment replacement)
        //{
        //    if (format == null || format.IndexOf('$') == -1)
        //    {
        //        return format;
        //    }

        //    Regex formatRegex = GetFormatRegex();
        //    return formatRegex.Replace(format, new MatchEvaluator(replacement.ProcessMatch));
        //}

        ///// <summary>
        ///// 参数格式化器
        ///// </summary>
        ///// <param name="format">格式化字符串</param>
        ///// <param name="target">参数容器，可以是 IDictionary&lt;string, string&gt;, IDictionary&lt;string, object&gt;, IDictionary, 或是属性对象</param>
        ///// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true</param>
        ///// <returns>
        ///// 格式化后的字符串
        ///// </returns>
        //public static string Format(string format, object target, bool ignore)
        //{
        //    IReplacment replacement = new ObjectReplacement(target, ignore);
        //    return Format(format, replacement);
        //}

        ///// <summary>
        ///// 参数格式化器
        ///// </summary>
        ///// <param name="format">格式化字符串</param>
        ///// <param name="target">参数容器，可以是 IDictionary&lt;string, string&gt;, IDictionary&lt;string, object&gt;, IDictionary, 或是属性对象</param>
        ///// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true</param>
        ///// <returns>
        ///// 格式化后的字符串
        ///// </returns>
        //public static string Format(string format, IDictionary target, bool ignore)
        //{
        //    IReplacment replacement = new DictionaryReplacement(target, ignore);
        //    return Format(format, replacement);
        //}

        ///// <summary>
        ///// 参数格式化器
        ///// </summary>
        ///// <param name="format">格式化字符串</param>
        ///// <param name="target">参数容器，可以是 IDictionary&lt;string, string&gt;, IDictionary&lt;string, object&gt;, IDictionary, 或是属性对象</param>
        ///// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true</param>
        ///// <returns>
        ///// 格式化后的字符串
        ///// </returns>
        //public static string Format<TValue>(string format, IDictionary<string, TValue> target, bool ignore)
        //{
        //    IReplacment replacement = new DictionaryReplacement<TValue>(target, ignore);
        //    return Format(format, replacement);
        //}

        //#region 属性替换方法接口

        ///// <summary>
        ///// 属性替换方法接口
        ///// </summary>
        //public interface IReplacment
        //{
        //    /// <summary>
        //    /// 属性替换方法.
        //    /// </summary>
        //    /// <param name="match">正则表达式匹配项.</param>
        //    /// <returns></returns>
        //    string ProcessMatch(Match match);
        //}

        ///// <summary>
        ///// 属性替换方法的基本实现
        ///// </summary>
        //public abstract class Replacement : IReplacment
        //{
        //    #region Fields

        //    /// <summary>为 false, 则在没有找到匹配的属性后抛出异常，否则为 true</summary>
        //    private readonly bool _ignore;

        //    #endregion

        //    #region Ctors

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="Replacement"/> class.
        //    /// </summary>
        //    /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        //    public Replacement(bool ignore)
        //    {
        //        this._ignore = ignore;
        //    }

        //    #endregion

        //    /// <summary>
        //    /// 属性替换方法.
        //    /// </summary>
        //    /// <param name="match">正则表达式匹配项.</param>
        //    /// <returns></returns>
        //    public string ProcessMatch(Match match)
        //    {
        //        if (match.Value == "$$")
        //        {
        //            return "$";
        //        }

        //        string value;
        //        string key = match.Groups[3].Value;
        //        if (TryGetValue(key, out value))
        //        {
        //            return value;
        //        }

        //        if (_ignore)
        //        {
        //            return match.Groups[2].Value;
        //        }
        //        throw new NormalException("找不到属性： " + key);
        //    }

        //    /// <summary>
        //    /// 偿试从KeyValueMap中获取属性值.
        //    /// </summary>
        //    /// <param name="key">属性名称.</param>
        //    /// <param name="value">如果成功获取，则以此返回获取到的属性值，否则为 null.</param>
        //    /// <returns>如果获取属性成功</returns>
        //    protected abstract bool TryGetValue(string key, out string value);
        //}

        ///// <summary>
        ///// 对象属性替换方法实现类
        ///// </summary>
        //public class ObjectReplacement : Replacement
        //{
        //    #region Fields

        //    /// <summary>目标对象</summary>
        //    private readonly object _target;

        //    #endregion

        //    #region Ctors

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="ObjectReplacement"/> class.
        //    /// </summary>
        //    /// <param name="target">目标对象.</param>
        //    /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        //    public ObjectReplacement(object target, bool ignore)
        //        : base(ignore)
        //    {
        //        this._target = target;
        //    }

        //    #endregion

        //    /// <summary>
        //    /// 偿试从KeyValueMap中获取属性值.
        //    /// </summary>
        //    /// <param name="key">属性名称.</param>
        //    /// <param name="value">如果成功获取，则以此返回获取到的属性值，否则为 null.</param>
        //    /// <returns>
        //    /// 如果获取属性成功
        //    /// </returns>
        //    protected override bool TryGetValue(string key, out string value)
        //    {
        //        value = null;
        //        Type t = _target.GetType();
        //        const BindingFlags BF = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        //        PropertyInfo property = t.GetProperty(key, BF);
        //        if (property != null)
        //        {
        //            object obj = property.GetValue(_target, null);
        //            if (obj != null)
        //            {
        //                value = obj.ToString();
        //            }

        //            return true;
        //        }

        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 泛型字典属性替换方法实现类
        ///// </summary>
        ///// <typeparam name="TValue">The type of the value.</typeparam>
        //public class DictionaryReplacement<TValue> : Replacement
        //{
        //    #region Fields

        //    /// <summary>目标对象</summary>
        //    private readonly IDictionary<string, TValue> _target;

        //    #endregion

        //    #region Ctors

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="ObjectReplacement"/> class.
        //    /// </summary>
        //    /// <param name="target">目标对象.</param>
        //    /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        //    public DictionaryReplacement(IDictionary<string, TValue> target, bool ignore)
        //        : base(ignore)
        //    {
        //        this._target = target;
        //    }

        //    #endregion

        //    /// <summary>
        //    /// 偿试从KeyValueMap中获取属性值.
        //    /// </summary>
        //    /// <param name="key">属性名称.</param>
        //    /// <param name="value">如果成功获取，则以此返回获取到的属性值，否则为 null.</param>
        //    /// <returns>
        //    /// 如果获取属性成功
        //    /// </returns>
        //    protected override bool TryGetValue(string key, out string value)
        //    {
        //        value = null;
        //        TValue obj;
        //        if (_target.TryGetValue(key, out obj))
        //        {
        //            if (obj != null)
        //            {
        //                value = obj.ToString();
        //            }

        //            return true;
        //        }

        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 泛型字典属性替换方法实现类
        ///// </summary>
        ///// <typeparam name="TKey">The type of the key.</typeparam>
        ///// <typeparam name="TValue">The type of the value.</typeparam>
        //public class DictionaryReplacement<TKey, TValue> : Replacement
        //{
        //    #region Fields

        //    /// <summary>目标对象</summary>
        //    private readonly IDictionary<TKey, TValue> _target;

        //    #endregion

        //    #region Ctors

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="ObjectReplacement"/> class.
        //    /// </summary>
        //    /// <param name="target">目标对象.</param>
        //    /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        //    public DictionaryReplacement(IDictionary<TKey, TValue> target, bool ignore)
        //        : base(ignore)
        //    {
        //        this._target = target;
        //    }

        //    #endregion

        //    /// <summary>
        //    /// 偿试从KeyValueMap中获取属性值.
        //    /// </summary>
        //    /// <param name="key">属性名称.</param>
        //    /// <param name="value">如果成功获取，则以此返回获取到的属性值，否则为 null.</param>
        //    /// <returns>
        //    /// 如果获取属性成功
        //    /// </returns>
        //    protected override bool TryGetValue(string key, out string value)
        //    {
        //        value = null;
        //        TValue obj;
        //        TKey tkey = ChangeType(key);
        //        if (_target.TryGetValue(tkey, out obj))
        //        {
        //            if (obj != null)
        //            {
        //                value = obj.ToString();
        //            }

        //            return true;
        //        }

        //        return false;
        //    }

        //    /// <summary>
        //    /// Changes the type.
        //    /// </summary>
        //    /// <param name="key">The key.</param>
        //    /// <returns></returns>
        //    private TKey ChangeType(string key)
        //    {
        //        if (typeof(TKey).IsAssignableFrom(typeof(string)))
        //        {
        //            return (TKey)(object)key;
        //        }
        //        return (TKey)(Convert.ChangeType(key, typeof(TKey)));
        //    }
        //}

        ///// <summary>
        ///// 字典属性替换方法实现类
        ///// </summary>
        //public class DictionaryReplacement : Replacement
        //{
        //    #region Fields

        //    /// <summary>目标对象</summary>
        //    private readonly IDictionary _target;

        //    #endregion

        //    #region Ctors

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="ObjectReplacement"/> class.
        //    /// </summary>
        //    /// <param name="target">目标对象.</param>
        //    /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        //    public DictionaryReplacement(IDictionary target, bool ignore)
        //        : base(ignore)
        //    {
        //        this._target = target;
        //    }

        //    #endregion

        //    /// <summary>
        //    /// 偿试从KeyValueMap中获取属性值.
        //    /// </summary>
        //    /// <param name="key">属性名称.</param>
        //    /// <param name="value">如果成功获取，则以此返回获取到的属性值，否则为 null.</param>
        //    /// <returns>
        //    /// 如果获取属性成功
        //    /// </returns>
        //    protected override bool TryGetValue(string key, out string value)
        //    {
        //        value = null;
        //        if (_target.Contains(key))
        //        {
        //            object obj = _target[key];
        //            if (obj != null)
        //            {
        //                value = obj.ToString();
        //            }
        //            return true;
        //        }
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 泛型集合属性替换方法实现类
        ///// </summary>
        ///// <typeparam name="TValue">The type of the value.</typeparam>
        //public class ListReplacement<TValue> : Replacement
        //{
        //    #region Fields

        //    /// <summary>目标对象</summary>
        //    private readonly IList<TValue> _target;

        //    #endregion

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="ObjectReplacement"/> class.
        //    /// </summary>
        //    /// <param name="target">目标对象.</param>
        //    /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        //    public ListReplacement(IList<TValue> target, bool ignore)
        //        : base(ignore)
        //    {
        //        this._target = target;
        //    }

        //    /// <summary>
        //    /// 偿试从KeyValueMap中获取属性值.
        //    /// </summary>
        //    /// <param name="key">属性名称.</param>
        //    /// <param name="value">如果成功获取，则以此返回获取到的属性值，否则为 null.</param>
        //    /// <returns>
        //    /// 如果获取属性成功
        //    /// </returns>
        //    protected override bool TryGetValue(string key, out string value)
        //    {
        //        value = null;
        //        int index;
        //        if (int.TryParse(key, out index))
        //        {
        //            if (index >= 0 && index < _target.Count)
        //            {
        //                TValue obj = _target[index];
        //                if (obj != null)
        //                {
        //                    value = obj.ToString();
        //                }
        //                return true;
        //            }
        //        }

        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 泛型集合属性替换方法实现类
        ///// </summary>
        //public class ListReplacement : Replacement
        //{
        //    #region Fields

        //    /// <summary>目标对象</summary>
        //    private readonly IList _target;

        //    #endregion

        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="ObjectReplacement"/> class.
        //    /// </summary>
        //    /// <param name="target">目标对象.</param>
        //    /// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true.</param>
        //    public ListReplacement(IList target, bool ignore)
        //        : base(ignore)
        //    {
        //        this._target = target;
        //    }

        //    /// <summary>
        //    /// 偿试从KeyValueMap中获取属性值.
        //    /// </summary>
        //    /// <param name="key">属性名称.</param>
        //    /// <param name="value">如果成功获取，则以此返回获取到的属性值，否则为 null.</param>
        //    /// <returns>
        //    /// 如果获取属性成功
        //    /// </returns>
        //    protected override bool TryGetValue(string key, out string value)
        //    {
        //        value = null;
        //        int index;
        //        if (int.TryParse(key, out index))
        //        {
        //            if (index >= 0 && index < _target.Count)
        //            {
        //                object obj = _target[index];
        //                if (obj != null)
        //                {
        //                    value = obj.ToString();
        //                }
        //                return true;
        //            }
        //        }

        //        return false;
        //    }
        //}

        //#endregion
    #endregion

    #region 删除的代码
        // 没有采用抽象策略的
        //private static Regex _formatRegex;

        ///// <summary>
        ///// 用于进行参数格式化的正则表达式
        ///// </summary>
        //private static Regex FormatRegex
        //{
        //    get
        //    {
        //        if (_formatRegex == null)
        //        {
        //            _formatRegex = new Regex(@"(\$\$)|(\$\{(\w+)\})", RegexOptions.Compiled);
        //        }
        //        return _formatRegex;
        //    }
        //}

        ///// <summary>
        ///// 参数格式化器
        ///// </summary>
        ///// <param name="format">格式化字符串</param>
        ///// <param name="keyValueMap">参数容器，可以是 IDictionary&lt;string, string&gt;, IDictionary&lt;string, object&gt;, IDictionary, 或是属性对象</param>
        ///// <param name="ignore">为 false, 则在没有找到匹配的属性后抛出异常，否则为 true</param>
        ///// <returns>格式化后的字符串</returns>
        //public static string Format(string format, object keyValueMap, bool ignore)
        //{
        //    if (format == null || format.IndexOf('$') == -1)
        //    {
        //        return format;
        //    }

        //    return FormatRegex.Replace(format, delegate(Match match)
        //    {
        //        if (match.Value == "$$")
        //        {
        //            return "$";
        //        }

        //        string value;
        //        string key = match.Groups[3].Value;
        //        if (TryGetValue(keyValueMap, key, out value))
        //        {
        //            return value;
        //        }

        //        if (ignore)
        //        {
        //            return match.Groups[2].Value;
        //        }
        //        throw new Exception("找不到属性： " + key);
        //    });
        //}

        ///// <summary>
        ///// 偿试从指定的KeyValueMap中获取属性值
        ///// </summary>
        ///// <param name="keyValueMap">KeyValueMap，可以是 IDictionary&lt;string, string&gt;, IDictionary&lt;string, object&gt;, IDictionary, 或是属性对象</param>
        ///// <param name="key">属性名</param>
        ///// <param name="value">如果获取属性成功，则以此返回属性值，否则为 null</param>
        ///// <returns>是否成功获取属性</returns>
        //private static bool TryGetValue(object keyValueMap, string key, out string value)
        //{
        //    value = null;

        //    // IDictionary<string, string>
        //    IDictionary<string, string> dic = keyValueMap as IDictionary<string, string>;
        //    if (dic != null)
        //    {
        //        return dic.TryGetValue(key, out value);
        //    }

        //    // IDictionary<string, object>
        //    IDictionary<string, object> dicObj = keyValueMap as IDictionary<string, object>;
        //    if (dicObj != null)
        //    {
        //        object obj;
        //        if (dicObj.TryGetValue(key, out obj))
        //        {
        //            if (obj != null)
        //            {
        //                value = obj.ToString();
        //            }
        //            return true;
        //        }

        //        value = null;
        //        return false;
        //    }

        //    // IDictionary
        //    IDictionary table = keyValueMap as IDictionary;
        //    if (table != null)
        //    {
        //        if (table.Contains(key))
        //        {
        //            object obj = table[key];
        //            if (obj == null)
        //            {
        //                value = null;
        //            }
        //            else
        //            {
        //                value = obj.ToString();
        //            }
        //            return true;
        //        }
        //        return false;
        //    }

        //    // object
        //    Type t = keyValueMap.GetType();
        //    PropertyInfo property = t.GetProperty(key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //    if (property != null)
        //    {
        //        object obj = property.GetValue(keyValueMap, null);
        //        if (obj != null)
        //        {
        //            value = obj.ToString();
        //        }

        //        return true;
        //    }

        //    value = null;
        //    return false;
        //}
    #endregion
    }
#endif

    public static partial class StringUtil
    {
        #region ToString

        /// <summary>
        /// 将 Int32 值转换为 toBase 指定进制的字符串, toBase 应在 2 与 36 之间
        /// </summary>
        /// <param name="value">目标值.</param>
        /// <param name="radix">基数.</param>
        /// <param name="upper">是否使用大写字母.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public static string ToString(int value, int radix, bool upper)
        {
            #region 修改实现方式，不使用 StringBuilder
            //if (radix < 2 || radix > 36)
            //{
            //    throw new NormalException("toBase不在 2 与 36 之间: " + radix);
            //}
            //StringBuilder sb = new StringBuilder();
            //do
            //{
            //    char ch = NumberUtil.GetDigital(value % radix, upper);
            //    sb.Append(ch);
            //    value = value / radix;
            //} while (value != 0);
            //return Reverse(sb).ToString();
            #endregion

            if (radix < 2 || radix > 36)
            {
                throw new NormalException("toBase不在 2 与 36 之间: " + radix);
            }
            unsafe
            {
                char* pResult = stackalloc char[32];
                int index = 32;
                do
                {
                    char ch = NumberUtil.GetDigital(value % radix, upper);
                    pResult[--index] = ch;
                    value = value / radix;
                } while (value != 0);
                return new string(pResult, index, 32 - index);
            }
        }

        /// <summary>
        /// 将 Int32 值转换为 toBase 指定进制的字符串, toBase 应在 2 与 36 之间
        /// </summary>
        /// <param name="value">目标值.</param>
        /// <param name="radix">基数.</param>
        /// <param name="upper">是否使用大写字母.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public static string ToString(long value, int radix, bool upper)
        {
            #region 修改实现方式，不使用 StringBuilder
            //if (radix < 2 || radix > 36)
            //{
            //    throw new NormalException("toBase不在 2 与 36 之间: " + radix);
            //}
            //StringBuilder sb = new StringBuilder();
            //do
            //{
            //    char ch = NumberUtil.GetDigital((int)(value % radix), upper);
            //    sb.Append(ch);
            //    value = value / radix;
            //} while (value != 0);
            //return Reverse(sb).ToString();
            #endregion

            if (radix < 2 || radix > 36)
            {
                throw new NormalException("toBase不在 2 与 36 之间: " + radix);
            }

            unsafe
            {
                char* pResult = stackalloc char[64];
                int index = 64;

                do
                {
                    char ch = NumberUtil.GetDigital((int)(value % radix), upper);
                    pResult[--index] = ch;
                    value = value / radix;
                } while (value != 0);
                return new string(pResult, index, 64 - index);
            }
        }

        #region 删除的代码
        ///// <summary>
        ///// 将 Int32 值转换为 toBase 指定进制的字符串, toBase 应在 2 与 36 之间
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="toBase"></param>
        ///// <returns></returns>
        //public static string ToString(int value, int toBase)
        //{
        //    if (toBase < 2 || toBase > 36)
        //    {
        //        throw new NormalException("toBase不在 2 与 36 之间: " + toBase);
        //    }
        //    StringBuilder sb = new StringBuilder();
        //    do
        //    {
        //        char ch = BaseChar(value % toBase);
        //        sb.Append(ch);
        //        value = value / toBase;
        //    } while (value != 0);
        //    return Reverse(sb).ToString();
        //}

        ///// <summary>
        ///// 将 Int32 值转换为 toBase 指定进制的字符串, toBase 应在 2 与 36 之间
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="toBase"></param>
        ///// <returns></returns>
        //public static string ToString(long value, int toBase)
        //{
        //    if (toBase < 2 || toBase > 36)
        //    {
        //        throw new NormalException("toBase不在 2 与 36 之间: " + toBase);
        //    }
        //    StringBuilder sb = new StringBuilder();
        //    do
        //    {
        //        char ch = BaseChar((int)(value % toBase));
        //        sb.Append(ch);
        //        value = value / toBase;
        //    } while (value != 0);
        //    return Reverse(sb).ToString();
        //}

        //private static char BaseChar(int value)
        //{
        //    if (value >= 0 && value <= 9)
        //    {
        //        return (char)('0' + value);
        //    }
        //    const char ch = (char)('A' - 10);
        //    return (char)(ch + value);
        //}
        #endregion

        #endregion

        #region EscapeUrlChars

        private static Regex _urlChars;

        /// <summary>
        /// url 中的特殊字符的匹配正则表达式
        /// </summary>
        private static Regex UrlChars
        {
            get
            {
                if (_urlChars == null)
                {
                    _urlChars = new Regex(@"[`=\\\[\];',\/~!#$%^&()+|\{\}:""<>\? ]", RegexOptions.Compiled | RegexOptions.CultureInvariant);
                }
                return _urlChars;
            }
        }

        /// <summary>
        /// 对字符串的的 url 特殊字符 (:/?&amp;=%\) 进行转义
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>转义后的字符串</returns>
        public static string EscapeUrlChars(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            return UrlChars.Replace(input, delegate (Match m)
            {
                if (m.Value == " ")
                {
                    return "+";
                }
                return "%" + NumberUtil.ToHexString((byte)m.Value[0], true);
            });
        }

        #endregion

        #region EscapeJsChars

        private static Regex _escapeJsCharsRegex;

        /// <summary>
        /// javascript 字符串中需要转义的字符串的匹配表达式
        /// </summary>
        private static Regex EscapeJsCharsRegex
        {
            get
            {
                if (_escapeJsCharsRegex == null)
                {
                    _escapeJsCharsRegex = new Regex(@"[\\'""\n\r]", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);
                }
                return _escapeJsCharsRegex;
            }
        }

        /// <summary>
        /// 对 javascript 字符串中的特殊字符进行转义
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static string EscapeJsChars(string input)
        {
            #region 删除的代码
            //if (string.IsNullOrEmpty(input))
            //{
            //    return input;
            //}

            //return input.Replace("\\", "\\\\")/*.Replace("'", "\\\'")*/.Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
            ////return s_regex.Replace(message, delegate(Match match)
            ////{
            ////    return "\\" + match.Value;
            ////});
            #endregion
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return EscapeJsCharsRegex.Replace(input, delegate (Match mc)
            {
                return ToEscape(mc.Value[0], false);
            });
        }

        #endregion

        #region Replace

        /// <summary>
        /// 在原字符串上进行字符串替换， 不生成新的字符串， 此用此方法时要注意源字符串将会被改变
        /// </summary>
        /// <param name="input">要替换的字符串</param>
        /// <param name="oldChar">要替换的旧字符</param>
        /// <param name="newChar">用于替换的新字符</param>
        public static void Replace(string input, char oldChar, char newChar)
        {
            if (string.IsNullOrEmpty(input) || oldChar == newChar)
            {
                return;
            }

            unsafe
            {
                fixed (char* p = input)
                {
                    for (int i = 0; i < input.Length; ++i)
                    {
                        if (p[i] == oldChar)
                        {
                            p[i] = newChar;
                        }
                    }
                }
            }
        }

        #endregion

        #region Is ... ?

        /// <summary>
        /// 判断输入是否是一个 guid
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsGuid(string input)
        {
            if (input == null || input.Length != 36)
            {
                return false;
            }
            for (int i = 0; i < input.Length; ++i)
            {
                char c = input[i];

                switch (i)
                {
                    case 8:
                    case 13:
                    case 18:
                    case 23:
                        if (c != '-')
                        {
                            return false;
                        }
                        break;
                    default:
                        if (
                               (c < '0')
                            || (c > '9' && c < 'A')
                            || (c > 'F' && c < 'a')
                            || (c > 'f')
                        )
                        {
                            return false;
                        }
                        break;
                }

                #region 删除的代码
                //if (
                //    (c == '-' && (i == 8) || i == 13 || i == 18 || i == 23)
                //    ||
                //    (
                //        (c >= '0' && c <= '9' || c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F')
                //        && (i != 8 && i != 13 && i != 18 && i != 23)
                //    )
                //    )
                //{
                //    continue;
                //}
                //else
                //{
                //    return false;
                //}
                #endregion
            }

            return true;
        }

        /// <summary>
        /// 判断一输入是否表示一个十进制整数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsInteger(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            for (int i = 0; i < input.Length; ++i)
            {
                char c = input[i];
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region CutString

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="input">要截取的字符串</param>
        /// <param name="length">支取长度</param>
        /// <returns>截取后的字符串</returns>
        public static string CutString(string input, int length)
        {
            if (input == null || input.Length <= length)
            {
                return input;
            }
            length = length < 4 ? 0 : length - 3;

            return input.Substring(0, length) + "...";
        }

        #endregion

        #region Split

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="seperator">分割字符</param>
        /// <returns></returns>
        public static SplitResult Split(string input, char seperator)
        {
            return new CharSplitResult(input, seperator);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="seperator">分割字符串</param>
        /// <returns></returns>
        public static SplitResult Split(string input, string seperator)
        {
            return new StringSplitResult(input, seperator);
        }

        #endregion

        #region NextString

        /// <summary>
        /// 获取当前字符串的下一个字符串(按 Ascii 码确定)
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="lowBound">下界字符, 十进制时应为 '0'</param>
        /// <param name="upBound">上界字符, 十进制时应为 '9'</param>
        /// <returns>更改过值后的 StringBuilder 对象</returns>
        public static StringBuilder NextString(StringBuilder input, char lowBound, char upBound)
        {
            #region 删除的代码
            //int index = input.Length - 1;
            //char ch;
            //while (index >= 0)
            //{
            //    ch = input[index];
            //    ++ch;
            //    if (ch <= upBound)
            //    {
            //        input[index] = ch;
            //        break;
            //    }

            //    input[index--] = lowBound;
            //}

            //return input;
            #endregion

            int index = input.Length;
            char ch;
            while (--index >= 0)
            {
                ch = input[index];
                ++ch;
                if (ch <= upBound)
                {
                    input[index] = ch;
                    break;
                }

                input[index] = lowBound;
            }

            return input;
        }

        /// <summary>
        /// 获取当前字符串数组的下一个字符串数组(按 Ascii 码确定)
        /// </summary>
        /// <param name="input">输入字符数组</param>
        /// <param name="lowBound">下界字符, 十进制时应为 '0'</param>
        /// <param name="upBound">上界字符, 十进制时应为 '9'</param>
        /// <returns></returns>
        public static char[] NextString(char[] input, char lowBound, char upBound)
        {
            #region 删除的代码
            //int index = input.Length - 1;
            //char ch;
            //while (index >= 0)
            //{
            //    ch = input[index];
            //    ++ch;
            //    if (ch <= upBound)
            //    {
            //        input[index] = ch;
            //        break;
            //    }

            //    input[index--] = lowBound;
            //}

            //return input;
            #endregion

            int index = input.Length;
            char ch;
            while (--index >= 0)
            {
                ch = input[index];
                ++ch;
                if (ch <= upBound)
                {
                    input[index] = ch;
                    break;
                }

                input[index] = lowBound;
            }

            return input;
        }

        /// <summary>
        /// 获取当前字符串的下一个字符串(按 Ascii 码确定)
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="lowBound">下界字符, 十进制时应为 '0'</param>
        /// <param name="upBound">上界字符, 十进制时应为 '9'</param>
        /// <returns></returns>
        public static string NextString(string input, char lowBound, char upBound)
        {
            return new string(NextString(input.ToCharArray(), lowBound, upBound));
        }

        /// <summary>
        /// 获取当前字符串的下一个字符串(十进制)
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static string NextString(string input)
        {
            return NextString(input, '0', '9');
        }

        /// <summary>
        /// 获取当前字符串的下一个字符串(十进制)
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="defaultValue">默认值(初始值)</param>
        /// <returns></returns>
        public static string NextString(string input, string defaultValue)
        {
            if (string.IsNullOrEmpty(input))
            {
                return defaultValue;
            }
            return NextString(input, '0', '9');
        }

        #endregion

        #region PreviousString

        /// <summary>
        /// 获取当前字符串的上一个字符串(按 Ascii 码确定)
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="lowBound">下界字符, 十进制时应为 '0'</param>
        /// <param name="upBound">上界字符, 十进制时应为 '9'</param>
        /// <returns>更改过值后的 StringBuilder 对象</returns>
        public static StringBuilder PreviousString(StringBuilder input, char lowBound, char upBound)
        {
            #region 删除的代码
            //int index = input.Length - 1;
            //char ch;
            //while (index >= 0)
            //{
            //    ch = input[index];
            //    --ch;
            //    if (ch >= lowBound)
            //    {
            //        input[index] = ch;
            //        break;
            //    }

            //    input[index--] = upBound;
            //}

            //return input;
            #endregion

            int index = input.Length;
            char ch;
            while (--index >= 0)
            {
                ch = input[index];
                --ch;
                if (ch >= lowBound)
                {
                    input[index] = ch;
                    break;
                }

                input[index] = upBound;
            }

            return input;
        }

        /// <summary>
        /// 获取当前字符串数组的上一个字符串数组(按 Ascii 码确定)
        /// </summary>
        /// <param name="input">输入字符数组</param>
        /// <param name="lowBound">下界字符, 十进制时应为 '0'</param>
        /// <param name="upBound">上界字符, 十进制时应为 '9'</param>
        /// <returns></returns>
        public static char[] PreviousString(char[] input, char lowBound, char upBound)
        {
            #region 删除的代码
            //int index = input.Length - 1;
            //char ch;
            //while (index >= 0)
            //{
            //    ch = input[index];
            //    --ch;
            //    if (ch >= lowBound)
            //    {
            //        input[index] = ch;
            //        break;
            //    }

            //    input[index--] = upBound;
            //}

            //return input;
            #endregion

            int index = input.Length;
            char ch;
            while (--index >= 0)
            {
                ch = input[index];
                --ch;
                if (ch >= lowBound)
                {
                    input[index] = ch;
                    break;
                }

                input[index] = upBound;
            }

            return input;
        }

        /// <summary>
        /// 获取当前字符串的上一个字符串(按 Ascii 码确定)
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="lowBound">下界字符, 十进制时应为 '0'</param>
        /// <param name="upBound">上界字符, 十进制时应为 '9'</param>
        /// <returns></returns>
        public static string PreviousString(string input, char lowBound, char upBound)
        {
            return new string(PreviousString(input.ToCharArray(), lowBound, upBound));
        }

        /// <summary>
        /// 获取当前字符串的上一个字符串(十进制)
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static string PreviousString(string input)
        {
            return PreviousString(input, '0', '9');
        }

        /// <summary>
        /// 获取当前字符串的上一个字符串(十进制)
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="defaultValue">默认值(初始值)</param>
        /// <returns></returns>
        public static string PreviousString(string input, string defaultValue)
        {
            if (string.IsNullOrEmpty(input))
            {
                return defaultValue;
            }
            return PreviousString(input, '0', '9');
        }

        #endregion

        #region Reverse

        /// <summary>
        /// 将字符数组反转
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public unsafe static void Reverse(char* input, int length)
        {
            int i = 0;
            int j = length;
            while (--j > i)
            {
                char ch = input[i];
                input[i] = input[j];
                input[j] = ch;
                ++i;
            }

            //return input;
        }

        /// <summary>
        /// 将字符数组反转
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static void Reverse(char[] input, int startIndex, int length)
        {
            int i = startIndex;
            int j = startIndex + length;
            while (--j > i)
            {
                char ch = input[i];
                input[i] = input[j];
                input[j] = ch;
                ++i;
            }

            //return input;
        }

        /// <summary>
        /// 将字符数组反转
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static void Reverse(char[] input)
        {
            Reverse(input, 0, input.Length);
        }

        /// <summary>
        /// 将字符串反转
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Reverse(string input)
        {
            char[] array = input.ToCharArray();
            Reverse(array);
            return new string(array);
        }

        /// <summary>
        /// 将字符串反转
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Reverse(string input, int startIndex, int length)
        {
            char[] array = input.ToCharArray();
            Reverse(array, startIndex, length);
            return new string(array);
        }

        /// <summary>
        /// 将一个 StringBuilder 反转
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        public static void Reverse(StringBuilder sb)
        {
            for (int i = 0, j = sb.Length; i < --j; ++i)
            {
                char ch = sb[i];
                sb[i] = sb[j];
                sb[j] = ch;
            }
            //return sb;
        }

        #endregion

        #region ParseDate

        /// <summary>
        /// 用于获取格式字符下标的函数
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="part1"></param>
        /// <param name="part2"></param>
        /// <returns></returns>
        private static int getPartIndex(string fmt, string part1, string part2)
        {
            int index = fmt.IndexOf(part1);
            if (index == -1 && null != part2)
            {
                index = fmt.IndexOf(part2);
            }
            if (index == -1)
                return 999;
            else
                return index;
        }

        /// <summary>
        /// 用于从匹配中获取值的函数
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private static int getMatchValue(Match mc, int i)
        {
            if (mc.Groups.Count > i)
            {
                return int.Parse(mc.Groups[i].Value);
            }
            return 0;
        }

        /// <summary>
        /// 将字符串转换为日期
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static DateTime ParseDate(string str, string fmt)
        {
            if (null == fmt)
                fmt = "yyyy-MM-dd";
            // 第个日期部分对应的下标和匹配中的组编号的实体数组
            int[] gis = {
            getPartIndex(fmt, "yyyy", "%y"),
            1,
            getPartIndex(fmt, "MM", "%M"),
            1,
            getPartIndex(fmt, "dd", "%d"),
            1,
            getPartIndex(fmt, "HH", "%H"),
            1,
            getPartIndex(fmt, "mm", "%m"),
            1,
            getPartIndex(fmt, "ss", "%s"),
            1,
            getPartIndex(fmt, "ms", null),
            1
        };
            // 根据日期部分下标排列组
            for (int i = 0; i != gis.Length / 2; ++i)
            {
                for (int j = 0; j != i; ++j)
                {
                    if (gis[i + i] > gis[j + j])
                        ++gis[i + i + 1];//gis[i].g = gis[i].g + 1;
                    else
                        ++gis[j + j + 1];//gis[j].g = gis[j].g + 1;
                }
            }
            // 替换为正则表达式
            //fmt = fmt.replace('(','[(]').replace(')','[)]').replace('yyyy', '(\\d{1,4})').replace('MM', '(\\d{1,2})').replace('dd', '(\\d{1,2})').replace('%y', '(\\d{1,4})').replace('%M', '(\\d{1,2})').replace('%d', '(\\d{1,2})').replace('HH', '(\\d{1,2})').replace('mm', '(\\d{1,2})').replace('ss', '(\\d{1,2})').replace('%H', '(\\d{1,2})').replace('%m', '(\\d{1,2})').replace('%s', '(\\d{1,2})');
            fmt = Regex.Replace(fmt, @"[(]|[)]|[\\]|[\/]|[|]", delegate (Match mc)
            {
                switch (mc.Value)
                {
                    case "\\":
                        return "[\\\\]";
                    default:
                        return '[' + mc.Value + ']';
                }
            });
            fmt = Regex.Replace(fmt, @"(yyyy)|(MM)|(dd)|(HH)|(mm)|(ss)|(ms)|(%y)|(%M)|(%d)|(%H)|(%m)|(%s)", delegate (Match mc)
            {
                if ("yyyy" == mc.Value)
                    return "(\\d{1,4})";
                else if ("ms" == mc.Value)
                    return "(\\d{1,3})";
                else
                    return "(\\d{1,2})";
            });
            Match mcc = Regex.Match(str, fmt);
            if (mcc == null)
            {
                throw new NormalException("格式不正确");
            }
            // 从匹配中分析日期
            return new DateTime(
                getMatchValue(mcc, gis[1]),
                getMatchValue(mcc, gis[3]),
                getMatchValue(mcc, gis[5]),
                getMatchValue(mcc, gis[7]),
                getMatchValue(mcc, gis[9]),
                getMatchValue(mcc, gis[11]),
                getMatchValue(mcc, gis[13]));
            //return new Date(getMatchValue(mc, gis[0].g), getMatchValue(mc, gis[1].g) - 1, getMatchValue(mc, gis[2].g), getMatchValue(mc, gis[3].g), getMatchValue(mc, gis[4].g), getMatchValue(mc, gis[5].g), getMatchValue(mc, gis[6].g));
        }

        #endregion

        #region KMPIndexOf

        /// <summary>
        /// 用 KMP 算法从一段文本里查找目标字符串的位置
        /// </summary>
        /// <param name="content">内容字符串</param>
        /// <param name="dest">要搜索的目标字符串</param>
        /// <returns>如果找到, 返回目标字符串在内容字符串中的第一个位置, 否则返回 -1</returns>
        public static int KMPIndexOf(string content, string dest)
        {
            return KMPIndexOf(content, dest, 0);
        }

        /// <summary>
        /// 用 KMP 算法从一段文本里查找目标字符串的位置
        /// </summary>
        /// <param name="content">内容字符串</param>
        /// <param name="dest">要搜索的目标字符串</param>
        /// <param name="startIndex">开始下标</param>
        /// <returns>如果找到, 返回目标字符串在内容字符串中的第一个位置, 否则返回 -1</returns>
        public static int KMPIndexOf(string content, string dest, int startIndex)
        {
            #region 删除的代码
            //int i = startIndex, j = 0;
            //int [] next = KMPNext(dest);

            //while (i < content.Length)
            //{
            //    if (j == -1 || content[i] == dest[j])
            //    {
            //        ++i;
            //        if (++j == dest.Length)
            //        {
            //            return i - j;
            //        }
            //    }
            //    else
            //    {
            //        j = next[j];
            //    }
            //}

            //return -1;
            #endregion

#if DEBUG
            if (dest == null) throw new ArgumentNullException("dest");
            if (content == null) throw new ArgumentNullException("content");
#endif
            if (content.Length == 0)
            {
                return startIndex;
            }

            // 定义 i, j 分别表示内容字符串和目标字符串中的下标位置
            int i = startIndex, j = 0;
            int[] next = KMPNext(dest);

            int length = content.Length;
            int destLength = dest.Length;
            // 循环比较相应位置的字符是否相等
            while (i < length)
            {
                // 如果相等, 则比较下一个
                if (j == -1 || content[i] == dest[j])
                {
                    ++i;
                    if (++j == destLength)
                    {
                        return i - j;
                    }
                }
                else
                {
                    // 如果不相等, 则取 next[j]
                    j = next[j];
                }
                // 如果 next[j] = -1, 则 ++i, ++j
                // 如果 j 已经达到目标字符串的尾部, 则表明找到, 返回
            }

            // 返回 -1
            return -1;
        }

        /// <summary>
        /// 求 KMP 算法的 next 值
        /// </summary>
        /// <param name="dest">目录字符串</param>
        /// <returns>KMP 算法的 next 值</returns>
        private static int[] KMPNext(string dest)
        {
            int[] next = new int[dest.Length];
            int i = 1, j = -1;
            next[0] = -1;

            int count = dest.Length - 1;
            while (i < count)
            {
                if (j == -1 || dest[i] == dest[j])
                {
                    if (dest[++i] == dest[++j])
                    {
                        next[i] = next[j];
                    }
                    else
                    {
                        next[i] = j;
                    }
                }
                else
                {
                    j = next[j];
                }
            }

            return next;
        }

        #endregion

        #region IndexOf

        /// <summary>
        /// 从一段文本里查找目标字符串的位置
        /// </summary>
        /// <param name="content">内容字符串</param>
        /// <param name="dest">要搜索的目标字符串</param>
        /// <returns>如果找到, 返回目标字符串在内容字符串中的第一个位置, 否则返回 -1</returns>
        public static int IndexOf(string content, string dest)
        {
            return IndexOf(content, dest, 0);
        }

        /// <summary>
        /// 从一段文本里查找目标字符串的位置
        /// </summary>
        /// <param name="content">内容字符串</param>
        /// <param name="dest">要搜索的目标字符串</param>
        /// <param name="startIndex">开始下标</param>
        /// <returns>如果找到, 返回目标字符串在内容字符串中的第一个位置, 否则返回 -1</returns>
        public static int IndexOf(string content, string dest, int startIndex)
        {
            int[] next = GetNext(dest);
            int j = 0, compareLength = content.Length - dest.Length;
            while (startIndex <= compareLength)
            {
                j = 0;
                while (j < dest.Length)
                {
                    if (content[startIndex + j] != dest[j])
                    {
                        goto loop;
                    }
                    ++j;
                }
                return startIndex;
                loop:
                startIndex += next[j];
            }
            return -1;

            #region 删除的代码
            //int[] next = GetNext(dest);
            //int i = 0, j = 0, compareLength = content.Length - dest.Length;
            //while (i <= compareLength)
            //{
            //    j = 0;
            //    bool flag = true;
            //    while (j < dest.Length)
            //    {
            //        if (content[i + j] != dest[j])
            //        {
            //            flag = false;
            //            break;
            //        }
            //        else
            //        {
            //            ++j;
            //        }
            //    }
            //    if (flag)
            //    {
            //        return i;
            //    }
            //    else
            //    {
            //        i += next[j];
            //    }
            //}
            //return -1;
            #endregion
        }

        /// <summary>
        /// 获取 Next 数组
        /// </summary>
        /// <param name="dest"></param>
        /// <returns></returns>
        private static int[] GetNext(string dest)
        {
            if (string.IsNullOrEmpty(dest))
            {
                return null;
            }
            int[] iarr = new int[dest.Length];
            iarr[0] = 1;
            for (int i = 1; i < iarr.Length; ++i)
            {
                iarr[i] = 1;
            }
            return iarr;
        }

        #endregion
    }

    // ToEscape
    public static partial class StringUtil
    {
        /// <summary>
        /// 将字符转换为其转义字符对应的串
        /// </summary>
        /// <param name="ch">The ch.</param>
        /// <param name="upper">是否使用大写字母.</param>
        /// <returns></returns>
        public unsafe static string ToEscape(char ch, bool upper)
        {
            char* pResult = stackalloc char[6];
            int length = ToEscape(ch, pResult, upper);
            return new string(pResult, 0, length);
        }

        /// <summary>
        /// 将字符转换为其转义字符对应的串.
        /// </summary>
        /// <param name="ch">The ch.</param>
        /// <param name="pResult">用于保存结果的指针.</param>
        /// <param name="upper">是否使用大写字母.</param>
        /// <returns>结果的字符个数</returns>
        public unsafe static int ToEscape(char ch, char* pResult, bool upper)
        {
            if (ch < 128)
            {
                *pResult = ch;
                return 1;
            }
            *(pResult++) = '\\'; *(pResult++) = 'u';

            int c = ch;
            for (int i = 4; i != 0;)
            {
                pResult[--i] = NumberUtil.GetDigital(c & 0xF, upper);
                c >>= 4;
            }

            return 6;
        }

        /// <summary>
        /// 将字符串转换为其转义字符对应的串
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="upper">是否使用大写字母.</param>
        /// <returns></returns>
        public unsafe static string ToEscape(string input, bool upper)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            char* pResult = stackalloc char[input.Length * 6];
            int pos = 0;
            fixed (char* pc = input)
            {
                for (int i = 0; i < input.Length; ++i)
                {
                    pos += ToEscape(pc[i], pResult + pos, upper);
                }
            }

            return new string(pResult, 0, pos);
        }
    }

    #region SplitResult

    /// <summary>
    /// 字符串分割结果
    /// </summary>
    public abstract class SplitResult : IEnumerator<string>
    {
        /// <summary>
        /// 开始下标
        /// </summary>
        protected int m_startIndex;
        /// <summary>
        /// 结束下标
        /// </summary>
        protected int m_endIndex;
        /// <summary>
        /// 分隔符长度
        /// </summary>
        protected int m_seperatorLength;
        /// <summary>
        /// 值
        /// </summary>
        protected string m_value;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="seperatorLength"></param>
        protected SplitResult(string value, int seperatorLength)
        {
            m_value = value;
            m_startIndex = 0;
            m_endIndex = -seperatorLength;
            m_seperatorLength = seperatorLength;
        }

        #region IEnumerator 成员

        /// <summary>
        /// 获取当前结果
        /// </summary>
        public string Current
        {
            get { return m_value.Substring(m_startIndex, m_endIndex - m_startIndex); }
        }

        /// <summary>
        /// 移动到下一个
        /// </summary>
        /// <returns></returns>
        public abstract bool MoveNext();

        /// <summary>
        /// 重置游标
        /// </summary>
        public void Reset()
        {
            m_startIndex = 0;
            m_endIndex = -m_seperatorLength;
        }

        #endregion

        #region IDisposable 成员

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            m_value = null;
        }

        #endregion

        #region IEnumerator 成员

        /// <summary>
        /// 获取当前元素
        /// </summary>
        object IEnumerator.Current
        {
            get { return Current; }
        }

        #endregion
    }

    /// <summary>
    /// 字符串分割结果
    /// </summary>
    internal class StringSplitResult : SplitResult
    {
        private string m_seperator;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="seperator"></param>
        internal StringSplitResult(string value, string seperator)
            : base(value, seperator.Length)
        {
            m_seperator = seperator;
        }

        /// <summary>
        /// 移动到下一个
        /// </summary>
        /// <returns></returns>
        public override bool MoveNext()
        {
            if (m_endIndex == m_value.Length)
            {
                return false;
            }
            int index = m_value.IndexOf(m_seperator, m_endIndex + m_seperatorLength);
            m_startIndex = m_endIndex + m_seperatorLength;
            m_endIndex = index;
            if (m_endIndex == -1)
            {
                m_endIndex = m_value.Length;
            }
            return true;
        }
    }

    /// <summary>
    /// 字符分隔结果
    /// </summary>
    internal class CharSplitResult : SplitResult
    {
        private char m_seperator;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="seperator"></param>
        internal CharSplitResult(string value, char seperator)
            : base(value, 1)
        {
            m_seperator = seperator;
        }

        /// <summary>
        /// 移动到下一个
        /// </summary>
        /// <returns></returns>
        public override bool MoveNext()
        {
            if (m_endIndex == m_value.Length)
            {
                return false;
            }
            int index = m_value.IndexOf(m_seperator, m_endIndex + 1);
            m_startIndex = m_endIndex + 1;
            m_endIndex = index;
            if (m_endIndex == -1)
            {
                m_endIndex = m_value.Length;
            }
            return true;
        }
    }

    #endregion

    // hash
    public static partial class StringUtil
    {
        //private static readonly System.Security.Cryptography.MD5
        //    md5 = System.Security.Cryptography.MD5.Create();

        /// <summary>
        /// 计算 MD5.
        /// </summary>
        /// <param name="buffer">待计算的数据缓冲区</param>
        /// <param name="offset">字节偏移量</param>
        /// <param name="count">字节数</param>
        /// <returns>MD5</returns>
        public static byte[] GetMD5(byte[] buffer, int offset, int count)
        {
            return EncryptUtil.ComputeMD5(buffer, offset, count);
            //return md5.ComputeHash(data, offset, count);
        }

        /// <summary>
        /// 计算 MD5.
        /// </summary>
        /// <param name="data">待计算的数据</param>
        /// <returns>MD5</returns>
        public static byte[] GetMD5(byte[] data)
        {
            return EncryptUtil.ComputeMD5(data);
            //return md5.ComputeHash(data, 0, data.Length);
        }

        /// <summary>
        /// 计算 MD5 字符串.
        /// </summary>
        /// <param name="buffer">待计算的数据缓冲区</param>
        /// <param name="offset">字节偏移量</param>
        /// <param name="count">字节数</param>
		/// <param name="upper">是否使用大写字母.</param>
        /// <returns>MD5字符串</returns>
        public unsafe static string GetMD5String(byte[] buffer, int offset, int count, bool upper)
        {
            //return NumberUtil.ToHexStringFast(upper, GetMD5(data, offset, count));
            byte* pResult = stackalloc byte[16];
            //byte[] result = new byte[16];
            fixed (byte* pInput = buffer)
            {
                EncryptUtil.ComputeMD5(pInput + offset, (ulong)count, pResult);
            }
            return NumberUtil.ToHexString(pResult, 16, upper);
        }

        /// <summary>
        /// 计算 MD5 字符串.
        /// </summary>
        /// <param name="data">待计算的数据</param>
		/// <param name="upper">是否使用大写字母.</param>
        /// <returns>MD5字符串</returns>
        public static string GetMD5String(byte[] data, bool upper)
        {
            return GetMD5String(data, 0, data.Length, upper);
        }

        /// <summary>
        /// 将str以UTF8编码，并计算 MD5 字符串.
        /// </summary>
        /// <param name="str">待计算的字符串</param>
        /// <param name="upper">是否使用大写字母.</param>
        /// <returns>MD5字符串</returns>
        public static string GetMD5String(string str, bool upper)
        {
            byte[] data = UTF8NoBOM.GetBytes(str);
            return GetMD5String(data, 0, data.Length, upper);
        }

        /// <summary>
        /// 获取表示文件大小的字符串表示，会自动处理 B/KB/MB/GB/TB/PB.
        /// </summary>
        /// <param name="size">大小（字节数）</param>
        /// <returns>大小的字符串</returns>
        public static string GetSizeString(long size)
        {
            if (size <= 1024)
            {
                return string.Format("{0:f2}B", size);
            }
            if (size <= 1024 * 1024)
            {
                return string.Format("{0:f2}KB", (double)size / 1024);
            }
            if (size <= 1024 * 1024 * 1024)
            {
                return string.Format("{0:f2}MB", (double)size / (1024 * 1024));
            }
            if (size <= 1024L * 1024 * 1024 * 1024)
            {
                return string.Format("{0:f2}G", (double)size / (1024 * 1024 * 1024));
            }
            if (size <= 1024L * 1024 * 1024 * 1024 * 1024)
            {
                return string.Format("{0:f2}TB", (double)size / (1024L * 1024 * 1024 * 1024));
            }
            //if (size <= 1024L * 1024 * 1024 * 1024 * 1024 * 1024)
            {
                return string.Format("{0:f2}PB", (double)size / (1024L * 1024 * 1024 * 1024 * 1024));
            }
        }
    }

    // URL 编解码
    public partial class StringUtil
    {
        /// <summary>
        /// 获取将指定的数据编码后的数据长度.
        /// </summary>
        /// <param name="buffer">要编码的数据.</param>
        /// <param name="offset">数据存放的开始下标.</param>
        /// <param name="count">数据的字节数.</param>
        /// <param name="size">通过此参数返回结果.</param>
        /// <returns>如果需要转换，则为 true，否则为 false</returns>
        public static bool GetUrlEncodeCount(byte[] buffer, int offset, int count, out int size)
        {
#if DEBUG
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (offset < 0 || offset >= buffer.Length) throw new IndexOutOfRangeException("offset");
            if (count < 0 || offset + count > buffer.Length) throw new IndexOutOfRangeException("count");
#endif
            const byte C_SPACE = (byte)' ';

            int end = offset + count;
            size = 0;
            bool need = false;
            for (int index = offset; index < end; ++index)
            {
                byte b = buffer[index];
                if (b == C_SPACE) need = true;
                else if (!StringUtil.IsUrlSafe(b)) ++size;
            }
            if (size != 0)
            {
                size = size + size + count;
                need = true;
            }
            else size = count;
            return need;
        }

        /// <summary>
        /// 进行 url 编码.
        /// </summary>
        /// <param name="buffer">要编码的数据.</param>
        /// <param name="offset">数据存放的开始下标.</param>
        /// <param name="count">数据的字节数.</param>
        /// <returns>url 编码结果</returns>
        public static byte[] UrlEncode(byte[] buffer, int offset, int count)
        {
#if DEBUG
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (offset < 0 || offset >= buffer.Length) throw new IndexOutOfRangeException("offset");
            if (count < 0 || offset + count > buffer.Length) throw new IndexOutOfRangeException("count");
#endif
            const byte C_SPACE = (byte)' ';
            int size;
            if (GetUrlEncodeCount(buffer, offset, count, out size))
            {
                byte[] result = new byte[size];
                int end = offset + count;
                for (int i = 0, j = 0; i < end; ++i)
                {
                    byte b = buffer[i];
                    if (StringUtil.IsUrlSafe(b)) result[j++] = b;
                    else if (b == C_SPACE) result[j++] = 0x2B;
                    else
                    {
                        result[j++] = 0x25;
                        result[j++] = GetDigital(b >> 4);
                        result[j++] = GetDigital(b & 0x0F);
                    }
                }
                return result;
            }

            return buffer;
        }

        /// <summary>
        /// 进行 url 编码. 类似于 JavaScript 中的 encodeURIComponent() 方法.
        /// 该方法不会对 ASCII 字母和数字进行编码，也不会对这些 ASCII 标点符号进行编码： - _ . ! ~ * ' ( ) 。
        /// 其他字符（比如 ：;/?:@&amp;=+$,# 这些用于分隔 URI 组件的标点符号），都是由一个或多个十六进制的转义序列替换的。
        /// </summary>
        /// <param name="str">要编码的字符串.</param>
        /// <param name="encoding">使用的编码.</param>
        /// <returns>url 编码结果</returns>
        /// <remarks>请注意 encodeURIComponent() 函数 与 encodeURI() 函数的区别之处，前者假定它的参数是 URI 的一部分
        /// （比如协议、主机名、路径或查询字符串）。因此 encodeURIComponent() 函数将转义用于分隔 URI 各个部分的标点符号。
        /// </remarks>
        public static string UrlEncode(string str, Encoding encoding)
        {
            if (encoding == null) throw new ArgumentNullException("encoding");
            if (string.IsNullOrEmpty(str)) return str;
            byte[] bytes = encoding.GetBytes(str);
            byte[] result = StringUtil.UrlEncode(bytes, 0, bytes.Length);
            if (object.ReferenceEquals(bytes, result)) return str;
            return Encoding.ASCII.GetString(result);
        }

        /// <summary>
        /// 进行 url 编码. 类似于 JavaScript 中的 encodeURIComponent() 方法（使用UTF-8进行字符编码）.
        /// 该方法不会对 ASCII 字母和数字进行编码，也不会对这些 ASCII 标点符号进行编码： - _ . ! ~ * ' ( ) 。
        /// 其他字符（比如 ：;/?:@&amp;=+$,# 这些用于分隔 URI 组件的标点符号），都是由一个或多个十六进制的转义序列替换的。
        /// </summary>
        /// <param name="str">要编码的字符串.</param>
        /// <returns>url 编码结果</returns>
        /// <remarks>请注意 encodeURIComponent() 函数 与 encodeURI() 函数的区别之处，前者假定它的参数是 URI 的一部分
        /// （比如协议、主机名、路径或查询字符串）。因此 encodeURIComponent() 函数将转义用于分隔 URI 各个部分的标点符号。
        /// </remarks>
        public static string UrlEncode(string str)
        {
            return UrlEncode(str, UTF8NoBOM);
        }

        /// <summary>
        /// 进行 url 解码.
        /// </summary>
        /// <param name="str">要解码的字符串.</param>
        /// <param name="encoding">使用的编码.</param>
        /// <returns>url 解码结果</returns>
        public unsafe static string UrlDecode(string str, Encoding encoding)
        {
            if (encoding == null) throw new ArgumentNullException("encoding");
            if (string.IsNullOrEmpty(str)) return str;

            int length = str.Length;
            fixed (char* pc = str)
            {
                sbyte* pResult = stackalloc sbyte[length];
                int index = 0, j = 0;
                sbyte b;
                bool need = false;
                while (index < length)
                {
                    char ch = pc[index++];
                    if (ch == '+') { b = (sbyte)' '; need = true; }
                    else if (ch == '%' && index + 1 < length)
                    {
                        ch = pc[index++];
                        b = (sbyte)(NumberUtil.GetDigitalValue(ch) << 4 | NumberUtil.GetDigitalValue(pc[index++]));
                    }
                    else b = (sbyte)ch;
                    pResult[j++] = b;
                }
                if (need || length != j) return new string(pResult, 0, j, encoding);
                return str;
            }
        }

        /// <summary>
        /// 进行 url 解码（使用UTF-8进行字符解码）.
        /// </summary>
        /// <param name="str">要解码的字符串.</param>
        /// <returns>url 解码结果</returns>
        public unsafe static string UrlDecode(string str)
        {
            return UrlDecode(str, UTF8NoBOM);
        }

        /// <summary>
        /// 获取数字字符.
        /// </summary>
        /// <param name="value">数字字符值， 0 &lt;= value &lt;= 35.</param>
        /// <returns>
        /// 相应的数字字符，如果返回 *，则表示没有 value 对应的数字字符
        /// </returns>
        internal static byte GetDigital(int value)
        {
            if (value <= 9)
                return (byte)(value + '0');

            const int CHAR_OFFSET_UPPER = 'A' - 10;
            return (byte)(value + CHAR_OFFSET_UPPER);
        }

        /// <summary>
        /// 检查字符是否是一个 url 安全字符.
        /// </summary>
        /// <param name="ch">The ch.</param>
        /// <returns>
        ///   <c>true</c> if the specified ch is safe; otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsUrlSafe(byte ch)
        {
            const byte C_a = (byte)'a';
            const byte C_z = (byte)'z';
            const byte C_A = (byte)'A';
            const byte C_Z = (byte)'Z';
            const byte C_0 = (byte)'0';
            const byte C_9 = (byte)'9';

            const byte C_XG = (byte)'\'';
            const byte C_LB = (byte)'(';
            const byte C_RB = (byte)')';
            const byte C_XH = (byte)'*';
            const byte C_HG = (byte)'-';
            const byte C_YD = (byte)'.';
            const byte C_DX = (byte)'_';
            const byte C_GT = (byte)'!';

            if ((((ch >= C_a) && (ch <= C_z)) || ((ch >= C_A) && (ch <= C_Z))) || ((ch >= C_0) && (ch <= C_9)))
            {
                return true;
            }
            switch (ch)
            {
                case C_XG:
                case C_LB:
                case C_RB:
                case C_XH:
                case C_HG:
                case C_YD:
                case C_DX:
                case C_GT:
                    return true;
            }
            return false;
        }
    }

    public partial class StringUtil
    {
        /// <summary>
        /// 将字符串按照 16 进制转化为 byte 数组
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="separators">分隔符</param>
        /// <returns>转话后的 byte 数组</returns>
        /// <exception cref="NormalException">无法将  + part +  转化为 byte</exception>
        public static byte[] FromHexString(string input, params string[] separators)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            string[] parts = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            byte[] data = new byte[parts.Length];
            for (int i = 0; i < parts.Length; ++i)
            {
                string part = parts[i];
                int v;
                if (NumberUtil.TryParse(part, 16, out v))
                {
                    checked
                    {
                        data[i] = (byte)v;
                    }
                }
                else
                {
                    throw new NormalException("无法将 " + part + " 转化为 byte");
                }
            }

            return data;
        }
    }
}
