﻿using EveryDayEpService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace StatusService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
             QuartzManaer.GetInstance().StartQuartz();
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new StatusService()
            //};
            //ServiceBase.Run(ServicesToRun);
        }
    }
}
