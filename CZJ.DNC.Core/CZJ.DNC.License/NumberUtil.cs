//******************************************************************************
//
// 文 件 名: NumberUtil.cs
//
// 描    述: 与数字操作相关的工具类
//
// 作    者: 陈飞
//
// 地    点: 襄阳运 7 酒店
//
// 创建时间: 2011-12-21
//******************************************************************************

using System;

namespace CZJ.DNC.License
{
	/// <summary>
	/// 与数字操作相关的工具类
	/// </summary>
	public unsafe static class NumberUtil
	{
		#region BitsToString
		/// <summary>
		/// 将 pInput 处开始的字节以 boBase 表示的进制的串表示出来.
		/// </summary>
		/// <param name="pInput">输入内容的首地址.</param>
		/// <param name="toBase">进制位数，即表示 2^toBase 进制，目前只能为
		/// 1：	2 进制
		/// 2：	4 进制
		/// 4：	16 进制
		/// 应满足 toBase * length == sizeof(*pInput)
		/// </param>
		/// <param name="length">输出的长度.</param>
		/// <param name="upper">是否使用大写字母.</param>
		/// <returns>
		/// 将 pInput 处开始的字节以 boBase 表示的进制的串表示
		/// </returns>
		public unsafe static string BitsToStringFast(void* pInput, int toBase, int length, bool upper)
		{
			char* pResult = stackalloc char[length];
			int len = BitsToString(pInput, pResult, toBase, length, upper);
			return new string(pResult, 0, len);
		}

		/// <summary>
		/// 将 pInput 处开始的字节以 boBase 表示的进制的串表示出来.
		/// </summary>
		/// <param name="pInput">输入内容的首地址.</param>
		/// <param name="toBase">进制位数，即表示 2^toBase 进制，目前只能为
		/// 1：	2 进制
		/// 2：	4 进制
		/// 4：	16 进制
		/// 应满足 toBase * length == sizeof(*pInput)
		/// </param>
		/// <param name="length">输出的长度.</param>
		/// <param name="upper">是否使用大写字母.</param>
		/// <returns>
		/// 将 pInput 处开始的字节以 boBase 表示的进制的串表示
		/// </returns>
		public unsafe static string BitsToString(void* pInput, int toBase, int length, bool upper)
		{
			char[] result = new char[length];
			fixed (char* pResult = result)
			{
				int len = BitsToString(pInput, pResult, toBase, length, upper);
				return new string(pResult, 0, len);
			}
		}

		/// <summary>
		/// 将 pInput 处开始的字节以 boBase 表示的进制的串表示出来.
		/// </summary>
		/// <param name="pInput">输入内容的首地址.</param>
		/// <param name="pResult">用于保存结果的指针.</param>
		/// <param name="toBase">进制位数，即表示 2^toBase 进制，目前只能为
		/// 1：	2 进制
		/// 2：	4 进制
		/// 4：	16 进制
		/// 应满足 toBase * length == sizeof(*pInput)
		/// </param>
		/// <param name="length">输出的长度.</param>
		/// <param name="upper">是否使用大写字母.</param>
		/// <returns>结果的长度，应与 length 相等</returns>
		public unsafe static int BitsToString(void* pInput, char* pResult, int toBase, int length, bool upper)
		{
#if DEBUG
			if (pInput == null) throw new ArgumentNullException("pInput");
			if (pResult == null) throw new ArgumentNullException("pResult");
			if (toBase != 1 && toBase != 2 && toBase != 4) throw new ArgumentException("只能为 1, 2　或 4", "toBase");
			if (length < 1) throw new ArgumentException("length");
#endif
			// 用于保存结果的数组
			//char[] result = new char[size];
			byte* p = (byte*)pInput;
			int mask = (1 << toBase) - 1;

			int i = 0, j = 0;
			while (true)
			{
				byte b = p[i++];
				for (int k = 0; k < 8; k += toBase)
				{
					++j;

					pResult[length - j] = GetDigital(b & mask, upper);

					b >>= toBase;
					if (j >= length)
					{
						return length;
					}
				}
			}
		}

