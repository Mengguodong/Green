using DapperExAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EveryDayEpService
{
   public class UserAccInfoModel
    {
       //level	AccountId	AccountName	UserId	Ep	Sacc	Sorce	LeftAchievement	RightAchievement	WaitRelease	CreateTime	TotalAssets	FreezeEp	FreezeSacc
        [DataMember(Name = "AccountId")]
        [Display(Name = "AccountId")]
        [Id(true)]
        public int AccountId { get; set; }
        /// <summary>
        /// 钱包地址
        /// </summary>
        [DataMember(Name = "AccountName")]
        [Display(Name = "AccountName")]
        public string AccountName { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember(Name = "UserId")]
        [Display(Name = "UserId")]
        public int UserId { get; set; }
        /// <summary>
        /// EP
        /// </summary>
        [DataMember(Name = "Ep")]
        [Display(Name = "Ep")]
        public decimal Ep { get; set; }
        /// <summary>
        /// Zfc
        /// </summary>
        [DataMember(Name = "Zfc")]
        [Display(Name = "Zfc")]
        public decimal Zfc { get; set; }


        /// <summary>
        /// 激活卡数量
        /// </summary>
        [DataMember(Name = "ActivationCount")]
        [Display(Name = "ActivationCount")]
        public int ActivationCount { get; set; }
        /// <summary>
        /// 左区业绩
        /// </summary>
        [DataMember(Name = "LeftAchievement")]
        [Display(Name = "LeftAchievement")]
        public decimal LeftAchievement { get; set; }
        /// <summary>
        /// 右区业绩
        /// </summary>
        [DataMember(Name = "RightAchievement")]
        [Display(Name = "RightAchievement")]
        public decimal RightAchievement { get; set; }
        /// <summary>
        /// 动态待释放的EP
        /// </summary>
        [DataMember(Name = "WaitRelease")]
        [Display(Name = "WaitRelease")]
        public decimal WaitRelease { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 总资产
        /// </summary>
        [DataMember(Name = "TotalAssets")]
        [Display(Name = "TotalAssets")]
        public decimal TotalAssets { get; set; }
        /// <summary>
        /// 冻结EP
        /// </summary>
        [DataMember(Name = "FreezeEp")]
        [Display(Name = "FreezeEp")]
        public decimal FreezeEp { get; set; }
        /// <summary>
        /// 冻结Zfc
        /// </summary>
        [DataMember(Name = "FreezeZfc")]
        [Display(Name = "FreezeZfc")]
        public decimal FreezeZfc { get; set; }
        /// <summary>
        /// 冻结静态待释放
        /// </summary>
        [DataMember(Name = "StaticsRelease")]
        [Display(Name = "StaticsRelease")]
        public decimal StaticsRelease { get; set; }

       /// <summary>
       /// 用户级别
       /// </summary>
        public int Level { get; set; }
        public string TeamParentId { get; set; }
        /// <summary>
        /// 左区ID
        /// </summary>
        public string LeftId { get; set; }
        /// <summary>
        /// 右区ID
        /// </summary>
        public string RightId { get; set; }

        public int YesTodayIsLogin { get; set; }
    }
}
