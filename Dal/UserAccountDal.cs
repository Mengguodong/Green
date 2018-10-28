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

using System.Data;
using DataModel.RequestModel;
using DataModel.ViewModel;
using EveryDayEpService;

namespace Dal
{
    public class UserAccountDal : BaseDal
    {
        public bool CleanHongBao()
        {
         
            bool result = false;
          string sql="update AccountInfo set LeftCount = 0, RightCount = 0";
            try
            {
                using (var db = WriteSanNongDunDbBase())
                {
                    result = db.DbConnecttion.Execute(sql,Engineer.ccc,null,null,null,null)>0;
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(UserAccountDal), "InsertCustomerAccount", Engineer.ggg, null, ex);
            }
            return result;

           
        }


        /// <summary>
        /// 获取所有账户
        /// </summary>
        /// <returns></returns>
        public List<AccountInfo> GetAllAcc()
        {
            List<AccountInfo> list = new List<AccountInfo>();
            string sql = @"select * from AccountInfo as a inner join UserInfo as u on u.UserId=a.UserId where u.UserType=1 and u.IsActivation=1 and u.UserStatus=1 and u.Level>0 ;";
            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    list = db.DbConnecttion.Query<AccountInfo>(sql, Engineer.ggg).ToList();

                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(UserAccountDal), "GetAllAcc", Engineer.ggg, null, ex);
            }
            return list;
            
        }

        /// <summary>
        /// 插入账户实体
        /// </summary>
        /// <param name="customerAccount"></param>
        /// <returns></returns>
        public bool AddCustomerAccount(AccountInfo customerAccount)
        {
            bool result = false;

            try
            {
                using (var db = WriteSanNongDunDbBase())
                {
                    result = db.Insert<AccountInfo>(customerAccount);
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(UserAccountDal), "InsertCustomerAccount", Engineer.ggg, customerAccount, ex);
            }
            return result;

        }

        /// <summary>
        /// 用户id查实体
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AccountInfo GetAccByUserId(int userId)
        {
            AccountInfo acc = null;

            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    acc = db.Query<AccountInfo>(SqlQuery<AccountInfo>.Builder(db).AndWhere(x => x.UserId, OperationMethod.Equal, userId)).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(UserAccountDal), "GetAccByUserId", Engineer.ccc, new { userId = userId }, ex);
            }
            return acc;
        }
        /// <summary>
        /// 用户id查实体
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AccountInfo GetAccByAccId(int accId)
        {
            AccountInfo acc = null;

            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    acc = db.Query<AccountInfo>(SqlQuery<AccountInfo>.Builder(db).AndWhere(x => x.AccountId, OperationMethod.Equal, accId)).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(UserAccountDal), "GetAccByAccId", Engineer.ccc, new { accId = accId }, ex);
            }
            return acc;
        }

        /// <summary>
        /// 用户账户地址查实体
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public AccountInfo GetAccByAccName(string accName)
        //{
        //    AccountInfo acc = null;
        //    try
        //    {
        //        using (var db = ReadOnlySanNongDunConn())
        //        {

        //            acc = db.Query<AccountInfo>(SqlQuery<AccountInfo>.Builder(db).AndWhere(x => x.AccountName, OperationMethod.Equal, accName)).FirstOrDefault();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog(typeof(UserAccountDal), "GetAccByAccName", Engineer.ccc, new { accName = accName }, ex);
        //    }
        //    return acc;
        //}


        /// <summary>
        /// 修改账户信息
        /// </summary>
        /// <param name="accInfo"></param>
        /// <returns></returns>

        public bool UpdateAccInfo(AccountInfo accInfo)
        {
            bool isTrue = false;
            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    isTrue = db.Update<AccountInfo>(accInfo);
                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(UserAccountDal), "", Engineer.ccc, accInfo, ex);
            }

            return isTrue;
        }


        ///// <summary>
        ///// 检查账户余额
        ///// </summary>
        ///// <param name="Qty"></param>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public bool CheckAccountBalance(int Qty, int type, int userId)
        //{
        //    bool result = false;

        //    AccountInfo account = new AccountInfo();

        //    try
        //    {
        //        using (var db = ReadOnlySanNongDunConn())
        //        {
        //            account = db.Query<AccountInfo>(SqlQuery<AccountInfo>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId)).FirstOrDefault();

        //        }
        //        if (type == 1) //卖EP
        //        {
        //            result = account.Ep >= Qty + 100; //保证金100ep
        //        }
        //        else//卖Zfc
        //        {
        //            result = account.Zfc >= Qty && account.Ep >= 100;//保证金100ep
        //        }



        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserAccountDal), "CheckAccountBalance", Engineer.ggg, new { Qty = Qty, type = type, userId = userId }, ex);
        //    }
        //    return result;

        //}
        /// <summary>
        /// 冻结金额
        /// </summary>
        /// <param name="Qty"></param>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public bool FreezeAccount(int Qty, int type, int userId, int IsFreeze)
        //{

        //    bool result = false;
        //    try
        //    {
        //        using (var db = ReadOnlySanNongDunConn())
        //        {

        //            var account = db.Query<AccountInfo>(SqlQuery<AccountInfo>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, userId)).FirstOrDefault();

        //            if (type == 1)
        //            {

        //                if (IsFreeze == 1)  //冻结  （100ep为保证金）
        //                {
        //                    account.Ep -= Qty + 100;
        //                    account.FreezeEp += Qty + 100;
        //                }
        //                else         //释放
        //                {
        //                    account.FreezeEp -= Qty + 100;
        //                    account.Ep += Qty + 100;
        //                }

        //            }
        //            else
        //            {

        //                if (IsFreeze == 1) //冻结  （100ep为保证金）
        //                {
        //                    account.Zfc -= Qty;
        //                    account.Ep -= 100;
        //                    account.FreezeZfc += Qty;
        //                    account.FreezeEp += 100;
        //                }
        //                else       //释放
        //                {
        //                    account.FreezeZfc -= Qty;
        //                    account.FreezeEp -= 100;
        //                    account.Ep += 100;
        //                    account.Zfc += Qty;
        //                }

        //            }

        //            result = db.Update<AccountInfo>(account);

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserAccountDal), "FreezeAccount", Engineer.ggg, new { Qty = Qty, type = type, userId = userId, IsFreeze = IsFreeze }, ex);
        //    }

        //    return result;
        //}
        /// <summary>
        /// 获取总资产（总部统计）
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalAssets(DateTime? begin, DateTime? end)
        {
            decimal TotalAssets = 0;
        
            string strWhere = "";
          
                if (begin != null && end != null)
                {
                    strWhere = " createtime >=@begin and createtime <=@end  ";
                }
                else if (begin == null && end != null)
                {
                    strWhere = " createtime <=@end  ";
                }
                else if (begin != null && end == null)
                {
                    strWhere = " createtime >=@begin  ";
                }
                else
                {
                    strWhere = " 1=1 ";
                }
                try
                {
                    string sql = "select ISNULL( SUM(GreenTotal),0)  from  AccountInfo where" + strWhere;

                    using (var db = ReadOnlySanNongDunConn())
                    {
                        TotalAssets = db.DbConnecttion.ExecuteScalar<decimal>(sql, Engineer.ggg, new { begin = begin, end = end });
                    }
                }
                catch (Exception ex)
                {

                    LogHelper.WriteLog(typeof(UserAccountDal), "GetTotalAssets", Engineer.ggg, null, ex);
                }

                return TotalAssets;
            }
       
        /// <summary>
        /// 获取所有激活用户总资产
        /// </summary>
        /// <returns></returns>
        public List<AccountInfoModel> GetUserTotalAssets()
        {
            List<AccountInfoModel> list = new List<AccountInfoModel>();
            string sql = @"select a.AccountId,a.TotalAssets,u.UserId,u.level from AccountInfo as a inner join UserInfo as u on u.UserId=a.UserId where u.UserType=1 and u.IsActivation=1 and u.UserStatus=1 ;";
            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    list = db.DbConnecttion.Query<AccountInfoModel>(sql, Engineer.ggg).ToList();

                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(UserAccountDal), "GetUserTotalAssets", Engineer.ggg, null, ex);
            }
            return list;

        }

        /// <summary>
        /// 创建激活卡记录
        /// </summary>
        /// <param name="activTable"></param>
        /// <returns></returns>
        //public bool AddActivTable(ActivationTable activTable, DbBase db, IDbTransaction dbTransaction)
        //{
        //    bool result = false;

        //    try
        //    {
        //        result = db.Insert<ActivationTable>(activTable, dbTransaction);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog(typeof(UserAccountDal), "AddActivTable", Engineer.ggg, activTable, ex);
        //    }
        //    return result;
        //}

        /// <summary>
        /// 使用激活卡流程
        /// </summary>
        /// <param name="activationUserName"></param>
        /// <returns></returns>
        //public bool ActivationCard(string activationUserName, UserInfo userInfo, UserInfo reUser, AccountInfo accInfo, AccountInfo reAccInfo)
        //{
        //    bool isTransaction = false;
        //    //事务处理激活流程
        //    using (DbBase dbBase = BaseDal.WriteSanNongDunDbBase())
        //    {
        //        IDbTransaction dbTransaction = dbBase.DbConnecttion.BeginTransaction();

        //        //1.修改激活账号的激活状态
        //        isTransaction = ActivationUser(reUser, dbBase, dbTransaction);
        //        if (!isTransaction)
        //        {
        //            dbTransaction.Rollback();
        //            return false;
        //        }
        //        //2.减掉激活卡数量
        //        accInfo.ActivationCount = accInfo.ActivationCount - 1;
        //        isTransaction = UpdateActivationCount(accInfo, dbBase, dbTransaction);
        //        if (!isTransaction)
        //        {
        //            dbTransaction.Rollback();
        //            return false;
        //        }

        //        //3.添加激活执行人的总资产 1200 * 9，剪掉一张激活卡  
        //        accInfo.TotalAssets += 1200 * 9;
        //        isTransaction = UpdateTotalAssets(accInfo, dbBase, dbTransaction);
        //        if (!isTransaction)
        //        {
        //            dbTransaction.Rollback();
        //            return false;
        //        }
        //        //4.添加激活账号总资产  2780 * 7 
        //        reAccInfo.TotalAssets += 2780 * 7;
        //        isTransaction = UpdateTotalAssets(reAccInfo, dbBase, dbTransaction);
        //        if (!isTransaction)
        //        {
        //            dbTransaction.Rollback();
        //            return false;
        //        }
        //        //5.添加激活记录
        //        ActivationTable at = new ActivationTable();
        //        at.CreateTime = DateTime.Now;
        //        at.UserId = userInfo.UserId;
        //        at.ReUserId = reUser.UserId;
        //        at.Remark = "使用激活卡";
        //        at.ActivationType = 1;
        //        at.ReUserName = reUser.UserName;
        //        isTransaction = AddActivTable(at, dbBase, dbTransaction);
        //        if (!isTransaction)
        //        {
        //            dbTransaction.Rollback();
        //            return false;
        //        }

        //        dbTransaction.Commit();
        //    }

        //    return isTransaction;
        //}

        /// <summary>
        /// 激活用户
        /// </summary>
        /// <returns></returns>
        public bool ActivationUser(UserInfo reUserInfo, DbBase db, IDbTransaction dbTransaction)
        {
            bool isTrue = false;
            try
            {
                string sql = "update UserInfo set IsActivation=1 where UserId=@reUserId";

                isTrue = db.DbConnecttion.Execute(sql, Engineer.ccc, new { reUserId = reUserInfo.UserId }, dbTransaction) > 0;

            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(UserAccountDal), "ActivationUser", Engineer.ggg, null, ex);
            }

            return isTrue;
        }

        /// <summary>
        /// 修改账户总资产
        /// </summary>
        /// <returns></returns>
        //public bool UpdateTotalAssets(AccountInfo accInfo, DbBase db, IDbTransaction dbTransaction)
        //{
        //    bool isTrue = false;
        //    try
        //    {
        //        //update AccountInfo set TotalAssets=1,ActivationCount=1 where AccountId=
        //        string sql = "update AccountInfo set TotalAssets=@totalAssets where AccountId=@accountId";

        //        isTrue = db.DbConnecttion.Execute(sql, Engineer.ccc, new { totalAssets = accInfo.TotalAssets, accountId = accInfo.AccountId }, dbTransaction) > 0;
        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserAccountDal), "UpdateTotalAssets", Engineer.ggg, null, ex);
        //    }

        //    return isTrue;
        //}

        /// <summary>
        /// 激活卡记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Page<ActicationCardExchangeModel> ActivationCardExchange(int userId, int activationType, int pageIndex, int pageSize)
        {
            int totalCount = 0;
            Page<ActicationCardExchangeModel> page = new Page<ActicationCardExchangeModel>();
            List<ActicationCardExchangeModel> list = new List<ActicationCardExchangeModel>();
            string sql = @"
                                select num,UserId,ReUserName,ReUserId,CreateTime,Remark,ActivationType
                               from(select ROW_NUMBER() over(order by at.CreateTime desc)as num, at.UserId,at.ReUserName,ReUserId,at.CreateTime,at.Remark,at.ActivationType
                                                             from ActivationTable as at inner join UserInfo as u on at.UserId=u.UserId 
                                        where u.UserId=@userId and at.ActivationType=@activationType
                                 )as t
                            where  t.num between  @pageSize * (@pageIndex - 1) + 1 and @pageIndex * @pageSize
                       ";

            string totalSql = @"select ROW_NUMBER() over(order by at.CreateTime desc)as num,at.UserId,at.ReUserName,ReUserId,at.CreateTime
                                 from ActivationTable as at inner join UserInfo as u on at.UserId=u.UserId
                                 where u.UserStatus=1 and IsActivation=1 and u.UserId=@userId";
            using (var db = BaseDal.ReadOnlySanNongDunConn())
            {
                list = db.DbConnecttion.Query<ActicationCardExchangeModel>(sql, Engineer.ccc, new { userId = userId, activationType = activationType, pageIndex = pageIndex, pageSize = pageSize }).ToList<ActicationCardExchangeModel>();




                totalCount = db.DbConnecttion.Query<ActicationCardExchangeModel>(totalSql, Engineer.ccc, new { userId = userId }).ToList<ActicationCardExchangeModel>().Count;


                if (list != null && totalCount > 0)
                {
                    page.Data = list;
                    page.PageIndex = pageIndex;
                    page.PageSize = pageSize;
                    page.TotalCount = totalCount;
                    page.PageYe = (int)Math.Ceiling(Convert.ToDouble(totalCount) / Convert.ToDouble(pageSize));
                }
            }
            return page;
        }


        /// <summary>
        /// 激活卡互转
        /// </summary>
        /// <param name="reUserName"></param>
        /// <param name="accInfo"></param>
        /// <param name="reAccInfo"></param>
        /// <returns></returns>
        //public bool GiveActivationCard(UserInfo userInfo, UserInfo reUserInfo, AccountInfo accInfo, AccountInfo reAccInfo)
        //{
        //    bool isTransaction = false;
        //    using (DbBase dbBase = BaseDal.WriteSanNongDunDbBase())
        //    {
        //        IDbTransaction dbTransaction = dbBase.DbConnecttion.BeginTransaction();

        //        isTransaction = UpdateActivationCount(accInfo, dbBase, dbTransaction);
        //        if (!isTransaction)
        //        {
        //            dbTransaction.Rollback();
        //            return false;
        //        }

        //        isTransaction = UpdateActivationCount(reAccInfo, dbBase, dbTransaction);
        //        if (!isTransaction)
        //        {
        //            dbTransaction.Rollback();
        //            return false;
        //        }
        //        ActivationTable at = new ActivationTable();
        //        at.CreateTime = DateTime.Now;
        //        at.UserId = userInfo.UserId;
        //        at.ReUserId = reUserInfo.UserId;
        //        at.Remark = "赠送激活卡";
        //        at.ReUserName = reUserInfo.UserName;
        //        at.ActivationType = 2;
        //        isTransaction = AddActivTable(at, dbBase, dbTransaction);
        //        if (!isTransaction)
        //        {
        //            dbTransaction.Rollback();
        //            return false;
        //        }

        //        dbTransaction.Commit();
        //    }

        //    return isTransaction;
        //}


        /// <summary>
        /// 修改激活卡数量
        /// </summary>
        /// <param name="accInfo"></param>
        /// <param name="db"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        //public bool UpdateActivationCount(AccountInfo accInfo, DbBase db, IDbTransaction dbTransaction)
        //{
        //    bool isTrue = false;
        //    try
        //    {
        //        //update AccountInfo set TotalAssets=1,ActivationCount=1 where AccountId=
        //        string sql = "update AccountInfo set ActivationCount=@activationCount where AccountId=@accountId";

        //        isTrue = db.DbConnecttion.Execute(sql, Engineer.ccc, new { activationCount = accInfo.ActivationCount, accountId = accInfo.AccountId }, dbTransaction) > 0;
        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserAccountDal), "UpdateActivationCount", Engineer.ggg, accInfo, ex);
        //    }

        //    return isTrue;
        //}

        /// <summary>
        /// 获取 团队上级 账户信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public UserAccInfoModel GetTeamParentAccInfo(string teamParentId)
        {
            UserAccInfoModel userAcc = new UserAccInfoModel();
            List<UserAccInfoModel> list = new List<UserAccInfoModel>();
            string sql = @"select u.RightId,u.LeftId,u.TeamParentId,u.[level],acc.* from AccountInfo acc
                                    inner join UserInfo u on u.UserId = acc.UserId
                                    where u.IsActivation=1 and u.UserStatus=1 and (u.LeftId=@teamParentId or u.RightId=@teamParentId)";
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    list = db.DbConnecttion.Query<UserAccInfoModel>(sql, Engineer.ccc, new { teamParentId = teamParentId }).ToList<UserAccInfoModel>();
                    if (list.Count > 0)
                    {
                        userAcc = list[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(UserAccountDal), "GetTeamParentUser", Engineer.ggg, teamParentId, ex);
            }

            return userAcc;
        }


        public GreenScore GetSumGreenAndScore(DateTime? begin, DateTime? end)
        {
            GreenScore gs = new GreenScore();
            string strWhere = "";
                try
                {
                    if (begin != null && end != null)
                    {
                        strWhere = " createtime >=@begin and createtime <=@end  ";
                    }
                    else if (begin == null && end != null)
                    {
                        strWhere = " createtime <=@end  ";
                    }
                    else if (begin != null && end == null)
                    {
                        strWhere = " createtime >=@begin  ";
                    }
                    else
                    {
                        strWhere = " 1=1 ";
                    } 
                    using (var db = ReadOnlySanNongDunConn())
                    {
                        string sql = string.Format(@"select ISNULL(SUM(GreenCount),0) from AccountInfo where{0}", strWhere);
                        string sqlScore = string.Format(@"select ISNULL(SUM(Score),0) from AccountInfo where{0}", strWhere);
                        gs.GreenCount = db.DbConnecttion.ExecuteScalar<decimal>(sql, Engineer.ggg, new { begin = begin, end = end });
                        gs.Score = db.DbConnecttion.ExecuteScalar<decimal>(sqlScore, Engineer.ggg, new { begin = begin, end = end });

                    }
                }
                catch (Exception ex)
                {

                    LogHelper.WriteLog(typeof(UserDal), "GetSumMoney", Engineer.ggg, null, ex);
                }

                return gs;
        
        }
    }
}
