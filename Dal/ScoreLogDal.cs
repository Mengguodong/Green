using Common;
using Database;
using DataModel;
using System;
using Dapper;
using DapperEx;
using DataModel.RequestModel;
using System.Collections.Generic;
using System.Linq;

namespace Dal
{
    public class ScoreLogDal:BaseDal
    {
        public bool Insert(ScoreLog scoreLog)
        {
            bool isTrue = false;
            try
            {
                using (var db = BaseDal.WriteSanNongDunDbBase())
                {
                    isTrue = db.Insert<ScoreLog>(scoreLog);
                    if (!isTrue)
                    {
                        LogHelper.WriteInfo(typeof(ScoreLogDal), "Insert--添加记录失败", Engineer.ccc, scoreLog);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(ScoreLogDal), "Insert", Engineer.ccc, scoreLog, ex);
            }
            return isTrue;
        }

        /// <summary>
        /// 分页红包记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="type">1:自己，2：业绩返利</param>
        /// <returns></returns>
        public Page<ScoreLog> GetScoreLog(int userId, int pageIndex, int pageSize)
        {

            Page<ScoreLog> page = new Page<ScoreLog>();

            List<ScoreLog> list = new List<ScoreLog>();
            int totalCount = 0;
         
            try
            {

                using (var db = ReadOnlySanNongDunConn())
                {

                 
                        list = db.Query<ScoreLog>(SqlQuery<ScoreLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId)).OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                        totalCount = db.Query<ScoreLog>(SqlQuery<ScoreLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId)).ToList().Count;
                    
                  


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

                LogHelper.WriteLog(typeof(ScoreLog), "GetScoreLog", Engineer.ggg, new { userId = userId, pageIndex = pageIndex, pageSize = pageSize }, ex);
            }

            return page;

        }

        /// <summary>
        /// 分页红包记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="type">1:自己，2：业绩返利</param>
        /// <returns></returns>
        public Page<ScoreLog> AdminGetScoreLog(int userId, int pageIndex, int pageSize)
        {

            Page<ScoreLog> page = new Page<ScoreLog>();

            List<ScoreLog> list = new List<ScoreLog>();
            int totalCount = 0;

            try
            {

                using (var db = ReadOnlySanNongDunConn())
                {
                    if (userId!=0)
                    {
                        list = db.Query<ScoreLog>(SqlQuery<ScoreLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal,userId)).OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                        totalCount = db.Query<ScoreLog>(SqlQuery<ScoreLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId)).ToList().Count;
                    }
                    else
                    {
                        list = db.Query<ScoreLog>(SqlQuery<ScoreLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Greater, 0)).OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                        totalCount = db.Query<ScoreLog>(SqlQuery<ScoreLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Greater, 0)).ToList().Count;
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

                LogHelper.WriteLog(typeof(ScoreLog), "AdminGetScoreLog", Engineer.ggg, new { userId = userId, pageIndex = pageIndex, pageSize = pageSize }, ex);
            }

            return page;

        }
    }
}