		/// <summary>
		/// 将 ulong 数 以 boBase 表示的进制的串表示出来.
		/// </summary>
		/// <param name="input">目标长整形数.</param>
		/// <param name="pResult">用于保存结果的指针.</param>
		/// <param name="toBase">进制数.</param>
		/// <param name="length">输出的长度.</param>
		/// <param name="upper">是否使用大写字母.</param>
		/// <returns>结果的长度，应与 length 相等</returns>
		public unsafe static int ULongToString(ulong input, char* pResult, uint toBase, int length, bool upper)
		{
#if DEBUG
			if (pResult == null) throw new ArgumentNullException("pResult");
			if (length < 1) throw new ArgumentException("length");
#endif

			int index = 0;
			while (index < length)
			{
				pResult[index] = GetDigital((int)(input % toBase), upper);
				input /= toBase;
				++index;
				if (input == 0L) break;
			}

			StringUtil.Reverse(pResult, index);
			return index;
		}

		/// <summary>
		/// 将 ulong 数 以 boBase 表示的进制的串表示出来.
		/// </summary>
		/// <param name="input">目标长整形数.</param>
		/// <param name="toBase">进制数.</param>
		/// <param name="upper">是否使用大写字母</param>
		/// <returns></returns>
		public static string ULongToString(ulong input, uint toBase, bool upper)
		{
			char* memory = stackalloc char[64];
			int length = ULongToString(input, memory, toBase, 64, upper);
			return new string(memory, 0, length);
		}

		/// <summary>
		/// 将 long 数 以 boBase 表示的进制的串表示出来.
		/// </summary>
		/// <param name="input">目标长整形数.</param>
		/// <param name="toBase">进制数.</param>
		/// <param name="upper">是否使用大写字母</param>
		/// <returns></returns>
		public static string LongToString(long input, uint toBase, bool upper)
		{
			if (input < 0L)
			{
				char* memory = stackalloc char[64];
				memory[0] = '-';
				int length = 1 + ULongToString((ulong)-input, memory + 1, toBase, 63, upper);
				return new string(memory, 0, length);
			}
			return ULongToString((ulong)input, toBase, upper);
		}

		#endregion

		#region Digital
		/// <summary>
		/// 获取数字字符.
		/// </summary>
		/// <param name="value">数字字符值， 0 &lt;= value &lt;= 35.</param>
		/// <param name="upper">是否使用大写字母.</param>
		/// <returns>相应的数字字符，如果返回 *，则表示没有 value 对应的数字字符.</returns>
		public static char GetDigital(int value, bool upper)
		{
			const int C_ZERO = '0';
			const int CHAR_OFFSET_UPPER = 'A' - 10;
			const int CHAR_OFFSET_LOWWER = 'a' - 10;

#if DEBUG
			if (value < 0) throw new ArgumentException("value");
#else
			const char CHAR_ERROR = '*';
			if (value < 0) return CHAR_ERROR;
#endif

			if (value <= 9)
				return (char)(value + C_ZERO);

			if (value <= 35)
				if (upper)
					return (char)(value + CHAR_OFFSET_UPPER);
				else
					return (char)(value + CHAR_OFFSET_LOWWER);

#if DEBUG
			throw new ArgumentException("value");
#else
			return CHAR_ERROR;
#endif
		}

		/// <summary>
		/// 获取一个十六进制字符所代表的值.
		/// </summary>
		/// <param name="charValue">数字字符.</param>
		/// <returns>数字字符的数值，如果输入字符不是有效的数字符，则返回 -1.</returns>
		public static int GetDigitalValue(char charValue)
		{
			const int CHAR_0_9 = '0';
			const int CHAR_A_Z = ('A' - 10);
			const int CHAR_a_z = ('a' - 10);

			if (charValue >= '0' && charValue <= '9')
			{
				return charValue - CHAR_0_9;
			}
			if (charValue >= 'a' && charValue <= 'z')
			{
				return charValue - CHAR_a_z;
			}
			if (charValue >= 'A' && charValue <= 'Z')
			{
				return charValue - CHAR_A_Z;
			}

#if DEBUG
			throw new ArgumentException(charValue.ToString() + " 不是有效的数字字符");
#else
			return -1;
#endif
		}
		#endregion

