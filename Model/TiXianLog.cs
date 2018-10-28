using DapperExAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class TiXianLog
    {
        //        TiXianLoggId int primary key ,
        //UserId int ,
        //LogCount decimal,
        //CreateTime datetime
        [Id(true)]
        public int TiXianLoggId { get; set; }
        public int UserId { get; set; }
        public decimal LogCount { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
