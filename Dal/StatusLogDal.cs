using Common;
using Database;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperEx;
using DataModel.RequestModel;

namespace Dal
{
    public class StatusLogDal:BaseDal
    {
        public bool Insert(StatusLog statusLog)
        {
            bool isTrue = false;
            try
            {
                using (var db = BaseDal.WriteSanNongDunDbBase())
                {
                    isTrue = db.Insert<StatusLog>(statusLog);
                    if (!isTrue)
                    {
                        LogHelper.WriteInfo(typeof(StatusLogDal), "Insert--添加记录失败", Engineer.ccc, statusLog);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(StatusLogDal), "Insert", Engineer.ccc, statusLog, ex);
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
        public Page<StatusLog> GetStatusLog(int userId, int pageIndex, int pageSize, int type)
        {

            Page<StatusLog> page = new Page<StatusLog>();

            List<StatusLog> list = new List<StatusLog>();
            int totalCount = 0;
          

            try
            {

                using (var db = ReadOnlySanNongDunConn())
                {

                        list = db.Query<StatusLog>(SqlQuery<StatusLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId).AndWhere(o => o.LogType, OperationMethod.Equal, type)).OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                        totalCount = db.Query<StatusLog>(SqlQuery<StatusLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId).AndWhere(o => o.LogType, OperationMethod.Equal, type)).ToList().Count;
            
                 

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

                LogHelper.WriteLog(typeof(StatusLog), "GetStatusLog", Engineer.ggg, new { userId = userId, pageIndex = pageIndex, pageSize = pageSize, type = type }, ex);
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
        public Page<StatusLog> AdminGetStatusLog(int userId, int pageIndex, int pageSize, int type)
        {

            Page<StatusLog> page = new Page<StatusLog>();

            List<StatusLog> list = new List<StatusLog>();
            int totalCount = 0;


            try
            {

                using (var db = ReadOnlySanNongDunConn())
                {
                   if(userId!=0){

                       list = db.Query<StatusLog>(SqlQuery<StatusLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId).AndWhere(o => o.LogType, OperationMethod.Equal, type)).OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                       totalCount = db.Query<StatusLog>(SqlQuery<StatusLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId).AndWhere(o => o.LogType, OperationMethod.Equal, type)).ToList().Count;

                    }
                    else 
                    {
                        list = db.Query<StatusLog>(SqlQuery<StatusLog>.Builder(db).AndWhere(o => o.LogType, OperationMethod.Equal, type)).OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                        totalCount = db.Query<StatusLog>(SqlQuery<StatusLog>.Builder(db).AndWhere(o => o.LogType, OperationMethod.Equal, type)).ToList().Count;
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

                LogHelper.WriteLog(typeof(StatusLog), "AdminGetStatusLog", Engineer.ggg, new { userId = userId, pageIndex = pageIndex, pageSize = pageSize, type = type }, ex);
            }

            return page;

        }

    }
}
