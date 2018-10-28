using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.ViewModel
{
    public class OrderAdminModel
    {
        /// <summary>
        /// 订单ID
        /// </summary>
      
        public int OrderId { get; set; }
        /// <summary>
        /// 买家ID
        /// </summary>
        public int BuyUserId { get; set; }
        /// <summary>
        /// 买家钱包地址
        /// </summary>
        public string BuyAccountName { get; set; }
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
        /// 订单状态 1:在售，2:已锁定（待付款） 3,待确认（待发货）  4:完成，5:已取消 6：已冻结
        /// </summary>
        public int Sataus { get; set; }
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
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 打款凭证图片
        /// </summary>
        public string imgPath { get; set; }

        public string BuyPhone { get; set; }
        public string SellPhone { get; set; }
    }
}
