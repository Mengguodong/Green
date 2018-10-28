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
   public  class CleanDataJob : IJob
    {
        GlobalConfigDal _GlobalConfigDal = new GlobalConfigDal();
        UserDal userDal = new UserDal();
        UserAccountDal accDal = new UserAccountDal();
        public void Execute(JobExecutionContext context)
        {
            //每日新增清0
            GlobalConfig gc = _GlobalConfigDal.GetGlobalConfig("EveryDate");
            //清除今日新增
            gc.ConfigValue = "0";
            bool isTrueGc = _GlobalConfigDal.UpdateGlobalConfig(gc);
            if (!isTrueGc)
            {
                LogHelper.WriteInfo(typeof(CleanDataJob), "清除今日新增失败！", Engineer.ccc, gc);
            }

            //每日业绩红包清0  左右区红包个数
           bool isTrue = accDal.CleanHongBao();
            if (!isTrue)
            {
                LogHelper.WriteInfo(typeof(CleanDataJob), "清除左右区新增红包失败！", Engineer.ccc, new { });
            }


        }
    }
}
