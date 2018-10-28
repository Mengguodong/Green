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
    public class RegisterController : BaseController
    {
        UserBll userBll = new UserBll();
        UserAccountBll _accBll = new UserAccountBll();
        //
        // GET: /Register/

        public ActionResult Index(string userName = "")
        {
            UserInfo userInfo = new UserInfo();

            UserInfo userLeft = null;
            UserInfo userRight = null;
            UserInfo userLeftTwo = null;
            UserInfo userLeftRightTwo = null;
            UserInfo userRightLeftTwo2 = null;
            UserInfo userRightTwo2 = null;

            decimal leftSum = 0;
            decimal rightSum = 0;
            string leftStr = "";
            string rightStr = "";


            if (!string.IsNullOrEmpty(userName))
            {
                userInfo = userBll.GetUserInfoByUserName(userName);
                if (userInfo != null)
                {
                    leftStr = userInfo.LeftId;
                    rightStr = userInfo.RightId;
                    AccountInfo acc = _accBll.GetAccByUserId(userInfo.UserId);
                    if (acc != null)
                    {
                        leftSum = acc.LeftAchievement;
                        rightSum = acc.RightAchievement;
                    }
                }


            }
            else
            {
                if (_ServiceContext != null && _ServiceContext.SND_CurrentUser != null && _ServiceContext.SND_CurrentUser.UserId > 0)
                {
                    userInfo = userBll.GetUserInfoById(_ServiceContext.SND_CurrentUser.UserId);



                    leftStr = userInfo.LeftId;
                    rightStr = userInfo.RightId;

                    AccountInfo acc = _accBll.GetAccByUserId(userInfo.UserId);
                    if (acc != null)
                    {
                        leftSum = acc.LeftAchievement;
                        rightSum = acc.RightAchievement;
                    }
                }
            }

            userLeft = userBll.GetUserByLeftId(leftStr);
            userRight = userBll.GetUserByRightId(rightStr);

            if (userLeft != null)
            {
                userLeftTwo = userBll.GetUserByLeftId(userLeft.LeftId);
                userLeftRightTwo = userBll.GetUserByLeftId(userLeft.RightId);
            }
            if (userRight != null)
            {
                userRightLeftTwo2 = userBll.GetUserByLeftId(userRight.LeftId);
                userRightTwo2 = userBll.GetUserByLeftId(userRight.RightId);
            }

            ViewBag.userLeft = userLeft;
            ViewBag.userRight = userRight;
            ViewBag.userLeftTwo = userLeftTwo;
            ViewBag.userLeftRightTwo = userLeftRightTwo;
            ViewBag.userRightLeftTwo2 = userRightLeftTwo2;
            ViewBag.userRightTwo2 = userRightTwo2;
            ViewBag.UserInfo = userInfo;
            ViewBag.leftSum = leftSum;
            ViewBag.rightSum = rightSum;


            return View();
        }




    }
}
