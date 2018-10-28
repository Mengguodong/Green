
﻿using Dal;
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
    public class OrderInfoBll{

        OrderInfoDal _orderDal = new OrderInfoDal();
        UserAccountDal _accDal = new UserAccountDal();
        //UserDal _userDal = new UserDal();
        /// <summary>
        /// 获取所有在售订单 (交易大盘使用)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<OrderViewModel> GetAllOnSaleOrderList(int pageIndex, int pageSize, int type, int isMoney)
        {
            return _orderDal.GetAllOnSaleOrderList(pageIndex,pageSize,type,isMoney);
        }
        /// <summary>
        /// 获取订单数量
        /// </summary>
        /// <returns></returns>
        public int GetAllOnSaleOrderCount(int type, int IsMoney)
        {
            return _orderDal.GetAllOnSaleOrderCount(type,IsMoney);

        }

        #region 订单查询和修改

        /// <summary>
        /// 分页查询用户购买订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Page<OrderInfo> GetOrderByBuyId(int buyUserId, int pageIndex, int pageSize)
        {
            Page<OrderInfo> page = new Page<OrderInfo>();
            page = _orderDal.GetOrderByBuyId(buyUserId, pageIndex, pageSize);
            return page;
        }
        /// <summary>
        /// 分页查询用户出售订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Page<OrderInfo> GetOrderBySellUserId(int sellUserId, int pageIndex, int pageSize)
        {
            Page<OrderInfo> page = new Page<OrderInfo>();
            page = _orderDal.GetOrderBySellUserId(sellUserId, pageIndex, pageSize);
            return page;
        }

        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public OrderInfo GetOrderByOrderId(int orderId)
        {
            OrderInfo data = _orderDal.GetOrderByOrderId(orderId);
          
            return data;
        }

        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public bool UpdateOrderInfo(OrderInfo orderInfo)
        {
            bool isTrue = _orderDal.UpdateOrderInfo(orderInfo);
            return isTrue;
        }


        
        #endregion


        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="Qty"></param>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        /// <param name="TotalPrice"></param>
        /// <returns></returns>
        public bool CreateOrder(int Qty, int type, int userId, decimal TotalPrice, int IsMoney)
        {

            bool result = false;
            OrderInfo order = new OrderInfo();
            //UserInfo user = _userDal.GetUserById(userId);
            AccountInfo account = _accDal.GetAccByUserId(userId);

            order.SellUserId = userId;
            order.SellAccountName = account.AccountName;
            order.Sataus = 1;
            order.Remark = "";
            order.Qty = Qty;
            order.OrderType = type;
            order.CreateTime = DateTime.Now;
            order.BuyMoney = TotalPrice;
            order.IsMoney = IsMoney;
            order.UpdateTime = DateTime.Parse("1900-01-01");
            //order.SellPhone = user.Phone;
            result = _orderDal.CreateOrder(order);
            


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
            return _orderDal.GetOrderListByUserName(phone,pageIndex,pageSize,type,status);
        }
        /// <summary>
        /// 获取个人在售订单个数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetOnSaleOrderCountByUserId(int userId)
        {
            return _orderDal.GetOnSaleOrderCountByUserId(userId);
        }
        /// <summary>
        /// 24小时内有没有产生交易的订单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CheckOrderCountByUserId(int userId)
        {
            //获取最近的一笔 未取消的售卖订单
            OrderInfo order = new OrderInfo();

            order = _orderDal.GetLastOrder(userId);



            if (order!=null&&order.CreateTime.AddDays(1)>=DateTime.Now)
            {
                return true;
            }

            return false;

        }
    }
}
