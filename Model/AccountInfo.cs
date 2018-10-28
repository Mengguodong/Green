using DapperExAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    [Serializable]
    [DataContract]
    public class AccountInfo
    {
//        AccountId int primary key ,
//UserName varchar(200),
//UserId int,               --  用户id
//Sorce decimal (18,4),     --积分
// GreenCount decimal (18,4), --绿氧总数量
//  LeftAchievement decimal (18,2), --左区业绩
//   RightAchievement decimal (18,2),--右区业绩
//    LeftCount  int ,--左区新增
//    RightCount  int,--右区新增
//    HongBao decimal (18,4),        --红包总数
//     StaticsRelease decimal (18,4),   --静态总数
//      CreateTime datetime,              
//FreezeGreen decimal (18,4),
//GreenTotal decimal (18,4)      --所有总数



        /// <summary>
        /// 账户ID
        /// </summary>
        [DataMember(Name = "AccountId")]
        [Display(Name = "AccountId")]
        [Id(true)]
        public int AccountId { get; set; }
      
        public string UserName { get; set; }

        public int UserId { get; set; }
        public decimal Score { get; set; }
        public decimal GreenCount { get; set; }
        public decimal LeftAchievement { get; set; }
        public decimal RightAchievement { get; set; }
        public int LeftCount { get; set; }
        public int RightCount { get; set; }
        public decimal HongBao { get; set; }
        public decimal StaticsRelease { get; set; }
        public DateTime CreateTime { get; set; }
        public decimal FreezeGreen { get; set; }
        public decimal GreenTotal { get; set; }

    }
}
