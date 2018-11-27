//******************************************************************************
//
// 文 件 名: BitHelper.cs
//
// 描    述: 用于处理和字节相关的工具类
//
// 作    者: 陈飞
//
// 地    点: 中科通达
//
// 创建时间: 2011-09-21
//
// 修改历史: 2011-09-21 陈飞创建
//******************************************************************************

using System;
using System.Text;

namespace CZJ.DNC.License
{
	/// <summary>
	/// 用于处理和字节相关的工具类
	/// </summary>
	public class BitHelper
	{
		#region Fields

		/// <summary>用于保存操作之前的下标</summary>
		private int _preIndex;

		#endregion

		#region Properties

		private byte[] _buffer;

		/// <summary>获取或设置要进行操作的字节数组</summary>
		public byte[] Buffer
		{
			get { return _buffer; }
			set { _buffer = value; }
		}

		private int _index;

		/// <summary>获取或设置当前位置</summary>
		public int Index
		{
			get { return _index; }
			set { _index = value; }
		}

		#endregion

		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="BitHelper"/> class.
		/// </summary>
		public BitHelper()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BitHelper"/> class.
		/// </summary>
		/// <param name="buffer">要进行操作的字节数组.</param>
		/// <param name="index">当前位置.</param>
		public BitHelper(byte[] buffer, int index)
		{
			this.Buffer = buffer;
			this.Index = index;
		}

		#endregion

		#region Static Methods

