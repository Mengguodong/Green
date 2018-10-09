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
   public class FinanceDal:BaseDal
    {
         /// <summary>
        /// 根据用户id查账户实体
        /// sj
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public Finance GetFinanceById(int userId)
        {
            Finance data = new Finance();
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    data = db.Query<Finance>(SqlQuery<Finance>.Builder(db).AndWhere(x => x.UserId, OperationMethod.Equal, userId)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(typeof(FinanceDal), ex);
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
        public Finance GetFinanceByLogName(string logName)
        {
            Finance data = new Finance();
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    data = db.Query<Finance>(SqlQuery<Finance>.Builder(db).AndWhere(x => x.LoginName, OperationMethod.Equal, logName)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(typeof(FinanceDal), ex);
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
        public bool UpdateFinance(Finance Finance)
        {

            bool result = false;
            try
            {
                if (Finance != null)
                {
                    using (var db = BaseDal.WriteSanNongDunDbBase())
                    {
                        result = db.Update(Finance);
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(FinanceDal), "UpdateFinance", Engineer.ccc, Finance, ex);
            }
            return result;
        }
    }
}
