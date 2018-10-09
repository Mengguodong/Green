using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class GlobalConfig
    {
        //        --全局配置表
        //create table GlobalConfig(

        //    GlobalConfigId INT NOT NULL PRIMARY KEY , 
        //    NodeCalculate decimal(18,5),
        //    SmartCalculateShop decimal(18,5)
        //   ,EveryDayRebate decimal(18,5)
        //   ,OutCoinRate decimal(18,5)
        //   ,SACCRMB decimal(18,2)
        //   ,OrderSmallValue  int
        //   ,OrderBigValue int
        //   ,UpdateTime date
        //   ,IdCardBigCount int
   
        //);

        public int GlobalConfigId { get; set; }
        public decimal NodeCalculate { get; set; }
        public decimal SmartCalculateShop { get; set; }
        public decimal EveryDayRebate { get; set; }
        public decimal SACCRMB { get; set; }
        public int OrderSmallValue { get; set; }
        public int OrderBigValue { get; set; }
        public DateTime UpdateTime { get; set; }
        public int IdCardBigCount { get; set; }
    }
}
