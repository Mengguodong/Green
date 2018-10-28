using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SndApi.Models
{
    public class ReturnModel
    {
        public bool IsTrue { get; set; }
        public string Msg { get; set; }
        public UserScore ScoreData { get; set; }
    }
    public class UserScore
    {
        public string UserName { get; set; }
        public decimal Ep { get; set; }

        public int Level { get; set; }
    }
}