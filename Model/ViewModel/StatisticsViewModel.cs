using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.ViewModel
{
  public  class StatisticsViewModel
    {

      /// <summary>
      /// 开始时间
      /// </summary>
        public DateTime? StartTime { get; set; }
      /// <summary>
      /// 结束时间
      /// </summary>
        public DateTime? EndTime { get; set; }
      /// <summary>
      /// 总资产
      /// </summary>
        public decimal TotalAssets { get; set; }
      /// <summary>
      /// 总激活人数
      /// </summary>
        public int TotalActivation { get; set; }
      /// <summary>
      /// 总人数
      /// </summary>
        public int TotalUser { get; set; }

      /// <summary>
      /// 总释放EP
      /// </summary>
        public decimal TotalEp { get; set; }
      /// <summary>
      /// 总释放Zfc
      /// </summary>
        public decimal TotalZfc { get; set; }
      /// <summary>
      /// 总充值EP
      /// </summary>
        public decimal TotalScore { get; set; }

        public int SumMoney { get; set; }

        public GreenScore SumGreenScore { get; set; }
        public decimal SumOutMoney { get; set; }
     
    }
  public class GreenScore 
  {
      public decimal GreenCount { get; set; }
      public decimal Score { get; set; }
  }
}
