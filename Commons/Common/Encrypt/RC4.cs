﻿using System;

namespace Common
{
    /// <summary>
    ///     Rc4加密算法
    ///    
    /// </summary>
    public class RC4 : BaseRC4
    {
        public static RC4 rc4 = new RC4();

        public override Byte[] EncryptEx(Byte[] data, String pass)
        {
            if (data == null || pass == null) return null;
            var output = new Byte[data.Length];
            Int64 i = 0;
            Int64 j = 0;
            var mBox = GetKey(Encode.GetBytes(pass), 256);
            // 加密
            for (Int64 offset = 0; offset < data.Length; offset++)
            {
                i = (i + 1)%mBox.Length;
                j = (j + mBox[i])%mBox.Length;
                var temp = mBox[i];
                mBox[i] = mBox[j];
                mBox[j] = temp;
                var a = data[offset];
                //Byte b = mBox[(mBox[i] + mBox[j] % mBox.Length) % mBox.Length];
                // mBox[j] 一定比 mBox.Length 小，不需要在取模
                var b = mBox[(mBox[i] + mBox[j])%mBox.Length];
                output[offset] = (Byte) (a ^ b);
            }
            return output;
        }

        public override Byte[] DecryptEx(Byte[] data, String pass)
        {
            return EncryptEx(data, pass);
        }

        /// <summary>
        ///     打乱密码
        /// </summary>
        /// <param name="pass">密码</param>
        /// <param name="kLen">密码箱长度</param>
        /// <returns>打乱后的密码</returns>
        private static Byte[] GetKey(Byte[] pass, Int32 kLen)
        {
            var mBox = new Byte[kLen];
            for (Int64 i = 0; i < kLen; i++)
            {
                mBox[i] = (Byte) i;
            }
            Int64 j = 0;
            for (Int64 i = 0; i < kLen; i++)
            {
                j = (j + mBox[i] + pass[i%pass.Length])%kLen;
                var temp = mBox[i];
                mBox[i] = mBox[j];
                mBox[j] = temp;
            }
            return mBox;
        }
    }
}