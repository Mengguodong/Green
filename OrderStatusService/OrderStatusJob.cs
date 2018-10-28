using Dal;
using DataModel;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace OrderStatusService
{
   public class OrderStatusJob:IJob
    {
       OrderInfoDal _orderDal = new OrderInfoDal();
       UserDal _userDal = new UserDal();

        public void Execute(JobExecutionContext context)
        {
            //获取所有交易中的订单（所有状态为2，3的订单）
            List<OrderInfo> list = new List<OrderInfo>();
            list = _orderDal.GetTradingOrder();

            bool result = false;
             

            if (list!=null&&list.Count>0)
            {
                //根据updateTime去计算是否超时未操作

                foreach (var item in list)
                {
                   
                    if (DateTime.Now>= item.UpdateTime.AddHours(6))
                    {
                        UserInfo userInfo = new UserInfo();
                        //如果超时未操作，根据状态记录 买家/卖家的违规次数   违规次数大于等于3 账号停用 
 
                        if (item.Sataus==2)//买家未付款
                        {
                            userInfo = _userDal.GetUserById(item.BuyUserId);

                            userInfo.ViolationsCount++;

                            if (userInfo.ViolationsCount>=3)
                            {
                                userInfo.UserStatus = 0;
                            }

                            result = _userDal.UpdateUserInfo(userInfo);

                            if (!result)
                            {
                                LogHelper.WriteInfo(typeof(OrderStatusJob),"更新用户违规次数失败，userID："+item.BuyUserId);
                            }

                        }
                        else if (item.Sataus==3) //卖家未确认
                        {
                            userInfo = _userDal.GetUserById(item.SellUserId);

                            userInfo.ViolationsCount++;

                            if (userInfo.ViolationsCount >= 3)
                            {
                                userInfo.UserStatus = 0;
                            }

                            result = _userDal.UpdateUserInfo(userInfo);

                            if (!result)
                            {
                                LogHelper.WriteInfo(typeof(OrderStatusJob), "更新用户违规次数失败，userID：" + item.SellUserId);
                            }
                        }


                        //将状态异常的订单设置为已冻结，联系客服  决定 交易流向

                        item.Sataus = 6;
                        item.UpdateTime = DateTime.Now;

                        result = _orderDal.UpdateOrderInfo(item);

                        if (!result)
                        {
                             LogHelper.WriteInfo(typeof(OrderStatusJob), "异常订单冻结失败，userID：" + item.SellUserId+"  OrderId:"+item.OrderId);
                        }
                        
                    }
                }

             
            }

          
        }
    }
}
