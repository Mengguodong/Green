using Bll;
using Common.Web;
using DataModel;
using DataModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanNongDunManagerment.Controllers
{
    public class AccountInfoController : BaseController
    {
        //
        // GET: /AccountInfo/
        UserBll _userBll = new UserBll();
        UserAccountBll _accBll = new UserAccountBll();
   
        public ActionResult Index(int userId)
        {
            UserIndexModel userIndex = _userBll.GetAdminUserIndexModel(userId);
           ViewData["viewModel"] = userIndex;
           ViewBag.HeaderTitle = "修改用户信息";
           return View();
        }

        /// <summary>
        /// 修改总资产
        /// </summary>
        /// <param name="TotalAssets"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResult UpdateTotalAssets(string TotalAssets,int userId)
        {
            //验证
            bool result = false;
            decimal totalAssets = 0;

            result = decimal.TryParse(TotalAssets,out totalAssets);

            if (!result)
            {
                return Json(new { result=false,msg="请输入有效的数字！"});
            }
            AccountInfo userAccount = _accBll.GetAccByUserId(userId);

            userAccount.GreenTotal = totalAssets;

            result = _accBll.UpdateAccInfo(userAccount);
            if (!result)
            {
                 return Json(new { result=false,msg="修改总资产失败！"});
            }
            return Json(new { result = true});

       }

        /// <summary>
        /// 修改绿氧
        /// </summary>
        /// <param name="ZFC"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResult UpdateZFC(string ZFC, int userId)
        {
            //验证
            bool result = false;
            decimal greenCount = 0;

            result = decimal.TryParse(ZFC, out greenCount);

            if (!result)
            {
                return Json(new { result = false, msg = "请输入有效的数字！" });
            }
            AccountInfo userAccount = _accBll.GetAccByUserId(userId);

            userAccount.GreenCount = greenCount;

            result = _accBll.UpdateAccInfo(userAccount);
            if (!result)
            {
                return Json(new { result = false, msg = "修改ZFC失败！" });
            }
            return Json(new { result = true });


        }

        /// <summary>
        /// 用户充值
        /// </summary>
        /// <returns></returns>
        public JsonResult Recharge(int userId, int logType, int adminLogType, int count)
        {

            string result = "no";
            if (count < 0)
            {
                return Json(result);
            }
            AccountInfo acc = new AccountInfo();

            acc = _accBll.GetAccByUserId(userId);
            acc.Score = count;
            //switch (adminLogType) 
            //{
            //    case 1:
            //        acc.Ep += count;
            //        break;
            //    case 2:
            //        acc.Zfc+= count;
            //        break;

            //}
            bool isTrue = _accBll.UpdateAccInfo(acc);
            //LogInfo log = new LogInfo();
            //log.UserId = userId;
            //log.Number = count;
            //log.LogType = logType;
            //log.AdminLogType = adminLogType;
            //log.CreateTime = DateTime.Now;
            //if (isTrue)
            //{
            //    isTrue = _logBll.Insert(log);
            //}
            if (isTrue)
            {
                result = "ok";
            }
            return Json(result);
        }

    }
}
