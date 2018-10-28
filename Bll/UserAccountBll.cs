using Dal;
using DataModel;
using DataModel.RequestModel;
using DataModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using EveryDayEpService;

namespace Bll
{
    public class UserAccountBll
    {
        UserAccountDal _accDal = new UserAccountDal();
        //LogBll logBll = new LogBll();
        //GlobalConfigBll configBll = new GlobalConfigBll();

        /// <summary>
        /// 用户id查实体
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AccountInfo GetAccByUserId(int userId)
        {
            AccountInfo acc = _accDal.GetAccByUserId(userId);
            return acc;
        }


        ///// <summary>
        ///// 用户账户地址查实体
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public AccountInfo GetAccByAccName(string accName)
        //{
        //    AccountInfo acc = _accDal.GetAccByAccName(accName);
        //    return acc;
        //}


        /// <summary>
        /// 修改账户信息
        /// </summary>
        /// <param name="accInfo"></param>
        /// <returns></returns>

        public bool UpdateAccInfo(AccountInfo accInfo)
        {
            bool isTrue = _accDal.UpdateAccInfo(accInfo);

            return isTrue;
        }


        /// <summary>
        /// 检查账户余额
        /// </summary>
        /// <param name="Qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>

        //public bool CheckAccountBalance(int Qty, int type, int userId)
        //{
        //    bool result = false;


        //    result = _accDal.CheckAccountBalance(Qty, type, userId);


        //    return result;

        //}
        /// <summary>
        /// 订单创建成功，冻结账户
        /// </summary>
        /// <param name="Qty"></param>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public bool FreezeAccount(int Qty, int type, int userId, int IsFreeze)
        //{
        //    return _accDal.FreezeAccount(Qty, type, userId, IsFreeze);
        //}

        /// <summary>
        /// 创建激活卡记录
        /// </summary>
        /// <param name="activTable"></param>
        /// <returns></returns>
        //public bool AddActivTable(ActivationTable activTable) 
        //{
        //    return _accDal.AddActivTable(activTable);
        //}

        /// <summary>
        /// 使用激活卡流程
        /// </summary>
        /// <param name="activationUserName"></param>
        //public bool ActivationCard(string activationUserName, UserInfo userInfo, UserInfo reUser, AccountInfo accInfo, AccountInfo reAccInfo)
        //{
        //    return _accDal.ActivationCard(activationUserName, userInfo, reUser, accInfo, reAccInfo);
        //}

        /// <summary>
        /// 激活卡记录
        /// </summary>
        /// <param name="p"></param>
        public Page<ActicationCardExchangeModel> ActivationCardExchange(int userId, int activationType, int pageIndex, int pageSize)
        {
            return _accDal.ActivationCardExchange(userId, activationType, pageIndex, pageSize);
        }

        /// <summary>
        /// 激活卡赠送
        /// </summary>
        /// <param name="reUserName"></param>
        /// <param name="accInfo"></param>
        /// <param name="reAccInfo"></param>
        /// <returns></returns>
        //public bool GiveActivationCard(UserInfo userInfo, UserInfo reUserInfo, AccountInfo accInfo, AccountInfo reAccInfo)
        //{
        //    return _accDal.GiveActivationCard(userInfo, reUserInfo, accInfo, reAccInfo);
        //}


        ///// <summary>
        ///// 释放静态并生成记录
        ///// </summary>
        ///// <param name="p"></param>
        ///// <returns></returns>
        //public bool StaticDistribution(int userId)
        //{
        //    LogInfo log = new LogInfo();
        //    bool result = false;
        //    AccountInfo account = new AccountInfo();

        //    account = GetAccByUserId(userId);
        //    string zfcPrice = configBll.GetValueByConfigName("ZfcPrice");



        //    account.TotalAssets -= account.StaticsRelease;
        //    account.Ep += account.StaticsRelease * (decimal)0.65;
        //    account.Zfc += account.StaticsRelease * (decimal)0.35/decimal.Parse(zfcPrice);
        //    if (account.TotalAssets<=0)
        //    {
        //        account.TotalAssets = 0;
        //    }

