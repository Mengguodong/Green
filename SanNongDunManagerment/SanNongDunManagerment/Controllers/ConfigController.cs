using Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using DataModel;
using Common.Web;

namespace SanNongDunManagerment.Controllers
{
    public class ConfigController : BaseController
    {

        GlobalConfigBll _configBll = new GlobalConfigBll();
        UserBll _userBll = new UserBll();

        //
        // GET: /Config/

        public ActionResult Index()
        {

            string value = _configBll.GetValueByConfigName("EveryDate");

            ViewBag.ZfcPrice = value;

            return View();
        }

        public JsonResult UpdateZfcPrice(string price)
        {
            bool result = false;

            decimal ZfcPrice = 0;

            result = decimal.TryParse(price, out ZfcPrice);

            if (!result)
            {
                return Json(new { result = result, msg = "请输入正确格式的单价！" });
            }

            GlobalConfig config = new GlobalConfig();

            config.ConfigKey = "EveryDate";
            config.ConfigValue = ZfcPrice.ToString();

            result = _configBll.UpdateConfig(config);
            if (!result)
            {
                return Json(new { result = result, msg = "价格设置失败！请稍后再试" });
            }

            return Json(result);
        }


        public JsonResult UpdateAdminPwd(string pwd)
        {
            bool result = false;

         ;

         if (string.IsNullOrEmpty(pwd))
            {
                return Json(new { result = result, msg = "密码不能为空！" });
            }
        UserInfo userInfo = _userBll.GetUserInfoByUserName("admin");
        if (userInfo == null) 
        {
            return Json(new { result = result, msg = "修改有误！" });
        }
        userInfo.Pwd = Auxiliary.Md5Encrypt(pwd);
        result = _userBll.UpdateUserInfo(userInfo);
           
          

            return Json(result);
        }

    }
}