		#region Part0
		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 64 位有符号整数.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <returns>返回由字节数组中指定位置的四个字节转换来的 64 位有符号整数.</returns>
		public static long ToInt64(byte[] buffer, int index)
		{
			long result =
				(long)buffer[index]
				| ((long)buffer[index + 1] << 8)
				| ((long)buffer[index + 2] << 16)
				| ((long)buffer[index + 3] << 24)
				| ((long)buffer[index + 4] << 32)
				| ((long)buffer[index + 5] << 40)
				| ((long)buffer[index + 6] << 48)
				| ((long)buffer[index + 7] << 56);

			return result;
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 32 位有符号整数.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <returns>返回由字节数组中指定位置的四个字节转换来的 32 位有符号整数.</returns>
		public static int ToInt32(byte[] buffer, int index)
		{
			int result =
				buffer[index]
				| (buffer[index + 1] << 8)
				| (buffer[index + 2] << 16)
				| (buffer[index + 3] << 24);

			return result;
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 16 位有符号整数.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <returns>返回由字节数组中指定位置的四个字节转换来的 16 位有符号整数.</returns>
		public static short ToInt16(byte[] buffer, int index)
		{
			short result =
				(short)(buffer[index]
				| (buffer[index + 1] << 8));

			return result;
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 8 位无符号整数.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <returns>返回由字节数组中指定位置的四个字节转换来的 8 位无符号整数.</returns>
		public static byte ToByte(byte[] buffer, int index)
		{
			byte result =
				buffer[index];

			return result;
		}
		#endregion

		#region Part1
		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 64 位有符号整数.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <returns>返回由字节数组中指定位置的四个字节转换来的 64 位有符号整数.</returns>
		public static long ToInt64(byte[] buffer, ref int index)
		{
			const int SIZE = sizeof(long);
			long result = ToInt64(buffer, index);
			index += SIZE;
			return result;
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 32 位有符号整数.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <returns>返回由字节数组中指定位置的四个字节转换来的 32 位有符号整数.</returns>
		public static int ToInt32(byte[] buffer, ref int index)
		{
			const int SIZE = sizeof(int);
			int result = ToInt32(buffer, index);
			index += SIZE;
			return result;
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 16 位有符号整数.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <returns>返回由字节数组中指定位置的四个字节转换来的 16 位有符号整数.</returns>
		public static short ToInt16(byte[] buffer, ref int index)
		{
			const int SIZE = sizeof(short);
			short result = ToInt16(buffer, index);
			index += SIZE;
			return result;
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 8 位无符号整数.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <returns>返回由字节数组中指定位置的四个字节转换来的 8 位无符号整数.</returns>
		public static byte ToByte(byte[] buffer, ref int index)
		{
			const int SIZE = sizeof(byte);
			byte result = ToByte(buffer, index);
			index += SIZE;
			return result;
		}
		#endregion

		#region Part2
		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 64 位无符号整数.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <returns>返回由字节数组中指定位置的四个字节转换来的 64 位无符号整数.</returns>
		public static ulong ToUInt64(byte[] buffer, ref int index)
		{
			return (ulong)ToInt64(buffer, ref index);
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 32 位无符号整数.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <returns>返回由字节数组中指定位置的四个字节转换来的 32 位无符号整数.</returns>
		public static uint ToUInt32(byte[] buffer, ref int index)
		{
			return (uint)ToInt32(buffer, ref index);
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 16 位无符号整数.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <returns>返回由字节数组中指定位置的四个字节转换来的 16 位无符号整数.</returns>
		public static ushort ToUInt16(byte[] buffer, ref int index)
		{
			return (ushort)ToInt16(buffer, ref index);
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 8 位有符号整数.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <returns>返回由字节数组中指定位置的四个字节转换来的 8 位有符号整数.</returns>
		public static sbyte ToSByte(byte[] buffer, ref int index)
		{
			return (sbyte)ToByte(buffer, ref index);
		}
		#endregion

		#region Part3
		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 64 位无符号整数.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <returns>返回由字节数组中指定位置的四个字节转换来的 64 位无符号整数.</returns>
		public static ulong ToUInt64(byte[] buffer, int index)
		{
			return (ulong)ToInt64(buffer, index);
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 32 位无符号整数.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <returns>返回由字节数组中指定位置的四个字节转换来的 32 位无符号整数.</returns>
		public static uint ToUInt32(byte[] buffer, int index)
		{
			return (uint)ToInt32(buffer, index);
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 16 位无符号整数.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <returns>返回由字节数组中指定位置的四个字节转换来的 16 位无符号整数.</returns>
		public static ushort ToUInt16(byte[] buffer, int index)
		{
			return (ushort)ToInt16(buffer, index);
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 8 位有符号整数.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <returns>返回由字节数组中指定位置的四个字节转换来的 8 位有符号整数.</returns>
		public static sbyte ToSByte(byte[] buffer, int index)
		{
			return (sbyte)ToByte(buffer, index);
		}
		#endregion

		/// <summary>
		/// 返回由字节数组中指定位置的以 0 为结尾的字符串（或为最大长度 size），字符串在 buffer 中的最大占用字节数为 size.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <param name="size">字符串在 buffer 中的最大占用字节数.</param>
		/// <param name="encoding">要使用的编码.</param>
		/// <returns>
		/// 返回由字节数组中指定位置的以 0 为结尾的字符串（或为最大长度 size），字符串在 buffer 中的最大占用字节数为 size.
		/// </returns>
		public static string ToString(byte[] buffer, int index, int size, Encoding encoding)
		{
			//int length = 0;
			//while (length < size)
			//{
			//    if (buffer[index + length] == 0)
			//    {
			//        break;
			//    }
			//    ++length;
			//}

			//string result = encoding.GetString(buffer, index, length);
			//index += size;

			//return result;

			//int length = size;
			//while (length > 0)
			//{
			//    if (buffer[index + --length] != 0)
			//    {
			//        break;
			//    }
			//}

			//string result = encoding.GetString(buffer, index, length + 1);
			////index += size;

			//return result;
			int length = 0;
			while (length < size)
			{
				if (buffer[index + length] == 0) break;
				++length;
			}
			string result = encoding.GetString(buffer, index, length);
			index += size;

			return result;
		}

		/// <summary>
		/// 返回由字节数组中指定位置的以 0 为结尾的字符串（或为最大长度 size），字符串在 buffer 中的最大占用字节数为 size.
		/// </summary>
		/// <param name="buffer">字节数组.</param>
		/// <param name="index">起始地址.</param>
		/// <param name="size">字符串在 buffer 中的最大占用字节数.</param>
		/// <param name="encoding">要使用的编码.</param>
		/// <returns>
		/// 返回由字节数组中指定位置的以 0 为结尾的字符串（或为最大长度 size），字符串在 buffer 中的最大占用字节数为 size.
		/// </returns>
		public static string ToString(byte[] buffer, ref int index, int size, Encoding encoding)
		{
			string result = ToString(buffer, index, size, encoding);
			index += size;
			return result;
		}

		#region GetBytes Part0

		/// <summary>
		/// 将 64 位有符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		public static void GetBytes(long value, byte[] buffer, int index)
		{
			const int SIZE = sizeof(long);
			for (int i = 0; i != SIZE; ++i)
			{
				buffer[index + i] = (byte)(value & 0xFF);
				value >>= 8;
			}
		}

		/// <summary>
		/// 将 32 位有符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		public static void GetBytes(int value, byte[] buffer, int index)
		{
			const int SIZE = sizeof(int);
			for (int i = 0; i != SIZE; ++i)
			{
				buffer[index + i] = (byte)(value & 0xFF);
				value >>= 8;
			}
		}

		/// <summary>
		/// 将 16 位有符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		public static void GetBytes(short value, byte[] buffer, int index)
		{
			const int SIZE = sizeof(short);
			for (int i = 0; i != SIZE; ++i)
			{
				buffer[index + i] = (byte)(value & 0xFF);
				value >>= 8;
			}
		}

		/// <summary>
		/// 将 8 位无符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		public static void GetBytes(byte value, byte[] buffer, int index)
		{
			const int SIZE = sizeof(byte);
			for (int i = 0; i != SIZE; ++i)
			{
				buffer[index + i] = (byte)(value & 0xFF);
				value >>= 8;
			}
		}

		#endregion

		#region GetBytes Part1

		/// <summary>
		/// 将 64 位有符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		public static void GetBytes(long value, byte[] buffer, ref int index)
		{
			const int SIZE = sizeof(long);
			GetBytes(value, buffer, index);
			index += SIZE;
		}

		/// <summary>
		/// 将 32 位有符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		public static void GetBytes(int value, byte[] buffer, ref int index)
		{
			const int SIZE = sizeof(int);
			GetBytes(value, buffer, index);
			index += SIZE;
		}

		/// <summary>
		/// 将 16 位有符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		public static void GetBytes(short value, byte[] buffer, ref int index)
		{
			const int SIZE = sizeof(short);
			GetBytes(value, buffer, index);
			index += SIZE;
		}

		/// <summary>
		/// 将 8 位无符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		public static void GetBytes(byte value, byte[] buffer, ref int index)
		{
			const int SIZE = sizeof(byte);
			GetBytes(value, buffer, index);
			index += SIZE;
		}

		#endregion

		#region GetBytes Part2

		/// <summary>
		/// 将 64 位无符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		public static void GetBytes(ulong value, byte[] buffer, ref int index)
		{
			GetBytes((long)value, buffer, ref index);
		}

		/// <summary>
		/// 将 32 位无符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		public static void GetBytes(uint value, byte[] buffer, ref int index)
		{
			GetBytes((int)value, buffer, ref index);
		}

		/// <summary>
		/// 将 16 位无符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		public static void GetBytes(ushort value, byte[] buffer, ref int index)
		{
			GetBytes((short)value, buffer, ref index);
		}

		/// <summary>
		/// 将 8 位有符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		public static void GetBytes(sbyte value, byte[] buffer, ref int index)
		{
			GetBytes((byte)value, buffer, ref index);
		}

		#endregion

		#region GetBytes Part2

		/// <summary>
		/// 将 64 位无符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		public static void GetBytes(ulong value, byte[] buffer, int index)
		{
			GetBytes((long)value, buffer, index);
		}

		/// <summary>
		/// 将 32 位无符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		public static void GetBytes(uint value, byte[] buffer, int index)
		{
			GetBytes((int)value, buffer, index);
		}

		/// <summary>
		/// 将 16 位无符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		public static void GetBytes(ushort value, byte[] buffer, int index)
		{
			GetBytes((short)value, buffer, index);
		}

		/// <summary>
		/// 将 8 位有符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		public static void GetBytes(sbyte value, byte[] buffer, int index)
		{
			GetBytes((byte)value, buffer, index);
		}

		#endregion

		/// <summary>
		/// 将字符串转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置，并以 0 为结尾，最大长度不超过 size.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		/// <param name="size">字符串在 buffer 中的最大占用字节数.</param>
		/// <param name="encoding">要使用的编码.</param>
		public static void GetBytes(string value, byte[] buffer, int index, int size, Encoding encoding)
		{
			byte[] result = encoding.GetBytes(value);
			int length = Math.Min(size, result.Length);
			System.Buffer.BlockCopy(result, 0, buffer, index, length);
			if (size > result.Length)
			{
				//buffer[index + result.Length] = 0;
				for (int i = result.Length; i < size; ++i)
				{
					buffer[index + i] = 0;
				}
			}
			//index += size;
		}

		/// <summary>
		/// 将字符串转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置，并以 0 为结尾，最大长度不超过 size.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="buffer">用于保存结果的缓冲区.</param>
		/// <param name="index">要放入的起始位置.</param>
		/// <param name="size">字符串在 buffer 中的最大占用字节数.</param>
		/// <param name="encoding">要使用的编码.</param>
		public static void GetBytes(string value, byte[] buffer, ref int index, int size, Encoding encoding)
		{
			GetBytes(value, buffer, index, size, encoding);
			index += size;
		}

		#endregion

		#region Instance Methods

		#region Part1

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 64 位有符号整数.
		/// </summary>
		/// <returns>
		/// 返回由字节数组中指定位置的四个字节转换来的 64 位有符号整数.
		/// </returns>
		public long ToInt64()
		{
			SaveIndex();
			return ToInt64(_buffer, ref _index);
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 32 位有符号整数.
		/// </summary>
		/// <returns>
		/// 返回由字节数组中指定位置的四个字节转换来的 32 位有符号整数.
		/// </returns>
		public int ToInt32()
		{
			SaveIndex();
			return ToInt32(_buffer, ref _index);
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 16 位有符号整数.
		/// </summary>
		/// <returns>
		/// 返回由字节数组中指定位置的四个字节转换来的 16 位有符号整数.
		/// </returns>
		public short ToInt16()
		{
			SaveIndex();
			return ToInt16(_buffer, ref _index);
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 8 位无符号整数.
		/// </summary>
		/// <returns>
		/// 返回由字节数组中指定位置的四个字节转换来的 8 位无符号整数.
		/// </returns>
		public byte ToByte()
		{
			SaveIndex();
			return ToByte(_buffer, ref _index);
		}

		#endregion

		#region Part2
		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 64 位无符号整数.
		/// </summary>
		/// <returns>
		/// 返回由字节数组中指定位置的四个字节转换来的 64 位无符号整数.
		/// </returns>
		public ulong ToUInt64()
		{
			SaveIndex();
			return ToUInt64(_buffer, ref _index);
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 32 位无符号整数.
		/// </summary>
		/// <returns>
		/// 返回由字节数组中指定位置的四个字节转换来的 32 位无符号整数.
		/// </returns>
		public uint ToUInt32()
		{
			SaveIndex();
			return ToUInt32(_buffer, ref _index);
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 16 位无符号整数.
		/// </summary>
		/// <returns>
		/// 返回由字节数组中指定位置的四个字节转换来的 16 位无符号整数.
		/// </returns>
		public ushort ToUInt16()
		{
			SaveIndex();
			return ToUInt16(_buffer, ref _index);
		}

		/// <summary>
		/// 返回由字节数组中指定位置的四个字节转换来的 8 位有符号整数.
		/// </summary>
		/// <returns>
		/// 返回由字节数组中指定位置的四个字节转换来的 8 位有符号整数.
		/// </returns>
		public sbyte ToSByte()
		{
			SaveIndex();
			return ToSByte(_buffer, ref _index);
		}

		#endregion

		/// <summary>
		/// 返回由字节数组中指定位置的以 0 为结尾的字符串（或为最大长度 size），字符串在 buffer 中的最大占用字节数为 size.
		/// </summary>
		/// <param name="size">字符串在 buffer 中的最大占用字节数.</param>
		/// <param name="encoding">要使用的编码.</param>
		/// <returns>
		/// 返回由字节数组中指定位置的以 0 为结尾的字符串（或为最大长度 size），字符串在 buffer 中的最大占用字节数为 size.
		/// </returns>
		public string ToString(int size, Encoding encoding)
		{
#if DEBUG
			if (encoding == null) throw new ArgumentNullException("encoding");
#endif
			SaveIndex();
			return ToString(_buffer, ref _index, size, encoding);
		}

		#region GetBytes Part1

		/// <summary>
		/// 将 64 位有符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		public BitHelper GetBytes(long value)
		{
			SaveIndex();
			GetBytes(value, _buffer, ref _index);
			return this;
		}

		/// <summary>
		/// 将 32 位有符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		public BitHelper GetBytes(int value)
		{
			SaveIndex();
			GetBytes(value, _buffer, ref _index);
			return this;
		}

		/// <summary>
		/// 将 16 位有符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		public BitHelper GetBytes(short value)
		{
			SaveIndex();
			GetBytes(value, _buffer, ref _index);
			return this;
		}

		/// <summary>
		/// 将 8 位无符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		public BitHelper GetBytes(byte value)
		{
			SaveIndex();
			GetBytes(value, _buffer, ref _index);
			return this;
		}

		#endregion

		#region GetBytes Part2

		/// <summary>
		/// 将 64 位无符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		public BitHelper GetBytes(ulong value)
		{
			SaveIndex();
			GetBytes(value, _buffer, ref _index);
			return this;
		}

		/// <summary>
		/// 将 32 位无符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		public BitHelper GetBytes(uint value)
		{
			SaveIndex();
			GetBytes(value, _buffer, ref _index);
			return this;
		}

		/// <summary>
		/// 将 16 位无符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		public BitHelper GetBytes(ushort value)
		{
			SaveIndex();
			GetBytes(value, _buffer, ref _index);
			return this;
		}

		/// <summary>
		/// 将 8 位有符号整数转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		public BitHelper GetBytes(sbyte value)
		{
			SaveIndex();
			GetBytes(value, _buffer, ref _index);
			return this;
		}

		#endregion

		/// <summary>
		/// 将字符串转换为字节表示，字节放入到 buffer 中，index 为要放入的起始位置，并以 0 为结尾，最大长度不超过 size.
		/// </summary>
		/// <param name="value">要转换的数字.</param>
		/// <param name="size">字符串在 buffer 中的最大占用字节数.</param>
		/// <param name="encoding">要使用的编码.</param>
		public BitHelper GetBytes(string value, int size, Encoding encoding)
		{
#if DEBUG
			if (encoding == null) throw new ArgumentNullException("encoding");
#endif
			SaveIndex();
			GetBytes(value, _buffer, ref _index, size, encoding);
			return this;
		}

		/// <summary>
		/// 将下标返回退到刚执行操作前的状态并返回，只支持一次回退.
		/// </summary>
		/// <returns></returns>
		public BitHelper Back()
		{
			_index = _preIndex;
			return this;
		}

		/// <summary>
		/// Saves the index.
		/// </summary>
		private void SaveIndex()
		{
			_preIndex = _index;
		}

		#endregion
	}
}
