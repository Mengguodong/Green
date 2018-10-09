using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
  public  class Finance
    {
            //    --财务明细表   每日返利
            //create table Finance(
            //    FinanceId INT NOT NULL PRIMARY KEY , 
            //    UserId int ,
            //    LoginName  VARCHAR(255)  ,
            //    InCoin   decimal(18,5),
            //    OutCoin    decimal(18,5)
            //   ,Remarks varchar(255)
            //   ,CreateTime date
            //   ,FinanceStatus int
   
            //);


      public int FinanceId { get; set; }
      public int UserId { get; set; }
      public string LoginName { get; set; }
      public decimal InCoin { get; set; }
      public decimal OutCoin { get; set; }
      public string Remarks { get; set; }
      public DateTime CreateTime { get; set; }
      public int FinanceStatus { get; set; }
     

    }
}
