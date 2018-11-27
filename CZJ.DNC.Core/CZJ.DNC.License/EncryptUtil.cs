/* ****************************************************************************
 * 描述：EncryptUtil 是一个加密解密用的工具类
 * 作者：陈飞
 * 地点：新立迅
 * 时间：2010.3.26 11:28
 * ****************************************************************************/

using System;
using System.Text;

namespace CZJ.DNC.License
{
    /// <summary>
    /// EncryptUtil 是一个加密解密用的工具类
    /// </summary>
    public static class EncryptUtil
    {
        #region md5

        /// <summary>
        /// 计算 md5.
        /// </summary>
        /// <param name="input">输入.</param>
        /// <param name="size">输入的长度（字节数）.</param>
        /// <returns>MD5</returns>
        public unsafe static byte[] ComputeMD5(IReadable input, ulong size)
        {
            byte[] result = new byte[16];
            fixed (byte* pResult = result)
            {
                byte[] buffer = new byte[64];
                ComputeMD5(input, size, buffer, pResult);
            }
            return result;
        }

        /// <summary>
        /// 计算 md5.
        /// </summary>
        /// <param name="input">输入.</param>
        /// <param name="size">输入的长度（字节数）.</param>
        /// <param name="result">结果通过此数组返回</param>
        public unsafe static void ComputeMD5(IReadable input, ulong size, byte[] result)
        {
            fixed (byte* pResult = result)
            {
                byte[] buffer = new byte[64];
                ComputeMD5(input, size, buffer, pResult);
            }
        }

        /// <summary>
        /// 计算 md5.
        /// </summary>
        /// <param name="input">输入.</param>
        /// <param name="size">输入的长度（字节数）.</param>
        /// <param name="buffer">缓冲区，必须是 64 字节或以上.</param>
        /// <param name="pResult">以此输出结果，结果一定是 16 个字节的，请传入一个 16 字节或以上的指针.</param>
        public unsafe static void ComputeMD5(IReadable input, ulong size, byte[] buffer, byte* pResult)
        {
            // 被忽悠了，常数 A 应为 0x67452301，而不是 0X01234567
            const uint A = 0X67452301U;
            const uint B = 0XEFCDAB89U;
            const uint C = 0X98BADCFEU;
            const uint D = 0X10325476U;

            uint* u32p = (uint*)pResult;
            *u32p = A; u32p[1] = B; u32p[2] = C; u32p[3] = D;
            __MD5(input, size, buffer, u32p);
        }

        /// <summary>
        /// 计算 md5，可以通过 u32p 传入初始参数，以定制自定义的 md5 算法.
        /// </summary>
        /// <param name="input">输入.</param>
        /// <param name="size">输入的长度（字节数）.</param>
        /// <param name="buffer">缓冲区，必须是 64 字节或以上.</param>
        /// <param name="pio">可以通过 pio 传入初始参数，以定制自定义的 md5 算法，同时以此输出结果，结果一定是 16 个字节的，请传入一个 16 字节或以上的指针.</param>
        public unsafe static void __MD5(IReadable input, ulong size, byte[] buffer, void* pio)
        {
            // 分组相关常量: 分组大小（位数）的宽度，分组大小（位数），分组余数掩码
            const int GROUP_SIZE_WIDTH = 6;
            const int GROUP_SIZE = 0x00000001 << GROUP_SIZE_WIDTH;
            const int GROUP_SIZE_MASK = GROUP_SIZE - 1;

            const int DATA_SIZE_SIZE = sizeof(ulong);

            // 完整分组数目
            ulong groupCount = size >> GROUP_SIZE_WIDTH;
            // 不足一组的字节数
            int remain = (int)(size) & GROUP_SIZE_MASK;
            // 用于以 UInt32 操作数据的指针
            fixed (byte* pInput = buffer)
            {
                uint* M = (uint*)pInput;
                uint* u32p = (uint*)pio;

                // 当设置好这四个链接变量后，就开始进入算法的四轮循环运算。循环的次数是信息中512位信息分组的数目。
                // 将上面四个链接变量复制到另外四个变量中：A到a，B到b，C到c，D到d。
                for (ulong index = 0; index < groupCount; ++index)
                {
                    input.Read(buffer, 0, GROUP_SIZE);
                    ProcessGroup(M, u32p);
                }

                int i = input.Read(buffer, 0, remain);
                buffer[i++] = 0x80;
                if (i > (GROUP_SIZE - DATA_SIZE_SIZE))
                {
                    while (i < GROUP_SIZE) buffer[i++] = 0x00;
                    ProcessGroup(M, u32p);
                    i = 0;
                }
                while (i < (GROUP_SIZE - DATA_SIZE_SIZE)) buffer[i++] = 0x00;
                *((ulong*)(pInput + i)) = ((ulong)size) << 3;

                ProcessGroup(M, u32p);

                //uint* pUintResult = (uint*)pResult;
                //pUintResult[0] = a;
                //pUintResult[1] = b;
                //pUintResult[2] = c;
                //pUintResult[3] = d;
            }
        }