		#region ToHex
		/// <summary>
		/// 将一个字节转换为两位十六进制字符表示.
		/// </summary>
		/// <param name="b">要转换的字节.</param>
		/// <param name="pResult">用于保存结果的指针.</param>
		/// <param name="upper">是否使用大写字母.</param>
		/// <returns>结果的长度，一定为 2.</returns>
		public static int ToHex(byte b, char* pResult, bool upper)
		{
#if DEBUG
			if (pResult == null) throw new ArgumentNullException("pResult");
#endif
			pResult[0] = GetDigital(b >> 4, upper);
			pResult[1] = GetDigital(b & 0x0F, upper);
			return 2;
		}

		/// <summary>
		/// 将一个字节数组转换为十六进制字符数组.
		/// </summary>
		/// <param name="pInput">输入指针.</param>
		/// <param name="count">输入的字节数.</param>
		/// <param name="pResult">用于保存结果的指针.</param>
		/// <param name="upper">是否使用大写字母.</param>
		/// <returns>结果的长度.</returns>
		public unsafe static int ToHex(void* pInput, int count, char* pResult, bool upper)
		{
#if DEBUG
			if (pInput == null) throw new ArgumentNullException("pInput");
			if (pResult == null) throw new ArgumentNullException("pResult");
#endif
			byte* pb = (byte*)pInput;
			int pos = 0;
			for (int i = 0; i < count; ++i)
			{
				byte b = pb[i];
				pos += ToHex(b, pResult + pos, upper);
			}

			return pos;
		}

		/// <summary>
		/// 将一个字节数组转换为十六进制字符数组.
		/// </summary>
		/// <param name="input">输入数组.</param>
		/// <param name="offset">缓冲区数组中开始写入的偏移量.</param>
		/// <param name="count">要读取的字符数.</param>
		/// <param name="pResult">用于保存结果的指针.</param>
		/// <param name="upper">是否使用大写字母.</param>
		/// <returns>结果的长度.</returns>
		public unsafe static int ToHex(byte[] input, int offset, int count, char* pResult, bool upper)
		{
#if DEBUG
			if (input == null) throw new ArgumentNullException("input");
			if (pResult == null) throw new ArgumentNullException("pResult");
			if (offset < 0 || offset >= input.Length) throw new ArgumentOutOfRangeException("offset");
			if (count < 0 || offset + count > input.Length) throw new ArgumentOutOfRangeException("count");
#endif
			fixed (byte* pInput = input)
			{
				return ToHex(pInput + offset, count, pResult, upper);
			}
		}

		/// <summary>
		/// 将一个字节数组转换为十六进制字符数组.
		/// </summary>
		/// <param name="pResult">用于保存结果的指针.</param>
		/// <param name="upper">是否使用大写字母.</param>
		/// <param name="input">输入数组.</param>
		/// <returns>结果的长度.</returns>
		public unsafe static int ToHex(char* pResult, bool upper, params byte[] input)
		{
			return ToHex(input, 0, input.Length, pResult, upper);
		}
		#endregion

		#region ToHexString
		/// <summary>
		/// 将一个字节转换为两位十六进制字符表示.
		/// </summary>
		/// <param name="b">要转换的字节.</param>
		/// <param name="upper">是否使用大写字母.</param>
		/// <returns>字节转换为两位十六进制字符表示.</returns>
		public static string ToHexString(byte b, bool upper)
		{
			char[] result = new char[2];
			fixed (char* pResult = result)
			{
				ToHex(b, pResult, upper);
				return new string(pResult, 0, 2);
			}
		}

