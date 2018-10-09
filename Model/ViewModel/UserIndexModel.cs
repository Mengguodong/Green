using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.ViewModel
{
    public class UserIndexModel
    {
//        UserId,LoginName,LoginPWD,Phone,IdentityCard,RealName,BankName,BankCard,TradePWD,CreateTime,SonSum,ParentId
//,IsTestUser,LevelStatus,UserStatus,AccountId,SumCoin,LockCoin
        public int Num { get; set; }
        public int UserId { get; set; }
        public string LoginName { get; set; }
        public string LoginPWD { get; set; }
        public string Phone { get; set; }
        public string IdentityCard { get; set; }
        public string RealName { get; set; }
        public string BankName { get; set; }
        public string BankCard { get; set; }
        public string TradePWD { get; set; }
        public DateTime CreateTime { get; set; }
        public int SonSum { get; set; }
        public int ParentId { get; set; }
        public int IsTestUser { get; set; }
        public int LevelStatus { get; set; }
        public int UserStatus { get; set; }
        public int AccountId { get; set; }
        public decimal SumCoin { get; set; }
        public decimal LockCoin { get; set; }
        //,MotherCoin,SmartCalculate,LinkCalculate,NodeCalculate	,LeftScore,RightScore
        public decimal MotherCoin { get; set; }
        public decimal SmartCalculate { get; set; }
        public decimal LinkCalculate { get; set; }
        public decimal NodeCalculate { get; set; }
        public decimal LeftScore { get; set; }
        public decimal RightScore { get; set; }
        public string ParentLoginName { get; set; }

    }
}