        /// <summary>
        /// 计算 md5.
        /// </summary>
        /// <param name="input">输入.</param>
        /// <returns></returns>
        public unsafe static byte[] ComputeMD5(byte[] input)
        {
            return ComputeMD5(input, 0, input.Length);
        }

        /// <summary>
        /// 计算 md5.
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="offset">字节偏移量</param>
        /// <param name="count">字节数</param>
        /// <returns>MD5</returns>
        public unsafe static byte[] ComputeMD5(byte[] input, int offset, int count)
        {
            byte[] result = new byte[16];
            fixed (byte* pInput = input, pResult = result)
            {
                ComputeMD5(pInput + offset, (ulong)count, pResult);
            }
            return result;
        }

        /// <summary>
        /// 计算 md5.
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="result">结果</param>
        public unsafe static void ComputeMD5(byte[] input, byte[] result)
        {
#if DEBUG
            if (input == null) throw new ArgumentNullException("input");
            if (result == null) throw new ArgumentNullException("result");
            if (result.Length != 16) throw new ArgumentException("result");
#endif
            fixed (byte* pInput = input, pResult = result)
            {
                ComputeMD5(pInput, (ulong)input.Length, pResult);
            }
        }

        /// <summary>
        /// 计算 md5.
        /// </summary>
        /// <param name="pInput">输入指针.</param>
        /// <param name="size">输入的长度（字节数）.</param>
        /// <param name="pResult">以此输出结果，结果一定是 16 个字节的，请传入一个 16 字节或以上的指针.</param>
        public unsafe static void ComputeMD5(void* pInput, ulong size, byte* pResult)
        {
            // 被忽悠了，常数 A 应为 0x67452301，而不是 0X01234567
            const uint A = 0X67452301U;
            const uint B = 0XEFCDAB89U;
            const uint C = 0X98BADCFEU;
            const uint D = 0X10325476U;

            uint* u32p = (uint*)pResult;
            *u32p = A; u32p[1] = B; u32p[2] = C; u32p[3] = D;
            __MD5(pInput, size, u32p);
        }

        /// <summary>
        /// 计算 md5，可以通过 u32p 传入初始参数，以定制自定义的 md5 算法.
        /// </summary>
        /// <param name="pInput">输入指针.</param>
        /// <param name="size">输入的长度（字节数）.</param>
        /// <param name="pio">
        /// 可以通过 u32p 传入初始参数，以定制自定义的 md5 算法，同时以此输出结果，结果一定是 16 个字节的，
        /// 请传入一个 16 字节或以上的指针.</param>
        public unsafe static void __MD5(void* pInput, ulong size, uint* pio)
        {
            // 分组相关常量: 分组大小（位数）的宽度，分组大小（位数），分组余数掩码
            const int GROUP_SIZE_WIDTH = 6;
            const int GROUP_SIZE = 0x00000001 << GROUP_SIZE_WIDTH;
            const int GROUP_SIZE_MASK = GROUP_SIZE - 1;

            const int DATA_SIZE_SIZE = sizeof(ulong);

            // 完整分组数目
            ulong groupCount = size >> GROUP_SIZE_WIDTH;
            // 不足一组的字节数
            int remain = (int)(size) & GROUP_SIZE_MASK;
            // 用于以 UInt32 操作数据的指针
            uint* M = (uint*)pInput;
            uint* u32p = (uint*)pio;

            // 当设置好这四个链接变量后，就开始进入算法的四轮循环运算。循环的次数是信息中512位信息分组的数目。
            // 将上面四个链接变量复制到另外四个变量中：A到a，B到b，C到c，D到d。
            for (ulong index = 0; index < groupCount; ++index)
            {
                ProcessGroup(M, u32p);// ref a, ref b, ref c, ref d);
                M += 16;
            }

            byte* buffer = stackalloc byte[GROUP_SIZE];
            byte* pRemain = (byte*)M;
            int i = 0;
            while (i < remain) { buffer[i] = pRemain[i]; ++i; }
            buffer[i++] = 0x80;
            M = (uint*)buffer;
            if (i > GROUP_SIZE - DATA_SIZE_SIZE)
            {
                while (i < GROUP_SIZE) buffer[i++] = 0x00;
                ProcessGroup(M, u32p);// ref a, ref b, ref c, ref d);
                i = 0;
            }
            while (i < GROUP_SIZE - DATA_SIZE_SIZE) buffer[i++] = 0x00;
            *((ulong*)(buffer + i)) = (size) << 3;

            ProcessGroup(M, u32p); //ref a, ref b, ref c, ref d);

            //uint* pUintResult = (uint*)pResult;
            //pUintResult[0] = a;
            //pUintResult[1] = b;
            //pUintResult[2] = c;
            //pUintResult[3] = d;
        }

