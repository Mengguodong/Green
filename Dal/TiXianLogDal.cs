using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Dapper;
using DapperEx;
using Database;
using DataModel;
using DataModel.RequestModel;

namespace Dal
{
    public class TiXianLogDal:BaseDal
    {
        public bool Insert(TiXianLog tiXianLog)
        {
            bool isTrue = false;
            try
            {
                using (var db = BaseDal.WriteSanNongDunDbBase())
                {
                    isTrue = db.Insert<TiXianLog>(tiXianLog);
                    if (!isTrue)
                    {
                        LogHelper.WriteInfo(typeof(TiXianLogDal), "Insert--添加记录失败", Engineer.ccc, tiXianLog);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(TiXianLogDal), "Insert", Engineer.ccc, tiXianLog, ex);
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
        public Page<TiXianLog> GetTiXianLog(int userId, int pageIndex, int pageSize)
        {

            Page<TiXianLog> page = new Page<TiXianLog>();

            List<TiXianLog> list = new List<TiXianLog>();
            int totalCount = 0;

            try
            {

                using (var db = ReadOnlySanNongDunConn())
                {


                    list = db.Query<TiXianLog>(SqlQuery<TiXianLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId)).OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                    totalCount = db.Query<TiXianLog>(SqlQuery<TiXianLog>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId)).ToList().Count;




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

                LogHelper.WriteLog(typeof(TiXianLog), "GetTiXianLog", Engineer.ggg, new { userId = userId, pageIndex = pageIndex, pageSize = pageSize }, ex);
            }

            return page;

        }

        public decimal SumTiXian(DateTime? begin,DateTime? end)
        {
            decimal sum = 0;
            string strWhere = "";

            if (begin != null && end != null)
            {
                strWhere = " createtime >=@begin and createtime <=@end  ";
            }
            else if (begin == null && end != null)
            {
                strWhere = " createtime <=@end  ";
            }
            else if (begin != null && end == null)
            {
                strWhere = " createtime >=@begin  ";
            }
            else
            {
                strWhere = " 1=1 ";
            }
            string sql = "select ISNULL(SUM(LogCount),0)  from TiXianLog where "+strWhere;
            using (var db = BaseDal.ReadOnlySanNongDunConn())
            {
                sum = db.DbConnecttion.ExecuteScalar<decimal>(sql, Engineer.ccc, new { begin = begin, end = end });
            }
            return sum;
        }

    }
}