		/// <summary>
		/// 将一段数据转换为十六进制字符串.
		/// </summary>
		/// <param name="pInput">输入指针.</param>
		/// <param name="count">输入的字节数.</param>
		/// <param name="upper">是否使用大写字母.</param>
		/// <returns>数据转换为十六进制字符串.</returns>
		public unsafe static string ToHexString(void* pInput, int count, bool upper)
		{
			char[] result = new char[count + count];
			fixed (char* pResult = result)
			{
				count = ToHex(pInput, count, pResult, upper);
				return new string(pResult, 0, count);
			}
		}

		/// <summary>
		/// 将一个字节数组转换为十六进制字符串.
		/// </summary>
		/// <param name="input">输入数组.</param>
		/// <param name="offset">缓冲区数组中开始写入的偏移量.</param>
		/// <param name="count">要读取的字符数.</param>
		/// <param name="upper">是否使用大写字母.</param>
		/// <returns>字节数组转换为十六进制字符串.</returns>
		public unsafe static string ToHexString(byte[] input, int offset, int count, bool upper)
		{
#if DEBUG
			if (input == null) throw new ArgumentNullException("input");
			if (offset < 0 || offset >= input.Length) throw new ArgumentOutOfRangeException("offset");
			if (count < 0 || offset + count > input.Length) throw new ArgumentOutOfRangeException("count");
#endif
			fixed (byte* pInput = input)
			{
				return ToHexString(pInput + offset, count, upper);
			}
		}

		/// <summary>
		/// 将一个字节数组转换为十六进制字符串.
		/// </summary>
		/// <param name="upper">是否使用大写字母.</param>
		/// <param name="input">输入数组.</param>
		/// <returns>字节数组转换为十六进制字符串.</returns>
		public unsafe static string ToHexString(bool upper, params byte[] input)
		{
			return ToHexString(input, 0, input.Length, upper);
		}
		#endregion

		#region ToHexStringFast
		/// <summary>
		/// 将一个字节转换为两位十六进制字符表示.
		/// </summary>
		/// <param name="b">要转换的字节.</param>
		/// <param name="upper">是否使用大写字母.</param>
		/// <returns>字节转换为两位十六进制字符表示.</returns>
		public static string ToHexStringFast(byte b, bool upper)
		{
			char* pResult = stackalloc char[2];
			ToHex(b, pResult, upper);
			return new string(pResult, 0, 2);
		}

		/// <summary>
		/// 将一段数据转换为十六进制字符串.
		/// </summary>
		/// <param name="pInput">输入指针.</param>
		/// <param name="count">输入的字节数.</param>
		/// <param name="upper">是否使用大写字母.</param>
		/// <returns>数据转换为十六进制字符串.</returns>
		public unsafe static string ToHexStringFast(void* pInput, int count, bool upper)
		{
			char* pResult = stackalloc char[count + count];
			count = ToHex(pInput, count, pResult, upper);
			return new string(pResult, 0, count);
		}

		/// <summary>
		/// 将一个字节数组转换为十六进制字符串.
		/// </summary>
		/// <param name="input">输入数组.</param>
		/// <param name="offset">缓冲区数组中开始写入的偏移量.</param>
		/// <param name="count">要读取的字符数.</param>
		/// <param name="upper">是否使用大写字母.</param>
		/// <returns>字节数组转换为十六进制字符串.</returns>
		public unsafe static string ToHexStringFast(byte[] input, int offset, int count, bool upper)
		{
#if DEBUG
			if (input == null) throw new ArgumentNullException("input");
			if (offset < 0 || offset >= input.Length) throw new ArgumentOutOfRangeException("offset");
			if (count < 0 || offset + count > input.Length) throw new ArgumentOutOfRangeException("count");
#endif
			fixed (byte* pInput = input)
			{
				return ToHexStringFast(pInput + offset, count, upper);
			}
		}

		/// <summary>
		/// 将一个字节数组转换为十六进制字符串.
		/// </summary>
		/// <param name="upper">是否使用大写字母.</param>
		/// <param name="input">输入数组.</param>
		/// <returns>字节数组转换为十六进制字符串.</returns>
		public unsafe static string ToHexStringFast(bool upper, params byte[] input)
		{
			return ToHexStringFast(input, 0, input.Length, upper);
		}
		#endregion

