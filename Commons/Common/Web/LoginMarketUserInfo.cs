using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Web
{
   public class LoginMarketUserInfo
    {
       /// <summary>
       /// 市场用户ID
       /// </summary>
        public int ID { get; set; }
       /// <summary>
       /// 用户名
       /// </summary>
        public string UserName { get; set; }
       /// <summary>
       /// 密码
       /// </summary>
        public string Pwd { get; set; }

       /// <summary>
       /// 用户ID
       /// </summary>
        public int UserId { get; set; }
       /// <summary>
       /// 是否删除
       /// </summary>
        public int IsDelete { get; set; }


    }
}