        //    result = UpdateAccInfo(account);

        //    if (result)//添加静态记录
        //    {
        //        log.CreateTime = DateTime.Now;
        //        log.Ep = account.StaticsRelease * (decimal)0.65;
        //        log.Zfc = account.StaticsRelease * (decimal)0.35 / decimal.Parse(zfcPrice);
        //        log.UserId = userId;
        //        log.LogType = 3;
        //        result = logBll.Insert(log);
        //        if (!result)
        //        {
        //            LogHelper.WriteInfo(typeof(UserAccountBll), "添加静态释放记录失败，用户ID：" + userId + " 当日释放总量：" + account.StaticsRelease);
        //        }
        //        else 
        //        {
        //           AccountInfo acc = GetAccByUserId(userId);
        //           acc.StaticsRelease = 0;
        //           result = UpdateAccInfo(acc);
        //        }
        //    }
        //    else
        //    {
        //        LogHelper.WriteInfo(typeof(UserAccountBll), "用户释放静态出错，用户ID：" + userId + " 当日释放总量：" + account.StaticsRelease);
        //    }

        //    return result;


        //}
        ///// <summary>
        ///// 释放动态并生成记录
        ///// </summary>
        ///// <param name="p"></param>
        ///// <returns></returns>
        //public bool DynamicDistribution(int userId)
        //{
        //    LogInfo log = new LogInfo();
        //    bool result = false;
        //    AccountInfo account = new AccountInfo();

        //    account = GetAccByUserId(userId);

        //    if (account.WaitRelease==0)
        //    {
        //        return true;
        //    }

        //    account.Ep += account.WaitRelease;
        //    account.TotalAssets -= account.WaitRelease;

        //    if (account.TotalAssets <= 0)
        //    {
        //        account.TotalAssets = 0;
        //    }
           
        //    result = UpdateAccInfo(account);

        //    if (result)//添加动态记录
        //    {
        //        log.CreateTime = DateTime.Now;
        //        log.Ep = account.WaitRelease;
        //        log.UserId = userId;
        //        log.LogType = 4;
        //        result = logBll.Insert(log);
        //        if (!result)
        //        {
        //            LogHelper.WriteInfo(typeof(UserAccountBll), "添加动态释放记录失败，用户ID：" + userId + " 当日释放总量：" + account.WaitRelease);
        //        }
        //        else 
        //        {
        //            AccountInfo acc = GetAccByUserId(userId);
        //            acc.WaitRelease = 0;
        //            result = UpdateAccInfo(acc);
        //        }
        //    }
        //    else
        //    {
        //        LogHelper.WriteInfo(typeof(UserAccountBll), "用户释放动态出错，用户ID：" + userId + " 当日释放总量：" + account.WaitRelease);
        //    }

        //    return result;
        //}

        /// <summary>
        /// 添加业绩
        /// </summary>
        /// <param name="userId"></param>
        public void PlusAchievement(UserInfo userInfo)
        {
            string teamParentId = userInfo.TeamParentId;
            bool isTrue = true;
            while (isTrue)
            {
                UserAccInfoModel userAcc = new UserAccInfoModel();
                userAcc = _accDal.GetTeamParentAccInfo(teamParentId);
                if (userAcc != null)
                {

                    if (userAcc.LeftId == teamParentId)
                    {
                       AccountInfo accInfo = GetAccByUserId(userAcc.UserId);
                       accInfo.LeftCount += 1;
                       accInfo.LeftAchievement = accInfo.LeftCount * 3980;
                       UpdateAccInfo(accInfo);
                    }
                    else if (userAcc.RightId == teamParentId) 
                    {
                        AccountInfo accInfo = GetAccByUserId(userAcc.UserId);
                        accInfo.RightCount += 1;
                        accInfo.RightAchievement = accInfo.RightCount * 3980;
                        UpdateAccInfo(accInfo);
                    }
                        teamParentId = userAcc.TeamParentId;
                        if (string.IsNullOrEmpty(teamParentId)) 
                        {
                            isTrue = false;
                        }
                }
               

            }
        }

    }
}
