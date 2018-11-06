using Common;
using Dal;
using DataModel;
using SndApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SndApi.Controllers
{
    public class GreenApiController : ApiController
    {

        UserDal _userDal = new UserDal();
        UserAccountDal _accDal = new UserAccountDal();
        ScoreLogDal _scoreLogDal = new ScoreLogDal();
        StatusLogDal _statusLogDal = new StatusLogDal();
        GlobalConfigDal _GlobalConfigDal = new GlobalConfigDal();
        [HttpGet]
        public bool Index()
        {
            return true;
        }


        #region get
        ////<summary>
        ////查询用户ep 
        ////</summary>
        ////<param name="userName"></param>
        ////<returns></returns>
        [HttpGet]
        public ReturnModel GetUserEp(string userName)
        {
            ReturnModel model = new ReturnModel();
            UserScore userScore = _userDal.GetShopByLoginName(userName);
            if (userScore != null)
            {
                model.IsTrue = true;
                model.Msg = "ok";
                model.ScoreData = userScore;

            }
            else
            {
                model.IsTrue = false;
                model.Msg = "用户不存在！";
            }
            return model;
        }

        //[HttpGet]
        //public ReturnModel GetUserEpA(string userName)
        //{
        //    ReturnModel model = new ReturnModel();
        //    UserEp userEp = _userDal.GetShopByLoginName(userName);
        //    if (userEp != null)
        //    {
        //        model.IsTrue = true;
        //        model.Msg = "ok";
        //        model.EpData = userEp;

        //    }
        //    else
        //    {
        //        model.IsTrue = false;
        //        model.Msg = "用户不存在！";
        //    }
        //    return model;
        //}

        /// <summary>
        /// 减掉ep消耗
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="score"></param>
        ///<param name="plusType">1：添加积分，2：扣减积分</param>
        /// <returns></returns>
        [HttpGet]
        public ReturnModel UpdateUserScore(string userName, decimal score, int plusType = 1)
        {
           
            ReturnModel model = new ReturnModel();
            UserInfo userInfo = _userDal.GetUserByLoginName(userName);

            if (userInfo == null)
            {
                model.IsTrue = false;
                model.Msg = "用户不存在！";
                return model;
            }

            AccountInfo acc = _accDal.GetAccByUserId(userInfo.UserId);
            if (acc == null)
            {
                model.IsTrue = false;
                model.Msg = "用户个人账户不存在！";
                return model;

            }

            if (plusType == 1)
            {
                acc.Score += score;
                model.Msg = "商城添加score";
            }
            else if (plusType == 2)
            {
                if (acc.Score - score < 0 && score <= 0)
                {
                    model.IsTrue = false;
                    model.Msg = "该score数量不足！";
                    return model;
                }
                acc.Score -= score;
                model.Msg = "商城减掉score";
            }
            else
            {
                model.IsTrue = false;
                model.Msg = "plusType参数错误！";
                return model;
            }

            model.IsTrue = _accDal.UpdateAccInfo(acc);
            LogHelper.WriteInfo(typeof(UserInfo), " UpdateUserScore ===== 修改账户ep记录", Engineer.ccc, new { userName = userName, ep = score, plusType = plusType });
            if (model.IsTrue)
            {
                ScoreLog scoreLog = new ScoreLog();
                scoreLog.UserId = acc.UserId;
                scoreLog.Remark = "商城积分兑换";
                scoreLog.LogCount = Convert.ToDecimal(score);
                scoreLog.CreateTime = DateTime.Now;

                _scoreLogDal.Insert(scoreLog);
                LogHelper.WriteInfo(typeof(UserInfo), " UpdateUserScore ===== 添加电商api===score加减记录", Engineer.ccc, scoreLog);
                model.Msg = "更新成功！";
            }
            else
            {
                model.Msg = "更新失败！";
            }
            return model;
        }

        /// <summary>
        /// 添加激活卡数量
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public ReturnModel PlusActivationCard(string userName, int count)
        //{
        //    ReturnModel model = new ReturnModel();
        //    UserInfo userInfo = _userDal.GetUserByLoginName(userName);
        //    if (userInfo == null)
        //    {
        //        model.IsTrue = false;
        //        model.Msg = "用户不存在！";
        //        return model;
        //    }
        //    int oldLevel = userInfo.Level;
        //    int level = userInfo.Level + count;

        //    AccountInfo acc = _accDal.GetAccByUserId(userInfo.UserId);
        //    if (acc == null)
        //    {
        //        model.IsTrue = false;
        //        model.Msg = "用户个人账户不存在！";
        //        return model;

        //    }
        //    if (count <= 0)
        //    {
        //        model.IsTrue = false;
        //        model.Msg = "该激活卡数量错误！";
        //        return model;
        //    }
        //    acc.ActivationCount += count;
        //    model.IsTrue = _accDal.UpdateAccInfo(acc);
        //    LogHelper.WriteInfo(typeof(UserInfo), " PlusActivationCard ===== 修改账户激活卡记录", Engineer.ccc, acc);
        //    if (model.IsTrue)
        //    {
        //        model.Msg = "激活卡添加成功！";
        //        ShopRecord shop = new ShopRecord();
        //        shop.UserName = userName;
        //        shop.CreateTime = DateTime.Now;
        //        shop.Number = count;
        //        shop.ShopType = 3;
        //        shop.Remark = "生成激活卡";
        //        _userDal.InsertShopRecord(shop);
        //        LogHelper.WriteInfo(typeof(UserInfo), " PlusActivationCard ===== 添加激活卡记录", Engineer.ccc, shop);

        //        //修改用户等级
        //        if (level == 2)
        //        {
        //            userInfo.Level = 2;
        //        }
        //        else if (level == 3)
        //        {
        //            userInfo.Level = 3;
        //        }
        //        else if (level == 4)
        //        {
        //            userInfo.Level = 4;
        //        }
        //        else if (level >= 5)
        //        {
        //            userInfo.Level = 5;
        //        }
        //        _userDal.UpdateUserInfo(userInfo);
        //        LogHelper.WriteInfo(typeof(UserInfo), " PlusActivationCard ===== 修改用户级别记录", Engineer.ccc, new { userId = userInfo.UserId, userLevel = oldLevel, level = level });
        //    }
        //    else
        //    {
        //        model.Msg = "激活卡添加失败！";
        //    }

        //    return model;
        //}

        #endregion


        /// <summary>
        /// 订单结束流程
        /// </summary> 
        [HttpGet]
        public ReturnModel OrderOverApi(string userName, int count)
        {
            ReturnModel returnModel = new ReturnModel();
            returnModel.IsTrue = false;

            UserInfo user = _userDal.GetUserByLoginName(userName);
            if (user != null)
            {
                user.Level += count;
                user.OutNum += count;
                if (user.Level >= 2)
                {
                    user.OutNum = 2;
                    user.Level = 2;
                }


                user.IsActivation = 1;
                returnModel.IsTrue = _userDal.UpdateUserInfo(user);
                if (returnModel.IsTrue)
                {
                    //查看直推人是否激活
                    UserInfo parentUser = _userDal.GetParentIdByUserId(user.ParentId);
                    #region //查看直推人是否激活   直推返利
                    if (parentUser.IsActivation == 1)
                    {
                        decimal zhitui = 0;
                        switch (parentUser.Level)
                        {
                            case 1:
                                zhitui = Convert.ToDecimal(330 * 0.15);
                                break;
                            case 2:
                                zhitui = Convert.ToDecimal(660 * 0.25);
                                break;
                        }
                        //添加直推人账户
                        AccountInfo parentAcc = _accDal.GetAccByUserId(parentUser.UserId);
                        parentAcc.Score += zhitui * Convert.ToDecimal(0.1);
                        parentAcc.GreenCount += zhitui * Convert.ToDecimal(0.9);
                        parentAcc.GreenTotal += zhitui;
                        returnModel.IsTrue = _accDal.UpdateAccInfo(parentAcc);
                        if (!returnModel.IsTrue)
                        {
                            returnModel.Msg = "直推返利，账户更新失败！";
                        }
                        //添加积分记录

                        ScoreLog scoreLog = new ScoreLog();
                        scoreLog.UserId = parentUser.UserId;
                        scoreLog.Remark = "推荐积分" + user.UserName;
                        scoreLog.LogCount = zhitui * Convert.ToDecimal(0.1);
                        scoreLog.CreateTime = DateTime.Now;
                        returnModel.IsTrue = _scoreLogDal.Insert(scoreLog);
                        if (!returnModel.IsTrue)
                        {
                            returnModel.Msg = "直推积分添加记录失败！";
                            LogHelper.WriteInfo(typeof(UserInfo), " OrderOverApi =====直推积分添加记录失败 ", Engineer.ccc, scoreLog);
                        }
                        //添加绿氧记录
                        StatusLog statusLog = new StatusLog();
                        statusLog.LogCount = zhitui * Convert.ToDecimal(0.9);
                        statusLog.CreateTime = DateTime.Now;
                        statusLog.LogType = 2;//1,静态  2，直推
                        statusLog.UserId = parentUser.UserId;
                        statusLog.ReUserId = user.UserId;
                        returnModel.IsTrue = _statusLogDal.Insert(statusLog);
                        if (!returnModel.IsTrue)
                        {
                            returnModel.Msg = "直推绿氧添加记录失败！";
                            LogHelper.WriteInfo(typeof(UserInfo), " OrderOverApi =====直推绿氧添加记录失败 ", Engineer.ccc, scoreLog);
                        }
                    }
                    #endregion

                    //向上递归添加业绩
                    GetUpTeamList(user, count);
                    //添加每日新增业绩
                    GlobalConfig gc = _GlobalConfigDal.GetGlobalConfig("EveryDate");
                    gc.ConfigValue = (Convert.ToDecimal(gc.ConfigValue) + (count * 330)).ToString();
                    _GlobalConfigDal.UpdateGlobalConfig(gc);
                }
                else
                {
                    returnModel.Msg = "修改级别，激活失败";
                }

            }

            return returnModel;
        }

        private void GetUpTeamList(UserInfo user,int count)
        {
            if (user != null) 
            {
            
            }
          UserInfo leftUser= _userDal.GetLeftUserByTeamParentId(user.TeamParentId);
            UserInfo rightUser=_userDal.GetRightUserByTeamParentId(user.TeamParentId);
            if (leftUser == null&& rightUser==null)
            {
                return;
            }
            if (leftUser != null)
            {
                AccountInfo leftAcc = _accDal.GetAccByUserId(leftUser.UserId);
                leftAcc.LeftCount += count;
                leftAcc.LeftAchievement += count * 330;
                _accDal.UpdateAccInfo(leftAcc);
                GetUpTeamList(leftUser, count);
            }
            if (rightUser != null)
            {
                AccountInfo rightAcc = _accDal.GetAccByUserId(rightUser.UserId);
                rightAcc.RightCount += count;
                rightAcc.RightAchievement += count * 330;
                _accDal.UpdateAccInfo(rightAcc);
                GetUpTeamList(rightUser, count);
            }

           
           
        }


        #region post
        ///// <summary>
        ///// 查询用户ep 
        ///// </summary>
        ///// <param name="userName"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ReturnModel GetUserEp(string userName)
        //{
        //    ReturnModel model = new ReturnModel();
        //    UserEp userEp = _userDal.GetShopByLoginName(userName);
        //    if (userEp != null)
        //    {
        //        model.IsTrue = true;
        //        model.Msg = "ok";
        //        model.EpData = userEp;

        //    }
        //    else
        //    {
        //        model.IsTrue = false;
        //        model.Msg = "用户不存在！";
        //    }
        //    return model;
        //}

        ///// <summary>
        ///// 减掉ep消耗
        ///// </summary>
        ///// <param name="userName"></param>
        ///// <param name="ep"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ReturnModel UpdateUserEp(string userName, decimal ep, int plusType = 1)
        //{
        //    string remark = "";
        //    ReturnModel model = new ReturnModel();
        //    UserInfo userInfo = _userDal.GetUserByLoginName(userName);

        //    if (userInfo == null)
        //    {
        //        model.IsTrue = false;
        //        model.Msg = "用户不存在！";
        //        return model;
        //    }

        //    AccountInfo acc = _accDal.GetAccByUserId(userInfo.UserId);
        //    if (acc == null)
        //    {
        //        model.IsTrue = false;
        //        model.Msg = "用户个人账户不存在！";
        //        return model;

        //    }

        //    if (plusType == 1)
        //    {
        //        acc.Ep += ep;
        //        remark = "商城添加ep";
        //    }
        //    else if (plusType == 2)
        //    {
        //        if (acc.Ep - ep < 0 && ep <= 0)
        //        {
        //            model.IsTrue = false;
        //            model.Msg = "该ep数量不足！";
        //            return model;
        //        }
        //        acc.Ep -= ep;
        //        remark = "商城减掉ep";
        //    }
        //    else
        //    {
        //        model.IsTrue = false;
        //        model.Msg = "plusType参数错误！";
        //        return model;
        //    }

        //    model.IsTrue = _accDal.UpdateAccInfo(acc);
        //    LogHelper.WriteInfo(typeof(UserInfo), " UpdateUserEp ===== 修改账户ep记录", Engineer.ccc, new { userName = userName, ep = ep, plusType = plusType });
        //    if (model.IsTrue)
        //    {
        //        ShopRecord shop = new ShopRecord();
        //        shop.UserName = userName;
        //        shop.CreateTime = DateTime.Now;
        //        shop.Number = ep;
        //        shop.ShopType = plusType;
        //        shop.Remark = remark;
        //        _userDal.InsertShopRecord(shop);
        //        LogHelper.WriteInfo(typeof(UserInfo), " UpdateUserEp ===== 添加电商api===ep加减记录", Engineer.ccc, shop);
        //        model.Msg = "更新成功！";
        //    }
        //    else
        //    {
        //        model.Msg = "更新失败！";
        //    }
        //    return model;
        //}

        ///// <summary>
        ///// 添加激活卡数量
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ReturnModel PlusActivationCard(string userName, int count)
        //{
        //    ReturnModel model = new ReturnModel();
        //    UserInfo userInfo = _userDal.GetUserByLoginName(userName);
        //    if (userInfo == null)
        //    {
        //        model.IsTrue = false;
        //        model.Msg = "用户不存在！";
        //        return model;
        //    }
        //    int oldLevel = userInfo.Level;
        //    int level = userInfo.Level + count;

        //    AccountInfo acc = _accDal.GetAccByUserId(userInfo.UserId);
        //    if (acc == null)
        //    {
        //        model.IsTrue = false;
        //        model.Msg = "用户个人账户不存在！";
        //        return model;

        //    }
        //    if (count <= 0)
        //    {
        //        model.IsTrue = false;
        //        model.Msg = "该激活卡数量错误！";
        //        return model;
        //    }
        //    acc.ActivationCount += count;
        //    model.IsTrue = _accDal.UpdateAccInfo(acc);
        //    LogHelper.WriteInfo(typeof(UserInfo), " PlusActivationCard ===== 修改账户激活卡记录", Engineer.ccc, acc);
        //    if (model.IsTrue)
        //    {
        //        model.Msg = "激活卡添加成功！";
        //        ShopRecord shop = new ShopRecord();
        //        shop.UserName = userName;
        //        shop.CreateTime = DateTime.Now;
        //        shop.Number = count;
        //        shop.ShopType = 3;
        //        shop.Remark = "生成激活卡";
        //        _userDal.InsertShopRecord(shop);
        //        LogHelper.WriteInfo(typeof(UserInfo), " PlusActivationCard ===== 添加激活卡记录", Engineer.ccc, shop);

        //        //修改用户等级
        //        if (level == 2)
        //        {
        //            userInfo.Level = 2;
        //        }
        //        else if (level == 3)
        //        {
        //            userInfo.Level = 3;
        //        }
        //        else if (level == 4)
        //        {
        //            userInfo.Level = 4;
        //        }
        //        else if (level >= 5)
        //        {
        //            userInfo.Level = 5;
        //        }
        //        _userDal.UpdateUserInfo(userInfo);
        //        LogHelper.WriteInfo(typeof(UserInfo), " PlusActivationCard ===== 修改用户级别记录", Engineer.ccc, new { userId = userInfo.UserId, userLevel = oldLevel, level = level });
        //    }
        //    else
        //    {
        //        model.Msg = "激活卡添加失败！";
        //    }

        //    return model;
        //} 
        #endregion



    }
}
