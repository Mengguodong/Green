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
    public class ScoreLogBll
    {
        ScoreLogDal _ScoreLogDal = new ScoreLogDal();
        public bool Insert(ScoreLog scoreLog)
        {
            return _ScoreLogDal.Insert(scoreLog);
        }
        public Page<ScoreLog> GetScoreLog(int userId, int pageIndex, int pageSize)
        {
            return _ScoreLogDal.GetScoreLog(userId, pageIndex, pageSize);
        }


        public Page<ScoreLog> AdminGetScoreLog(int userId, int pageIndex, int pageSize)
        {
            return _ScoreLogDal.AdminGetScoreLog(userId, pageIndex, pageSize);
        }
    }
}
