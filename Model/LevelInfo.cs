using DapperExAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{

    public class LevelInfo
    {
  

            //--级别表
            //create table LevelInfo(
            //    LevelId INT NOT NULL PRIMARY KEY , 
          
            //   ,SmallValue int
            //   ,BigValue int
            //   ,SmartCalculate decimal(18,5)
            //   ,LinkCalculate decimal(18,5)
            //   ,LevelStatus int
            //);
     

        public int LevelId { get; set; }

        public int SmallValue { get; set; }
        public int BigValue { get; set; }
        public decimal SmartCalculate { get; set; }
        public decimal LinkCalculate { get; set; }
        public int LevelStatus { get; set; }

    }

    public enum LevelType
    {
       
        /// <summary>
        /// 酿酒工
        /// </summary>
        JiuNong = 1,
        /// <summary>
        /// 酒坊
        /// </summary>
        ZuoFang = 2,
        /// <summary>
        /// 酒坊主
        /// </summary>
        CheJian = 3,
        /// <summary>
        /// 庄园
        /// </summary>
        JiuChang = 4
    }
}
