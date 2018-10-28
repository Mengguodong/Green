using Common;
using LevelUpService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace OrderStatusService
{
    public partial class OrderStatusService : ServiceBase
    {
        public OrderStatusService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {

                LogHelper.WriteInfo(typeof(OrderStatusService), "服务OnStart函数开始执行");
                QuartzManaer.GetInstance().StartQuartz();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(OrderStatusService), "OnStart", Engineer.ccc, null, ex);
                var sc = new ServiceController("WineGameService");
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                }
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);

                LogHelper.WriteLog(typeof(OrderStatusService), "OnStart", Engineer.ccc, new { Status = sc.Status }, ex);
            }
            LogHelper.WriteInfo(typeof(OrderStatusService), "服务WineGameService函数：OnStart执行结束");
        }

        protected override void OnStop()
        {
            LogHelper.WriteInfo(typeof(OrderStatusService), "服务OnStop函数结束执行");
        }
    }
}
