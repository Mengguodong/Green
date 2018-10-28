using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.ViewModel
{
   public class UserLogInfoModel
    {
        //LogId, LogType, UserId, Number, CreateTime, AdminLogType
       public int num { get; set; }
       public int UserId { get; set; }
       public decimal Number { get; set; }
       public DateTime CreateTime { get; set; }
       public int AdminLogType { get; set; }
       public string UserName { get; set; }
       public string RealName { get; set; }
       public int LogType { get; set; }
       public decimal Ep { get; set; }
       public decimal Zfc { get; set; }
    
    }
}
