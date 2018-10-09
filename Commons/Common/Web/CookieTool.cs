using System;

namespace Common.Web
{
    /// <summary>
    ///     COOKIE存储，删除等操作集中营
    ///     创建人:
    ///     创建时间:2014-10-20
    /// </summary>
    public class CookieTool
    {
        //private static string _CookieDomain = System.Configuration.ConfigurationManager.AppSettings["domain"].ToString();

        /// <summary>
        ///     添加登陆Cookie
        /// </summary>
        /// <param name="userDate">数据</param>
        /// <param name="cookieName">COOKIE名称</param>
        /// <param name="isautologin">是否记住登录（记住登录保存有效期为30天）</param>
        public static void SetLoginCookie(string userDate, string cookieName, bool isautologin)
        {
            if (!string.IsNullOrEmpty(userDate))
            {
                DateTime? time = null;//非自动登录后不设置Cookie的过期时间
                if (isautologin)
                    time = DateTime.Now.AddYears(1);
                CookieHelper.SetCookie(cookieName, userDate, time);
            }
        }

        /// <summary>
        ///     添加Cookie
        /// </summary>
        /// <param name="userDate"></param>
        /// <param name="cookieName"></param>
        public static void SetCookie(string userDate, string cookieName, DateTime? ExpireTime)
        {
            if (!string.IsNullOrEmpty(userDate))
            {
                CookieHelper.SetCookie(cookieName, userDate, ExpireTime);
            }
        }

        /// <summary>
        ///     获取指定CookieName的值
        /// </summary>
        /// <param name="cookieName">CookieName</param>
        /// <returns>指定Cookie的值</returns>
        public static string GetCookie(string cookieName)
        {
            if (!string.IsNullOrEmpty(cookieName))
            {
                return CookieHelper.GetCookie(cookieName);
            }
            return null;
        }

        /// <summary>
        ///     删除登陆Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        public static void RemoveCookie(string cookieName)
        {
            //删除正常Cookie
            CookieHelper.DelCookie(cookieName);
        }

        /// <summary>
        ///     添加引导页COOKIE
        /// </summary>
        /// <param name="cookieName"></param>
        public static void SetGuideCookie(string cookieName)
        {
            var time = DateTime.Now.AddDays(7);
            var userDate = Guid.NewGuid().ToString();
            CookieHelper.SetCookie(cookieName, userDate, time);
        }
    }
}