        /// <summary>
        /// 循环左移位.
        /// </summary>
        /// <param name="x">要移位的数字.</param>
        /// <param name="n">要移的位数.</param>
        /// <returns>循环移位后的结果</returns>
        private static uint ROL(uint x, int n)
        {
            const int WIDTH = sizeof(uint) << 3;
            return (x << n) | (x >> (WIDTH - n));
        }

        private static uint F(uint X, uint Y, uint Z) { return (X & Y) | ((~X) & Z); }
        private static uint G(uint X, uint Y, uint Z) { return (X & Z) | (Y & (~Z)); }
        private static uint H(uint X, uint Y, uint Z) { return X ^ Y ^ Z; }
        private static uint I(uint X, uint Y, uint Z) { return Y ^ (X | (~Z)); }

        private static void FF(ref uint a, uint b, uint c, uint d, uint Mj, int s, uint ti) { a = b + ROL((a + F(b, c, d) + Mj + ti), s); }
        private static void GG(ref uint a, uint b, uint c, uint d, uint Mj, int s, uint ti) { a = b + ROL((a + G(b, c, d) + Mj + ti), s); }
        private static void HH(ref uint a, uint b, uint c, uint d, uint Mj, int s, uint ti) { a = b + ROL((a + H(b, c, d) + Mj + ti), s); }
        private static void II(ref uint a, uint b, uint c, uint d, uint Mj, int s, uint ti) { a = b + ROL((a + I(b, c, d) + Mj + ti), s); }

