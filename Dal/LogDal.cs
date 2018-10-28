using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperEx;
using DataModel;
using Common;
using DataModel.ViewModel;
using DataModel.RequestModel;

namespace Dal
{
    public class LogDal : BaseDal
    {
        public bool Insert(DataModel.LogInfo log)
        {
            bool isTrue = false;
            try
            {
                using (var db = BaseDal.WriteSanNongDunDbBase())
                {
                    isTrue = db.Insert<LogInfo>(log);
                    if (!isTrue) 
                    {
                        LogHelper.WriteInfo(typeof(LogDal), "Insert--添加记录失败", Engineer.ccc, log);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(LogDal), "Insert", Engineer.ccc, log, ex);
            }
            return isTrue;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="adminLogType"></param>
        /// <param name="logType"></param>
        /// <returns></returns>
        public Page<UserLogInfoModel> GetPageUserLog(string userName, int pageIndex, int pageSize, int adminLogType, int logType)
        {
            string userNameSql = "";
            string adminLogTypeSql = "";

            if (!string.IsNullOrEmpty(userName))
            {
                userNameSql = " and UserName like @userName";
            }
            if (logType == 1 )
            {
                adminLogTypeSql = " and AdminLogType = @adminLogType";
            }

            Page<UserLogInfoModel> page = new Page<UserLogInfoModel>();
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    string sql = string.Format(@"
                                 
                                             select num,UserId,UserName,RealName,Number,AdminLogType,CreateTime,LogType,Ep,Zfc
                                 
                                              from (select ROW_NUMBER() over (order by l.CreateTime desc)as num,u.UserId,UserName,RealName,Number,AdminLogType,l.CreateTime,LogType,Ep,Zfc
                                                         from UserInfo u inner join LogInfo l on u.UserId=l.UserId

                                                          where  UserStatus=1 and IsActivation=1  and LogType = @logType {0} {1}
                                             ) as t
                                             where t.num between  @pageSize * (@pageIndex - 1) + 1 and @pageSize * @pageIndex

                      ", userNameSql, adminLogTypeSql, pageIndex);

                    page.Data = db.DbConnecttion.Query<UserLogInfoModel>(sql, Engineer.ccc, new { userName = "%" + userName + "%", pageIndex = pageIndex, pageSize = pageSize, adminLogType = adminLogType, logType = logType }).ToList();


                    string sqlCount = string.Format(@"select COUNT(1) from LogInfo l inner join UserInfo u on
                                        u.UserId=l.UserId
                                        where UserStatus=1 and IsActivation=1  and LogType = @logType {0} {1}", userNameSql, adminLogTypeSql);

                    int totalCount = (int)db.DbConnecttion.ExecuteScalar(sqlCount, new { userName = "%" + userName + "%", adminLogType = adminLogType, logType = logType });

                    page.TotalCount = totalCount;
                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(LogDal), "GetPageUserLog", Engineer.ccc, new { userName = userName, pageIndex = pageIndex, pageSize = pageSize, adminLogType = adminLogType, logType = logType }, ex);
            }
            return page;
        }


        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public StatisticsViewModel GetTotalData(DateTime? begin, DateTime? end)
        {
            StatisticsViewModel model = new StatisticsViewModel();

            string strWhere = string.Empty;
            try
            {
                if (begin != null && end != null)
                {
                    strWhere = " createtime >=@begin and createtime <=@end and ";
                }
                else if (begin == null && end != null)
                {
                    strWhere = " createtime <=@end and ";
                }
                else if (begin != null && end == null)
                {
                    strWhere = " createtime >=@begin and ";
                }
                else
                {
                    strWhere = " 1=1 ";
                }

                string sql1 = @"select ISNULL(SUM(Ep),0) as TotalEp  from loginfo where " + strWhere ;
                string sql2 = @"select ISNULL(SUM(Number),0) as TotalScore from LogInfo where  LogType=1 and"+strWhere;
          
                string sql4 = @"select ISNULL( SUM(Zfc),0) as TotalZfc from loginfo where " + strWhere ;

                using (var db = ReadOnlySanNongDunConn())
                {
                    decimal totalEp = db.DbConnecttion.ExecuteScalar<decimal>(sql1, Engineer.ggg, new { begin = begin, end = end });
                    decimal totalScore = db.DbConnecttion.ExecuteScalar<decimal>(sql2, Engineer.ggg, new { begin = begin, end = end });
              
                    decimal totalZfc = db.DbConnecttion.ExecuteScalar<decimal>(sql4, Engineer.ggg, new { begin = begin, end = end });
                    model.TotalEp = totalEp;
                    model.TotalZfc = totalZfc;
                    model.TotalScore = totalScore;
            
                }



            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(LogDal), "GetTotalData", Engineer.ggg, new { begin = begin, end = end }, ex);
            }
            return model;
        }
        /// <summary>
        /// 插入提现单记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public bool InsertWithdrawScoreLog(WithdrawScoreLog model)
        //{
        //    bool result = false;

        //    try
        //    {
        //        using (var db = WriteSanNongDunDbBase())
        //        {

        //            result = db.Insert<WithdrawScoreLog>(model);
        //        }


        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(LogDal), "InsertWithdrawScoreLog", Engineer.ggg, model, ex);
        //    }


        //    return result;
        //}
        ///// <summary>
        ///// 获取个人提现记录
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="pageIndex"></param>
        ///// <param name="pageSize"></param>
        ///// <returns></returns>
        //public Page<WithdrawScoreLog> GetScoreLogs(int userId, int pageIndex, int pageSize)
        //{

        //    Page<WithdrawScoreLog> page = new Page<WithdrawScoreLog>();

        //    List<WithdrawScoreLog> list = new List<WithdrawScoreLog>();

