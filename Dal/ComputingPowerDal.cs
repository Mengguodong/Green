using DapperEx;
using Database;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Common;

namespace Dal
{
    public class ComputingPowerDal:BaseDal
    {
        /// <summary>
        /// 根据用户id查账户实体
        /// sj
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public ComputingPower GetComputingPowerById(int userId)
        {
            ComputingPower data = new ComputingPower();
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    data = db.Query<ComputingPower>(SqlQuery<ComputingPower>.Builder(db).AndWhere(x => x.UserId, OperationMethod.Equal, userId)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(typeof(ComputingPowerDal), ex);
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
        public ComputingPower GetComputingPowerByLogName(string logName)
        {
            ComputingPower data = new ComputingPower();
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    data = db.Query<ComputingPower>(SqlQuery<ComputingPower>.Builder(db).AndWhere(x => x.LoginName, OperationMethod.Equal, logName)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(typeof(ComputingPowerDal), ex);
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
        public bool UpdateComputingPower(ComputingPower computingPower)
        {

            bool result = false;
            try
            {
                if (computingPower != null)
                {
                    using (var db = BaseDal.WriteSanNongDunDbBase())
                    {
                        result = db.Update(computingPower);
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(ComputingPowerDal), "UpdateComputingPower", Engineer.ccc, computingPower, ex);
            }
            return result;
        }

    }
}
