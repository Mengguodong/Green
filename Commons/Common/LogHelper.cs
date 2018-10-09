using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using log4net.Repository;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Common
{
    /// <summary>
    ///     错误日志记录
    ///    
    ///     2015年1月20日13:30:40
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        ///     客户端ip
        /// </summary>
        //private static string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
        private static readonly string ip = "";
      //  protected delegate bool EmailDelegate(string subject, string mailBody, string author); //邮件发送委托
        /// <summary>
        ///     请求的Url
        /// </summary>
        //private static string rawUrl = System.Web.HttpContext.Current.Request.RawUrl + "当前请求的Url:" + System.Web.HttpContext.Current.Request.Url.ToString();
        private static readonly string rawUrl = "";

        private const string InfoLogger = "Info";
        private const string ErrorLogger = "Error";
        private const string FatalLogger = "Fatal";

        #region 写入日志(通用)
        /// <summary>
        ///     写入日志
        ///     2015年1月20日15:06:46
        /// </summary>
        /// <param name="t">当前类</param>
        /// <param name="functionName">方法名</param>
        /// <param name="author">作者</param>
        /// <param name="dic">{"参数名称",参数值}</param>
        /// <param name="ex">错误信息</param>
        public static void WriteLog(Type t, string functionName, string author, Dictionary<string, object> dic,
            Exception ex)
        {
            //类型：Int32参数名称：ssid参数值：3
            var sb = new StringBuilder();
            sb.Append("");
            if (dic != null)
            {
                foreach (var item in dic)
                {
                    sb.AppendFormat("类型：{0} 参数名称：{1} 参数值：{2}", item.Value.GetType(), item.Key,
                        (item.Value == null ? "" : item.Value));
                    sb.Append("\r\n");
                }
            }
            var log = LogManager.GetLogger(t);
            var strInfo = "Error:方法名：" + functionName + "--作者：" + author + "\r\n传入参数：\r\n" + sb + "\r\n错误信息：" +
                          ex.Message + "\r\n" + ex.StackTrace + "\r\n" + "客户端ip:" + ip + ",请求连接：" + rawUrl;
            log.Error(strInfo);
            ////开发中暂时注释 edit by liuzq 2015-12-18
            //EmailDelegate dn = new EmailDelegate(new SendEmail().Send);
            //IAsyncResult ias = dn.BeginInvoke("屌丝你程序出错了", strInfo, author.Trim(), null, dn);
            //dn.EndInvoke(ias);
        }


        /// <summary>
        ///     写入日志
        /// 
        ///     2015年1月20日15:06:46
        /// </summary>
        /// <param name="t">当前类</param>
        /// <param name="functionName">方法名</param>
        /// <param name="author">作者</param>
        /// <param name="dic">{"参数名称",参数值}</param>
        /// <param name="ex">错误信息</param>
        public static void WriteLog(Type t, string functionName, Engineer author, Dictionary<string, object> dic,
            Exception ex)
        {
            //类型：Int32参数名称：ssid参数值：3
            var sb = new StringBuilder();
            sb.Append("");
            if (dic != null)
            {
                foreach (var item in dic)
                {
                    sb.AppendFormat("类型：{0} 参数名称：{1} 参数值：{2}", item.Value.GetType(), item.Key,
                        (item.Value == null ? "" : item.Value));
                    sb.Append("\r\n");
                }
            }
            var log = LogManager.GetLogger(t);
            var strInfo = "Error:方法名：" + functionName + "--作者：" + author.ToString() + "\r\n传入参数：\r\n" + sb + "\r\n错误信息：" +
                          ex.Message + "\r\n" + ex.StackTrace + "\r\n" + "客户端ip:" + ip + ",请求连接：" + rawUrl;
            log.Error(strInfo);
            //EmailDelegate dn = new EmailDelegate(new SendEmail().Send);
            //IAsyncResult ias = dn.BeginInvoke("屌丝你程序出错了", strInfo, author.ToString(), null, dn);
            //dn.EndInvoke(ias);
        }


        /// <summary>
        ///     写入日志
        ///     baochen
        ///     2015年1月20日15:06:53
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="t">当前类</param>
        /// <param name="functionName">方法名称</param>
        /// <param name="author">作者</param>
        /// <param name="entity">参数实体</param>
        /// <param name="ex">错误信息</param>
        public static void WriteLog<T>(Type t, string functionName, Engineer author, T entity, Exception ex)
        {
            try
            {
                string str = string.Empty;
                if (entity != null)
                {
                    str = JsonConvertTool.SerializeObject(entity);
                }
                var log = LogManager.GetLogger(t);
                var strInfo = "Error:方法名：" + functionName + "--作者：" + author.ToString() + "\r\n传入参数：\r\n" + str + "\r\n错误信息：" +
                              ex.Message + "\r\n" + ex.StackTrace + "\r\n" + "客户端ip:" + ip + ",请求连接：" + rawUrl;
                log.Error(strInfo);
                //EmailDelegate dn = new EmailDelegate(new SendEmail().Send);
                //IAsyncResult ias = dn.BeginInvoke("屌丝你程序出错了", strInfo, author.ToString().Trim(), null, dn);
                //dn.EndInvoke(ias);
            }
            catch
            {
                //记录日志时有可能出现异常
            }
        }


        /// <summary>
        ///     写入日志
        ///     baochen
        ///     2015年1月20日15:06:53
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="t">当前类</param>
        /// <param name="functionName">方法名称</param>
        /// <param name="author">作者</param>
        /// <param name="entity">参数实体</param>
        /// <param name="ex">错误信息</param>
        public static void WriteLog<T>(Type t, string functionName, string author, T entity, Exception ex)
        {
            try
            {
                string str = string.Empty;
                if (entity != null)
                {
                    str = JsonConvertTool.SerializeObject(entity);
                }
                var log = LogManager.GetLogger(t);
                var strInfo = "Error:方法名：" + functionName + "--作者：" + author + "\r\n传入参数：\r\n" + str + "\r\n错误信息：" +
                              ex.Message + "\r\n" + ex.StackTrace + "\r\n" + "客户端ip:" + ip + ",请求连接：" + rawUrl;
                log.Error(strInfo);
                ////开发中暂时注释 edit by liuzq 2015-12-18
                //EmailDelegate dn = new EmailDelegate(new SendEmail().Send);
                //IAsyncResult ias = dn.BeginInvoke("屌丝你程序出错了", strInfo, author.Trim(), null, dn);
                //dn.EndInvoke(ias);
            }
            catch
            {
                //记录日志时有可能出现异常
            }
        }
        #endregion


        #region 写入日志（其他）
        /// <summary>
        ///     记录普通信息
        /// </summary>
        /// <param name="infomation">信息</param>
        public static void WriteInfo(Type t, string infomation)
        {
            var log = LogManager.GetLogger(t);
            log.Info(infomation);
        }

        /// <summary>
        ///     写入日志
        ///     sunj
        ///     2015年1月20日15:06:53
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="t">当前类</param>
        /// <param name="functionName">方法名称</param>
        /// <param name="author">作者</param>
        /// <param name="entity">参数实体</param>
        /// <param name="ex">错误信息</param>
        public static void WriteInfo<T>(Type t, string functionName, Engineer author, T entity)
        {
            try
            {
                string str = string.Empty;
                if (entity != null)
                {
                    str = JsonConvertTool.SerializeObject(entity);
                }
                var log = LogManager.GetLogger(t);
                var strInfo = "Error:方法名：" + functionName + "--作者：" + author.ToString() + "\r\n传入参数：\r\n" + str +  "\r\n" + "客户端ip:" + ip + ",请求连接：" + rawUrl;
                log.Info(strInfo);
              
            }
            catch
            {
                //记录日志时有可能出现异常
            }
        }


        public static void WriteError(Type t, Exception ex)
        {
            var log = LogManager.GetLogger(t);
            log.Error(ex);
        }

        public static void Info(object message)
        {
            var log = LogManager.GetLogger(InfoLogger);
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
        }

        public static void Error(object message)
        {
            try
            {
                var log = LogManager.GetLogger(ErrorLogger);
                if (log.IsErrorEnabled)
                {
                    log.Error(message);
                }
            }
            catch { }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void ErrorFormat(string format, params object[] args)
        {
            var log = LogManager.GetLogger(ErrorLogger);
            if (log.IsErrorEnabled)
            {
                log.ErrorFormat(format, args);
            }
        }

        /// <summary>
        /// 致命的错误
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(object message)
        {
            var log = LogManager.GetLogger(FatalLogger);
            if (log.IsFatalEnabled)
            {
                log.Fatal(message);
            }
        }

        /// <summary>
        /// 致命的错误
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(object message, Exception ex)
        {
            var log = LogManager.GetLogger(FatalLogger);
            if (log.IsFatalEnabled)
            {
                log.Fatal(message, ex);
            }
        }

        /// <summary>
        /// 致命的错误
        /// </summary>
        /// <param name="message"></param>
        public static void FatalFormat(string format, params object[] args)
        {
            var log = LogManager.GetLogger(FatalLogger);
            if (log.IsFatalEnabled)
            {
                log.FatalFormat(format, args);
            }
        }

        /// <summary>
        /// 记录异常
        /// author:xiaoy
        /// 2016-04-04 14:42:06 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void Error(string msg, Exception ex)
        {
            var log = LogManager.GetLogger(ErrorLogger);
            log.Error(msg, ex);
        }
        #endregion
    }
}