        //    int totalCount = 0;

        //    try
        //    {

        //        using (var db = ReadOnlySanNongDunConn())
        //        {
        //            list = db.Query<WithdrawScoreLog>(SqlQuery<WithdrawScoreLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId)).OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        //            totalCount = db.Query<WithdrawScoreLog>(SqlQuery<WithdrawScoreLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId)).ToList().Count;


        //            if (list != null && totalCount > 0)
        //            {
        //                page.Data = list;
        //                page.PageIndex = pageIndex;
        //                page.PageSize = pageSize;
        //                page.TotalCount = totalCount;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(LogDal), "GetScoreLogs", Engineer.ggg, new {  userId=userId,  pageIndex=pageIndex,  pageSize=pageSize}, ex);
        //    }

        //    return page;

        //}
        /// <summary>
        /// 获取每日释放记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Page<LogInfo> GetDailyReleaseLogs(int userId, int pageIndex, int pageSize,int type)
        {

            Page<LogInfo> page = new Page<LogInfo>();

            List<LogInfo> list = new List<LogInfo>();
            int totalCount = 0;
         //   int[] logTypes = {3,4};  //静态收益，动态收益

            try
            {

                using (var db = ReadOnlySanNongDunConn())
                {

                    if (type==1)//静态
                    {
                        list = db.Query<LogInfo>(SqlQuery<LogInfo>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId).AndWhere(o => o.LogType, OperationMethod.Equal, 3)).OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                        totalCount = db.Query<LogInfo>(SqlQuery<LogInfo>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId).AndWhere(o => o.LogType, OperationMethod.Equal, 3)).ToList().Count;
                    }
                    else //动态
                    {
                        list = db.Query<LogInfo>(SqlQuery<LogInfo>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId).AndWhere(o => o.LogType, OperationMethod.Equal, 4)).OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                        totalCount = db.Query<LogInfo>(SqlQuery<LogInfo>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId).AndWhere(o => o.LogType, OperationMethod.Equal, 4)).ToList().Count;
                    }
                  

                    if (list != null && totalCount > 0)
                    {
                        page.Data = list;
                        page.PageIndex = pageIndex;
                        page.PageSize = pageSize;
                        page.TotalCount = totalCount;
                        page.PageYe = (int)Math.Ceiling(Convert.ToDouble(totalCount) / Convert.ToDouble(pageSize));
                    }

                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(LogDal), "GetDailyReleaseLogs", Engineer.ggg, new { userId = userId, pageIndex = pageIndex, pageSize = pageSize }, ex);
            }

            return page;

        }
        /// <summary>
        /// 获取复投记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Page<LogInfo> GetEpRepeatLogs(int userId, int pageIndex, int pageSize)
        {

            Page<LogInfo> page = new Page<LogInfo>();

            List<LogInfo> list = new List<LogInfo>();
            int totalCount = 0;
      

            try
            {

                using (var db = ReadOnlySanNongDunConn())
                {

                    list = db.Query<LogInfo>(SqlQuery<LogInfo>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId).AndWhere(o => o.LogType, OperationMethod.Equal,5)).OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                    totalCount = db.Query<LogInfo>(SqlQuery<LogInfo>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId).AndWhere(o => o.LogType, OperationMethod.Equal, 5)).ToList().Count;

                    if (list != null && totalCount > 0)
                    {
                        page.Data = list;
                        page.PageIndex = pageIndex;
                        page.PageSize = pageSize;
                        page.TotalCount = totalCount;
                        page.PageYe =(int)Math.Ceiling(Convert.ToDouble(totalCount) /Convert.ToDouble(pageSize));
                    }

                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(LogDal), "GetEpRepeatLogs", Engineer.ggg, new { userId = userId, pageIndex = pageIndex, pageSize = pageSize }, ex);
            }

            return page;
        }
        /// <summary>
        /// 插入互转纪录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertCrossRotationLog(CrossRotation model)
        {
            bool result = false;
            try
            {
                using (var db = WriteSanNongDunDbBase())
                {
                    result = db.Insert<CrossRotation>(model);
                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(LogDal), "InsertCrossRotationLog", Engineer.ggg,model, ex);
            }
            return result;
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
            Page<CrossRotation> page = new Page<CrossRotation>();

            string sql = @"select * from (select *,ROW_NUMBER() over (order by createtime desc) as n from CrossRotation  where (CRUserId=@userId or REUserId=@userId) and CRType=@pCRType) as t 
where t.n between @begin and @end";
            string sqlCount = @"select count(1) from  CrossRotation  where (CRUserId=@userId or REUserId=@userId) and CRType=@pCRType";

            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    var list = db.DbConnecttion.Query<CrossRotation>(sql, Engineer.ggg, new { userId = userId, pCRType = pCRType, begin = (pageIndex - 1) * pageSize + 1, end = pageSize * pageIndex }).ToList();

                    if (list!=null&&list.Count>0)
                    {
                        page.TotalCount = db.DbConnecttion.ExecuteScalar<int>(sqlCount,Engineer.ggg,new { userId = userId, pCRType = pCRType});
                    }
                    page.Data = list;
                    page.PageIndex = pageIndex;
                    page.PageSize = pageSize;
                    if (page.TotalCount > 0)
                    {
                        page.PageYe = (int)Math.Ceiling(Convert.ToDouble(page.TotalCount) / Convert.ToDouble(pageSize));
                    }
                    else 
                    {
                        page.PageYe = 1;
                    }

                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(LogDal), "getCrossRotationLogs", Engineer.ggg, new {userId=userId,  pageIndex=pageIndex,  pageSize=pageSize,  pCRType=pCRType }, ex);
            }
            return page;

        }
    }
}
