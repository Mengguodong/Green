using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Web
{
   public class SND_LoginUserInfo
    {


       /// <summary>
       /// 用户ID
       /// </summary>
        public int UserId { get; set; }
       /// <summary>
       /// 用户名称
       /// </summary>
        public string UserName { get; set; }
       /// <summary>
       /// 密码
       /// </summary>
        public string Pwd { get; set; }
       /// <summary>
       /// 支付密码
       /// </summary>
        public string PayPwd { get; set; }
       /// <summary>
       /// 电话
       /// </summary>
        public string Phone { get; set; }
       /// <summary>
       /// 身份证号
       /// </summary>
        public string IdCard { get; set; }
       /// <summary>
       /// 真实姓名
       /// </summary>
        public string RealName { get; set; }
       /// <summary>
       /// 银行名称
       /// </summary>
        public string BankName { get; set; }
       /// <summary>
       /// 银行卡号
       /// </summary>
        public string BankNumber { get; set; }
       /// <summary>
       /// 创建时间
       /// </summary>
        public DateTime CreateTime { get; set; }
       /// <summary>
       /// 左区ID
       /// </summary>
        public string LeftId { get; set; }
       /// <summary>
       /// 右区ID
       /// </summary>
        public string RightId { get; set; }
       /// <summary>
       /// 团队父ID
       /// </summary>
        public string TeamParentId { get; set; }
       /// <summary>
       /// 推荐人ID
       /// </summary>
        public int ParentId { get; set; }
     /// <summary>
     /// 等级  1,2，3，4,5
     /// </summary>
        public int Level{ get; set; }
       /// <summary>
       /// 用户状态  0禁用 1 启用
       /// </summary>
        public int UserStatus { get; set; }
       /// <summary>
       /// 是否激活 0未激活 1已激活
       /// </summary>
        public int IsActivation { get; set; }
       /// <summary>
       /// 用户类型 1前端用户  2管理端用户  
       /// </summary>
        public SND_UserType UserType { get; set; }
        /// <summary>
        /// 是否加入团队 1已加入团队 2未加入团队
        /// </summary>
        public int TeamType { get; set; }
   

        /// <summary>
        /// 今日是否登录
        /// </summary>
        public int TodayIsLogin { get; set; }
        /// <summary>
        /// 昨日是否登录
        /// </summary>
        public int YesTodayIsLogin { get; set; }
    }
}
public enum SND_UserType
{
    General = 1,
    Admin = 2,
    SupperAdmin = 3,
    Mother = 4
}