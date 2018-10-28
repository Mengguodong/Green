using DapperExAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class HongBaoLog
    {
        //        LogId int primary key ,
        //LogType int,                 --1 :自己   2：业绩返利
        //UserId int,  
        //ReUserId int,                  --来源id
        //LogCount decimal, 
        //HongBaoCount int ,           --LogType类型为1的时候使用
        //CreateTime datetime
        [Id(true)]
        public int LogId { get; set; }
        public int LogType { get; set; }
        public int UserId { get; set; }
        public int ReUserId { get; set; }
        public decimal LogCount { get; set; }
        public int HongBaoCount { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
