using Common;
using EveryDayEpService;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using StatusService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EveryDayEpService
{
    /// <summary>
    /// 定时任务.
    /// </summary>
    public sealed class QuartzManaer
    {
        private static readonly QuartzManaer quartzManager = new QuartzManaer();
        private string cronTriggerLambda = "00 03 18 * * ?";//每天23点执行时间,"秒 分 时 天 月 年"
        private string cronTriggerLambdaHongBao = "00 10 18 * * ?";//每天23点执行时间,"秒 分 时 天 月 
        private string cronTriggerLambdaCheckReCast = "00 27 18 * * ?";
        private string cronTriggerLambdaCleanData = "00 31 18 * * ?";

        private QuartzManaer()
        {
          //cronTriggerLambda = Auxiliary.ConfigKey("CronTriggerLambda");
        }

        public static QuartzManaer GetInstance()
        {
            return quartzManager;
        }
        public void StartQuartz()
        {
            try
            {
                LogHelper.WriteInfo(typeof(QuartzManaer), "QuartzManaer执行");

               
                IScheduler sched;
                ISchedulerFactory sf = new StdSchedulerFactory();
                sched = sf.GetScheduler();



                #region 每日静态

                JobDetail jobEveryDayJob = new JobDetail("job1", "group1", typeof(StatusJob));//IndexJob为实现了IJob接口的类
                DateTime tsEveryDayJob = TriggerUtils.GetNextGivenSecondDate(null, 0);//7天以后第一次运行
                Trigger triggertEveryDayJob = new CronTrigger("jobEveryDayJob", "group1", "job1", "group1", tsEveryDayJob, null,
                                                        cronTriggerLambda);//每天23点执行时间,"秒 分 时 天 月 年"
                #endregion

                // 红包      1--循环取左右最小值，累加总数，当日新增业绩15 % 除以总数得平均值，平均值乘以每人最小值，修改账户，添加记录LogType类型为1--
                //2,直推红包 向上返N层，修改账户，添加记录LogType类型为2
                #region //红包      1--循环取左右最小值，累加总数，当日新增业绩15%  除以总数得平均值，平均值乘以每人最小值，修改账户，添加记录LogType类型为1 --2,直推红包 向上返4层，修改账户，添加记录LogType类型为2



                //JobDetail jobHongBaoJob = new JobDetail("job2", "group2", typeof(HongBaoEveryJob));//IndexJob为实现了IJob接口的类
                //DateTime tsHongBaoJob = TriggerUtils.GetNextGivenSecondDate(null, 0);//7天以后第一次运行
                //Trigger triggertHongBaoJob = new CronTrigger("jobHongBaoJob", "group2", "job2", "group2", tsHongBaoJob, null,
                //                                        cronTriggerLambdaHongBao);//每天23点执行时间,"秒 分 时 天 月 年"



                #endregion
                #region 每日复投检查

                //JobDetail jobCheckReCastJob = new JobDetail("job3", "group3", typeof(CheckReCastJob));//IndexJob为实现了IJob接口的类
                //DateTime tsCheckReCastJob = TriggerUtils.GetNextGivenSecondDate(null, 0);//7天以后第一次运行
                //Trigger triggertCheckReCastJob = new CronTrigger("jobCheckReCastJob", "group3", "job3", "group3", tsCheckReCastJob, null,
                //                                        cronTriggerLambdaCheckReCast);//每天23点执行时间,"秒 分 时 天 月 年"
                #endregion


                #region 每日新增清0

                JobDetail jobCleanDataJob = new JobDetail("job4", "group4", typeof(CleanDataJob));//IndexJob为实现了IJob接口的类
                DateTime tsCleanDataJob = TriggerUtils.GetNextGivenSecondDate(null, 0);//7天以后第一次运行
                Trigger triggertCleanDataJob = new CronTrigger("jobCleanDataJob", "group4", "job4", "group4", tsCleanDataJob, null,
                                                        cronTriggerLambdaCleanData);//每天23点执行时间,"秒 分 时 天 月 年"
                #endregion

                //每日静态
                //sched.AddJob(jobEveryDayJob, true);
                //sched.ScheduleJob(triggertEveryDayJob);

                //红包
                //sched.AddJob(jobHongBaoJob, true);
                //sched.ScheduleJob(triggertHongBaoJob);

                ////每日复投检查
                //sched.AddJob(jobCheckReCastJob, true);
                //sched.ScheduleJob(triggertCheckReCastJob);

                ////每日新增清0
                sched.AddJob(jobCleanDataJob, true);
                sched.ScheduleJob(triggertCleanDataJob);

                sched.Start();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(QuartzManaer), "QuartzManaer异常",Engineer.ccc,null,ex);
            }

           
        }

    }
}


