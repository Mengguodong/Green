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
        ///<summary>
        ///用户分页列表+搜索
        ///Author：m
        ///Date：2016年12月22日17:24:22
        ///</summary>
        ///<param name="userName">用户名</param>
        ///<param name="pageSize">当前页码</param>
        ///<param name="pageIndex">每页条数</param>
        ///<returns></returns>
        public Page<UserIndexModel> GetUserInfoes(string userName, int pageSize, int pageIndex)
        {
            Page<UserIndexModel> pageList = null;
            try
            {
              pageList =  _userDal.GetUserInfoes(userName, pageSize, pageIndex);
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(UserBll), "GetUserInfoes", Engineer.ggg, new { userName = userName, pageSize = pageSize, pageIndex = pageIndex }, ex);
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
        public UserIndexModel GetUserIndexModel(int userId)
        {
            UserIndexModel model = null;
            try
            {
               model =  _userDal.GetUserIndexModel(userId);
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
            if (userInfo.UserStatus != (int)UserType.General)
            {
                msg = "用户不存在";
                return null;
            }
            //验证密码
            if (pwd != Auxiliary.Md5Decrypt(userInfo.LoginPWD))
            {
                msg = "用户密码不正确";

            }
            else
            {
                msg = "登录成功";

            }

            return userInfo;
        }
    
        /// <summary>
        /// 用户登录
        /// Author：孟国栋
        /// Date：2016年12月10日14:14:33
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="msg">返回信息</param>
        /// <returns></returns>
        //public UserInfo UserLogin(string userName, string pwd, out string msg)
        //{

        //    bool result = false;
        //    //判断用户名是否存在
        //    msg = "";
        //     UserInfo userInfo = _userDal.GetUserInfoByName(userName);
        //    if (userInfo == null)
        //    {
        //        msg = "用户不存在";
        //        return userInfo;

        //    }
        //    //判断用户是否是普通用户登录
        //    if (userInfo.UserStatus != UserType.General)
        //    {
        //        msg = "用户不存在";
        //        return null;
        //    }
        //    //验证密码
        //    if (pwd != Auxiliary.Md5Decrypt(userInfo.Pwd))
        //    {
        //        msg = "用户密码不正确";
        //        return null;
        //    }
        //    else
        //    {
        //        msg = "登录成功";

        //    }

        //    return userInfo;

        //}

     

        ///// <summary>
        ///// 注册
        ///// Author：mgd
        ///// Date：2016年12月11日13:30:03
        ///// </summary>
        ///// <param name="model">用户实体</param>
        ///// <param name="msg">错误消息</param>
        ///// <returns></returns>
        //public bool Register(UserRegisterModel model, out string msg)
        //{
        //    bool result = false;
        //    msg = "";
        //    //验证用户名是否存在
        //    UserInfo userInfo = _userDal.GetUserInfoByName(model.UserName);
        //    if (userInfo != null)
        //    {
        //        msg = "用户已存在";
        //        return result;
        //    }
        //    //验证推荐人是否存在
        //    UserInfo parentInfo = _userDal.GetUserInfoByName(model.ParentLoginName);
        //    if (parentInfo == null || (parentInfo.UserType != UserType.General && parentInfo.UserType != UserType.Mother))
        //    {
        //        msg = "推荐人无效";
        //        return result;
        //    }

        //    //注册
        //    UserInfo userEntity = new UserInfo();
        //    userEntity.UserName = model.UserName;
        //    userEntity.Pwd = Auxiliary.Md5Encrypt(model.Pwd);//密码md5加密
        //    userEntity.MobilePhone = model.UserName;
        //    userEntity.ParentId = parentInfo.UserId;
        //    userEntity.CreateTime = DateTime.Now;
        //    userEntity.UserStatues = 1;
        //    userEntity.IsActivate = 0;
        //    //获取酒农的等级ID    等级说明：type--1  酿酒工  2酒坊主   3 酒坊   4庄园
        //    userEntity.LevelId = GetLevelInfos().Where(x => x.LevelType == LevelType.JiuNong).FirstOrDefault().LevelId;
        //    //userEntity.LevelName = GetLevelInfos().Where(x => x.LevelType == LevelType.JiuNong).FirstOrDefault().LevelName;
        //    userEntity.UserType = UserType.General;    // 1.普通用户  2.管理员   3.超级管理员
        //    userEntity.IsDelete = 0;
        //    userEntity.IsTestAccount = 0;
        //    userEntity.IsReturn = 0;
        //    userEntity.IsTeam = 0;
        //    userEntity.DownCount = 0;
        //    userEntity.IsQueue = 0;
        //    userEntity.AuthenticationTime = DateTime.Now.AddYears(-100);
        //    userEntity.BecomeTeamTime = DateTime.Now.AddYears(-100);
        //    userEntity.BestLevelTime = DateTime.Now.AddYears(-100);

        //    int userId = InserUser(userEntity);
        //    if (userId <= 0)
        //    {
        //        msg = "注册失败";
        //        return result;
        //    }
        //    else  //注册账户信息
        //    {

        //        CustomerAccount customerAccount = new CustomerAccount();
        //        customerAccount.AccountStatus = 0;
        //        customerAccount.CreateTime = DateTime.Now;
        //        customerAccount.FinishedWineRealNum = 0;
        //        customerAccount.FinishedWineTotal = 0;
        //        customerAccount.IsDelete = 0;
        //        customerAccount.LeesCount = 0;
        //        customerAccount.Score = 0;
        //        customerAccount.UserId = userId;//注册返回的用户ID
        //        customerAccount.WCRemainder = 0;
        //        customerAccount.WCTotalCount = 0;
        //        customerAccount.WineCellar = 0;
        //        customerAccount.WineCoin = 0;
        //        bool resultAccount = AddCustomerAccount(customerAccount);
        //        result = resultAccount;

        //        if (resultAccount)//注册成功更新推荐人信息
        //        {
        //            parentInfo.DownCount += 1;
        //            if (parentInfo.DownCount >= 3)
        //            {
        //                parentInfo.IsTeam = 1;
        //            }
        //            bool resultParent = UpdateUserInfo(parentInfo);

        //        }

        //    }
        //    return result;
        //}


        /// <summary>
        /// 插入账户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public bool AddCustomerAccount(CustomerAccount model)
        //{
        //    bool result = false;
        //    try
        //    {
        //        string parameter = JsonConvertTool.SerializeObject(model);
        //        string url = PubConstant.WineGameWebApi + "api/account/insertcustomeraccount";
        //        result = HttpClientHelper.PostResponse(url, parameter) == "true" ? true : false;

        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserBLL), "AddCustomerAccount", Engineer.ggg, model, ex);
        //    }
        //    return result;

        //}

        /// <summary>
        /// 注册返利方法
        /// Author：mgd
        /// Date：2016年12月11日17:36:25
        /// </summary>
        /// <param name="parentId">推荐人ID</param>
        /// <returns></returns>
        //public bool RegisterReturn(int parentId)
        //{
        //    bool result = false;
        //    int returnType = 0;//0注册返利1酒窖返利
        //    int consumptionCount = 100;//消费酒币数量
        //    try
        //    {
        //        string url = string.Format(PubConstant.WineGameWebApi + "api/account/accountreturn?parentId={0}&returnType={1}&Count={2}", parentId, returnType, consumptionCount);

        //        result = HttpClientHelper.GetResponse(url) == "true" ? true : false;

        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserBLL), "RegisterReturn", Engineer.ggg, new { parentId = parentId }, ex);
        //        //throw;
        //    }



        //    return result;
        //}



        /// <summary>
        /// 获取等级信息
        /// </summary>
        /// <returns></returns>
        //public List<LevelInfo> GetLevelInfos()
        //{
        //    List<LevelInfo> list = new List<LevelInfo>();
        //    try
        //    {

        //        string url = PubConstant.WineGameWebApi + "api/user/GetLevelInfos";

        //        var result = HttpClientHelper.GetResponse<List<LevelInfo>>(url);

        //        if (result != null)
        //        {
        //            list = result;
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserBLL), "GetLevelInfoByLevelType", Engineer.ggg, null, ex);
        //    }

        //    return list;
        //}
      


        /// <summary>
        /// 绑定用户信息
        /// Author:m
        /// Date:2016年12月17日13:53:04
        /// </summary>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        //public bool BindingUser(UserInfo model)
        //{
        //    bool result = false;
        //    model.AuthenticationTime = DateTime.Now;
        //    try
        //    {
        //        string url = PubConstant.WineGameWebApi + "api/user/updateuser";
        //        string paramenterStr = JsonConvertTool.SerializeObject(model);
        //        result = HttpClientHelper.PostResponse(url, paramenterStr) == "true" ? true : false;

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog(typeof(UserBLL), "BindingUser", Engineer.ggg, model, ex);
        //    }


        //    return result;
        //}
        /// <summary>
        /// 修改用户信息
        /// Author:m
        /// Date:2016年12月17日13:53:04
        /// </summary>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        //public bool UpdateUserInfo(UserInfo model)
        //{
        //    bool result = false;

        //    try
        //    {
        //        string url = PubConstant.WineGameWebApi + "api/user/updateuserinfo";
        //        string paramenterStr = JsonConvertTool.SerializeObject(model);
        //        result = HttpClientHelper.PostResponse(url, paramenterStr) == "true" ? true : false;

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog(typeof(UserBLL), "UpdateUserInfo", Engineer.ggg, model, ex);
        //    }


        //    return result;
        //}

        /// <summary>
        /// 修改密码
        /// Author：m
        /// Date：2016年12月17日14:49:35
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <param name="newPwd">新密码</param>
        /// <param name="userId">用户ID</param>
        /// <param name="msg">错误消息</param>
        /// <returns></returns>
        //public bool UpdatePwd(string pwd, string newPwd, int userId, out string msg)
        //{
        //    bool result = false;
        //    msg = "";
        //    UserInfo userInfo = new UserInfo();
        //    userInfo = GetUserInfoById(userId);
        //    if (userInfo == null)
        //    {
        //        msg = "获取用户信息出错";
        //        return result;
        //    }
        //    if (pwd != Auxiliary.Md5Decrypt(userInfo.Pwd))//验证旧密码
        //    {
        //        msg = "旧密码错误";
        //        return result;
        //    }
        //    result = UpdatePwdByUserId(userId, newPwd);
        //    return result;

        //}
        /// <summary>
        /// 根据用户名重置密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        //public bool UpdatePwdByUserId(int userId, string newPwd)
        //{
        //    UserInfo userInfo = new UserInfo();
        //    bool result = false;

        //    userInfo.UserId = userId;
        //    userInfo.Pwd = Auxiliary.Md5Encrypt(newPwd);//设置新密码

        //    try
        //    {
        //        string url = PubConstant.WineGameWebApi + "api/user/updatepwd";
        //        string paramenterStr = JsonConvertTool.SerializeObject(userInfo);
        //        result = HttpClientHelper.PostResponse(url, paramenterStr) == "true" ? true : false;

        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserBLL), "UpdateUserInfo", Engineer.ggg, userInfo, ex);
        //    }
        //    return result;

        //}
     
       

        /// <summary>
        /// 激活用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="wineCoin">充值酒币数</param>
        /// <returns></returns>
        //public bool ActivateUserByUserId(int userId, decimal wineCoin, int adminId, out string msg)
        //{
        //    int typeActivate = 2;////1.用户充值， 2.用户激活

        //    bool result = false;
        //    msg = "";
        //    UserInfo user = new UserInfo();
        //    //1.将用户状态改为已激活
        //    user.UserId = userId;
        //    user.IsActivate = 1;
        //    //根据充值数量改变用户等级

        //    List<LevelInfo> levelList = GetLevelInfos();

        //    if (wineCoin < PubConstant.WorkHouse())//酒农
        //    {
        //        user.LevelId = levelList.Where(x => x.LevelType == LevelType.JiuNong).FirstOrDefault().LevelId;
        //        //user.LevelName = levelList.Where(x => x.LevelType == LevelType.JiuNong).FirstOrDefault().LevelName;
        //    }
        //    else if (wineCoin >= PubConstant.WorkHouse() && wineCoin < PubConstant.WorkShop())//作坊
        //    {
        //        user.LevelId = levelList.Where(x => x.LevelType == LevelType.ZuoFang).FirstOrDefault().LevelId;
        //        //user.LevelName = levelList.Where(x => x.LevelType == LevelType.ZuoFang).FirstOrDefault().LevelName;
        //    }
        //    else if (wineCoin == PubConstant.WorkShop())//车间
        //    {
        //        user.LevelId = levelList.Where(x => x.LevelType == LevelType.CheJian).FirstOrDefault().LevelId;
        //        //user.LevelName = levelList.Where(x => x.LevelType == LevelType.ZuoFang).FirstOrDefault().LevelName; ;
        //    }
        //    else
        //    {
        //        user.LevelId = levelList.Where(x => x.LevelType == LevelType.JiuNong).FirstOrDefault().LevelId;
        //    }
        //    bool resultUser = ActivateUser(user);
        //    if (!resultUser)
        //    {
        //        msg = "激活用户出错";
        //        return result;
        //    }
        //    else
        //    {
        //        UserInfo userinfo = GetUserInfoById(userId);
        //        if (userinfo != null)
        //        {
        //            marketBll.ActivateUpdateAllParentsDownCount(userinfo);
        //        }
        //        else
        //        {
        //            LogHelper.WriteInfo(typeof(UserBLL), "未获取到用户！", Engineer.ccc, user);
        //        }
        //    }

        //    //2.给用户账户充值
        //    bool resultCoin = coinExchangeBll.RechargeCoin(userId, wineCoin - 100, adminId, typeActivate);//激活账户，抽取100注册费


        //    //3.返利
        //    int parentUserId = GetParentIdByUserId(userId);
        //    if (resultCoin)
        //    {
        //        //返利
        //        bool isReturn = RegisterReturn(parentUserId);
        //        UserInfo userinfo = GetUserInfoById(userId);
        //        if (userinfo != null)
        //        {
        //            marketBll.RechargeUpdateAllParentsAchievement(userinfo, wineCoin - 100);
        //        }
        //        else
        //        {
        //            LogHelper.WriteInfo(typeof(UserBLL), "未获取到用户！", Engineer.ccc, new { userId = userId, money = wineCoin - 100 });
        //        }
        //        result = true;
        //    }
        //    else
        //    {
        //        msg = "账户充值失败";
        //        return result;
        //    }


        //    return result;
        //}

        //======================
        /// <summary>
        /// 赠送激活用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        //public bool DonateActivateUserByUserId(int userId, out string msg)
        //{
        //    msg = "";
        //    bool isTrue = false;
        //    UserInfo user = new UserInfo();
        //    //1.将用户状态改为已激活
        //    user.UserId = userId;
        //    user.IsActivate = 1;
        //    List<LevelInfo> levelList = GetLevelInfos();
        //    user.LevelId = levelList.Where(x => x.LevelType == LevelType.JiuNong).FirstOrDefault().LevelId;

        //    isTrue = ActivateUser(user);
        //    if (!isTrue)
        //    {
        //        msg = "激活用户出错";
        //    }
        //    else
        //    {
        //        UserInfo userinfo = GetUserInfoById(userId);
        //        if (user != null)
        //        {
        //            marketBll.ActivateUpdateAllParentsDownCount(user);
        //        }
        //        else
        //        {
        //            LogHelper.WriteInfo(typeof(UserBLL), "未获取到用户！", Engineer.ccc, user);
        //        }
        //    }
        //    return isTrue;
        //}



        /// <summary>
        /// 赠送酒窖
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="wineCellar"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        //public bool DonatWineCellar(int userId, int wineCellar, out string msg)
        //{
        //    bool isTrue = false;
        //    msg = "";
        //    CustomerAccount ca = accountBll.GetCustomerAccountByUserId(userId);
        //    ca.WineCellar += wineCellar;
        //    ca.WCRemainder += wineCellar * 10;
        //    isTrue = accountBll.UpdateCustomerAccount(ca);
        //    if (!isTrue)
        //    {
        //        msg = "赠送酒窖失败！";
        //    }
        //    return isTrue;
        //}
        //======================


        /// <summary>
        /// 根据用户ID获取父级Id
        /// Author：m
        /// Date：2016年12月23日15:06:30
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        //public int GetParentIdByUserId(int userId)
        //{
        //    int parentId = 0;
        //    try
        //    {
        //        string url = PubConstant.WineGameWebApi + "api/user/getparentidbyuserid?userId=" + userId;

        //        parentId = ConvertHelper.ToInt32(HttpClientHelper.GetResponse(url));
        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserBLL), "GetParentIdByUserId", Engineer.ggg, new { userId = userId }, ex);
        //        //throw;
        //    }
        //    return parentId;

        //}

        /// <summary>
        /// 根据用户ID激活用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public bool ActivateUser(UserInfo model)
        //{
        //    bool result = false;
        //    try
        //    {

        //        string parameter = JsonConvertTool.SerializeObject(model);

        //        string url = PubConstant.WineGameWebApi + "api/user/activateuser";

        //        result = HttpClientHelper.PostResponse(url, parameter) == "true" ? true : false;

        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserBLL), "ActivateUser", Engineer.ggg, model, ex);
        //    }

        //    return result;


        //}

        /// <summary>
        /// 获取用户账户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public UserAccountInfoModel GetUserAccountInfo(int userId)
        //{
        //    UserAccountInfoModel model = new UserAccountInfoModel();
        //    if (userId > 0)
        //    {
        //        CustomerAccount customer = accountBll.GetCustomerAccountByUserId(userId);
        //        UserInfo user = GetUserInfoById(userId);
        //        if (customer != null && user != null)
        //        {
        //            model.UserName = user.UserName;
        //            model.IsActivate = user.IsActivate;
        //            model.LevelId = user.LevelId;
        //            model.RealName = user.RealName;
        //            model.BankCardNo = user.BankCardNo;
        //            model.BankName = user.BankName;
        //            model.IDCard = user.IDCard;
        //            model.CreateTime = user.CreateTime;
        //            model.MobilePhone = user.MobilePhone;
        //            model.IsReturn = user.IsReturn;
        //            model.DownCount = user.DownCount;
        //            model.IsTeam = user.IsTeam;
        //            model.DeliveryAddress = user.DeliveryAddress;

        //            model.WineCoin = customer.WineCoin;
        //            model.LeesCount = customer.LeesCount;
        //            model.WineCellar = customer.WineCellar;
        //            model.WCRemainder = customer.WCRemainder;
        //            model.WCTotalCount = customer.WCTotalCount;
        //            model.Score = customer.Score;
        //            model.FinishedWineRealNum = customer.FinishedWineRealNum;
        //            model.FinishedWineTotal = customer.FinishedWineTotal;
        //            model.FreezeWine = customer.FreezeWine;

        //        }
        //        return model;

        //    }
        //    else
        //    {
        //        return model;
        //    }
        //}
    }
}
