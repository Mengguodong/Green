using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
   public class ComputingPower
    {
            //    --奖金明细表    	
            //create table ComputingPower(
            //    ComputingPowerId INT NOT NULL PRIMARY KEY , 
            //    UserId int ,
            //    LoginName  VARCHAR(255)  ,
            //    cpStatus   int,--1:链接算力	2:节点算力	3:智能算力
            //    Coin    decimal(18,5)
            //   ,Remarks varchar(255)
            //   ,CreateTime date
   
            //);


       public int ComputingPowerId { get; set; }
       public int UserId { get; set; }
       public string LoginName { get; set; }
       public int cpStatus { get; set; }
       public decimal Coin { get; set; }
       public int Remarks { get; set; }
       public int CreateTime { get; set; }

    }
}
