using Dal;
using DataModel;
using DataModel.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
   public class HongBaoLogBll
    {
        HongBaoLogDal _hongDal = new HongBaoLogDal();
        public bool Insert(HongBaoLog hongBaoLog)
        {
            return _hongDal.Insert(hongBaoLog);
        }


        public Page<HongBaoLog> GetHongBaoLog(int userId, int pageIndex, int pageSize, int type)
        {
            if (type == 3)
            {
                type = 1;
            }

            return _hongDal.GetHongBaoLog(userId, pageIndex, pageSize, type);
        }

        public Page<HongBaoLog> AdminGetHongBaoLog(int userId, int pageIndex, int pageSize, int type)
        {
            return _hongDal.AdminGetHongBaoLog(userId, pageIndex, pageSize, type);
        }
    }
}
