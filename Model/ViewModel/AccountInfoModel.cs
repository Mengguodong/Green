using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.ViewModel
{
   public class AccountInfoModel
    {
       /// <summary>
       /// 账户ID
       /// </summary>
        public int AccountId { get; set; }
       /// <summary>
       /// 总资产
       /// </summary>
        public decimal TotalAssets { get; set; }
       /// <summary>
       /// 用户ID
       /// </summary>
        public int UserId { get; set; }
       /// <summary>
       /// 等级
       /// </summary>
        public string level { get; set; }
    }
}
