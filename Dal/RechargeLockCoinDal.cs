using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DapperEx;
using Dapper;
using Common;
using DataModel;

namespace Dal
{
    public class RechargeLockCoinDal
    {
        /// <summary>
        /// 根据用户id查账户实体
        /// sj
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public RechargeLockCoin GetRechargeLockCoinById(string logName)
        {
            RechargeLockCoin data = new RechargeLockCoin();
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    data = db.Query<RechargeLockCoin>(SqlQuery<RechargeLockCoin>.Builder(db).AndWhere(x => x.LoginName, OperationMethod.Equal, logName)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(typeof(RechargeLockCoinDal), ex);
                data = null;
            }
            return data;
        }
        /// <summary>
        /// 根据用户id查账户实体
        /// sj
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public RechargeLockCoin GetRechargeLockCoinByLogName(string logName)
        {
            RechargeLockCoin data = new RechargeLockCoin();
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    data = db.Query<RechargeLockCoin>(SqlQuery<RechargeLockCoin>.Builder(db).AndWhere(x => x.LoginName, OperationMethod.Equal, logName)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(typeof(RechargeLockCoinDal), ex);
                data = null;
            }
            return data;
        }



        /// <summary>
        /// 修改账户  
        /// sj
        /// </summary>
        /// <param name="customerAccount">修改账户实体</param>
        /// <returns></returns>
        public bool UpdateRechargeLockCoin(RechargeLockCoin RechargeLockCoin)
        {

            bool result = false;
            try
            {
                if (RechargeLockCoin != null)
                {
                    using (var db = BaseDal.WriteSanNongDunDbBase())
                    {
                        result = db.Update(RechargeLockCoin);
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(RechargeLockCoinDal), "UpdateRechargeLockCoin", Engineer.ccc, RechargeLockCoin, ex);
            }
            return result;
        }
    }
}
