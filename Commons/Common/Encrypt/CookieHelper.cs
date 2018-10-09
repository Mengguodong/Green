using System;
using System.Configuration;
using System.Web;

namespace Common
{
    /// <summary>
    ///     cookie操作公用方法
    ///     author:baochen 2014年6月13日16:01:51
    /// </summary>
    public class CookieHelper
    {
        private static readonly string CookieDomain = ConfigurationManager.AppSettings["domain"];

        /// <summary>
        ///     设置加密过的COOKIE
        ///     
        /// </summary>
        /// <param name="CookieName"></param>
        /// <param name="CookieValue"></param>
        public static void SetCookie(string CookieName, string CookieValue)
        {
            SetCookie(CookieName, CookieValue, null);
        }

        /// <summary>
        ///     设置加密过的COOKIE
        ///    
        /// </summary>
        /// <param name="CookieName"></param>
        /// <param name="CookieValue"></param>
        /// <param name="ExpireTime"></param>
        public static void SetCookie(string CookieName, string CookieValue, DateTime? ExpireTime)
        {
            var cookie = new HttpCookie(CookieName);
            cookie.Domain = CookieDomain;
            if (ExpireTime != null)
            {
                cookie.Expires = (DateTime) ExpireTime;
            }
            CookieValue = RC4Encrypt(CookieValue);
            cookie.Value = CookieValue;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        ///     读取加密过的COOKIE
        ///     
        /// </summary>
        /// <param name="CookieName"></param>
        /// <returns></returns>
        public static string GetCookie(string CookieName)
        {
            var CookieValue = string.Empty;
            var cookie = HttpContext.Current.Request.Cookies[CookieName];
            if (cookie != null)
            {
                cookie.Domain = CookieDomain;
                CookieValue = RC4Decrypt(cookie.Value);
            }
            return CookieValue;
        }

        /// <summary>
        ///     删除COOKIE
        ///     
        /// </summary>
        /// <param name="CookieName"></param>
        public static void DelCookie(string CookieName)
        {
            SetCookie(CookieName, "", DateTime.Now.AddYears(-1));
        }

        /// <summary>
        ///     加密
        ///     
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string RC4Encrypt(string param)
        {
            var rc4 = new RC4();
            param = HttpUtility.UrlEncode(rc4.Encrypt(param, "wine"));
            return param;
        }

        /// <summary>
        ///     解密
        ///     
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string RC4Decrypt(string param)
        {
            var rc4 = new RC4();
            try
            {
                param = rc4.Decrypt(param, "wine");
            }
            catch
            {
                param = rc4.Decrypt(HttpUtility.UrlDecode(param), "wine");
            }
            return param;
        }
    }
}