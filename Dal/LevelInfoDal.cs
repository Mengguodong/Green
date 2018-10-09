using Common;
using DapperEx;
using Database;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Dal
{
    public class LevelInfoDal
    {
        /// <summary>
        /// 根据用户id查账户实体
        /// sj
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public LevelInfo GetLevelInfoById(int levelStatus)
        {
            LevelInfo data = new LevelInfo();
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    data = db.Query<LevelInfo>(SqlQuery<LevelInfo>.Builder(db).AndWhere(x => x.LevelStatus, OperationMethod.Equal, levelStatus)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(typeof(LevelInfoDal), ex);
                data = null;
            }
            return data;
        }




        /// <summary>
        /// 修改账户  
        /// sj
        /// </summary>
        /// <param name="customerAccount">修改账户实体</param>
        /// <returns></returns>
        public bool UpdateLevelInfo(LevelInfo levelInfo)
        {

            bool result = false;
            try
            {
                if (levelInfo != null)
                {
                    using (var db = BaseDal.WriteSanNongDunDbBase())
                    {
                        result = db.Update(levelInfo);
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(LevelInfoDal), "UpdateLevelInfo", Engineer.ccc, levelInfo, ex);
            }
            return result;
        }
    }
}
