using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Web
{
   public class SND_LoginUserInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public string Phone { get; set; }
        public string IdCard { get; set; }
        public string RealName { get; set; }
        public string BankName { get; set; }
        public string BankNumber { get; set; }
      //  public string TradePWD { get; set; }
        public DateTime CreateTime { get; set; }

        public string LeftId { get; set; }

        public string RightId { get; set; }

        public string TeamParentId { get; set; }
        //public int SonSum { get; set; }
        public int ParentId { get; set; }
        //public int IsTestUser { get; set; }
        public int Level{ get; set; }
        public int UserStatus { get; set; }

        public int IsActivation { get; set; }
      //  public string ParentLoginName { get; set; }

    }
}
public enum SND_UserType
{
    General = 1,
    Admin = 2,
    SupperAdmin = 3,
    Mother = 4
}