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
    public class OrderInfoDal : BaseDal
    {
        /// <summary>
        /// 根据用户id查账户实体
        /// sj
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public OrderInfo GetOrderInfoById(int userId)
        {
            OrderInfo data = new OrderInfo();
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    data = db.Query<OrderInfo>(SqlQuery<OrderInfo>.Builder(db).AndWhere(x => x.UserId, OperationMethod.Equal, userId)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(typeof(OrderInfoDal), ex);
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
        public OrderInfo GetOrderInfoBySellLogName(string sellLoginName)
        {
            OrderInfo data = new OrderInfo();
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    data = db.Query<OrderInfo>(SqlQuery<OrderInfo>.Builder(db).AndWhere(x => x.SellLoginName, OperationMethod.Equal, sellLoginName)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(typeof(OrderInfoDal), ex);
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
        public OrderInfo GetOrderInfoByBuyLogName(string BuyLogName)
        {
            OrderInfo data = new OrderInfo();
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    data = db.Query<OrderInfo>(SqlQuery<OrderInfo>.Builder(db).AndWhere(x => x.BuyLoginName, OperationMethod.Equal, BuyLogName)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(typeof(OrderInfoDal), ex);
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
        public bool UpdateOrderInfo(OrderInfo OrderInfo)
        {

            bool result = false;
            try
            {
                if (OrderInfo != null)
                {
                    using (var db = BaseDal.WriteSanNongDunDbBase())
                    {
                        result = db.Update(OrderInfo);
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(OrderInfoDal), "UpdateOrderInfo", Engineer.ccc, OrderInfo, ex);
            }
            return result;
        }
    }
}
