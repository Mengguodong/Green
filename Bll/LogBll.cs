using Dal;
using DataModel;
using DataModel.RequestModel;
using DataModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    public class LogBll
    {
        LogDal _logDal = new LogDal();
        public bool Insert(DataModel.LogInfo log)
        {
            return _logDal.Insert(log);
        }

        public Page<UserLogInfoModel> GetPageUserLog(string userName, int pageIndex, int pageSize, int adminLogType, int logType)
        {
            return _logDal.GetPageUserLog(userName, pageIndex, pageSize, adminLogType, logType);
        }

        /// <summary>
        /// 创建提现单记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        //public bool CreateScoreLog(int userId, decimal score)
        //{
        //    bool result = false;
        //    WithdrawScoreLog model = new WithdrawScoreLog();
        //    model.UserId = userId;
        //    model.Amount = score;
        //    model.CreateTime = DateTime.Now;
        //    model.WithdrawScoreStatus = 1;

        //    result = _logDal.InsertWithdrawScoreLog(model);

        //    return result;
        //}
        ///// <summary>
        ///// 获取个人积分提现记录
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="pageIndex"></param>
        ///// <param name="pageSize"></param>
        ///// <returns></returns>
        // public Page<WithdrawScoreLog> GetScoreLogs(int userId, int pageIndex, int pageSize)
        // {
        //     return _logDal.GetScoreLogs(userId,pageIndex,pageSize);
        // }
        /// <summary>
        /// 获取个人每日释放记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Page<LogInfo> GetUserLogs(int userId, int pageIndex, int pageSize, int type)
        {
            if (type==3)
            {
                type = 1;
            }

                return _logDal.GetDailyReleaseLogs(userId, pageIndex, pageSize,type);
        }
        /// <summary>
        /// 复投记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Page<LogInfo> GetEpRepeatLogs(int userId, int pageIndex, int pageSize)
        {
            return _logDal.GetEpRepeatLogs(userId, pageIndex, pageSize);
        }
        /// <summary>
        /// 插入互转纪录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertCrossRotationLog(CrossRotation model)
        {
            return _logDal.InsertCrossRotationLog(model);
        }
        /// <summary>
        /// 互转记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pCRType"></param>
        /// <returns></returns>
        public Page<CrossRotation> getCrossRotationLogs(int userId, int pageIndex, int pageSize, int pCRType)
        {
            return _logDal.getCrossRotationLogs(userId, pageIndex, pageSize, pCRType);
        }
    }
}
