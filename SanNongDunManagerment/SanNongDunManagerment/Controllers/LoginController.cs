using Bll;
using Common;
using Common.Web;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanNongDunManagerment.Controllers
{
    public class LoginController : Controller
    {
        UserBll userBll = new UserBll();
        //
        // GET: /LogInfo/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录请求
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult UserLogin(SND_LoginUserInfo model)
        {
            bool result = false;
            string msg = string.Empty;
            //数据验证
            if (string.IsNullOrEmpty(model.UserName))
            {
                msg = "用户名不能为空";
                return Json(new { result = result, Msg = msg });
            }
            if (string.IsNullOrEmpty(model.Pwd))
            {
                msg = "密码不能为空";
                return Json(new { result = result, Msg = msg });
            }


            UserInfo userInfo = userBll.AdminLogin(model.UserName, model.Pwd, out msg);
            if (userInfo != null)//登录成功 将登录信息存入cookie
            {
                string userStr = JsonConvertTool.SerializeObject(userInfo);

                CookieHelper.SetCookie(PubConstant.COOKIE_NAME, userStr, DateTime.Now.AddMinutes(ConvertHelper.ToInt32(Auxiliary.ConfigKey("UserLogTimeout"))));

                result = true;
            }


            return Json(new { result = result, Msg = msg });


        }

    }
}
