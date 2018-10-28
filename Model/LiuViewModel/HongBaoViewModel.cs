using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.LiuViewModel
{
    public class HongBaoViewModel
    {
       
        public int LogId { get; set; }
        public int LogType { get; set; }
        public int UserId { get; set; }
        public int ReUserId { get; set; }
        public decimal LogCount { get; set; }
        public int HongBaoCount { get; set; }
        public DateTime CreateTime { get; set; }
        public string UserName { get; set; }
        public string ReUserName { get; set;}
    }
}
