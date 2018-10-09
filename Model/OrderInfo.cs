using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class OrderInfo
    {
            //--SACC 订单表  和转币
            //create table OrderInfo(

            //    OrderId INT NOT NULL PRIMARY KEY , 
            //    OrderCode varchar(255),
            //    UserId int ,
            //    BuyLoginName  VARCHAR(255)
            //   ,BuyRealName varchar(255)
            //   ,SellRealName varchar(255)
            //   ,SellLoginName  VARCHAR(255)
            //   ,Remarks varchar(255)
            //   ,CreateTime date
            //   ,BuyTime date
            //   ,PaymentImageUrl VARCHAR(255)
            //   ,OrderStatus int --1:待购买 2:已付款 3:完成  4:取消 
            //   ,SellCoin int
            //   ,BuyMoney decimal(18,5)
            //);


        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public int UserId { get; set; }
        public string BuyLoginName { get; set; }
        public string BuyRealName { get; set; }
        public string SellRealName { get; set; }
        public string SellLoginName { get; set; }
        public string Remarks { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime BuyTime { get; set; }
        public string PaymentImageUrl { get; set; }
        public int OrderStatus { get; set; }
        public int SellCoin { get; set; }
        public decimal BuyMoney { get; set; }


  







    }
}
