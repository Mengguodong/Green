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
    public class TiXianLogBll
    {
        TiXianLogDal _TiXianLogDal = new TiXianLogDal();
        public bool Insert(TiXianLog tiXianLog)
        {
            return _TiXianLogDal.Insert(tiXianLog);
        }

        public Page<TiXianLog> GetScoreLog(int userId, int pageIndex, int pageSize)
        {
            return _TiXianLogDal.GetTiXianLog(userId, pageIndex, pageSize);
        }

    }
}
