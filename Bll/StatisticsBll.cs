using Dal;
using DataModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
   public class StatisticsBll
    {
       TiXianLogDal _tixian = new TiXianLogDal();
       UserDal _userDal = new UserDal();
       UserAccountDal _accountDal = new UserAccountDal();
        
        public StatisticsViewModel GetStatisticsModel(DateTime? begin, DateTime? end)
        {
            StatisticsViewModel model = new StatisticsViewModel();

            //获取充值总值
            model.SumMoney = _userDal.GetSumMoney(begin, end);
            // 获取总积分和总绿氧
            model.SumGreenScore=  _accountDal.GetSumGreenAndScore(null,null);
            //总激活人数
            model.TotalActivation = _userDal.GetAllActivationUser().Count;
            //用户总数
            model.TotalUser = _userDal.GetAllUser().Count;
            //总资产
            model.TotalAssets = _accountDal.GetTotalAssets(null, null);
            //总提现
            model.SumOutMoney = _tixian.SumTiXian(begin, end);

            return model;

        }
    }
}
