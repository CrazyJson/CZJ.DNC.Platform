// Copyright (c) 2007, Citms and/or its affiliates. All rights reserved.
// CITMS PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
// Author: chenf
// Create: 2018/2/4 20:42:17
// Remark: 自定义MD5

using System.Runtime.InteropServices;

namespace CZJ.DNC.License
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MD5KEY
    {
        public uint k1;
        public uint k2;
        public uint k3;
        public uint k4;
    }

    /// <summary>
    /// 自定义MD5
    /// </summary>
    public class CustomMD5
    {
        public static readonly CustomMD5 MD5 = new CustomMD5(
            0X67452301U,
            0XEFCDAB89U,
            0X98BADCFEU,
            0X10325476U
        );

        private readonly MD5KEY key;

        public CustomMD5(uint k1, uint k2, uint k3, uint k4)
        {
            this.key = new MD5KEY
            {
                k1 = k1,
                k2 = k2,
                k3 = k3,
                k4 = k4
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public unsafe CustomMD5(params byte[] key)
        {
            if (key == null || key.Length != 16)
            {
                throw new NormalException("key的长度必须为16");
            }
            fixed (MD5KEY* k = &this.key)
            {
                byte* pk = (byte*)k;
                for (int i = 0; i < 16; i++)
                {
                    pk[i] = key[i];
                }
            }
        }

        public CustomMD5(string key) : this(StringUtil.UTF8NoBOM.GetBytes(key))
        {

        }

        /// <summary>
        /// 计算 md5.
        /// </summary>
        /// <param name="pInput">输入指针.</param>
        /// <param name="size">输入的长度（字节数）.</param>
        /// <param name="pResult">以此输出结果，结果一定是 16 个字节的，请传入一个 16 字节或以上的指针.</param>
        public unsafe void ComputeMD5(void* pInput, ulong size, byte* pResult)
        {
            MD5KEY* p = (MD5KEY*)pResult;
            *p = this.key;
            EncryptUtil.__MD5(pInput, size, (uint*)p);
        }

        /// <summary>
        /// 计算 md5.
        /// </summary>
        /// <param name="input">输入.</param>
        /// <param name="size">输入的长度（字节数）.</param>
        /// <param name="buffer">缓冲区，必须是 64 字节或以上.</param>
        /// <param name="pResult">以此输出结果，结果一定是 16 个字节的，请传入一个 16 字节或以上的指针.</param>
        public unsafe void ComputeMD5(IReadable input, ulong size, byte[] buffer, byte* pResult)
        {
            MD5KEY* p = (MD5KEY*)pResult;
            *p = this.key;
            EncryptUtil.__MD5(input, size, buffer, (uint*)p);
        }

        /// <summary>
        /// 计算 md5.
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="offset">字节偏移量</param>
        /// <param name="count">字节数</param>
        /// <returns>MD5</returns>
        public unsafe byte[] ComputeMD5(byte[] input, int offset, int count)
        {
            byte[] result = new byte[16];
            fixed (byte* pInput = input, pResult = result)
            {
                this.ComputeMD5(pInput + offset, (ulong)count, pResult);
            }
            return result;
        }

        /// <summary>
        /// 计算 MD5.
        /// </summary>
        /// <param name="buffer">待计算的数据缓冲区</param>
        /// <param name="offset">字节偏移量</param>
        /// <param name="count">字节数</param>
        /// <returns>MD5</returns>
        public byte[] GetMD5(byte[] buffer, int offset, int count)
        {
            return this.ComputeMD5(buffer, offset, count);
            //return md5.ComputeHash(data, offset, count);
        }

        /// <summary>
        /// 计算 md5.
        /// </summary>
        /// <param name="input">输入.</param>
        /// <returns></returns>
        public unsafe byte[] ComputeMD5(byte[] input)
        {
            return this.ComputeMD5(input, 0, input.Length);
        }

        /// <summary>
        /// 计算 MD5.
        /// </summary>
        /// <param name="data">待计算的数据</param>
        /// <returns>MD5</returns>
        public byte[] GetMD5(byte[] data)
        {
            return this.ComputeMD5(data);
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
        public unsafe string GetMD5String(byte[] buffer, int offset, int count, bool upper)
        {
            //return NumberUtil.ToHexStringFast(upper, GetMD5(data, offset, count));
            byte* pResult = stackalloc byte[16];
            //byte[] result = new byte[16];
            fixed (byte* pInput = buffer)
            {
                this.ComputeMD5(pInput + offset, (ulong)count, pResult);
            }
            return NumberUtil.ToHexString(pResult, 16, upper);
        }

        /// <summary>
        /// 计算 MD5 字符串.
        /// </summary>
        /// <param name="data">待计算的数据</param>
		/// <param name="upper">是否使用大写字母.</param>
        /// <returns>MD5字符串</returns>
        public string GetMD5String(byte[] data, bool upper)
        {
            return this.GetMD5String(data, 0, data.Length, upper);
        }

        /// <summary>
        /// 将str以UTF8编码，并计算 MD5 字符串.
        /// </summary>
        /// <param name="str">待计算的字符串</param>
        /// <param name="upper">是否使用大写字母.</param>
        /// <returns>MD5字符串</returns>
        public string GetMD5String(string str, bool upper)
        {
            byte[] data = StringUtil.UTF8NoBOM.GetBytes(str);
            return this.GetMD5String(data, 0, data.Length, upper);
        }
    }
}
