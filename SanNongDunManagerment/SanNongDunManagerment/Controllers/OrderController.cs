using Bll;
using Common.Web;
using DataModel;
using DataModel.RequestModel;
using DataModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace SanNongDunManagerment.Controllers
{
    public class OrderController : BaseController
    {
        
        UserAccountBll _accountBll = new UserAccountBll();
        UserBll _userBll = new UserBll();
        //
        // GET: /Order/
        /// <summary>
        /// 交易记录
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="pageIndex"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Index(string phone, int pageIndex = 1, int type = -1, int status = -1)
        {
            Page<OrderInfo> page = new Page<OrderInfo>();
            Page<OrderAdminModel> pageAdmin = new Page<OrderAdminModel>();
            PagedList<OrderAdminModel> pageList = null;

            int pageSize = 20;

            page = _orderBll.GetOrderListByUserName(phone, pageIndex, pageSize, type, status);

            if (page != null && page.Data != null)
            {
                List<OrderAdminModel> list = new List<OrderAdminModel>();
                foreach (var item in page.Data)
                {
                    OrderAdminModel orderAdmin = new OrderAdminModel();
                    UserInfo buyUser = _userBll.GetUserInfoById(item.BuyUserId);
                    UserInfo sellUser = _userBll.GetUserInfoById(item.SellUserId);
                    if (buyUser != null) { orderAdmin.BuyPhone = buyUser.Phone; }
                    if (sellUser != null) { orderAdmin.SellPhone = sellUser.Phone; }

                    orderAdmin.BuyAccountName = item.BuyAccountName;
                    orderAdmin.BuyMoney = item.BuyMoney;
                    orderAdmin.BuyUserId = item.BuyUserId;
                    orderAdmin.CreateTime = item.CreateTime;
                    orderAdmin.imgPath = item.imgPath;
                    orderAdmin.IsMoney = item.IsMoney;
                    orderAdmin.OrderId = item.OrderId;
                    orderAdmin.OrderType = item.OrderType;
                    orderAdmin.Qty = item.Qty;
                    orderAdmin.Remark = item.Remark;
                    orderAdmin.Sataus = item.Sataus;
                    orderAdmin.SellAccountName = item.SellAccountName;
                    orderAdmin.SellUserId = item.SellUserId;
                    orderAdmin.UpdateTime = item.UpdateTime;
                    list.Add(orderAdmin);

                }
                pageAdmin.Data = list;
                pageAdmin.PageIndex = page.PageIndex;
                pageAdmin.PageSize = page.PageSize;
                pageList = new PagedList<OrderAdminModel>(pageAdmin.Data, pageAdmin.PageIndex, pageAdmin.PageSize);
            }


            if (Request.IsAjaxRequest())
            {
                return PartialView("_OrderList", pageList);
            }


            return View(pageList);
        }
        /// <summary>
        /// 继续交易
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="sellUserId"></param>
        /// <param name="buyUserId"></param>
        /// <returns></returns>
        public JsonResult ContinueTransaction(int orderId, int sellUserId, int buyUserId, int orderType)
        {
            bool result = false;
            string msg = "";
            AccountInfo buyAccount = new AccountInfo();
            AccountInfo saleAccount = new AccountInfo();
            OrderInfo orderModel = new OrderInfo();

            orderModel = _orderBll.GetOrderByOrderId(orderId);

            buyAccount = _accountBll.GetAccByUserId(buyUserId);
            saleAccount = _accountBll.GetAccByUserId(sellUserId);

            //释放冻结并退还卖家保证金
            if (orderType == 1)
            {
                buyAccount.Ep += orderModel.Qty;
                saleAccount.FreezeEp -= orderModel.Qty + 100;
                saleAccount.Ep += 100;

            }
            else
            {
                buyAccount.Zfc += orderModel.Qty;
                saleAccount.FreezeZfc -= orderModel.Qty;
                saleAccount.Ep += 100;
                saleAccount.FreezeEp -= 100;
            }
            bool resultsale = _accountBll.UpdateAccInfo(saleAccount);

            bool resultbuy = _accountBll.UpdateAccInfo(buyAccount);

            orderModel = _orderBll.GetOrderByOrderId(orderId);
            orderModel.Sataus = 4;
            orderModel.UpdateTime = DateTime.Now;

            result = _orderBll.UpdateOrderInfo(orderModel);
            if (!result)
            {
                return Json(new { result = result, msg = "操作失败" });
            }

            return Json(new { result = result, msg = "操作成功" });
        }
        /// <summary>
        /// 取消交易
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="sellUserId"></param>
        /// <param name="buyUserId"></param>
        /// <returns></returns>
        public JsonResult CancelTransaction(int orderId)
        {
            bool result = false;
            OrderInfo order = _orderBll.GetOrderByOrderId(orderId);
            order.UpdateTime = DateTime.Now;
            order.Sataus = 5;
            result = _orderBll.UpdateOrderInfo(order);
            //归还冻结
            if (result)
            {

                result = _accountBll.FreezeAccount(order.Qty, order.OrderType, order.SellUserId, 0);

                if (!result)
                {
                    return Json(new { result = false, msg = "冻结金额归还失败！" });
                }

            }
            else
            {
                return Json(new { result = false, msg = "取消订单失败，请稍后再试！" });
            }

            return Json(new { result = true, msg = "订单取消成功！" });

        }

    }
}
