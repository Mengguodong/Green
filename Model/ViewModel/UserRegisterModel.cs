using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.ViewModel
{
   /// <summary>
   /// 用户注册实体
   /// </summary>
  public  class UserRegisterModel
    {
      
      /// <summary>
      /// 用户名
      /// </summary>
        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
      /// <summary>
      /// 密码
      /// </summary>
        private string pwd;
        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }
        /// <summary>
        /// 验证码
        /// </summary>
        public string ValidateCode { get; set; }
      /// <summary>
      /// 推荐人手机号
      /// </summary>
        public string ParentLoginName { get; set; }
    }
}
