using DapperExAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class ScoreLog
    {
        //        ScoreLogId int primary key ,
        //UserId int , 
        //LogCount decimal,
        //CreateTime datetime,
        //Remark varchar(255)
        [Id(true)]
        public int ScoreLogId { get; set; }

        public int UserId { get; set; }

        public decimal LogCount { get; set; }

        public DateTime CreateTime { get; set; }

        public string Remark { get; set; }


    }
}
