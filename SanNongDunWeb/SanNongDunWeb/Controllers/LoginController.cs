using Common.Web;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Bll;
using Common;
using DataModel.ViewModel;
using System.Text;
using SanNongDunWeb.Models;

namespace SanNongDunWeb.Controllers
{
    public class LoginController : Controller
    {

        UserBll userBll = new UserBll();
        UserAccountBll accountBll = new UserAccountBll();



        /// <summary>
        /// 登录页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 登录请求
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult LoginSumbit(SND_LoginUserInfo model)
        {
            bool result = false;
            string msg = string.Empty;

            bool resultTemp = false;

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


            UserInfo userInfo = userBll.UserLogin(model.UserName, model.Pwd, out msg);



            if (userInfo != null)//登录成功 将登录信息存入cookie
            {

                string userStr = JsonConvertTool.SerializeObject(userInfo);

                CookieHelper.SetCookie(PubConstant.COOKIE_NAME, userStr, DateTime.Now.AddMinutes(ConvertHelper.ToInt32(Auxiliary.ConfigKey("UserLogTimeout"))));

                result = true;

                ////是否今日第一次登陆
                //if (userInfo.TodayIsLogin == 0)
                //{

                //    //静态分配并生成记录

                //    resultTemp = accountBll.StaticDistribution(userInfo.UserId);
                //    //动态分配并生成记录

                //    resultTemp = accountBll.DynamicDistribution(userInfo.UserId);

                //    //修改今日登录状态
                //    userInfo.TodayIsLogin = 1;

                //    resultTemp = userBll.UpdateUserInfo(userInfo);

                //}



            }


            return Json(new { result = result, Msg = msg });


        }

        #region 注册



        /// <summary>
        /// 注册页面(获取手机验证码)
        /// Author：mgd
        /// Date：2016年12月11日12:52:09
        /// </summary>
        /// <returns></returns>
        public ActionResult Register1(string mobilePhone = "", string token = "", string teamParentId = "", string parentLoginName = "")
        {
            ViewBag.teamParentId = teamParentId;
            ViewBag.parentLoginName = parentLoginName;
            ViewBag.mobilePhone = mobilePhone;
            ViewBag.token = token;
            return View();
        }

        /// <summary>
        /// 注册页面第二步
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public ActionResult Register2(string phone, string validcode, string teamParentId = "", string parentLoginName = "")
        {
            ViewBag.teamParentId = teamParentId;
            ViewBag.parentLoginName = parentLoginName;
            ViewBag.Phone = phone;
            ViewBag.ValidateCode = validcode;


            return View();
        }


