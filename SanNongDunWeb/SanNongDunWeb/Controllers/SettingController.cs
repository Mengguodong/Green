using Bll;
using Common;
using Common.Web;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanNongDunWeb.Controllers
{
    
    public class SettingController : BaseController /*CommonBaseController*/
    {
        UserBll _userBll = new UserBll();
        //
        // GET: /Setting/

        public ActionResult Index()
        {
            UserInfo userinfo = null;
            if (_ServiceContext != null && _ServiceContext.SND_CurrentUser != null && _ServiceContext.SND_CurrentUser.UserId > 0)
            {
                userinfo = _userBll.GetUserInfoById(_ServiceContext.SND_CurrentUser.UserId);
            }
       

            ViewBag.ShowFooter = false;
            ViewBag.ShowHeader = false;
            return View(userinfo);
        }


        public ActionResult SetUserInfo() 
        {
            UserInfo userinfo = null;
            if (_ServiceContext != null && _ServiceContext.SND_CurrentUser != null && _ServiceContext.SND_CurrentUser.UserId > 0)
            {
                userinfo = _userBll.GetUserInfoById(_ServiceContext.SND_CurrentUser.UserId);
            }
       
       
            ViewBag.ShowFooter = false;
            ViewBag.ShowHeader = false;
            return View(userinfo);
        }

        public JsonResult BindingUserinfo(int userId, string realName, string phone, string idCard, string bankName, string bankNumber,string pwd,string payPwd)
        {
            bool result = false;
            string msg = "修改失败！";
            UserInfo userInfo = new UserInfo();
            userInfo = _userBll.GetUserInfoById(userId);
            if (userInfo != null)
            {
                userInfo.Phone = phone;
                userInfo.RealName = realName;
                userInfo.BankName = bankName;
                userInfo.BankNumber = bankNumber;
                userInfo.Pwd = Auxiliary.Md5Encrypt(pwd);
                userInfo.PayPwd = Auxiliary.Md5Encrypt(payPwd); 
                result = _userBll.UpdateUserInfo(userInfo);
            }
            if (result)
            {
                msg = "修改成功！";
            }
            return Json(new { result = result, msg = msg });
        }
     


    }
}
