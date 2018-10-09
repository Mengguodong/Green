using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.ViewModel
{
    /// <summary>
    /// 用户账户视图Model
    /// </summary>
    public class UserAccountInfoModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户状态（是否激活）
        /// </summary>
        public int IsActivate { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int LevelId { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 开户银行
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 银行卡卡号
        /// </summary>
        public string BankCardNo { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string MobilePhone { get; set; }
        /// <summary>
        /// 是否收回本金
        /// </summary>
        public int IsReturn { get; set; }
        /// <summary>
        /// 下级数量
        /// </summary>
        public int DownCount { get; set; }
        /// <summary>
        /// 是否成团
        /// </summary>
        public int IsTeam { get; set; }

        public string DeliveryAddress { get; set; }
        /// <summary>
        /// 酒币数量
        /// </summary>
        public decimal WineCoin { get; set; }
        /// <summary>
        /// 酒糟数量
        /// </summary>
        public int LeesCount { get; set; }
        /// <summary>
        /// 酒窖数量
        /// </summary>
        public decimal WineCellar { get; set; }
        /// <summary>
        /// 酒窖剩余总次数
        /// </summary>
        public int WCRemainder { get; set; }
        /// <summary>
        /// 酒窖总次数
        /// </summary>
        public int WCTotalCount { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal Score { get; set; }
        /// <summary>
        /// 成酒实际酒量
        /// </summary>
        public decimal FinishedWineRealNum { get; set; }
        /// <summary>
        /// 积分加成酒总产量
        /// </summary>
        public decimal FinishedWineTotal { get; set; }
        /// <summary>
        /// 冻结成酒
        /// </summary>
        public decimal FreezeWine { get; set; }
    }
}