		#region HexToByte
		/// <summary>
		/// 将十六进制的字符转换为一个字节.
		/// </summary>
		/// <param name="high">十六进制高位字符.</param>
		/// <param name="low">十六进制低位字符.</param>
		/// <returns>十六进制的字符转换为一个字节</returns>
		public static byte HexToByte(char high, char low)
		{
			return (byte)(GetDigitalValue(high) << 4 | GetDigitalValue(low));
		}

		/// <summary>
		/// 将十六进制字符序列转换为字节数组.
		/// </summary>
		/// <param name="pHexChars">十六进制字符序列输入指针.</param>
		/// <param name="count">十六进制字符序列输入个数，应为 2 的倍数.</param>
		/// <param name="pResult">输出指针，容量应不小于 count / 2.</param>
		/// <returns>结果的字节数.</returns>
		public static int HexToByte(char* pHexChars, int count, byte* pResult)
		{
#if DEBUG
			if (pHexChars == null) throw new ArgumentNullException("pHexChars");
			if (pResult == null) throw new ArgumentNullException("pResult");
			if (count < 0) throw new ArgumentException("count");
#endif
			if (count % 2 != 0) throw new ArgumentException("长度不正确!" + count.ToString(), "count");

			int length = count / 2;
			for (int i = 0; i < length; ++i)
			{
				pResult[i] = HexToByte(pHexChars[i + i], pHexChars[i + i + 1]);
			}

			return length;
		}

		/// <summary>
		/// 将十六进制字符串转换为字节数组
		/// </summary>
		/// <param name="hexString">目标字符串.</param>
		/// <param name="index">开始下标.</param>
		/// <param name="count">十六进制字符序列输入个数，应为 2 的倍数.</param>
		/// <param name="pResult">输出指针，容量应不小于 count / 2.</param>
		/// <returns>结果的字节数.</returns>
		public static int HexToByte(string hexString, int index, int count, byte* pResult)
		{
#if DEBUG
			if (hexString == null) throw new ArgumentNullException("hexString");
#endif
			fixed (char* pHexChars = hexString)
			{
				return HexToByte(pHexChars + index, count, pResult);
			}
		}

		/// <summary>
		/// 将十六进制字符串转换为字节数组
		/// </summary>
		/// <param name="hexString">目标字符串.</param>
		/// <param name="pResult">输出指针，容量应不小于 count / 2.</param>
		/// <returns>结果的字节数.</returns>
		public static int HexToByte(string hexString, byte* pResult)
		{
			return HexToByte(hexString, 0, hexString.Length, pResult);
		}

		/// <summary>
		/// 将十六进制字符序列转换为字节数组.
		/// </summary>
		/// <param name="pHexChars">十六进制字符序列输入指针.</param>
		/// <param name="count">十六进制字符序列输入个数，应为 2 的倍数.</param>
		/// <returns>十六进制字符序列转换为字节数组.</returns>
		public static byte[] HexToByte(char* pHexChars, int count)
		{
			byte[] result = new byte[count / 2];
			fixed (byte* pResult = result)
			{
				HexToByte(pHexChars, count, pResult);
			}
			return result;
		}

		/// <summary>
		/// 将十六进制字符串转换为字节数组.
		/// </summary>
		/// <param name="hexString">目标字符串.</param>
		/// <param name="index">开始下标.</param>
		/// <param name="count">十六进制字符序列输入个数，应为 2 的倍数.</param>
		/// <returns>十六进制字符串转换为字节数组.</returns>
		public static byte[] HexToByte(string hexString, int index, int count)
		{
			fixed (char* pHexChars = hexString)
			{
				return HexToByte(pHexChars + index, count);
			}
		}

		/// <summary>
		/// 将十六进制字符串转换为字节数组.
		/// </summary>
		/// <param name="hexString">目标字符串.</param>
		/// <returns>十六进制字符串转换为字节数组.</returns>
		public static byte[] HexToByte(string hexString)
		{
			return HexToByte(hexString, 0, hexString.Length);
		}
		#endregion

