using Common;
using EveryDayEpService;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
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
        private string cronTriggerLambda = "00 00 01 * * ?";//每天23点执行时间,"秒 分 时 天 月 年"

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

                JobDetail jobEveryDayEpJob = new JobDetail("job1", "group1", typeof(EveryDayEpJob));//IndexJob为实现了IJob接口的类
                DateTime tsEveryDayEpJob = TriggerUtils.GetNextGivenSecondDate(null, 0);//7天以后第一次运行
                Trigger triggertEveryDayEpJob = new CronTrigger("jobUpdateLoginStatus", "group1", "job1", "group1", tsEveryDayEpJob, null,
                                                        cronTriggerLambda);//每天23点执行时间,"秒 分 时 天 月 年"



                sched.AddJob(jobEveryDayEpJob, true);
                sched.ScheduleJob(triggertEveryDayEpJob);
                sched.Start();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(QuartzManaer), "QuartzManaer异常",Engineer.ccc,null,ex);
            }

           
        }

    }
}


