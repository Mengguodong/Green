using Common;
using DapperEx;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class BaseDal
    {
        /// <summary>
        /// 写入对象
        /// </summary>
        /// <returns></returns>
        public static DbBase WriteSanNongDunDbBase()
        {
            return new DbBase(SanNongDunDBConnString.WriteSanNongDunConn);
        }
   
        /// <summary>
        /// 数据库读取对象
        /// </summary>
        /// <returns></returns>
        public static DbBase ReadOnlySanNongDunConn()
        {
            return new DbBase(SanNongDunDBConnString.GetReadConnectionString());
        }


    }
}
