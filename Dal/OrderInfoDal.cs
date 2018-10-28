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
using DataModel.RequestModel;
using DataModel.ViewModel;

namespace Dal
{
    public class OrderInfoDal : BaseDal
    {

        #region 订单查询和修改

        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public OrderInfo GetOrderByOrderId(int orderId)
        {
            OrderInfo data = null;
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    data = db.Query<OrderInfo>(SqlQuery<OrderInfo>.Builder(db).AndWhere(x => x.OrderId, OperationMethod.Equal, orderId)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(OrderInfoDal), "GetOrderByOrderId", Engineer.ccc, new { orderId = orderId }, ex);
                data = null;
            }
            return data;
        }

        ///// <summary>
        ///// 根据用户id查账户实体
        ///// sj
        ///// </summary>
        ///// <param name="userId">用户id</param>
        ///// <returns></returns>
        //public OrderInfo GetOrderInfoBySellUserId(int SellUserId)
        //{
        //    OrderInfo data = new OrderInfo();
        //    try
        //    {
        //        using (var db = BaseDal.ReadOnlySanNongDunConn())
        //        {
        //            data = db.Query<OrderInfo>(SqlQuery<OrderInfo>.Builder(db).AndWhere(x => x.SellUserId, OperationMethod.Equal, SellUserId)).FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteError(typeof(OrderInfoDal), ex);
        //        data = null;
        //    }
        //    return data;
        //}
        ///// <summary>
        ///// 根据用户name查账户实体
        ///// sj
        ///// </summary>
        ///// <param name="userId">用户id</param>
        ///// <returns></returns>
        //public OrderInfo GetOrderInfoBySellAccountName(string SellAccountName)
        //{
        //    OrderInfo data = new OrderInfo();
        //    try
        //    {
        //        using (var db = BaseDal.ReadOnlySanNongDunConn())
        //        {
        //            data = db.Query<OrderInfo>(SqlQuery<OrderInfo>.Builder(db).AndWhere(x => x.SellAccountName, OperationMethod.Equal, SellAccountName)).FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteError(typeof(OrderInfoDal), ex);
        //        data = null;
        //    }
        //    return data;
        //}

        ///// <summary>
        ///// 根据用户id查账户实体
        ///// sj
        ///// </summary>
        ///// <param name="userId">用户id</param>
        ///// <returns></returns>
        //public OrderInfo GetOrderInfoByBuyAccountName(string BuyAccountName)
        //{
        //    OrderInfo data = new OrderInfo();
        //    try
        //    {
        //        using (var db = BaseDal.ReadOnlySanNongDunConn())
        //        {
        //            data = db.Query<OrderInfo>(SqlQuery<OrderInfo>.Builder(db).AndWhere(x => x.BuyAccountName, OperationMethod.Equal, BuyAccountName)).FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteError(typeof(OrderInfoDal), ex);
        //        data = null;
        //    }
        //    return data;
        //}

        ///// <summary>
        ///// 根据用户id查账户实体
        ///// sj
        ///// </summary>
        ///// <param name="userId">用户id</param>
        ///// <returns></returns>
        //public OrderInfo GetOrderInfoByBuyUserId(string BuyUserId)
        //{
        //    OrderInfo data = new OrderInfo();
        //    try
        //    {
        //        using (var db = BaseDal.ReadOnlySanNongDunConn())
        //        {
        //            data = db.Query<OrderInfo>(SqlQuery<OrderInfo>.Builder(db).AndWhere(x => x.BuyUserId, OperationMethod.Equal, BuyUserId)).FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteError(typeof(OrderInfoDal), ex);
        //        data = null;
        //    }
        //    return data;
        //}
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


        /// <summary>
        /// 查询用户分页购买订单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Page<OrderInfo> GetOrderByBuyId(int buyUserId, int pageIndex, int pageSize)
        {
            Page<OrderInfo> page = new Page<OrderInfo>();
            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    page.Data = db.Query<OrderInfo>(SqlQuery<OrderInfo>.Builder(db).AndWhere(x => x.BuyUserId, OperationMethod.Equal, buyUserId)).Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.CreateTime).ToList<OrderInfo>();

