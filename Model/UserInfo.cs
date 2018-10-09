using DapperExAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;

namespace DataModel
{

    public class UserInfo
    {
        //  create table UserInfo(
        //    UserId  INT NOT NULL PRIMARY KEY ,
        //    LoginName  VARCHAR(255) ,
        //    LoginPWD  VARCHAR(255) ,
        //    Phone   varchar(255),
        //    IdentityCard     VARCHAR(255) 
        //   ,RealName varchar(255)
        //   ,BankName varchar(255)
        //   ,BankCard varchar(255)
        //   ,TradePWD varchar(255)
        //   ,CreateTime date
        //   ,SonSum  int 
        //   ,ParentId  int 
        //   ,IsTestUser int
        //   ,LevelId int
        //   ,UserStatus int
        //   ,ParentLoginName VARCHAR(255) 
        //);


        public int UserId { get; set; }
        public string LoginName { get; set; }
        public string LoginPWD { get; set; }
        public string Phone { get; set; }
        public string IdentityCard { get; set; }
        public string RealName { get; set; }
        public string BankName { get; set; }
        public string BankCard { get; set; }
        public string TradePWD { get; set; }
        public DateTime CreateTime { get; set; }
        public int SonSum { get; set; }
        public int ParentId { get; set; }
        public int IsTestUser { get; set; }
        public int LevelId { get; set; }
        public int UserStatus { get; set; }
        public string ParentLoginName { get; set; }




    }


    public enum UserType
    {
        General = 1,
        Admin = 2,
        SupperAdmin = 3,
        Mother = 4
    }


}
