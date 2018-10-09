using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
   public class UserAccount
    {
            //create table UserAccount(
            //    AccountId INT NOT NULL PRIMARY KEY , 
            //    UserId int ,
            //    LoginName  VARCHAR(255) ,
            //    SumCoin   decimal(18,5),
            //    LockCoin   decimal(18,5),
            //    MotherCoin    decimal(18,5)
            //   ,SmartCalculate decimal(18,5)
            //   ,LinkCalculate decimal(18,5)
            //   ,NodeCalculate decimal(18,5)
            //   ,LeftScore decimal(18,5)
            //   ,RightScore decimal(18,5)
            //   ,CreateTime date
   
            //);


       public int AccountId { get; set; }
       public int UserId { get; set; }
       public string LoginName { get; set; }
       public decimal SumCoin { get; set; }
       public decimal LockCoin { get; set; }
       public decimal MotherCoin { get; set; }
       public decimal SmartCalculate { get; set; }
       public decimal LinkCalculate { get; set; }
       public decimal NodeCalculate { get; set; }
       public decimal LeftScore { get; set; }
       public decimal RightScore { get; set; }
       public DateTime CreateTime { get; set; }
    }
}
