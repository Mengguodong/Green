using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.ViewModel
{
   public class ActicationCardExchangeModel
    {
       public int num { get; set; }
       public int UserId { get; set; }
       public int ReUserId { get; set; }
       public string ReUserName { get; set; }
       public DateTime CreateTime { get; set; }
       public string Remark { get; set; }
    }
}
