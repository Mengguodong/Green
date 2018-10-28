using Bll;
using Common.Web;
using DataModel;
using DataModel.RequestModel;
using DataModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using Common;

namespace SanNongDunManagerment.Controllers
{
    public class UserInfoController : BaseController
    {
        //
        // GET: /UserInfo/
        UserBll _userBll = new UserBll();
        public ActionResult Index(string userName = "", int pageIndex = 1)
        {
            Page<UserIndexModel> page = new Page<UserIndexModel>();
            PagedList<UserIndexModel> pageList = null;
            int pageSize = 15;

            page = _userBll.GetAdminUserInfoes(userName, pageSize, pageIndex);
            if (page.Data != null)
            {
                pageList = new PagedList<UserIndexModel>(page.Data, pageIndex, pageSize, page.TotalCount);


                ViewBag.TotalCount = page.TotalCount;
            }


            if (Request.IsAjaxRequest())
            {
                return PartialView("_UserInfoes", pageList);
            }
            ViewBag.HeaderTitle = "会员列表";
            return View(pageList);

        }

        public JsonResult UpdateUserInfo(int userId, string realName, string phone, string idCard, string bankName, string bankNumber,string pwd)
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
                if (string.IsNullOrEmpty(pwd))
                {
                    result = false;
                    msg = "密码不能为空！";
                }
                if (pwd!=userInfo.Pwd)
                {
                    userInfo.Pwd = Auxiliary.Md5Encrypt(pwd);
                }
             
               
                result = _userBll.UpdateUserInfo(userInfo);
            }
            if (result)
            {
                msg = "修改成功";
            }
            return Json(new { result = result, msg = msg });
        }

        /// <summary>
        /// 修改账户冻结状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
       
        public JsonResult UpdateStatus(int userId,int status) 
        {
           
            bool result = false;
            string msg = "修改失败！";
              UserInfo userInfo = new UserInfo();
            userInfo = _userBll.GetUserInfoById(userId);
            if (userInfo != null)
            {
                userInfo.UserStatus = status;
                result = _userBll.UpdateUserInfo(userInfo);
            }
            if (result)
            {
                msg = "修改成功";
            }
            return Json(new { result = result, msg = msg });
            //return Json(new { result = result, msg = msg }, JsonRequestBehavior.AllowGet);
        }

    }
}
