using Common;
using Dal;
using DataModel;
using DataModel.ViewModel;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpService
{
   public class UpdateLoginStatusJob:IJob
    {

       UserDal _userDal = new UserDal();

       UserAccountDal _accountDal = new UserAccountDal();

        //public void Execute(JobExecutionContext context)
        //{
        //    bool result = false;
        //  //获取所有用户（已激活，类型为1，已启用）
        //    List<UserInfo> list = new List<UserInfo>();

        //    list = _userDal.GetAllActivationUser();

        //    if (list!=null&&list.Count>0)
        //    {

        //        //将今日登陆状态写入到昨日登陆状态，并重置  并且清空动静态待分配
        //        foreach (var item in list)
        //        {

        //            item.YesTodayIsLogin = item.TodayIsLogin;
        //            item.TodayIsLogin = 0;

        //            result = _userDal.UpdateUserInfo(item);

        //            AccountInfo account = _accountDal.GetAccByUserId(item.UserId);

        //            account.WaitRelease = 0;
        //            account.StaticsRelease = 0;

        //            result = _accountDal.UpdateAccInfo(account);

        //            if (!result)
        //            {
        //                LogHelper.WriteInfo(typeof(UpdateLoginStatusJob), "重置登陆状态失败，用户ID：" + item.UserId);
        //                continue;
        //            }

        //        }
           
        //    }


        //}
    }
}
