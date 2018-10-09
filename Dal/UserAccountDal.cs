using Common;
using DapperEx;
using Database;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DataModel;

namespace Dal
{
    public class UserAccountDal
    {
        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public UserAccount GetAccountByLoginName(string loginName)
        {
            UserAccount userInfo = null;
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    userInfo = db.Query<UserAccount>(SqlQuery<UserAccount>.Builder(db).AndWhere(x => x.LoginName, OperationMethod.Equal, loginName)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(UserAccountDal), "GetAccountByLoginName", Engineer.ggg, new { LoginName = loginName }, ex);
            }
            return userInfo;

        }
    }
}
