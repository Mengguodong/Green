using Bll;
using Common.Web;
using DataModel;
using DataModel.RequestModel;
using DataModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using System.Drawing.Imaging;

namespace SanNongDunWeb.Controllers
{
    public class TransactionController : CommonBaseController
    {

        OrderInfoBll _bll = new OrderInfoBll();
        GlobalConfigBll _configBll = new GlobalConfigBll();
        UserAccountBll _accountBll = new UserAccountBll();
        LogBll _logBll = new LogBll();
        UserBll _userBll = new UserBll();
        //
        // GET: /Transaction/
        /// <summary>
        /// 交易大盘首页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult Index(int pageIndex=1,int type=1,int IsMoney=1)
        {

            Page<OrderViewModel> page = new Page<OrderViewModel>();

            List<OrderViewModel> list = new List<OrderViewModel>();

            page.PageSize = 15;
            
            //获取所有在售的商品（默认为EP）
             list = _bll.GetAllOnSaleOrderList(pageIndex,page.PageSize,type,IsMoney);

         
            if (list.Count!=0&&list!=null)
            {
                page.Data = list;
            }
            page.PageIndex = pageIndex;

            page.TotalCount = _bll.GetAllOnSaleOrderCount(type, IsMoney);

            page.PageYe = (int)Math.Ceiling(decimal.Parse(page.TotalCount.ToString())/decimal.Parse(page.PageSize.ToString()));

            decimal ZfcPrice = 0;

            ZfcPrice = decimal.Parse(_configBll.GetValueByConfigName("ZfcPrice"));

            ViewBag.ZfcPrice = ZfcPrice;

            ViewBag.Type = type;
            ViewBag.IsMoney = IsMoney;


            if (Request.IsAjaxRequest())
            {
                return PartialView("_Index",page);
            }
            return View(page);
        }
        /// <summary>
        /// 我的订单
        /// </summary>
        /// <returns></returns>
        public ActionResult MyOrders(int pageIndex=1,int type=1)
        {
            Page<OrderInfo> page = new Page<OrderInfo>();
            int pageSize = 15;

            if (type==1)//我买入的订单（所有订单）
            {
                page = _bll.GetOrderByBuyId(_ServiceContext.SND_CurrentUser.UserId, pageIndex, pageSize);
               
            }
            else //我卖出的订单（所有订单）
            {
                page = _bll.GetOrderBySellUserId(_ServiceContext.SND_CurrentUser.UserId,pageIndex,pageSize);

            }

            ViewBag.Type = type;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_MyOrders", page);
            }

            return View(page);
        }
        /// <summary>
        /// 买家取消订单（已锁定状态）
        /// </summary>
        /// <returns></returns>
        public JsonResult CancelOrderBuyer(int orderId)
        {
            bool result = false;
            OrderInfo order = _bll.GetOrderByOrderId(orderId);

            order.UpdateTime = DateTime.Now;
            order.Sataus = 1;
            order.BuyUserId = 0;
            order.BuyAccountName = "";

            result = _bll.UpdateOrderInfo(order);

            if (!result)
            {
                 return Json(new { result = false, msg = "取消订单失败，请稍后再试！" });
            }

            return Json(new { result=true,msg="订单取消成功！"});
        }
        /// <summary>
        /// 卖家取消订单（在售订单）
        /// </summary>
        /// <returns></returns>
        public JsonResult CancelOrderSaler(int orderId)
        {
            bool result = false;
            OrderInfo order = _bll.GetOrderByOrderId(orderId);
            order.UpdateTime = DateTime.Now;
            order.Sataus = 5;
            result = _bll.UpdateOrderInfo(order);
            //归还冻结
            if (result)
            {

                result = _accountBll.FreezeAccount(order.Qty,order.OrderType,_ServiceContext.SND_CurrentUser.UserId,0);

                if (!result)
                {
                     return Json(new { result = false, msg = "冻结金额归还失败，请联系客服解决！" });
                }

            }
            else
            {
                return Json(new { result = false, msg = "取消订单失败，请稍后再试！" });
            }

            return Json(new { result = true, msg = "订单取消成功！" });
        }
        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult OrderDetail(int orderId)
        {
            OrderInfo order = new OrderInfo();
            order = _bll.GetOrderByOrderId(orderId);
            return View(order);
        }

