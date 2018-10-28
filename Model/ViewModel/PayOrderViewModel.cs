using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.ViewModel
{
    public  class PayOrderViewModel
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 卖家ID
        /// </summary>
        public int SellUserId { get; set; }
        /// <summary>
        /// 卖家钱包地址
        /// </summary>
        public string SellAccountName { get; set; }
        /// <summary>
        /// --是否是现金交易
        /// </summary>
        public int IsMoney { get; set; }

        /// <summary>
        /// 订单类型   1:卖EP，2:卖Zfc
        /// </summary>
        public int OrderType { get; set; }
     
        /// <summary>
        /// 交易数量
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// 购买总值    订单类型为1  该值为Zfc  订单类型为2 该值为积分
        /// </summary>
        public decimal BuyMoney { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankNumber { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