        private unsafe static void ProcessGroup(uint* M, uint* u32p)// uint* aa, uint* bb, ref uint* cc, uint* dd)
        {
            uint a = *u32p, b = u32p[1], c = u32p[2], d = u32p[3];
            // 主循环有四轮（MD4只有三轮），每轮循环都很相似。第一轮进行16次操作。
            // 每次操作对a、b、c和d中的其中三个作一次非线性函数运算，然后将所得结果
            // 加上第四个变量，文本的一个子分组和一个常数。再将所得结果向右环移一个
            // 不定的数，并加上a、b、c或d中之一。最后用该结果取代a、b、c或d中之一。
            // 以一下是每次操作中用到的四个非线性函数（每轮一个）。

            // （4） 处理位操作函数
            // 首先定义4个辅助函数，每个函数的输入是三个32位的字，输出是一个32位的字。
            // X，Y，Z为32位整数。 

            // F(X,Y,Z) =(X&Y)|((~X)&Z)
            // G(X,Y,Z) =(X&Z)|(Y&(~Z))
            // H(X,Y,Z) =X^Y^Z
            // I(X,Y,Z)=Y^(X|(~Z))
            // （&是与，|是或，~是非，^是异或）
            // 这四个函数的说明：如果X、Y和Z的对应位是独立和均匀的，那么结果的每一位
            // 也应是独立和均匀的。F是一个逐位运算的函数。即，如果X，那么Y，否则Z。函数H是逐位奇偶操作符。
            FF(ref a, b, c, d, M[0], 7, 0xd76aa478U);
            FF(ref d, a, b, c, M[1], 12, 0xe8c7b756U);
            FF(ref c, d, a, b, M[2], 17, 0x242070dbU);
            FF(ref b, c, d, a, M[3], 22, 0xc1bdceeeU);
            FF(ref a, b, c, d, M[4], 7, 0xf57c0fafU);
            FF(ref d, a, b, c, M[5], 12, 0x4787c62aU);
            FF(ref c, d, a, b, M[6], 17, 0xa8304613U);
            FF(ref b, c, d, a, M[7], 22, 0xfd469501U);
            FF(ref a, b, c, d, M[8], 7, 0x698098d8U);
            FF(ref d, a, b, c, M[9], 12, 0x8b44f7afU);
            FF(ref c, d, a, b, M[10], 17, 0xffff5bb1U);
            FF(ref b, c, d, a, M[11], 22, 0x895cd7beU);
            FF(ref a, b, c, d, M[12], 7, 0x6b901122U);
            FF(ref d, a, b, c, M[13], 12, 0xfd987193U);
            FF(ref c, d, a, b, M[14], 17, 0xa679438eU);
            FF(ref b, c, d, a, M[15], 22, 0x49b40821U);

            GG(ref a, b, c, d, M[1], 5, 0xf61e2562U);
            GG(ref d, a, b, c, M[6], 9, 0xc040b340U);
            GG(ref c, d, a, b, M[11], 14, 0x265e5a51U);
            GG(ref b, c, d, a, M[0], 20, 0xe9b6c7aaU);
            GG(ref a, b, c, d, M[5], 5, 0xd62f105dU);
            GG(ref d, a, b, c, M[10], 9, 0x02441453U);
            GG(ref c, d, a, b, M[15], 14, 0xd8a1e681U);
            GG(ref b, c, d, a, M[4], 20, 0xe7d3fbc8U);
            GG(ref a, b, c, d, M[9], 5, 0x21e1cde6U);
            GG(ref d, a, b, c, M[14], 9, 0xc33707d6U);
            GG(ref c, d, a, b, M[3], 14, 0xf4d50d87U);
            GG(ref b, c, d, a, M[8], 20, 0x455a14edU);
            GG(ref a, b, c, d, M[13], 5, 0xa9e3e905U);
            GG(ref d, a, b, c, M[2], 9, 0xfcefa3f8U);
            GG(ref c, d, a, b, M[7], 14, 0x676f02d9U);
            GG(ref b, c, d, a, M[12], 20, 0x8d2a4c8aU);

            HH(ref a, b, c, d, M[5], 4, 0xfffa3942U);
            HH(ref d, a, b, c, M[8], 11, 0x8771f681U);
            HH(ref c, d, a, b, M[11], 16, 0x6d9d6122U);
            HH(ref b, c, d, a, M[14], 23, 0xfde5380cU);
            HH(ref a, b, c, d, M[1], 4, 0xa4beea44U);
            HH(ref d, a, b, c, M[4], 11, 0x4bdecfa9U);
            HH(ref c, d, a, b, M[7], 16, 0xf6bb4b60U);
            HH(ref b, c, d, a, M[10], 23, 0xbebfbc70U);
            HH(ref a, b, c, d, M[13], 4, 0x289b7ec6U);
            HH(ref d, a, b, c, M[0], 11, 0xeaa127faU);
            HH(ref c, d, a, b, M[3], 16, 0xd4ef3085U);
            HH(ref b, c, d, a, M[6], 23, 0x04881d05U);
            HH(ref a, b, c, d, M[9], 4, 0xd9d4d039U);
            HH(ref d, a, b, c, M[12], 11, 0xe6db99e5U);
            HH(ref c, d, a, b, M[15], 16, 0x1fa27cf8U);
            HH(ref b, c, d, a, M[2], 23, 0xc4ac5665U);

            II(ref a, b, c, d, M[0], 6, 0xf4292244U);
            II(ref d, a, b, c, M[7], 10, 0x432aff97U);
            II(ref c, d, a, b, M[14], 15, 0xab9423a7U);
            II(ref b, c, d, a, M[5], 21, 0xfc93a039U);
            II(ref a, b, c, d, M[12], 6, 0x655b59c3U);
            II(ref d, a, b, c, M[3], 10, 0x8f0ccc92U);
            II(ref c, d, a, b, M[10], 15, 0xffeff47dU);
            II(ref b, c, d, a, M[1], 21, 0x85845dd1U);
            II(ref a, b, c, d, M[8], 6, 0x6fa87e4fU);
            II(ref d, a, b, c, M[15], 10, 0xfe2ce6e0U);
            II(ref c, d, a, b, M[6], 15, 0xa3014314U);
            II(ref b, c, d, a, M[13], 21, 0x4e0811a1U);
            II(ref a, b, c, d, M[4], 6, 0xf7537e82U);
            II(ref d, a, b, c, M[11], 10, 0xbd3af235U);
            II(ref c, d, a, b, M[2], 15, 0x2ad7d2bbU);
            II(ref b, c, d, a, M[9], 21, 0xeb86d391U);

            *u32p += a; u32p[1] += b; u32p[2] += c; u32p[3] += d;
        }