        /// <summary>
        /// 买入订单继续支付(页面展示)
        /// </summary>
        /// <returns></returns>
        public ActionResult Pay(int orderId)
        {
            OrderInfo order = new OrderInfo();
            order = _bll.GetOrderByOrderId(orderId);

          
            PayOrderViewModel model = new PayOrderViewModel();

            if (order != null)
            {
                UserInfo userInfo = new UserInfo();
                userInfo = _userBll.GetUserInfoById(order.SellUserId);

                model.BankName = userInfo.BankName;
                model.RealName = userInfo.RealName;
                model.BankNumber = userInfo.BankNumber;
                model.BuyMoney = order.BuyMoney;
                model.CreateTime = order.CreateTime;
                model.IsMoney = order.IsMoney;
                model.OrderId = order.OrderId;
                model.OrderType = order.OrderType;
                model.Qty = order.Qty;
                model.SellAccountName = order.SellAccountName;
                model.SellUserId = order.SellUserId;
                model.UpdateTime = order.UpdateTime;
            }


            return View(model);
        }
        /// <summary>
        /// 更新订单状态（确认打款）
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateOrderStatusBuy(int orderId,string imgStr)
        {
            bool result = false;
            OrderInfo order = new OrderInfo();
            if (string.IsNullOrEmpty(imgStr))
            {
                return Json(new { result = false, msg = "请上传打款凭证！" }); 
            }
            order = _bll.GetOrderByOrderId(orderId);

            if (order == null)
            {
                return Json(new { result = false, msg = "订单信息有误，提交失败！" });
            }

            string imgPath = Base64StringToImage(imgStr);

            order.Sataus = 3;
            order.imgPath = imgPath;
            result = _bll.UpdateOrderInfo(order);
            if (!result)
            {
               return Json(new { result = false, msg = "确认打款失败，请稍后再试！" }); 
            }

            return Json(new {result=true });
        }

