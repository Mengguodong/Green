using Common;
using Dal;
using DataModel;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveryDayEpService
{
    public class EveryDayEpJob : IJob
    {
        UserDal _userDal = new UserDal();

        UserAccountDal _accountDal = new UserAccountDal();

        public void Execute(JobExecutionContext context)
        {
            bool result = false;

            List<UserAccInfoModel> list = new List<UserAccInfoModel>();
            //获取所有用户 查询条件： 不禁止状态=1，    总资产>o

            list = _userDal.GetEpAllUserInfo();


            foreach (var item in list)
            {

                decimal dayStaticsEp = 0;
                //静态
                switch (item.Level)
                {
                    case 1:
                        dayStaticsEp = item.TotalAssets * (decimal)0.002M;
                        break;
                    case 2:
                        dayStaticsEp = item.TotalAssets * (decimal)0.005M;
                        break;
                    case 3:
                        dayStaticsEp = item.TotalAssets * (decimal)0.008M;
                        break;
                    case 4:
                        dayStaticsEp = item.TotalAssets * (decimal)0.01M;
                        break;
                    case 5:
                        dayStaticsEp = item.TotalAssets * (decimal)0.02M;
                        break;
                }

                AccountInfo accInfo = _accountDal.GetAccByAccId(item.AccountId);
                if (accInfo == null)
                {
                    LogHelper.WriteInfo(typeof(AccountInfo), "静态流程账户为空！", Engineer.ccc, item);
                    continue;
                }
                accInfo.StaticsRelease = dayStaticsEp;
                bool staticIsTrue = _accountDal.UpdateAccInfo(accInfo);
                if (!staticIsTrue)
                {
                    LogHelper.WriteInfo(typeof(AccountInfo), "静态修改用户待释放资产失败！", Engineer.ccc, accInfo);
                }

                if (item.YesTodayIsLogin > 0)
                {
                    //动态
                    DynamicEP(list, item.TeamParentId, dayStaticsEp);
                }


            }


        }
        //V1级别享受第一层30%
        //V2级别享受第一层30%，第二层20%
        //V3级别享受第一层30%，第二层20%，第三层10%
        //V4级别享受第一层30%，第二层20%，第三层10%，第四层5%
        //V5级别享受第一层30%，第二层20%，第三层10%，第四层5%，第五层2%
        public void DynamicEP(List<UserAccInfoModel> list, string teamParentId, decimal dayStaticsEp)
        {

            decimal ratio = 0;
            for (int i = 1; i <= 5; i++)
            {

                IEnumerable<UserAccInfoModel> userAccEum = from item in list
                                                           where item.LeftId == teamParentId || item.RightId == teamParentId
                                                           select item;
                if (userAccEum != null && userAccEum.Count() == 1)
                {
                    UserAccInfoModel userAcc = new UserAccInfoModel();
                    userAcc = userAccEum.First();

                    teamParentId = userAcc.TeamParentId;
                    switch (i)
                    {
                        case 1:
                            ratio = 0.15M;
                            break;
                        case 2:
                            ratio = 0.12M;
                            break;
                        case 3:
                            ratio = 0.08M;
                            break;
                        case 4:
                            ratio = 0.05M;
                            break;
                        case 5:
                            ratio = 0.02M;
                            break;
                    }
                    if (userAcc.Level >= i)
                    {
                        AccountInfo accInfo = _accountDal.GetAccByAccId(userAcc.AccountId);
                        if (accInfo == null)
                        {
                            LogHelper.WriteInfo(typeof(AccountInfo), "DynamicEP----动态流程账户为空！", Engineer.ccc, accInfo);
                            continue;
                        }
                     
                        accInfo.WaitRelease += dayStaticsEp * (decimal)0.65 * ratio;

                        bool waitIsTrue = _accountDal.UpdateAccInfo(accInfo);
                        if (!waitIsTrue)
                        {
                            LogHelper.WriteInfo(typeof(AccountInfo), "DynamicEP----动态修改待释放资产失败！", Engineer.ccc, accInfo);
                        }
                    }


                }
                else
                {
                    break;
                }

            }

        }


    }
}
