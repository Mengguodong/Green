﻿using DapperExAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class StatusLog
    {
        //        StatusLogId int primary key ,
        //UserId int ,
        //LogType int,                 --1 :直推   2：静态
        //LogCount decimal,
        //CreateTime datetime
        [Id(true)]
        public int StatusLogId { get; set; }
        public int UserId { get; set; }

        public int LogType { get; set; }
        public decimal LogCount { get; set; }
        public DateTime CreateTime { get; set; }
        public int ReUserId{get;set;}
    }
}
