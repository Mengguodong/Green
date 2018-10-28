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
    public class CheckReCastJob : IJob
    {
        UserDal userDal = new UserDal();
        UserAccountDal accDal = new UserAccountDal();
        public void Execute(JobExecutionContext context)
        {
            List<UserInfo> userList =  userDal.GetAllUser();

            foreach (var item in userList)
            {
                AccountInfo acc = accDal.GetAccByUserId(item.UserId);
                if (acc.GreenTotal >= (item.OutNum * 4))
                {
                    item.IsActivation = 0;
                   bool isTrue = userDal.UpdateUserInfo(item);
                    if (!isTrue)
                    {
                        LogHelper.WriteInfo(typeof(CheckReCastJob), "清理激活失败！", Engineer.ccc, acc);
                    }
                }

            }
        }
    }
}
