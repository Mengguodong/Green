using Common;
using OrderStatusService;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LevelUpService
{
    /// <summary>
    /// 定时任务.
    /// </summary>
    public sealed class QuartzManaer
    {
        private static readonly QuartzManaer quartzManager = new QuartzManaer();
      //  private string cronTriggerLambda = "59 59 23 * * ?";//每天23点执行时间,"秒 分 时 天 月 年"

        private QuartzManaer()
        {
          //  cronTriggerLambda = Auxiliary.ConfigKey("CronTriggerLambda");
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


                //====================每隔1小时执行任务===================
                JobDetail jobOrderStatus = new JobDetail("job1", "group1", typeof(OrderStatusJob));//IndexJob为实现了IJob接口的类
                DateTime tsOrderStatus = TriggerUtils.GetNextGivenSecondDate(null, 0);//7天以后第一次运行
                TimeSpan intervaOrderStatus = TimeSpan.FromMinutes(5);//每隔1小时执行任务
                Trigger triggertOrderStatus = new SimpleTrigger("jobOrderStatus", "group1", "job1", "group1", tsOrderStatus, null,
                                                        SimpleTrigger.RepeatIndefinitely, intervaOrderStatus);//每若干小时运行一次，小时间隔由appsettings中的IndexIntervalHour参数指定

                ////========最后一次=======================
                //JobDetail jobUpdateLoginStatus = new JobDetail("job1", "group1", typeof(UpdateLoginStatusJob));//IndexJob为实现了IJob接口的类
                //DateTime tsUpdateLoginStatus = TriggerUtils.GetNextGivenSecondDate(null, 0);//7天以后第一次运行
                //Trigger triggertUpdateLoginStatus = new CronTrigger("jobUpdateLoginStatus", "group1", "job1", "group1", tsUpdateLoginStatus, null,
                //                                        cronTriggerLambda);//每天23点执行时间,"秒 分 时 天 月 年"



                sched.AddJob(jobOrderStatus, true);
                sched.ScheduleJob(triggertOrderStatus);
                sched.Start();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(QuartzManaer), "QuartzManaer异常",Engineer.ccc,null,ex);
            }

           
        }

    }
}


