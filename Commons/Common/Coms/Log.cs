using System;
using System.IO;

/*************************************************************************
*GUID：
*CLR：4.0.30319.17929
*Machine:  USER-KVT0SIKF25
*Creater：Hollson
*Email：Hollson@QQ.com
*Time：2014/3/25 15:01:45
*Code Caption：
***************************************************************************/

namespace Zhtx.Common
{
    public class Log
    {
        private static readonly object oLock = new object();

        /// <summary>
        ///     写日志
        /// </summary>
        public static void WriteLog(string msg)
        {
            var strRootPath = AppDomain.CurrentDomain.BaseDirectory; //应用程序目录
            var strLogPath = Path.Combine(strRootPath, "Log"); //日志目录
            var strLogFileName = Path.Combine(strLogPath, "Log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log");
                //日志文件名称
            lock (oLock)
            {
                if (!Directory.Exists(strLogPath))
                {
                    Directory.CreateDirectory(strLogPath);
                }
                var file = new FileInfo(strLogFileName);

                StreamWriter sw = null;
                try
                {
                    if (!file.Exists)
                    {
                        sw = file.CreateText();
                        sw.WriteLine("THIS IS A TEMPORARY FILE, THE USER CAN ACCORDING TO NEED TO DELETE IT !");
                    }
                    else
                        sw = file.AppendText();
                    sw.WriteLine("{0} — {1}", DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff]"), msg);
                }
                catch
                {
                }
                finally
                {
                    if (sw != null)
                        sw.Close();
                }
            }
        }
    }
}