		#region TryParse
		/// <summary>
		/// 尝试将字符串按 toBase 指定的进制数转换为一个整数.
		/// </summary>
		/// <param name="s">输入字符串.</param>
		/// <param name="toBase">toBase 用于指定进制数.</param>
		/// <param name="result">如果转换成功，则以此输出结果，否则为 0.</param>
		/// <returns>
		/// 如果转换成功，则为 true，否则为 false.
		/// </returns>
		public static bool TryParse(string s, int toBase, out int result)
		{
			fixed (char* psz = s)
			{
				return TryParse(psz, s.Length, toBase, out result);
			}
		}

		/// <summary>
		/// 尝试将字符序列按 toBase 指定的进制数转换为一个整数.
		/// </summary>
		/// <param name="psz">指向字符序列的指针.</param>
		/// <param name="length">字符序列的长度.</param>
		/// <param name="toBase">toBase 用于指定进制数.</param>
		/// <param name="result">如果转换成功，则以此输出结果，否则为 0.</param>
		/// <returns>如果转换成功，则为 true，否则为 false.</returns>
		public static unsafe bool TryParse(char* psz, int length, int toBase, out int result)
		{
			if (psz == null) throw new ArgumentNullException("psz");
			if (length < 1) throw new ArgumentException("length");

			result = 0;

			bool negative = false;
			int index = 0;

			//while (Char.IsWhiteSpace(psz[index]) && index < length)
			//{
			//    ++index;
			//}
			// 判断符号位
			char c = psz[index];
			switch (c)
			{
				case '+':
					++index;
					break;
				case '-':
					++index;
					negative = true;
					break;
				default:
					break;
			}

			// 遍历字符串
			while (index < length)
			{
				// 得到字符
				c = psz[index];

				// 处理字符
				int v = GetDigitalValue(c);
				if (v < 0 || v >= toBase)
				{
					result = 0;
					return false;
				}
				result = result * toBase + v;
				++index;
			}

			if (negative) result = -result;
			return true;
		}

		/// <summary>
		/// 尝试将字符串按 toBase 指定的进制数转换为一个整数.
		/// </summary>
		/// <param name="s">输入字符串.</param>
		/// <param name="toBase">toBase 用于指定进制数.</param>
		/// <param name="result">如果转换成功，则以此输出结果，否则为 0.</param>
		/// <returns>
		/// 如果转换成功，则为 true，否则为 false.
		/// </returns>
		public static bool TryParse(string s, int toBase, out long result)
		{
			fixed (char* psz = s)
			{
				return TryParse(psz, s.Length, toBase, out result);
			}
		}

		/// <summary>
		/// 尝试将字符序列按 toBase 指定的进制数转换为一个整数.
		/// </summary>
		/// <param name="psz">指向字符序列的指针.</param>
		/// <param name="length">字符序列的长度.</param>
		/// <param name="toBase">toBase 用于指定进制数.</param>
		/// <param name="result">如果转换成功，则以此输出结果，否则为 0.</param>
		/// <returns>如果转换成功，则为 true，否则为 false.</returns>
		public static unsafe bool TryParse(char* psz, int length, int toBase, out long result)
		{
			if (psz == null) throw new ArgumentNullException("psz");
			if (length < 1) throw new ArgumentException("length");

			result = 0L;

			bool negative = false;
			int index = 0;

			while (Char.IsWhiteSpace(psz[index]) && index < length)
			{
				++index;
			}

			// 判断符号位
			char c = psz[index];
			switch (c)
			{
				case '+':
					++index;
					break;
				case '-':
					++index;
					negative = true;
					break;
				default:
					break;
			}

			// 遍历字符串
			while (index < length)
			{
				// 得到字符
				c = psz[index];

				// 处理字符
				int v = GetDigitalValue(c);
				if (v < 0 || v >= toBase)
				{
					result = 0L;
					return false;
				}
				result = result * toBase + v;
				++index;
			}

			if (negative) result = -result;
			return true;
		}
		#endregion
	}
}
