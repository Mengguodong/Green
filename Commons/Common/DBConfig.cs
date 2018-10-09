using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
namespace Common
{


    public static class SanNongDunDBConnString
    {
        /// <summary>
        /// 写入
        /// </summary>
        public static string WriteSanNongDunConn
        {
            //get { return Auxiliary.ConfigConnectionStrings("WriteWineGameConn"); }
            get { return "WriteSanNongDunConn"; }
        }


        /// <summary>
        /// 获取只读数据库的连接字符串
        /// </summary>
        /// <returns></returns>
        public static string GetReadConnectionString()
        {
            List<string> vrconnectionList = new List<string>() 
            {
                ReadOnlySanNongDunConn1,
                ReadOnlySanNongDunConn2,
                ReadOnlySanNongDunConn3,
                ReadOnlySanNongDunConn4
                //Auxiliary.ConfigConnectionStrings("ReadOnlySanNongDunConn1"),
                //Auxiliary.ConfigConnectionStrings("ReadOnlySanNongDunConn2"),
                //Auxiliary.ConfigConnectionStrings("ReadOnlySanNongDunConn3"),
                //Auxiliary.ConfigConnectionStrings("ReadOnlySanNongDunConn4")
            };
            List<string> connectionList = new List<string>();
            foreach (var item in vrconnectionList)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    connectionList.Add(item);
                }
            }
            int length = connectionList.Count;
            Random ran = new Random(DateTime.Now.Millisecond);
            int number = ran.Next();
            int index = number % length;
            return connectionList[index];
        }

        public static string ReadOnlySanNongDunConn1
        {
            get { return "ReadOnlySanNongDunConn1"; }
        }

        public static string ReadOnlySanNongDunConn2
        {
            get { return "ReadOnlySanNongDunConn2"; }
        }
        public static string ReadOnlySanNongDunConn3
        {
            get { return "ReadOnlySanNongDunConn3"; }
        }
        public static string ReadOnlySanNongDunConn4
        {
            get { return "ReadOnlySanNongDunConn4"; }
        }
    }
}