using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.LiuViewModel
{
    public class ScoreViewModel
    {
        public int ScoreLogId { get; set; }

        public int UserId { get; set; }

        public decimal LogCount { get; set; }

        public DateTime CreateTime { get; set; }

        public string Remark { get; set; }
       
        public string UserName { get; set; }
    }
}
