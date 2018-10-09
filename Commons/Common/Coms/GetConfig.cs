using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Coms
{
    public class GetConfig
    {
        public static string ImgUrl
        {
            get { return GetConfigString("stat"); }
        }

        /// <summary>
        ///     获取配置文件中配置的信息
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigString(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        #region 邮件用户 Hegn

        public static string SMTPServer
        {
            get { return GetConfigString("SMTPServer"); }
        }

        public static string User
        {
            get { return GetConfigString("SMTPUser"); }
        }

        public static string Pwd
        {
            get { return GetConfigString("SMTPPassword"); }
        }

        public static string SMTPCUser
        {
            get { return GetConfigString("SMTPCUser"); }
        }

        #endregion
    }
}
