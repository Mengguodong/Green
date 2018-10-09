using Common;
using DapperEx;
using Database;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Dal
{
    public class GlobalConfigDal : BaseDal
    {
        /// <summary>
        /// 根据用户id查账户实体
        /// sj
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public GlobalConfig GetGlobalConfig( )
        {
         
            GlobalConfig data = new GlobalConfig();
            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    string sql = @"select * from GlobalConfig";
                    data = (GlobalConfig) db.DbConnecttion.ExecuteScalar(sql);
                   
                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(GlobalConfigDal), "GetGlobalConfig", Engineer.ggg, null, ex);
            }
         
            return data;
        }
      


        /// <summary>
        /// 修改账户  
        /// sj
        /// </summary>
        /// <param name="customerAccount">修改账户实体</param>
        /// <returns></returns>
        public bool UpdateGlobalConfig(GlobalConfig globalConfig)
        {

            bool result = false;
            try
            {
                if (globalConfig != null)
                {
                    using (var db = BaseDal.WriteSanNongDunDbBase())
                    {
                        result = db.Update(globalConfig);
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(GlobalConfigDal), "UpdateGlobalConfig", Engineer.ccc, globalConfig, ex);
            }
            return result;
        }
    }
}
