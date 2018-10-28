using Bll;
using Common;
using Common.Web;
using DataModel;
using DataModel.RequestModel;
using DataModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanNongDunWeb.Controllers
{
    public class AccountController : CommonBaseController
    {
        UserBll _userBll = new UserBll();
        UserAccountBll _accBll = new UserAccountBll();

        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

     
        public ActionResult ActivationIndex()
        {
            UserInfo userinfo = null;
            if (_ServiceContext != null && _ServiceContext.SND_CurrentUser != null && _ServiceContext.SND_CurrentUser.UserId > 0)
            {
                userinfo = _userBll.GetUserInfoById(_ServiceContext.SND_CurrentUser.UserId);
            }
            //ViewBag.ShowFooter = false;
            //ViewBag.ShowHeader = false;
            return View(userinfo);
        }

        /// <summary>
        /// 使用激活卡
        /// </summary>
        /// <param name="activationUserName"></param>
        /// <returns></returns>
        public JsonResult ActivationCard(string activationUserName)
        {
            bool result = false;
            string msg = "激活失败！";
            if (string.IsNullOrEmpty(activationUserName))
            {
                return Json(new { result=result,msg="激活账号不能为空！"});
            }

            if (!Auxiliary.IsMobilePhoneNum(activationUserName)) 
            {
                return Json(new { result = result, msg = "激活账号格式有误！" });
            }
           UserInfo reUserInfo =  _userBll.GetUserInfoByUserName(activationUserName);
           if (reUserInfo == null)
           {
               return Json(new { result = result, msg = "激活账号无效！" });
           }
           if (reUserInfo.UserId==_ServiceContext.SND_CurrentUser.UserId) 
           {
               return Json(new { result = result, msg = "激活账号错误！" });
           }

           UserInfo userInfo = _userBll.GetUserInfoById(_ServiceContext.SND_CurrentUser.UserId);
           if (userInfo == null)
           {
               return Json(new { result = result, msg = "账户有误！" });
           }
         
          AccountInfo accInfo = _accBll.GetAccByUserId(_ServiceContext.SND_CurrentUser.UserId);
          if (accInfo.ActivationCount<=0)
          {
              return Json(new { result = result, msg = "请生成激活卡！" });
          }
          AccountInfo reAccInfo = _accBll.GetAccByUserId(reUserInfo.UserId);
        
          if (accInfo == null || reAccInfo == null) 
          {
              return Json(new { result = result, msg = "激活个人账户错误！" });
          }
          if (reUserInfo.IsActivation == 1) 
          {
              return Json(new { result = result, msg = "此账户已激活！" });
          }
        

            //使用激活卡流程
            result = _accBll.ActivationCard(activationUserName, userInfo, reUserInfo, accInfo, reAccInfo);
            if (result)
            {
                msg = "激活成功！";
                _accBll.PlusAchievement(reUserInfo);
                
            }
            else 
            {
                msg = "激活失败！";
            }

            return Json(new { result = result, msg = msg });
        }

        /// <summary>
        /// 激活卡使用记录
        /// </summary>
        /// <returns></returns>
        public ActionResult ActivationCardExchange(int activationType,int pageIndex=1,int pageSize=15) 
        {
           Page<ActicationCardExchangeModel> page = new Page<ActicationCardExchangeModel>();
            if (_ServiceContext != null && _ServiceContext.SND_CurrentUser != null && _ServiceContext.SND_CurrentUser.UserId > 0)
            {
                page = _accBll.ActivationCardExchange(_ServiceContext.SND_CurrentUser.UserId, activationType, pageIndex, pageSize);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ActivationCardExchange", page);
            }
            ViewBag.activationType = activationType;
            return View(page);
        }

        /// <summary>
        /// 激活卡赠送
        /// </summary>
        /// <param name="reUserName"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public JsonResult GiveActivationCard(string reUserName,int count) 
        {
            bool result = false;
            string msg = "赠送失败！";
            if (string.IsNullOrEmpty(reUserName))
            {
                return Json(new { result = result, msg = "赠送帐号不能为空！" });
            }
            if (!Auxiliary.IsMobilePhoneNum(reUserName))
            {
                return Json(new { result = result, msg = "赠送账号格式有误！" });
            }
            if (!(count > 0)) 
            {
                return Json(new { result = result, msg = "赠送数量必须大于0！" });
            }

            AccountInfo accInfo = _accBll.GetAccByUserId(_ServiceContext.SND_CurrentUser.UserId);
            if (accInfo.ActivationCount - count<0) 
            {
                return Json(new { result = result, msg = "激活卡数量不足！" });
            }
            UserInfo reUserInfo = _userBll.GetUserInfoByUserName(reUserName);
            if (reUserInfo == null)
            {
                return Json(new { result = result, msg = "赠送账号无效！" });
            }
            if (reUserInfo.UserId == _ServiceContext.SND_CurrentUser.UserId) 
            {
                return Json(new { result = result, msg = "赠送账号不能是自己！" });
            }
            AccountInfo reAccInfo = _accBll.GetAccByUserId(reUserInfo.UserId);

            if (accInfo == null || reAccInfo == null)
            {
                return Json(new { result = result, msg = "赠送个人账户错误！" });
            }
            UserInfo userInfo = _userBll.GetUserInfoById(_ServiceContext.SND_CurrentUser.UserId);
            if (userInfo == null)
            {
                return Json(new { result = result, msg = "账户有误！" });
            }
            //减去赠送激活卡数量
            accInfo.ActivationCount -=  count;

            //增加被赠送激活卡数量

            reAccInfo.ActivationCount += count;

           result=  _accBll.GiveActivationCard(userInfo,reUserInfo, accInfo, reAccInfo);
           if (result)
           {
               msg = "赠送成功！";
           }
           else 
           {
               msg = "赠送失败！";
           }


            return Json(new { result = result, msg = msg });
        }

     

    }
}
