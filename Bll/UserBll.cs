
﻿using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Sms.Model.V20160927;
using Common;
using Dal;
using DataModel;
using DataModel.RequestModel;
using DataModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    public class UserBll
    {
        UserDal _userDal = new UserDal();
        UserAccountDal _accountDal = new UserAccountDal();

        public UserInfo GetUserByLeftId(string leftId)
        {
            return _userDal.GetUserByLeftId(leftId);
        }
              public UserInfo GetUserByRightId(string rightId)
        {
            return _userDal.GetUserByRightId(rightId);
        }

              public List<int> AdminGetUserIdList(string userName)
        {
            return _userDal.AdminGetUserIdList(userName);
        }

        /// <summary>
        /// 获取下级用户
        /// </summary>
        /// <param name="leftId"></param>
        /// <param name="rightId"></param>
        /// <returns></returns>
        //public Dictionary<string,UserInfo> GetUserLowTeamId(string leftId, string rightId) 
        //{
        //    Dictionary<string, UserInfo> dic = new Dictionary<string,UserInfo>();
        //   UserInfo userLeft= _userDal.GetUserByLeftId(leftId);
        //  UserInfo userRight=  _userDal.GetUserByRightId(rightId);
        //  dic.Add(leftId, userLeft);
        //  dic.Add(rightId, userRight);
        //  if (userLeft != null)
        //  {
        //      UserInfo userLeftTwo = _userDal.GetUserByLeftId(userLeft.LeftId);
        //      UserInfo userRightTwo = _userDal.GetUserByLeftId(userLeft.RightId);
        //      dic.Add(userLeft.LeftId, userLeftTwo);
        //      dic.Add(userLeft.RightId, userRightTwo);
        //  }
        //  //else 
        //  //{
        //  //    dic.Add(userLeft.LeftId, null);
        //  //    dic.Add(userLeft.RightId,null);
        //  //}
        //  if (userRight != null)
        //  {
        //      UserInfo userLeftTwo2 = _userDal.GetUserByLeftId(userRight.LeftId);
        //      UserInfo userRightTwo2 = _userDal.GetUserByLeftId(userRight.RightId);
        //      dic.Add(userRight.LeftId, userLeftTwo2);
        //      dic.Add(userRight.RightId, userRightTwo2);
        //  }
        //  //else {
        //  //    dic.Add(userRight.LeftId, null);
        //  //    dic.Add(userRight.RightId, null);
        //  //}

        //    return dic;
        //}


        public UserInfo GetUserInfoById(int userId)
        {
            return _userDal.GetUserInfoById(userId);
        }

        ///<summary>
        ///用户分页列表+搜索
        ///Author：m
        ///Date：2016年12月22日17:24:22
        ///</summary>
        ///<param name="userName">用户名</param>
        ///<param name="pageSize">当前页码</param>
        ///<param name="pageIndex">每页条数</param>
        ///<returns></returns>
        public Page<UserIndexModel> GetAdminUserInfoes(string userName, int pageSize, int pageIndex)
        {
            Page<UserIndexModel> pageList = null;
            try
            {
                pageList = _userDal.GetAdminUserInfoes(userName, pageSize, pageIndex);
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(UserBll), "GetAdminUserInfoes", Engineer.ggg, new { userName = userName, pageSize = pageSize, pageIndex = pageIndex }, ex);
            }
            return pageList;
        }

        /// <summary>
        /// 获取修改用户信息
        /// Author：m
        /// Date：2016年12月23日00:50:16
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public UserIndexModel GetAdminUserIndexModel(int userId)
        {
            UserIndexModel model = null;
            try
            {
                model = _userDal.GetAdminUserIndexModel(userId);
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(UserBll), "GetUserIndexModel", Engineer.ggg, new { userId = userId }, ex);
            }
            return model;
        }




        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public UserInfo AdminLogin(string logName, string pwd, out string msg)
        {
            bool result = false;
            //判断用户名是否存在
            msg = "";
            UserInfo userInfo = _userDal.GetUserByLoginName(logName);
            if (userInfo == null)
            {
                msg = "用户不存在";
                return userInfo;

            }
            //判断用户是否是管理员用户登录
            if (userInfo.UserType != SND_UserType.Admin)
            {
                msg = "用户不存在";
                return null;
            }

            //验证密码
            if (pwd != Auxiliary.Md5Decrypt(userInfo.Pwd))
            {
                msg = "用户密码不正确";

            }
            else
            {
                msg = "登录成功";
                result = true;
            }
            if (result)
            {
                return userInfo;
            }
            else 
            {
                return userInfo=null;
            }
            
        }
        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByUserName(string userName)
        {
            UserInfo user = new UserInfo();

            user = _userDal.GetUserByLoginName(userName);

            return user;
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public UserInfo UserLogin(string userName, string pwd, out string msg)
        {
            bool resultTemp = false;

            bool result = false;
            //判断用户名是否存在
            msg = "";
            UserInfo userInfo = _userDal.GetUserByLoginName(userName);
            if (userInfo == null)
            {
                msg = "用户不存在";
                return userInfo;

            }
            //判断用户是否是普通用户登录
            if (userInfo.UserType != SND_UserType.General)
            {
                msg = "用户不存在";
                return null;
            }
            //验证密码
            if (pwd != Auxiliary.Md5Decrypt(userInfo.Pwd))
            {
                msg = "用户密码不正确";
                return null;
            }
            else
            {
                ////根据左右区业绩变动修改等级

                //resultTemp = UpdateUserLevel(userInfo.UserId);

                //if (resultTemp)
                //{
                //    userInfo = _userDal.GetUserByLoginName(userName);
                //}
                msg = "登录成功";

            }
            if (userInfo.UserStatus==0)
            {
                msg = "该账号已被停用，请联系客服解决";
                return null;
            }
           

            return userInfo;

        }



        /// <summary>
        /// 注册
        /// Author：mgd
        /// Date：2016年12月11日13:30:03
        /// </summary>
        /// <param name="model">用户实体</param>
        /// <param name="msg">错误消息</param>
        /// <returns></returns>
        public bool Register(UserRegisterModel model, out string msg)
        {
            int i = 1;
            Random r = new Random();
            i = r.Next(0, 100);
            bool result = false;
            msg = "";
            //验证用户名是否存在
            UserInfo userInfo = _userDal.GetUserByLoginName(model.UserName);
            if (userInfo != null)
            {
                msg = "用户已存在";
                return result;
            }
            //验证推荐人是否存在
            UserInfo parentInfo = _userDal.GetUserByLoginName(model.ParentLoginName);
            if (parentInfo == null || (parentInfo.UserType != SND_UserType.General && parentInfo.UserType != SND_UserType.Mother))
            {
                msg = "推荐人无效";
                return result;
            }

            //注册
            UserInfo userEntity = new UserInfo();
            userEntity.UserName = model.UserName;
            userEntity.Pwd = Auxiliary.Md5Encrypt(model.Pwd);//密码md5加密
            userEntity.CreateTime = DateTime.Now;
            userEntity.Level = 0;
            userEntity.IsActivation = 0;
            userEntity.LeftId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            userEntity.ParentId = parentInfo.UserId;
            userEntity.TeamType = 0;
            userEntity.UserStatus = 1;
            userEntity.UserType = SND_UserType.General;
            userEntity.Phone = model.UserName;
            userEntity.RightId = DateTime.Now.ToString("yyyyMMddHHmmssfff")+i.ToString();
            userEntity.TeamParentId = model.TeamParentId;
            userEntity.PayPwd = Auxiliary.Md5Encrypt(model.PayPwd);

            int userId = _userDal.InserUser(userEntity);
            if (userId <= 0)
            {
                msg = "注册失败";
                return result;
            }
            else  //注册账户信息
            {

                AccountInfo customerAccount = new AccountInfo();

              
                customerAccount.CreateTime = DateTime.Now;
                customerAccount.FreezeGreen = 0;
                customerAccount.LeftAchievement = 0;
                customerAccount.RightAchievement = 0;
                customerAccount.GreenCount = 0;

                customerAccount.GreenTotal = 0;
                customerAccount.HongBao = 0;
                customerAccount.LeftCount = 0;
                customerAccount.Score = 0;
                customerAccount.RightCount = 0;
                customerAccount.StaticsRelease = 0;
                customerAccount.UserId = userId;
                customerAccount.UserName = model.UserName; ;

                bool resultAccount = _accountDal.AddCustomerAccount(customerAccount);
                result = resultAccount;

                if (resultAccount)//注册成功推送信息到商城
                {
                    //parentInfo.DownCount += 1;
                    //if (parentInfo.DownCount >= 3)
                    //{
                    //    parentInfo.IsTeam = 1;
                    //}
                    //bool resultParent = UpdateUserInfo(parentInfo);
                    string url = string.Format(PubConstant.ShopApi+"api/home/register") ;
                    bool resultShop = ShopRegister(model.UserName,model.Pwd,url);
                    if (!resultShop)
                    {
                        LogHelper.WriteInfo(typeof(UserBll),string.Format("商城用户注册失败，UserName:{0}",model.UserName));
                    }

                }

            }
            return result;
        }
        /// <summary>
        /// 调用商城注册接口
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="pwd"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool ShopRegister(string mobile,string pwd ,string url)
        {
            bool result = false;

            url = string.Format( url + "?mobile={0}&pwd={1}",mobile,pwd);

            try
            {

                result = HttpClientHelper.GetResponse(url) == "true" ? true : false;
            }
            catch (Exception ex)
            {

                LogHelper.WriteError(typeof(UserBll),ex);
            }

            return result;
                
        }


        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="smsRequest"></param>
        /// <returns></returns>
        public bool SendSmsNew(SMSCodeRequest smsRequest)
        {
            bool result = false;
            IClientProfile profile = DefaultProfile.GetProfile("cn-beijing", "LTAIGFELCpSJxBno", "0P75iXBYEEy2mOUv6itFhzxkFaWwXU");
            //IClientProfile profile = DefaultProfile.GetProfile("cn-beijing", "LTAIVpTlM5V7bC8j", "f3R43cN5IWv2Hd4CscciiojCsWDPQs");
            IAcsClient client = new DefaultAcsClient(profile);
            SingleSendSmsRequest requestSms = new SingleSendSmsRequest();
            try
            {
                requestSms.SignName = "众联农业科技";
                requestSms.TemplateCode = "SMS_70170128";
                requestSms.RecNum = smsRequest.Phone;
                requestSms.ParamString = "{'code':'" + smsRequest.Code + "'}";
                SingleSendSmsResponse httpResponse = client.GetAcsResponse(requestSms);
                result = true;
            }
            catch (ServerException e)
            {
                result = false;
                LogHelper.Error(string.Format("手机号：{0}，{1}", smsRequest.Phone, e.ErrorMessage));
            }
            catch (ClientException e)
            {
                result = false;
                LogHelper.Error(string.Format("手机号：{0}，{1}", smsRequest.Phone, e.ErrorMessage));
            }
            return result;
        }






        public bool UpdateUserInfo(UserInfo userInfo)
        {
            return _userDal.UpdateUserInfo(userInfo);
        }
        /// <summary>
        /// 更新用户等级信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool UpdateUserLevel(int userId)
        {

            bool result = false;
            int level = 1;

            AccountInfo account = new AccountInfo();
            account = _accountDal.GetAccByUserId(userId);
            UserInfo userInfo = new UserInfo();
            userInfo = GetUserInfoById(userId);

            decimal achievement = 0;

            if (account.LeftAchievement>account.RightAchievement)
            {
                achievement = account.RightAchievement;
            }
            else
            {
                achievement = account.LeftAchievement;
            }



            if (achievement < 1000000)
            {
                level = 1;
            }
            else if (1000000 <= achievement && achievement < 3500000)
            {
                level = 2;
            }
            else if (3500000 <= achievement && achievement < 7000000)
            {
                level = 3;
            }
            else if (7000000 <= achievement && achievement < 21000000)
            {
                level = 4;
            }
            else if (21000000 <= achievement)
            {
                level = 5;
            }

            if (userInfo.Level!=level) //等级发生改变
            {
                userInfo.Level = level;

            }

            result = UpdateUserInfo(userInfo);

            if (!result)
            {
                LogHelper.WriteInfo(typeof(UserBll),"更新等级失败，用户ID："+userId);
            }

            return result;
        }





        public UserInfo GetEndUserLeft(UserInfo user)
        {
            UserInfo userInfo = _userDal.GetUserLeftDown(user.LeftId);
            if (userInfo != null)
            {
                user = GetEndUserLeft(userInfo);
            }
     

            return user;
        }
    }
}


