using Common;
using Dal;
using DataModel;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusService
{
    public class HongBaoEveryJob : IJob
    {
        UserDal _UserDal = new UserDal();
        GlobalConfigDal _GlobalConfigDal = new GlobalConfigDal();
        UserAccountDal _UserAccountDal = new UserAccountDal();
        StatusLogDal _statusLogDal = new StatusLogDal();
        ScoreLogDal _scoreLogDal = new ScoreLogDal();
        HongBaoLogDal _hongBaoLogDal = new HongBaoLogDal();

        public void Execute(JobExecutionContext context)
        {
            List<AccountInfo> accAllList = new List<AccountInfo>();
            //List<UserInfo> list = new List<UserInfo>();
            GlobalConfig gc = _GlobalConfigDal.GetGlobalConfig("EveryDate");
            //list = _UserDal.GetAllUser();
            accAllList = _UserAccountDal.GetAllAcc();
            int hongBaoSum = 0;
            //获取红包总数
            foreach (var item in accAllList)
            {
                AccountInfo acc = new AccountInfo();
                acc = _UserAccountDal.GetAccByUserId(item.UserId);
                if (acc.LeftCount <= acc.RightCount)
                {
                    hongBaoSum += acc.LeftCount;
                }
                else
                {
                    hongBaoSum += acc.RightCount;
                }
            }

            //获取每个红包平均值
            double everyDate = Convert.ToDouble(gc.ConfigValue);
            double everyAge = everyDate / Convert.ToDouble(hongBaoSum) * 0.15;

            foreach (var item in accAllList)
            {
                UserInfo user = _UserDal.GetUserById(item.UserId);
                
                double userSum = 0;
                int hongbaoCount = 0;
                if (item.LeftAchievement <= item.RightAchievement)
                {
                    userSum += Convert.ToDouble(item.LeftCount) * everyAge;
                    hongbaoCount = item.LeftCount;
                }
                else
                {
                    userSum += Convert.ToDouble(item.RightCount) * everyAge;
                    hongbaoCount = item.RightCount;
                }
              //获取每人  积分 和绿养
                double userGreen = userSum * 0.9;
                double userScore = userSum * 0.1;

                //直推返利四层
                #region 直推返利四层
                GetUpFourUserAccount(user, userGreen, userScore, hongbaoCount);
            

                #endregion


                AccountInfo acc = new AccountInfo();
                acc = _UserAccountDal.GetAccByUserId(item.UserId);
                if (acc != null)
                {
                    acc.Score += Convert.ToDecimal(userScore);
                    acc.GreenCount += Convert.ToDecimal(userGreen);
                    acc.StaticsRelease += Convert.ToDecimal(userGreen);
                    acc.GreenTotal += Convert.ToDecimal(userSum);
                    bool isTrue = _UserAccountDal.UpdateAccInfo(acc);
                    if (!isTrue)
                    {
                        LogHelper.WriteInfo(typeof(StatusJob), "红包用户待释放资产失败！", Engineer.ccc, new { userid = item.UserId, userSum = userSum, userGreen = userGreen, userScore = userScore, sumUserCount = hongBaoSum, everyDate = everyDate, everyAge = everyAge });
                    }

                    //添加红包记录
                    HongBaoLog hongBaoLog = new HongBaoLog();
                    hongBaoLog.LogCount = Convert.ToDecimal(userGreen);
                    hongBaoLog.CreateTime = DateTime.Now;
                    hongBaoLog.HongBaoCount = hongbaoCount;
                    hongBaoLog.UserId = item.UserId;
                    hongBaoLog.LogType = 1;//1，自己业绩红包  2，返利4层
                    hongBaoLog.ReUserId = item.UserId;
                    bool isTrueStatus = _hongBaoLogDal.Insert(hongBaoLog);
                    if (!isTrueStatus)
                    {
                        LogHelper.WriteInfo(typeof(StatusJob), "红包日志失败！", Engineer.ccc, hongBaoLog);
                    }

                    //添加红包积分记录
                    ScoreLog scoreLog = new ScoreLog();
                    scoreLog.UserId = item.UserId;
                    scoreLog.Remark = "红包积分";
                    scoreLog.LogCount = Convert.ToDecimal(userScore);
                    scoreLog.CreateTime = DateTime.Now;

                    bool isTrueScore = _scoreLogDal.Insert(scoreLog);
                    if (!isTrueScore)
                    {
                        LogHelper.WriteInfo(typeof(StatusJob), "红包积分日志失败！", Engineer.ccc, scoreLog);
                    }
                }
            }

        }


        /// <summary>
        /// 获取直推四层账户  返利
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public void GetUpFourUserAccount(UserInfo user,double userGreen, double userScore,int hongbaoCount)
        {
            List<AccountInfo> list = new List<AccountInfo>();

            #region 第一层
            UserInfo user1 = _UserDal.GetParentIdByUserId(user.ParentId);
            if (user1 == null)
            {
                return;
            }
            AccountInfo acc1 = _UserAccountDal.GetAccByUserId(user1.UserId);
            decimal rebateScore1 = Convert.ToDecimal(userScore * 0.3);
            decimal rebateGreen1 = Convert.ToDecimal(userGreen * 0.3);
            acc1.Score += rebateScore1;
            acc1.GreenCount += rebateGreen1;
            acc1.GreenTotal += Convert.ToDecimal(rebateScore1 + rebateGreen1);

            bool isTrue = _UserAccountDal.UpdateAccInfo(acc1);
            if (!isTrue)
            {
                LogHelper.WriteInfo(typeof(StatusJob), "红包返利第一层失败！", Engineer.ccc, acc1);
            }
            //添加返利红包记录
            #region 添加返利红包记录
            HongBaoLog hongBaoLog = new HongBaoLog();
            hongBaoLog.LogCount = rebateGreen1;
            hongBaoLog.CreateTime = DateTime.Now;
            hongBaoLog.HongBaoCount = hongbaoCount;
            hongBaoLog.UserId = acc1.UserId;
            hongBaoLog.LogType = 2;//1，自己业绩红包  2，返利4层
            hongBaoLog.ReUserId = user.UserId;
            bool isTrueStatus = _hongBaoLogDal.Insert(hongBaoLog);
            if (!isTrueStatus)
            {
                LogHelper.WriteInfo(typeof(StatusJob), "红包日志失败！", Engineer.ccc, hongBaoLog);
            }


            #endregion
            //添加红包积分记录
            #region 返利红包记录
            ScoreLog scoreLog1 = new ScoreLog();
            scoreLog1.UserId = acc1.UserId;
            scoreLog1.Remark = "业绩推荐积分";
            scoreLog1.LogCount = rebateScore1;
            scoreLog1.CreateTime = DateTime.Now;

            bool isTrueScore = _scoreLogDal.Insert(scoreLog1);
            if (!isTrueScore)
            {
                LogHelper.WriteInfo(typeof(StatusJob), "红包积分日志失败！", Engineer.ccc, scoreLog1);
            }

            #endregion

            #endregion

            #region 第二层

            UserInfo user2 = _UserDal.GetParentIdByUserId(user1.ParentId);
            if (user2 == null)
            {
                return;
            }
            AccountInfo acc2 = _UserAccountDal.GetAccByUserId(user2.UserId);
            decimal rebateScore2 = rebateScore1 * Convert.ToDecimal(0.3);
            decimal rebateGreen2 = rebateGreen1 * Convert.ToDecimal(0.3);
            acc2.Score += rebateScore2;
            acc2.GreenCount += rebateGreen2;
            acc2.GreenTotal += Convert.ToDecimal(rebateScore2 + rebateGreen2);
            bool isTrue2 = _UserAccountDal.UpdateAccInfo(acc2);
            if (!isTrue2)
            {
                LogHelper.WriteInfo(typeof(StatusJob), "红包返利第二层失败！", Engineer.ccc, acc2);
            }


            //添加返利红包记录
            #region 添加返利红包记录
            HongBaoLog hongBaoLog2 = new HongBaoLog();
            hongBaoLog2.LogCount = Convert.ToDecimal(rebateGreen2);
            hongBaoLog2.CreateTime = DateTime.Now;
            hongBaoLog2.HongBaoCount = hongbaoCount;
            hongBaoLog2.UserId = user2.UserId;
            hongBaoLog2.LogType = 2;//1，自己业绩红包  2，返利4层
            hongBaoLog2.ReUserId = user.UserId;
            bool isTrueStatus2 = _hongBaoLogDal.Insert(hongBaoLog2);
            if (!isTrueStatus2)
            {
                LogHelper.WriteInfo(typeof(StatusJob), "业绩推荐日志失败！", Engineer.ccc, hongBaoLog2);
            }


            #endregion
            //添加红包积分记录
            #region 返利红包记录
            ScoreLog scoreLog2 = new ScoreLog();
            scoreLog2.UserId = acc2.UserId;
            scoreLog2.Remark = "业绩推荐积分";
            scoreLog2.LogCount = rebateScore2;
            scoreLog2.CreateTime = DateTime.Now;

            bool isTrueScore2 = _scoreLogDal.Insert(scoreLog2);
            if (!isTrueScore2)
            {
                LogHelper.WriteInfo(typeof(StatusJob), "推荐积分日志失败！", Engineer.ccc, scoreLog2);
            }

            #endregion


            #endregion

            #region 第三层

            UserInfo user3 = _UserDal.GetParentIdByUserId(user2.ParentId);
            if (user3 == null)
            {
                return;
            }
            AccountInfo acc3 = _UserAccountDal.GetAccByUserId(user3.UserId);
            decimal rebateScore3 = rebateScore2 * Convert.ToDecimal(0.3);
            decimal rebateGreen3 = rebateGreen2 * Convert.ToDecimal(0.3);
            acc3.Score += rebateScore3;
            acc3.GreenCount += rebateGreen3;
            acc3.GreenTotal += Convert.ToDecimal(rebateScore3 + rebateGreen3);
            bool isTrue3 = _UserAccountDal.UpdateAccInfo(acc3);
            if (!isTrue3)
            {
                LogHelper.WriteInfo(typeof(StatusJob), "业绩返利第三层失败！", Engineer.ccc, acc3);
            }



            //添加返利红包记录
            #region 添加返利红包记录
            HongBaoLog hongBaoLog3 = new HongBaoLog();
            hongBaoLog3.LogCount = Convert.ToDecimal(userGreen);
            hongBaoLog3.CreateTime = DateTime.Now;
            hongBaoLog3.HongBaoCount = hongbaoCount;
            hongBaoLog3.UserId = user3.UserId;
            hongBaoLog3.LogType = 2;//1，自己业绩红包  2，返利4层
            hongBaoLog3.ReUserId = user.UserId;
            bool isTrueStatus3 = _hongBaoLogDal.Insert(hongBaoLog3);
            if (!isTrueStatus3)
            {
                LogHelper.WriteInfo(typeof(StatusJob), "业绩返利第三层红包日志失败！", Engineer.ccc, hongBaoLog3);
            }


            #endregion
            //添加红包积分记录
            #region 返利红包记录
            ScoreLog scoreLog3 = new ScoreLog();
            scoreLog3.UserId = acc3.UserId;
            scoreLog3.Remark = "业绩推荐积分";
            scoreLog3.LogCount = Convert.ToDecimal(userScore);
            scoreLog3.CreateTime = DateTime.Now;

            bool isTrueScore3 = _scoreLogDal.Insert(scoreLog3);
            if (!isTrueScore3)
            {
                LogHelper.WriteInfo(typeof(StatusJob), "业绩积分日志失败！", Engineer.ccc, scoreLog3);
            }

            #endregion
            #endregion

            #region 第四层

            UserInfo user4 = _UserDal.GetParentIdByUserId(user3.ParentId);
            if (user4 == null)
            {
                return;
            }
            AccountInfo acc4 = _UserAccountDal.GetAccByUserId(user4.UserId);

            decimal rebateScore4 = rebateScore3 * Convert.ToDecimal(0.3);
            decimal rebateGreen4 = rebateGreen3 * Convert.ToDecimal(0.3);
            acc4.Score += rebateScore4;
            acc4.GreenCount += rebateGreen4;
            acc4.GreenTotal += Convert.ToDecimal(rebateScore4 + rebateGreen4);
            bool isTrue4 = _UserAccountDal.UpdateAccInfo(acc3);
            if (!isTrue4)
            {
                LogHelper.WriteInfo(typeof(StatusJob), "红包返利第一层失败！", Engineer.ccc, acc4);
            }

            //添加返利红包记录
            #region 添加返利红包记录
            HongBaoLog hongBaoLog4 = new HongBaoLog();
            hongBaoLog4.LogCount = Convert.ToDecimal(userGreen);
            hongBaoLog4.CreateTime = DateTime.Now;
            hongBaoLog4.HongBaoCount = hongbaoCount;
            hongBaoLog4.UserId = acc4.UserId;
            hongBaoLog4.LogType = 2;//1，自己业绩红包  2，返利4层
            hongBaoLog4.ReUserId = user.UserId;
            bool isTrueStatus4 = _hongBaoLogDal.Insert(hongBaoLog4);
            if (!isTrueStatus4)
            {
                LogHelper.WriteInfo(typeof(StatusJob), "第4层返利推荐失败！", Engineer.ccc, hongBaoLog4);
            }


            #endregion
            //添加红包积分记录
            #region 返利红包记录
            ScoreLog scoreLog4 = new ScoreLog();
            scoreLog4.UserId = acc4.UserId;
            scoreLog4.Remark = "业绩推荐积分";
            scoreLog4.LogCount = Convert.ToDecimal(userScore);
            scoreLog4.CreateTime = DateTime.Now;

            bool isTrueScore4 = _scoreLogDal.Insert(scoreLog4);
            if (!isTrueScore4)
            {
                LogHelper.WriteInfo(typeof(StatusJob), "业绩推荐积分日志失败！", Engineer.ccc, scoreLog4);
            }

            #endregion 
            #endregion
          
            
        }

    }
}