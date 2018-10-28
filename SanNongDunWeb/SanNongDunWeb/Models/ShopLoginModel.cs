using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanNongDunWeb.Models
{
    public class ShopLoginModel
    {
           //"referee_username={0}" +
           //     "&parent_username={1}" +
           //     "&user_username={2}" +
           //     "&user_name={3}" +
           //     "&user_password={4}" +
           //     "&user_password2={5}" +
           //     "&user_fx={6}");
           public string referee_username { get; set; }
        public string parent_username { get; set; }
        public string user_name { get; set; }
        public string user_username { get; set; }
        public string user_password { get; set; }
        public string user_password2 { get; set; }
        public string user_fx { get; set; }

    }
}