        #endregion

        #region old
        private static readonly Encoding s_encoding = Encoding.GetEncoding("iso8859-1");

        /// <summary>
        /// 进行 MD5 加密，并转为 16 进制字符串
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="upper">是否使用大写字母.</param>
        /// <returns></returns>
        public static string MD5EncryptToHex(string input, bool upper)
        {
            byte[] bs = MD5Encrypt(input);
            return NumberUtil.ToHexStringFast(upper, bs);
        }

        /// <summary>
        /// 进行 MD5 加密，并转为 Base64 编码字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5EncryptToBase64(string input)
        {
            return Convert.ToBase64String(MD5Encrypt(input));
        }

        //private static MD5 s_md5 = MD5.Create();

        ///// <summary>
        ///// 进行 MD5 加密
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public static byte[] MD5Encrypt(byte[] input)
        //{
        //    //MD5 md5 = MD5.Create();
        //    return s_md5.ComputeHash(input);
        //}

        /// <summary>
        /// 对字符串进行 MD5 加密
        /// </summary>
        /// <param name="input">要加密的字符串</param>
        /// <returns>加密数据</returns>
        public static byte[] MD5Encrypt(string input)
        {
            return ComputeMD5(s_encoding.GetBytes(input));
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="key">加密密钥</param>
        /// <returns>加密数据</returns>
        public static byte[] ToggleEncrypt(byte[] data, byte[] key)
        {
            return ToggleEncrypt(data, data.Length, key);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="key">加密密钥</param>
        /// <param name="length">要加密的数据的长度（不对所有的数据进行加密）</param>
        /// <returns>加密数据</returns>
        public static byte[] ToggleEncrypt(byte[] data, int length, byte[] key)
        {
            if (key == null) { return data; }
            int keyLength = key.Length;
            if (keyLength == 0) { return data; }
            if (length > data.Length) { throw new NormalException("长度超出范围!"); }

            #region 删除的代码
            //for (int i = 0, j = data.Length; i < j; ++i)
            //{
            //    --j;
            //    data[i] ^= key[i % keyLength];
            //    data[j] ^= key[j % keyLength];
            //    byte tmp = data[i];
            //    data[i] = data[j];
            //    data[j] = tmp;
            //}

            //return data;
            #endregion

            for (int i = 0; i < length; ++i)
            {
                data[i] ^= key[i % keyLength];
            }

            return data;
        }
        #endregion

        /// <summary>
        /// 进行 hash 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="password">加密密码, 不能为空, 长度不能为 0</param>
        /// <param name="additionalKey">加密附加信息, 不能为空, 长度不能为 0</param>
        /// <returns>加密的 hash 值</returns>
        public static byte[] HashEncrypt(byte[] data, byte[] password, byte[] additionalKey)
        {
            // 计算 hash
            byte[] hash = ComputeMD5(data);
            // 从原始数据中抽取部分数据, 与 additionalKey 和 password 进行异或
            int d = data.Length / hash.Length;
            for (int i = 0, j = 0; i < hash.Length; ++i, j += d)
            {
                hash[i] ^= (byte)(data[j] ^ password[i % password.Length] ^ additionalKey[i % additionalKey.Length]);
            }
            // 返回
            return hash;
        }

        /// <summary>
        /// 进行 hash 加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="password">加密密码, 不能为空, 长度不能为 0</param>
        /// <param name="additionalKey">加密附加信息, 不能为空, 长度不能为 0</param>
        /// <returns>加密的 hash 值</returns>
        public static byte[] HashEncrypt(string data, byte[] password, byte[] additionalKey)
        {
            // 计算 hash
            if (data == null)
            {
                return null;
            }
            byte[] hash = ComputeMD5(s_encoding.GetBytes(data));
            // 从原始数据中抽取部分数据, 与 additionalKey 和 password 进行异或
            int d = data.Length / hash.Length;
            for (int i = 0, j = 0; i < hash.Length; ++i, j += d)
            {
                hash[i] ^= (byte)(data[j] ^ password[i % password.Length] ^ additionalKey[i % additionalKey.Length]);
            }
            // 返回
            return hash;
        }
    }
}