                    page.TotalCount = db.Query<OrderInfo>(SqlQuery<OrderInfo>.Builder(db).AndWhere(x => x.BuyUserId, OperationMethod.Equal, buyUserId)).ToList<OrderInfo>().Count;

                    page.PageSize = pageSize;
                    page.PageIndex = pageIndex;
                    page.PageYe = (int)Math.Ceiling((decimal)page.TotalCount/page.PageSize);

                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(OrderInfoDal), "GetOrderByBuyId", Engineer.ccc, new { buyUserId = buyUserId }, ex);
            }

            return page;
        }


        /// <summary>
        /// 查询用户分页出售订单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Page<OrderInfo> GetOrderBySellUserId(int sellUserId, int pageIndex, int pageSize)
        {
            Page<OrderInfo> page = new Page<OrderInfo>();
            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    page.Data = db.Query<OrderInfo>(SqlQuery<OrderInfo>.Builder(db).AndWhere(x => x.SellUserId, OperationMethod.Equal, sellUserId)).Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.CreateTime).ToList<OrderInfo>();

                    page.TotalCount = db.Query<OrderInfo>(SqlQuery<OrderInfo>.Builder(db).AndWhere(x => x.SellUserId, OperationMethod.Equal, sellUserId)).ToList<OrderInfo>().Count;

                    page.PageSize = pageSize;
                    page.PageIndex = pageIndex;
                    page.PageYe = (int)Math.Ceiling((decimal)page.TotalCount / page.PageSize);
                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(OrderInfoDal), "GetOrderBySellUserId", Engineer.ccc, new { sellUserId = sellUserId }, ex);
            }

            return page;
        }

        #endregion

        /// <summary>
        /// 获取在售的订单列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<OrderViewModel> GetAllOnSaleOrderList(int pageIndex, int pageSize, int type, int isMoney)
        {

            List<OrderViewModel> list = new List<OrderViewModel>();

            string sql = @" select * from  
(select o.OrderId, o.BuyUserId, o.BuyAccountName, o.SellUserId, o.SellAccountName, o.IsMoney, o.OrderType, o.Sataus, o.Qty, o.BuyMoney, o.CreateTime, o.Remark, o.UpdateTime, o.imgPath,u.UserName as SalerUserName,ROW_NUMBER() over (order by o.CreateTime ) as num
 from
 OrderInfo as o inner join UserInfo as u on o.SellUserId = u.UserId  where Sataus = 1 and OrderType=@type and IsMoney=@isMoney ) as t where t.num between @pageBegin and @pageEnd";

            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    list = db.DbConnecttion.Query<OrderViewModel>(sql, Engineer.ggg, new { type = type, isMoney = isMoney, pageBegin = (pageIndex - 1) * pageSize + 1, pageEnd = pageIndex * pageSize }).ToList();

                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(OrderInfoDal), "GetAllOnSaleOrderList", Engineer.ggg, new { type = type, pageIndex = pageIndex, pageSize = pageSize }, ex);
            }
            return list;
        }

        /// <summary>
        /// 获取所有在售商品的数量
        /// </summary>
        /// <returns></returns>
        public int GetAllOnSaleOrderCount(int type, int IsMoney)
        {
            int result = 0;
            string sql = " select count(1) from   OrderInfo  where Sataus = 1 and OrderType=@type and IsMoney=@IsMoney ";

            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    result = db.DbConnecttion.ExecuteScalar<int>(sql, Engineer.ggg, new { type = type, IsMoney = IsMoney });

                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(OrderInfoDal), "GetAllOnSaleOrderCount", Engineer.ggg, new { type = type, IsMoney = IsMoney }, ex);
            }
            return result;
        }
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool CreateOrder(OrderInfo order)
        {
            bool result = false;

            try
            {
                using (var db = WriteSanNongDunDbBase())
                {

                    result = db.Insert<OrderInfo>(order);

                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(OrderInfoDal), "CreateOrder", Engineer.ggg, order, ex);
            }
            return result;

        }
        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="pageIndex"></param>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Page<OrderInfo> GetOrderListByUserName(string phone, int pageIndex, int pageSize, int type, int status)
        {
            Page<OrderInfo> page = new Page<OrderInfo>();

            string strWhere = "";
            string typeWhere = "";
            string statusWhere = "";
            if (type > 0) 
            {
                typeWhere = " and OrderType=@type ";
            }
            if (status > 0) 
            {
                statusWhere = " and Sataus=@status ";
            }

            if (!string.IsNullOrEmpty(phone))
            {
                strWhere = " and u.UserName like @phone";
            }
            try
            {
                string sql = @"select * from 
                                (select OrderId, BuyUserId, BuyAccountName, SellUserId, SellAccountName, OrderType, Sataus, Qty, BuyMoney, o.CreateTime, Remark,ROW_NUMBER() over(Order by o.CreateTime desc) as num 
                                from OrderInfo as o inner join UserInfo as u on (o.BuyUserId=u.UserId or o.SellUserId = u.UserId)
                                where 1=1  " + typeWhere + statusWhere + strWhere + " )as t where t.num between @pageBegin and @pageEnd";

                string sqlCount = @" select COUNT(1) from OrderInfo as o inner join UserInfo as u on (o.BuyUserId=u.UserId or o.SellUserId = u.UserId)
                                       where 1=1   " + typeWhere + statusWhere + strWhere;

                using (var db = ReadOnlySanNongDunConn())
                {
                    var list = db.DbConnecttion.Query<OrderInfo>(sql, Engineer.ggg, new { type = type, status = status, phone = "%" + phone + "%", pageBegin = (pageIndex - 1) * pageSize + 1, pageEnd = pageIndex * pageSize }).ToList();

                    if (list != null)
                    {

                        page.Data = list;

                        page.TotalCount = db.DbConnecttion.ExecuteScalar<int>(sqlCount, Engineer.ggg, new { type = type, status = status, phone = "%" + phone + "%" });

                        page.PageIndex = pageIndex;
                        page.PageSize = pageSize;


                    }

                }



            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(OrderInfoDal), "GetOrderListByUserName", Engineer.ggg, new { type = type, status = status, phone = phone, pageIndex = pageIndex, pageSize = pageSize }, ex);
            }

            return page;

        }
        /// <summary>
        /// 获取个人在售订单数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetOnSaleOrderCountByUserId(int userId)
        {
            int result = 0;
            string sql = @"select COUNT(1) from orderInfo where Sataus in (1,2,3) and SellUserId=@userId";
            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    result = db.DbConnecttion.ExecuteScalar<int>(sql, Engineer.ggg, new { userId = userId });
                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(OrderInfoDal), "GetOnSaleOrderCountByUserId", Engineer.ggg, new { userId = userId }, ex);
            }
            return result;
        }
        /// <summary>
        /// 获取所有交易中的订单
        /// </summary>
        /// <returns></returns>
        public List<OrderInfo> GetTradingOrder()
        {
            List<OrderInfo> list = new List<OrderInfo>();

            int[] status = {2,3};
            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {

                    list = db.Query<OrderInfo>(SqlQuery<OrderInfo>.Builder(db).AndWhere(o => o.Sataus, OperationMethod.In, status)).ToList();

                }

            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(OrderInfoDal), "GetTradingOrder", Engineer.ggg, null, ex);
            }
            return list;
        }
       
        /// <summary>
        /// 获取未被取消的最后一笔售卖订单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OrderInfo GetLastOrder(int userId)
        {
            OrderInfo order = new OrderInfo();
            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    order = db.Query<OrderInfo>(SqlQuery<OrderInfo>.Builder(db).AndWhere(o => o.SellUserId, OperationMethod.Equal, userId).AndWhere(o => o.Sataus, OperationMethod.NotEqual, 5).OrderBy(o => o.CreateTime, true)).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(OrderInfoDal), "GetLastOrder", Engineer.ggg, new { userId=userId}, ex); 
            }
            return order;
        }
    }
}