        //threeebase64编码的字符串转为图片
        protected string Base64StringToImage(string strbase64)
        {
            Bitmap bmp = null;
            string  imgPath = "";
            try
            {
                string temp = strbase64.Split(',')[1];
                temp = temp.Replace(" ", "+");
                int mod4 = temp.Length % 4;
                if (mod4 > 0)
                {
                    temp += new string('=', 4 - mod4);
                }

                byte[] arr = Convert.FromBase64String(temp);
                string rootPath = Auxiliary.ConfigKey("ImgPath");
                DateTime date = DateTime.Now;
                //生成目录
                string path = rootPath + string.Format("{0}/{1}/{2}", date.Year, date.Month, date.Day);

                MemoryStream ms = new MemoryStream(arr);
               
                    using ( bmp = new Bitmap(ms))
                    {

                        Bitmap B = new Bitmap(bmp.Width, bmp.Height); //新建一个理想大小的图像文件
                        Graphics g = Graphics.FromImage(B);//实例一个画板的对象,就用上面的图像的画板
                        g.DrawImage(bmp, 0, 0);//把目标图像画在这个图像文件的画板上

                        if (!Directory.Exists(path))
                             Directory.CreateDirectory(path);

                        B.Save(path+"/"+date.ToString("yyyyMMddHHmmssfff")+".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);//通过这个图像来保存

                        ms.Close();
                        ms.Dispose();

                        if (B != null)
                        {
                            imgPath = PubConstant.ImageBaseUrl + string.Format("{0}/{1}/{2}", date.Year, date.Month, date.Day) + "/" + date.ToString("yyyyMMddHHmmssfff") + ".jpg";
                            return imgPath;
                        }
                    
                    }
               
               
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(typeof(TransactionController), ex);
            }
            return imgPath;
         
        }


        /// <summary>
        /// 更新订单状态（确认收款）
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult UpdateOrderStatus(int orderId,int type)
        {

            bool result = false;
            AccountInfo buyAccount = new AccountInfo();
            AccountInfo saleAccount = new AccountInfo();
            OrderInfo orderModel = new OrderInfo();

            orderModel = _bll.GetOrderByOrderId(orderId);

            buyAccount = _accountBll.GetAccByUserId(orderModel.BuyUserId);
            saleAccount = _accountBll.GetAccByUserId(orderModel.SellUserId);

            //释放冻结并退还卖家保证金
            if (type==1)
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

            orderModel = _bll.GetOrderByOrderId(orderId);
            orderModel.Sataus = 4;
            orderModel.UpdateTime = DateTime.Now;

            result = _bll.UpdateOrderInfo(orderModel);

            if (resultbuy && resultsale && result)
            {
                return Json(new { result = true, msg = "交易完成，可以在我的订单中查看已完成订单！" });
            }
            else
            {
                return Json(new { result = false, msg = "交易出错，请联系客服解决！" });
            }

        }

        public ActionResult LaunchSale(string zfcPrice)
        {
            
            ViewBag.ZFCPrice = zfcPrice;

            return View();
        }

        /// <summary>
        /// 发起售卖
        /// </summary>
        /// <returns></returns>
        public JsonResult StartSale(string payPwd,int type, int Qty, decimal TotalPrice, int IsMoney=1)
        {

            //验证支付密码
            if (Auxiliary.Md5Encrypt(payPwd) != _ServiceContext.SND_CurrentUser.PayPwd)
            {
                return Json(new { result = false, msg = "支付密码错误！" });
            }
            bool result = false;

            string msg = "";

            int userId = _ServiceContext.SND_CurrentUser.UserId;

            UserInfo user = _userBll.GetUserInfoById(userId);

            if (string.IsNullOrEmpty(user.BankNumber))
            {
                 return Json(new { result = result, msg = "您的银行卡不能为空，请完善您的个人信息！" });
            }
            if (Qty < 200)
            {
                return Json(new { result = result, msg = "EP/ZFC售卖单笔最低数量为200！" });
            }
            if (Qty > 20000)
            {
                return Json(new { result = result, msg = "EP/ZFC售卖单笔最大数量为20000！" });
            }
            if (Qty % 200 != 0)
            {
                return Json(new { result = result, msg = "EP/ZFC售卖数量必须为200的倍数！" });
            }

            //判断账户余额是否足够 
            result = _accountBll.CheckAccountBalance(Qty,type,userId);

            if (!result)
            {

                if (type==1)
                {
                    return Json(new { result = result, msg = "当前EP余额不足或没有足够的保证金！" });
                }
                else
                {
                    return Json(new { result = result, msg = "当前Zfc余额不足或没有足够的保证金！" });
                }

            }
            //判断是否有在售订单
            int orderCount = _bll.GetOnSaleOrderCountByUserId(userId);

            if (orderCount>0)
            {
                 return Json(new { result = result, msg = "您有一笔尚未完成的售卖订单，暂时不能发起新的售卖！" });
            }

            //判断近24小时有没有订单

             result = _bll.CheckOrderCountByUserId(userId);

            if (result)
            {
                 return Json(new { result = result, msg = "您在24小时之内产生过一笔交易，暂时不能发起新的售卖！" });
            }



            //创建订单

            result = _bll.CreateOrder(Qty, type, userId, TotalPrice,IsMoney);

            //订单创建成功，冻结账户(包括保证金)

            if (result)
            {
                result = _accountBll.FreezeAccount(Qty, type, userId,1);

            }
            else  
            {
                return Json(new { result = result, msg = "创建订单失败，请稍后再试！" });
            }

            return Json(new { result = result, msg = "发起挂卖单成功！" });
        }
        /// <summary>
        /// 购买资产
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="PayPwd"></param>
        /// <param name="type"></param>
        /// <param name="isMoeny"></param>
        /// <returns></returns>
        public JsonResult Buy(int orderId,string PayPwd,int type,int isMoeny,int saleUserId)
        {
            bool result = false;

            AccountInfo buyAccount = new AccountInfo();
            AccountInfo saleAccount = new AccountInfo();
            OrderInfo orderModel = new OrderInfo();

            //验证支付密码
            if (Auxiliary.Md5Encrypt(PayPwd) != _ServiceContext.SND_CurrentUser.PayPwd)
            {
                return Json(new { result = false, msg = "支付密码错误！" });
            }

            orderModel = _bll.GetOrderByOrderId(orderId);

            //验证订单状态
            if (orderModel.Sataus!=1)
            {
                return Json(new { result = false, msg = "此商品状态已发生改变，请刷新页面后重新选择！" });
            }

            if (_ServiceContext.SND_CurrentUser.UserId == saleUserId)
            {
                return Json(new { result = false, msg = "您不能购买您自己挂卖的订单！" });
            }

            #region  ZFC购买EP

            if (type==1&&isMoeny==0) //非现金购买EP
            {

                buyAccount = _accountBll.GetAccByUserId(_ServiceContext.SND_CurrentUser.UserId);

                if (buyAccount.Zfc<orderModel.BuyMoney)
                {
                     return Json(new { result = false, msg = "账户ZFC余额不足！" });
                }
                //先锁定
                orderModel.BuyAccountName = buyAccount.AccountName;
                orderModel.BuyUserId = _ServiceContext.SND_CurrentUser.UserId;
                orderModel.Sataus = 2; //已锁定
                orderModel.UpdateTime = DateTime.Now;
                result = _bll.UpdateOrderInfo(orderModel);

                if (!result)
                {
                     return Json(new { result = false, msg = "锁定订单失败，请稍后再试！" });
                }
                //在扣款并退还卖家保证金
                saleAccount = _accountBll.GetAccByUserId(saleUserId);
                buyAccount.Zfc -= orderModel.BuyMoney;
                buyAccount.Ep += orderModel.Qty;

                saleAccount.FreezeEp -= orderModel.Qty+100;
                saleAccount.Zfc += orderModel.BuyMoney;
                saleAccount.Ep += 100;

              bool   resultsale = _accountBll.UpdateAccInfo(saleAccount);

              bool  resultbuy = _accountBll.UpdateAccInfo(buyAccount);

              orderModel = _bll.GetOrderByOrderId(orderId);
              orderModel.Sataus = 4;
              orderModel.UpdateTime = DateTime.Now;

              result = _bll.UpdateOrderInfo(orderModel);

              if (resultbuy&&resultsale&&result)
              {
                     return Json(new { result = true, msg = "交易完成，可以到我的订单中查看已完成订单！" });
              }
              else
              {
                  return Json(new { result = false, msg = "交易出错，请联系客服解决！" });
              }

            }

            #endregion

            //锁定订单
            buyAccount = _accountBll.GetAccByUserId(_ServiceContext.SND_CurrentUser.UserId);
           

            orderModel.BuyAccountName = buyAccount.AccountName;
            orderModel.BuyUserId = _ServiceContext.SND_CurrentUser.UserId;
            orderModel.Sataus = 2;
            orderModel.UpdateTime = DateTime.Now;

            result = _bll.UpdateOrderInfo(orderModel);

            if (!result)
            {
                 return Json(new { result = false, msg = "锁定订单失败，请稍后再试！" });
            }

            return Json(new { result = true, msg = "订单已成功锁定，请到我的订单继续完成支付！" });
        }
 

        
    }
}
