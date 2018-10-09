using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Web
{
    /// <summary>
    /// ip助手类
    /// 2016年2月24日14:50:56
    ///
    /// </summary>
    public class IpHelper
    {
        public static string GetIp()
        {
            string ip = null;
            HttpContext httpContext = HttpContext.Current;
            ip = httpContext.Request.Headers["Cdn-Src-Ip"];
            if (ip == null)
            {
                ip = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (ip == null)
                {
                    ip = httpContext.Request.UserHostAddress;
                }
            }
            return ip;
        }
    }
}
