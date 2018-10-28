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
    public class HomeController : BaseController
    {
        UserAccountBll _accBll = new UserAccountBll();
        //GlobalConfigBll _congigBll = new GlobalConfigBll();
        UserBll _userBll = new UserBll();
        //
        // GET: /Home/

        public ActionResult Index()
        {
            string levelName = "V0";
            if (_ServiceContext != null && _ServiceContext.SND_CurrentUser != null && _ServiceContext.SND_CurrentUser.UserId > 0)
            {
                UserInfo userInfo = _userBll.GetUserInfoById(_ServiceContext.SND_CurrentUser.UserId);
                switch (userInfo.Level)
                {
                    case 0:
                        levelName = "V0";
                        break;
                    case 1:
                        levelName = "V1";
                        break;
                    case 2:
                        levelName = "V2";
                        break;
                    case 3:
                        levelName = "V3";
                        break;
                    case 4:
                        levelName = "V4";
                        break;
                    case 5:
                        levelName = "V5";
                        break;
                }
            }

            ViewBag.ShowFooter = false;
            ViewBag.ShowHeader = false;
            ViewBag.LevelName = levelName;
            //查询用户的总产值,需要确认
            ////ViewBag.Total = _congigBll.GetValueByConfigName("ZfcPrice");
            ViewBag.Total = 1;
            return View();

        }

        public JsonResult GetCustomerEnity() 
        {
            AccountInfo acc = new AccountInfo();
            if (_ServiceContext != null && _ServiceContext.SND_CurrentUser != null && _ServiceContext.SND_CurrentUser.UserId > 0)
            {
               acc= _accBll.GetAccByUserId(_ServiceContext.SND_CurrentUser.UserId);
            }
              

            return Json(acc);
        }

      


        /// <summary>
        /// 获取用户账户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //public ActionResult GetCustomerEnity()
        //{
        //    HeaderModel HeaderInfo = null;
        //    LoginUserInfo userInfo = _ServiceContext.CurrentUser;

        //    if (userInfo != null && userInfo.UserId > 0)
        //    {
        //        UserInfo user = _userBll.GetUserInfoById(userInfo.UserId);
        //        userInfo.LevelId = user.LevelId;

        //        HeaderInfo = customerBll.GetCustomerAccountByUserId(userInfo.UserId).AsHeaderModel();
        //        HeaderInfo.WCLevelType = userInfo.LevelType;
        //    }

        //    return Json(HeaderInfo);
        //}

        /// <summary>
        /// 申请激活页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Activate()
        {
            ViewBag.ShowHeader = false;
            return View();
        }
        /// <summary>
        /// 错误页
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut()
        {
            var cookieName = System.Configuration.ConfigurationManager.AppSettings["cookie_name"].ToString();
            CookieHelper.DelCookie(cookieName);
            return Redirect(PubConstant.WineGameShowUrl("login", "index"));
        }

    }
}
