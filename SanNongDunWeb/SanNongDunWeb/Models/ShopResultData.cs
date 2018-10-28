using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanNongDunWeb.Models
{
    public class T_IM_XYYCApp
    {
        public HeadClass HEAD { get; set; }
        public DataClass DATA { get; set; }
    }
    public class DataClass
    {
        public RowClass ROW { get; set; }
    }
    public class RowClass
    {
        public int forum_check { get; set; }
        public string forum_addTime { get; set; }
    }
    public class HeadClass
    {
        public string MSGCODE { get; set; }
        public string MSGID { get; set; }
        public string MSGNAME { get; set; }
        public string SOURCESYS { get; set; }
        public string TARGETSYS { get; set; }
        public string CREATETIME { get; set; }
    }

}