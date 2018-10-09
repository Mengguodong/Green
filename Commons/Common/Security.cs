using System;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    /// <summary>
    ///     加密算法，不可逆
    ///     des:胡云锋
    ///     date:2015-04-15
    /// </summary>
    public class Security
    {
        /// <summary>
        ///     256位散列加密（不可逆）
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>密文</returns>
        public static string Sha256(string plainText)
        {
            var _sha256 = new SHA256Managed();
            var _cipherText = _sha256.ComputeHash(Encoding.Default.GetBytes(plainText));
            return Convert.ToBase64String(_cipherText);
        }

        /// <summary>
        /// MD5 16位加密
        /// xiejiang
        /// </summary>
        /// <param name="ConvertString"></param>
        /// <returns></returns>

        public static string GetMd5Str(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }
    }
}