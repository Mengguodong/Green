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
    public class StatusJob : IJob
    {
        UserDal _UserDal = new UserDal();
        GlobalConfigDal _GlobalConfigDal = new GlobalConfigDal();
        UserAccountDal _UserAccountDal = new UserAccountDal();
        StatusLogDal _statusLogDal = new StatusLogDal();
        ScoreLogDal _scoreLogDal = new ScoreLogDal();

        public void Execute(JobExecutionContext context)
        {
           
            List<UserInfo> list = new List<UserInfo>();
            GlobalConfig gc = _GlobalConfigDal.GetGlobalConfig("EveryDate");

            list =  _UserDal.GetAllUser();

            int sumUserCount = 0;
            foreach (var item in list)
            {
                sumUserCount += item.Level;
            }
           double everyDate =Convert.ToDouble(gc.ConfigValue);
            double everyAge = everyDate/ Convert.ToDouble(sumUserCount) * 0.2;

            foreach (var item in list)
            {
                double userSum = Convert.ToDouble(item.Level) * everyAge;
                double userGreen = userSum * 0.9;
                double userScore = userSum * 0.1;
                AccountInfo acc = new AccountInfo();
                acc= _UserAccountDal.GetAccByUserId(item.UserId);
                if (acc != null)
                {
                    acc.Score += Convert.ToDecimal(userScore); 
                    acc.GreenCount += Convert.ToDecimal(userGreen);
                    acc.StaticsRelease += Convert.ToDecimal(userGreen);
                    acc.GreenTotal += Convert.ToDecimal(userSum);
                   bool isTrue = _UserAccountDal.UpdateAccInfo(acc);
                    if (!isTrue)
                    {
                        LogHelper.WriteInfo(typeof(StatusJob), "静态修改用户待释放资产失败！", Engineer.ccc, new{userid=item.UserId, userSum= userSum, userGreen= userGreen, userScore= userScore, sumUserCount= sumUserCount , everyDate = everyDate , everyAge = everyAge });
                    }

                    //添加静态绿养记录
                    StatusLog statusLog = new StatusLog();
                    statusLog.LogCount= Convert.ToDecimal(userGreen);
                    statusLog.CreateTime = DateTime.Now;
                    statusLog.LogType = 1;//1,静态  2，直推
                    statusLog.UserId = item.UserId;
                    bool isTrueStatus = _statusLogDal.Insert(statusLog);
                    if (!isTrueStatus)
                    {
                        LogHelper.WriteInfo(typeof(StatusJob), "静态绿养日志失败！", Engineer.ccc, statusLog);
                    }

                    //添加静态积分记录
                    ScoreLog scoreLog = new ScoreLog();
                    scoreLog.UserId = item.UserId;
                    scoreLog.Remark = "静态积分";
                    scoreLog.LogCount = Convert.ToDecimal(userScore);
                    scoreLog.CreateTime = DateTime.Now;

                    bool isTrueScore = _scoreLogDal.Insert(scoreLog);
                    if (!isTrueScore)
                    {
                        LogHelper.WriteInfo(typeof(StatusJob), "静态积分日志失败！", Engineer.ccc, scoreLog);
                    }
                }

                ////清除今日新增
                //gc.ConfigValue = "0";
                //bool isTrueGc= _GlobalConfigDal.UpdateGlobalConfig(gc);
                //if (!isTrueGc)
                //{
                //    LogHelper.WriteInfo(typeof(StatusJob), "清除今日新增失败！", Engineer.ccc, gc);
                //}

            }




        }
    }
}
