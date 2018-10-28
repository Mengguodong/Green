using Bll;
using Common.Web;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanNongDunWeb.Controllers
{
    public class AchievementController : CommonBaseController
    {
        UserAccountBll _accountBll = new UserAccountBll();
        //
        // GET: /Achievement/
        /// <summary>
        /// 我的业绩
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            int userId = _ServiceContext.SND_CurrentUser.UserId;

            AccountInfo model = _accountBll.GetAccByUserId(userId);



            return View(model);
        }

    }
}
