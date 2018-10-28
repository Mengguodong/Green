using Common;
using Database;
using System;
using System.Collections.Generic;
using System.Linq;

using Dapper;
using DapperEx;
using DataModel;
using DataModel.RequestModel;




namespace Dal
{
    public  class HongBaoLogDal : BaseDal
    {
        public bool Insert(HongBaoLog hongBao)
        {
            bool isTrue = false;
            try
            {
                using (var db = BaseDal.WriteSanNongDunDbBase())
                {
                    isTrue = db.Insert<HongBaoLog>(hongBao);
                    if (!isTrue)
                    {
                        LogHelper.WriteInfo(typeof(HongBaoLogDal), "Insert--添加记录失败", Engineer.ccc, hongBao);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(HongBaoLogDal), "Insert", Engineer.ccc, hongBao, ex);
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
        public Page<HongBaoLog> GetHongBaoLog(int userId, int pageIndex, int pageSize, int type)
        {

            Page<HongBaoLog> page = new Page<HongBaoLog>();

            List<HongBaoLog> list = new List<HongBaoLog>();
            int totalCount = 0;
            //   int[] logTypes = {3,4};  //静态收益，动态收益

            try
            {

                using (var db = ReadOnlySanNongDunConn())
                {

                 
                        list = db.Query<HongBaoLog>(SqlQuery<HongBaoLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId).AndWhere(o => o.LogType, OperationMethod.Equal, type)).OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                        totalCount = db.Query<HongBaoLog>(SqlQuery<HongBaoLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId).AndWhere(o => o.LogType, OperationMethod.Equal, type)).ToList().Count;
                   
                 


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

                LogHelper.WriteLog(typeof(HongBaoLog), "GetHongBaoLog", Engineer.ggg, new { userId = userId, pageIndex = pageIndex, pageSize = pageSize,type=type }, ex);
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
        public Page<HongBaoLog> AdminGetHongBaoLog(int userId, int pageIndex, int pageSize, int type)
        {

            Page<HongBaoLog> page = new Page<HongBaoLog>();

            List<HongBaoLog> list = new List<HongBaoLog>();
            int totalCount = 0;
            //   int[] logTypes = {3,4};  //静态收益，动态收益

            try
            {

                using (var db = ReadOnlySanNongDunConn())
                {
                    if (userId!= 0)
                    {

                        list = db.Query<HongBaoLog>(SqlQuery<HongBaoLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal,userId).AndWhere(o => o.LogType, OperationMethod.Equal, type)).OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                        totalCount = db.Query<HongBaoLog>(SqlQuery<HongBaoLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal,userId).AndWhere(o => o.LogType, OperationMethod.Equal, type)).ToList().Count;

                    }
                    else
                    {
                        list = db.Query<HongBaoLog>(SqlQuery<HongBaoLog>.Builder(db).AndWhere(o => o.LogType, OperationMethod.Equal, type)).OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                        totalCount = db.Query<HongBaoLog>(SqlQuery<HongBaoLog>.Builder(db).AndWhere(o => o.LogType, OperationMethod.Equal, type)).ToList().Count;
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

                LogHelper.WriteLog(typeof(HongBaoLog), "AdminGetHongBaoLog", Engineer.ggg, new { userId = userId, pageIndex = pageIndex, pageSize = pageSize, type = type }, ex);
            }

            return page;

        }
    }
}
