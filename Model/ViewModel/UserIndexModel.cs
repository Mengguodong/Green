using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.ViewModel
{
    public class UserIndexModel
    {
        //UserId int primary key ,
        //UserName varchar(250),   --登录id
        //Pwd varchar(250),        --登录密码
        //PayPwd varchar(250),     --交易密码
        //IdCard varchar(250),     --身份证号
        //BankName varchar(250),   --银行名称
        //BankNumber varchar(250), --银行卡号
        //Phone varchar(250),      --电话号
        //ParentId int,            --直推父级ID
        //LeftId varchar(250),     --左区团队ID
        //RightId varchar(250),    --右区团队ID
        //TeamParentId varchar(250),-- 父级团队ID
        //IsActivation int ,        --1:激活，0:未激活
        //RealName varchar(250),    --真实姓名
        //CreateTime date,
        //TeamTime date,            --加入团队时间
        //TeamType int,              --1:加入团队，0:未加入团队
        //[level] int,               --等级
        //UserStatus int,           --用户状态
        //UserType int              --用户类型
        public int num { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public string PayPwd { get; set; }
        public string IdCard { get; set; }
        public string BankName { get; set; }
        public string BankNumber { get; set; }
        public string Phone { get; set; }
        public int ParentId { get; set; }
        public string LeftId { get; set; }
        public string RightId { get; set; }
        public string TeamParentId { get; set; }
        public int IsActivation { get; set; }
        public string RealName { get; set; }
        public DateTime CreateTime { get; set; }
        public int TeamType { get; set; }
        public DateTime TeamTime { get; set; }
        public int Level { get; set; }
        public int UserStatus { get; set; }
        public int UserType { get; set; }
     

        //AccountId int primary key ,
        //AccountName varchar(250), --  用户钱包地址  guid32位字符串
        //UserId int,               --  用户id
        //Ep decimal(18,8),         
        //Zfc decimal(18,8),
        //Sorce int,
        //LeftAchievement decimal(18,2), --左区业绩
        //RightAchievement decimal(18,2),--右区业绩
        //WaitRelease decimal(18,8),     --待动态释放
        //CreateTime date,
        //TotalAssets  decimal(18,2)    -------总资产             

        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal GreenCount { get; set; }
        public decimal Sorce { get; set; }
        public decimal LeftAchievement { get; set; }
        public decimal RightAchievement { get; set; }

        public int GreenTotal { get; set; }
        public decimal StaticsRelease { get; set; }

    }
}
