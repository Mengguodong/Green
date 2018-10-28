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
    public class StatusLogBll
    {
        StatusLogDal _StatusLogDal = new StatusLogDal();
        public bool Insert(StatusLog statusLog)
        {
            return _StatusLogDal.Insert(statusLog);
        }


        public Page<StatusLog> GetStatusLog(int userId, int pageIndex, int pageSize, int type)
        {
            if (type == 3)
            {
                type = 1;
            }

            return _StatusLogDal.GetStatusLog(userId, pageIndex, pageSize, type);
        }

        public Page<StatusLog> AdminGetStatusLog(int userId, int pageIndex, int pageSize, int type)
        {
            if (type == 3)
            {
                type = 1;
            }

            return _StatusLogDal.AdminGetStatusLog(userId, pageIndex, pageSize, type);
        }
    }
}