        /// <summary>
        /// 获取手机号，并发送验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public JsonResult GetPhoneCode(string phone)
        {
            bool result = false;

            if (!phone.IsMobilePhoneNum())
            {
                return Json(new { result = result, msg = "手机号码格式不正确" }, JsonRequestBehavior.AllowGet);
            }
            var user = userBll.GetUserInfoByUserName(phone);
            if (user != null)
            {

                return Json(new { result = result, msg = "手机号已存在，不可重复注册" }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                int sendStatus = GeneratorCode(phone);

                if (sendStatus == 0)
                {
                    return Json(new { result = result, msg = "暂时无法获取验证码，请稍后重试！" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);

            }

        }
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        private int GeneratorCode(string phone)
        {
            //TODO:严格验证手机号码5
            if (!phone.IsMobilePhoneNum())
            {
                return 0;
            }
            // 手机验证码只有数字
            var code = Auxiliary.GenerateRandomCode(6);
            try
            {

                Session["Phone"] = phone;
                Session["RandomCode"] = Auxiliary.ConfigKey("RealVerifyCode") == "true" ? code : "123456";

                if (Auxiliary.ConfigKey("RealVerifyCode") == "true")
                {

                    string defaultSMSReceiver = Auxiliary.ConfigKey("DefaultSMSReceiver");
                    if (defaultSMSReceiver == "1")
                    {

                        SMSCodeRequest model = new SMSCodeRequest() { Code = code, Phone = phone };
                        if (userBll.SendSmsNew(model))
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return 0;
            }


        }
        /// <summary>
        /// 验证手机号，验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="validcode"></param>
        /// <returns></returns>
        public JsonResult CheckValidateCode(string UserName, string ValidateCode)
        {
            bool result = false;
            if (string.IsNullOrEmpty(UserName))
            {
                return Json(new { result = result, Msg = "手机号为空" });
            }
            if (string.IsNullOrEmpty(ValidateCode))
            {
                return Json(new { result = result, Msg = "校验码为空" });
            }
            string mobile = "";
            if (Session["Phone"] != null)
            {
                mobile = Session["Phone"].ToString();
            }
            else
            {
                return Json(new { result = result, Msg = "验证码不正确" });
            }


            if (!mobile.Equals(UserName))
            {
                return Json(new { result = result, Msg = "手机号不正确" });
            }
            string code = "";
            if (Session["RandomCode"] != null)
            {
                code = Session["RandomCode"].ToString();
            }
            else
            {
                return Json(new { result = result, Msg = "验证码不正确" });
            }


            if (!code.Equals(ValidateCode))
            {
                return Json(new { result = result, Msg = "验证码不正确" });
            }

            return Json(new { result = true });


        }


        /// <summary>
        /// 提交注册信息
        /// Author：mgd
        /// Date：2016年12月11日12:55:48
        /// </summary>
        /// <param name="model">用户信息实体</param>
        /// <param name="mobileCode">短信验证码</param>
        /// <returns></returns>

        public JsonResult RegisterAjax(UserRegisterModel model)
        {
            string msg = "";
            bool result = false;
            //验证用户名
            if (string.IsNullOrEmpty(model.UserName))
            {
                msg = "用户名不能为空";
                return Json(new { result = result, Msg = msg });
            }
            //验证密码
            if (string.IsNullOrEmpty(model.Pwd))
            {
                msg = "密码不能为空";
                return Json(new { result = result, Msg = msg });
            }

            if (string.IsNullOrEmpty(model.ValidateCode))
            {
                msg = "验证码验证未通过";
                return Json(new { result = result, Msg = msg });
            }


            //验证推荐人
            if (string.IsNullOrEmpty(model.ParentLoginName))
            {
                msg = "推荐人不能为空";
                return Json(new { result = result, Msg = msg });
            }


            #region 通过分享链接进行注册的需做验证
            ////通过分享链接进行注册的需做验证
            //if (!string.IsNullOrEmpty(token))
            //{
            //    string tempToken = Auxiliary.Md5Encrypt(model.ParentLoginName + Auxiliary.Md5Encrypt(Auxiliary.ConfigKey("LinkTokenKey")));

            //    if (token != tempToken)
            //    {
            //        msg = "推荐人无效";
            //        return Json(new { result = result, Msg = msg });
            //    }


            //} 
            #endregion

            #region 没对接商城版本

            //获取session中的验证码
            string validateCode = Session["RandomCode"].ToString();
            if (model.ValidateCode.Equals(validateCode, StringComparison.OrdinalIgnoreCase))//验证通过
            {

                //注册
                result = userBll.Register(model, out msg);
                //清空session
                Session["RandomCode"] = null;
            }
            else { msg = "验证码已过期"; }
            return Json(new { result = result, Msg = msg });
            #endregion

            #region 对接商城
            ////获取session中的验证码
            //string validateCode = Session["RandomCode"].ToString();
            //if (model.ValidateCode.Equals(validateCode, StringComparison.OrdinalIgnoreCase))//验证通过
            //{

            //    //注册
            //    result = userBll.Register(model, out msg);
            //    //清空session
            //    Session["RandomCode"] = null;


            //    #region shop注册——孙健


            //    //http://120.27.204.40/hn_zfl/registeruser_interface?referee_username=123&parent_username=123
            //    //    &user_username = 125 & user_name = 126 & user_password = 127 & user_password2 = 128 & user_fx = 2


            //    ShopLoginModel shop = new ShopLoginModel();

            //    #region 商城get请求
            //    //string url = string.Format("http://120.27.204.40/hn_zfl/registeruser_interface?" +
            //    //      "referee_username={0}" +
            //    //      "&parent_username={1}" +
            //    //      "&user_username={2}" +
            //    //      "&user_name={3}" +
            //    //      "&user_password={4}" +
            //    //      "&user_password2={5}" +
            //    //      "&user_fx={6}", model.ParentLoginName, "无", model.UserName, "无", model.Pwd, model.PayPwd);

            //    //T_IM_XYYCApp shopReslut = HttpClientHelper.GetResponse<T_IM_XYYCApp>(url); 
            //    #endregion
            //    UserInfo userLeft = userBll.GetUserByLeftId(model.TeamParentId);
            //    UserInfo userRight = userBll.GetUserByRightId(model.TeamParentId);
            //    if (userLeft != null)
            //    {
            //        shop.parent_username = userLeft.UserName;
            //        shop.user_fx = "1";
            //    }

            //    if (userRight != null)
            //    {
            //        shop.parent_username = userRight.UserName;
            //        shop.user_fx = "2";
            //    }

            //    shop.referee_username = model.ParentLoginName;
            //    shop.user_username = model.UserName;
            //    shop.user_name = "";
            //    shop.user_password = model.Pwd;
            //    shop.user_password2 = model.PayPwd;

            //    string url = "http://120.27.204.40/hn_zfl/registeruser_interface";
            //    T_IM_XYYCApp postResult = HttpClientHelper.PostResponse<T_IM_XYYCApp>(url, shop);

            //    if (postResult == null)
            //    {
            //        return Json(new { result = false, Msg = "商城注册失败！" });
            //    }

            //    if (postResult.DATA.ROW.forum_check == 0)
            //    {
            //        //注册
            //        result = userBll.Register(model, out msg);
            //        //清空session
            //        Session["RandomCode"] = null;

            //    }
            //    else if (postResult.DATA.ROW.forum_check == 1)
            //    {
            //        return Json(new { result = false, Msg = "商城手机号码重复！" });
            //    }
            //    else if (postResult.DATA.ROW.forum_check == 2)
            //    {
            //        return Json(new { result = false, Msg = "商城该位置有人！" });
            //    }
            //    else if (postResult.DATA.ROW.forum_check == 3)
            //    {
            //        return Json(new { result = false, Msg = "商城上级不存在！" });
            //    }
            //    else if (postResult.DATA.ROW.forum_check == 4)
            //    {
            //        return Json(new { result = false, Msg = "推荐人不存在！" });
            //    }

            //    #endregion

            //}
            //else { msg = "验证码已过期"; }
            //return Json(new { result = result, Msg = msg }); 
            #endregion
        }

        #endregion

    }
}
