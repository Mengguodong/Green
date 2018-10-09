using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
   public class RechargeLockCoin
    {
            //        --增加锁仓和充值记录
            //create table RechargeLockCoin(
            //    RechargeLockCoinId INT NOT NULL PRIMARY KEY , 
            //    UserId int ,
            //    LoginName  VARCHAR(255)  

            //   ,Remarks decimal(18,5)
            //   ,CreateTime date
  
            //   ,LockStatus int   --1：锁仓  2：充值
   
            //);



        public int RechargeLockCoinId { get; set; }
        public int UserId { get; set; }
        public string LoginName { get; set; }
        public string Remarks { get; set; }
        public DateTime CreateTime { get; set; }

        public int LockStatus { get; set; }
 





    }
}
