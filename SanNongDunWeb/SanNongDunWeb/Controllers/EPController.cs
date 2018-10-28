using Bll;
using Common.Web;
using DataModel;
using DataModel.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanNongDunWeb.Controllers
{
    public class EPController : CommonBaseController
    {
        UserAccountBll _accountBll = new UserAccountBll();
        LogBll _logBll = new LogBll();
        UserBll _userBll = new UserBll();
        //
        // GET: /AssetManagement/
        //积分提现，EP复投
        /// <summary>
        /// 展示每日动静态释放记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Index(int pageIndex = 1, int type = 1)
        {
            Page<LogInfo> page = new Page<LogInfo>();

            int pageSize = 15;

            int userId = _ServiceContext.SND_CurrentUser.UserId;

            page = _logBll.GetUserLogs(userId, pageIndex, pageSize, type);//释放记录 1静态 2动态

            ViewBag.Type = type;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Index", page);
            }
         

            return View(page);

        }
      
        /// <summary>
        /// EP复投
        /// </summary>
        /// <param name="Ep"></param>
        /// <returns></returns>
        public JsonResult EPRepeat(string Ep)
        {
            bool result = false;

            decimal EpAmount = 0;

            int userId = _ServiceContext.SND_CurrentUser.UserId;

            result = decimal.TryParse(Ep, out EpAmount);

            if (!result)
            {
                return Json(new { result = result, msg = "请输入正确的EP数量" });
            }

            //验证
            if (EpAmount == 0)
            {
                return Json(new { result = false, msg = "请输入正确的EP数量" });

            }

            if (EpAmount%200!=0)
            {
                 return Json(new { result = false, msg = "EP数量必须为200的倍数！" });
            }

            //验证账户EP余额
            AccountInfo account = _accountBll.GetAccByUserId(userId);

            if (EpAmount > account.Ep)
            {
                return Json(new { result = false, msg = "EP数量不足！" });
            }

            //复投  放大资产

            account.Ep -= EpAmount;
            switch (_ServiceContext.SND_CurrentUser.Level) //不同级别 使用不同杠杆
            {
                case 1:
                    account.TotalAssets += EpAmount * 7;
                    break;
                case 2:
                    account.TotalAssets += EpAmount * 6;
                    break;
                case 3:
                    account.TotalAssets += EpAmount * 5;
                    break;
                case 4:
                    account.TotalAssets += EpAmount * 4;
                    break;
                case 5:
                    account.TotalAssets += EpAmount * 3;
                    break;
                default:
                    break;
            }

            result = _accountBll.UpdateAccInfo(account);
            

            // 添加复投记录
            if (result)
            {

                

              //  result = _userBll.UpdateUserLevel(userId);

                LogInfo model = new LogInfo();
                model.CreateTime = DateTime.Now;
                model.LogType = 5;
                model.Number = EpAmount;
                model.UserId = userId;

                result = _logBll.Insert(model);                

            }
            else
            {
                return Json(new { result = result, msg = "资产扩充失败，请稍后再试！" });
            }



            return Json(new { result = result,msg="资产扩充成功！" });
        }
       
      
        /// <summary>
        /// 复投记录
        /// </summary>
        /// <returns></returns>
        public ActionResult EpRepeatLogs(int pageIndex=1)
        {
            Page<LogInfo> page = new Page<LogInfo>();

            int pageSize = 15;
            int userId = _ServiceContext.SND_CurrentUser.UserId;

            page = _logBll.GetEpRepeatLogs(userId, pageIndex, pageSize);//复投记录 

            if (Request.IsAjaxRequest())
            {
                return PartialView("_EpRepeatLogs", page);
            }

            return View(page);
        }
        /// <summary>
        /// EP互转记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult EpCrossRotationLogs(int pageIndex=1)
        {
            Page<CrossRotation> page = new Page<CrossRotation>();
            int pageSize = 15;
            int userId = _ServiceContext.SND_CurrentUser.UserId;

            page = _logBll.getCrossRotationLogs(userId,pageIndex,pageSize,1);//type=1 代表EP

            if (Request.IsAjaxRequest())
            {
                return PartialView("_EpCrossRotationLogs", page);
            }

            return View(page);
        }
        /// <summary>
        /// EP转账
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <param name="Ep"></param>
        /// <returns></returns>
        public JsonResult EPSend(string mobileNumber,string Ep)
        {
            bool result = false;
            decimal ep = 0;
            //验证数据
            var userInfo = _userBll.GetUserInfoByUserName(mobileNumber);//接受人的信息
            if (userInfo==null)
            {
                return Json(new { result=false,msg="输入的用户不存在！"});
            }
            if (userInfo.UserId==_ServiceContext.SND_CurrentUser.UserId)
            {
                return Json(new { result=false,msg="不能送EP给自己！"});
            }
            result = decimal.TryParse(Ep,out ep);

            if (!result)
            {
                return Json(new { result=false,msg="请输入正确的数量！"});
            }
            if (ep<1)
            {
                return Json(new { result=false,msg="数量不能少于1！"});
            }
            AccountInfo sendAccount = new AccountInfo();
            AccountInfo reAccount = new AccountInfo();

            sendAccount = _accountBll.GetAccByUserId(_ServiceContext.SND_CurrentUser.UserId);
            reAccount = _accountBll.GetAccByUserId(userInfo.UserId);

            if (sendAccount.Ep<ep)
            {
                return Json(new { result=false,msg="账户EP不足！"});
            }

            //进行转账
            sendAccount.Ep -= ep;
            reAccount.Ep += ep;

            result = _accountBll.UpdateAccInfo(sendAccount);
            if (result)
            {
                result = _accountBll.UpdateAccInfo(reAccount);
            }
            else
            {
                return Json(new { result = false, msg = "赠送失败，请稍后再试！" });
            }

            //添加EP转账记录

            CrossRotation model = new CrossRotation();

            model.CRAccountName = sendAccount.AccountName;
            model.CreateTime = DateTime.Now;
            model.CRType = 1;
            model.CRUserId = _ServiceContext.SND_CurrentUser.UserId;
            model.Qty = ep;
            model.REAccountName = reAccount.AccountName;
            model.Remark = "";
            model.REUserId = userInfo.UserId;

            result = _logBll.InsertCrossRotationLog(model);



            return Json(new { result=true,msg="赠送成功！" });
              
        }
        /// <summary>
        /// ZFC转账
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <param name="zfc"></param>
        /// <returns></returns>
        public JsonResult ZFCSend(string mobileNumber, string ZFC)
        {
            bool result = false;
            decimal zfc = 0;
            //验证数据
            var userInfo = _userBll.GetUserInfoByUserName(mobileNumber);//接受人的信息
            if (userInfo == null)
            {
                return Json(new { result = false, msg = "输入的用户不存在！" });
            }
            if (userInfo.UserId == _ServiceContext.SND_CurrentUser.UserId)
            {
                return Json(new { result = false, msg = "不能送zfc给自己！" });
            }
            result = decimal.TryParse(ZFC, out zfc);

            if (!result)
            {
                return Json(new { result = false, msg = "请输入正确的数量！" });
            }
            if (zfc < 1)
            {
                return Json(new { result = false, msg = "数量不能少于1！" });
            }
            AccountInfo sendAccount = new AccountInfo();
            AccountInfo reAccount = new AccountInfo();

            sendAccount = _accountBll.GetAccByUserId(_ServiceContext.SND_CurrentUser.UserId);
            reAccount = _accountBll.GetAccByUserId(userInfo.UserId);

            if (sendAccount.Zfc < zfc)
            {
                return Json(new { result = false, msg = "账户zfc不足！" });
            }

            //进行转账
            sendAccount.Zfc -= zfc;
            reAccount.Zfc += zfc;

            result = _accountBll.UpdateAccInfo(sendAccount);
            if (result)
            {
                result = _accountBll.UpdateAccInfo(reAccount);
            }
            else
            {
                return Json(new { result = false, msg = "赠送失败，请稍后再试！" });
            }

            //添加zfc转账记录

            CrossRotation model = new CrossRotation();

            model.CRAccountName = sendAccount.AccountName;
            model.CreateTime = DateTime.Now;
            model.CRType = 2;
            model.CRUserId = _ServiceContext.SND_CurrentUser.UserId;
            model.Qty = zfc;
            model.REAccountName = reAccount.AccountName;
            model.Remark = "";
            model.REUserId = userInfo.UserId;

            result = _logBll.InsertCrossRotationLog(model);



            return Json(new { result = true, msg = "赠送成功！" });

        }
      
        /// <summary>
        /// ZFC互转记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult ZfcCrossRotationLogs(int pageIndex = 1)
        {
            Page<CrossRotation> page = new Page<CrossRotation>();
            int pageSize = 15;
            int userId = _ServiceContext.SND_CurrentUser.UserId;

            page = _logBll.getCrossRotationLogs(userId, pageIndex, pageSize, 2);//type=2 代表zfc

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ZfcCrossRotationLogs", page);
            }

            return View(page);
        }

    }
}
