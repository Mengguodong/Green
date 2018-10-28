using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperEx;
using Common;
using Common.Enums.EnmUser;
using DataModel;
using DataModel.RequestModel;
using DataModel.ViewModel;
using EveryDayEpService;
using SndApi.Models;


namespace Dal
{
    public class UserDal : BaseDal
    {

        #region 商城api
        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public UserScore GetShopByLoginName(string loginName)
        {
            List<UserScore> list = new List<UserScore>();
            UserScore userEp = null;
            string sql = @"select ac.Sorce,u.UserName,u.Level from UserInfo u
                            inner join AccountInfo ac on u.UserId=ac.UserId
                            where u.IsActivation=1 and u.UserStatus=1 and u.UserName=@loginName";
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    list = db.DbConnecttion.Query<UserScore>(sql, Engineer.ccc, new { loginName = loginName }).ToList();
                    if (list.Count > 0) 
                    {
                        userEp = list[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(typeof(UserDal), ex);//(typeof(UserDal), "",Engineer.ggg, new { loginName = loginName }, ex);
            }
            return userEp;

        }


        //public bool InsertShopRecord(ShopRecord shop) 
        //{
        //    bool isTrue = false;
        //    try
        //    {
        //        using (var db = BaseDal.ReadOnlySanNongDunConn())
        //        {
        //          isTrue=  db.Insert<ShopRecord>(shop);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteError(typeof(UserDal), ex);//(typeof(UserDal), "",Engineer.ggg, new { loginName = loginName }, ex);
        //    }
        //    return isTrue;
        //}
        #endregion


        /// <summary>
        /// 获取用户列表
        /// Author：m
        /// Date：2016年12月23日00:50:16
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public Page<UserIndexModel> GetAdminUserInfoes(string userName, int pageSize, int pageIndex)
        {
            Page<UserIndexModel> page = new Page<UserIndexModel>();
            page.Data = new List<UserIndexModel>();

            string strWhere = "";

            if (!string.IsNullOrEmpty(userName))
            {
                strWhere = " and u.UserName like @userName";
            }

            string sql = string.Format(@"select num,UserId, UserName, Pwd, PayPwd, IdCard, BankName, BankNumber, Phone, ParentId
                                        , LeftId, RightId, TeamParentId, IsActivation, RealName, CreateTime
                                        , TeamTime, TeamType, [level], UserStatus, UserType,AccountId, Score
                                        , GreenCount, LeftAchievement, RightAchievement, OutNum, GreenTotal

                                              from (select ROW_NUMBER() over (order by u.CreateTime desc)as num,u.UserId, u.UserName, Pwd, PayPwd, IdCard, BankName, BankNumber, Phone, ParentId
                                        , LeftId, RightId, TeamParentId, IsActivation, RealName, u.CreateTime
                                        , TeamTime, TeamType, [level], UserStatus, UserType,AccountId, GreenCount
                                        , Score, LeftAchievement, RightAchievement, OutNum, GreenTotal
                                                    from UserInfo u inner join AccountInfo acc on u.UserId=acc.UserId

                                                         where  1=1 {0}) as t
                                 
                                     where t.num between {1} and {2}", strWhere, pageSize * (pageIndex - 1) + 1, pageSize * pageIndex);

            string sqlCount = string.Format(@" select Count(1)
                                 from (select ROW_NUMBER() over (order by u.CreateTime desc)as num ,u.* from UserInfo u 
                                  inner join AccountInfo ua on u.UserId=ua.UserId
                                where   1=1 {0}) as t",
                                 strWhere);
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {

                    var pageData = db.DbConnecttion.Query<UserIndexModel>(sql, Engineer.ggg, new { userName = "%" + userName + "%" }).ToList();
                    if (pageData.Count > 0)
                    {
                        page.Data = pageData;
                    }

                    int totalCount = (int)db.DbConnecttion.ExecuteScalar(sqlCount, new { userName = "%" + userName + "%" });

                    page.TotalCount = totalCount;

                }

            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(UserDal), "GetAdminUserInfoes", Engineer.ggg, new { userName = userName, pageIndex = pageIndex, pageSize = pageSize }, ex);
            }
            return page;
        }

        /// <summary>
        /// 获取修改用户信息
        /// Author：m
        /// Date：2016年12月23日00:50:16
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public UserIndexModel GetAdminUserIndexModel(int userId)
        {
            UserIndexModel model = null;
            List<UserIndexModel> list = new List<UserIndexModel>();
            string sql = string.Format(@"select 0 num,u.UserId, u.UserName, Pwd, PayPwd, IdCard, BankName, BankNumber, Phone, ParentId
                                        , LeftId, RightId, TeamParentId, IsActivation, RealName, u.CreateTime
                                        , TeamTime, TeamType, [level], UserStatus, UserType,AccountId, Score
                                        , GreenCount, LeftAchievement, RightAchievement, GreenTotal,StaticsRelease
                                        from UserInfo u inner join AccountInfo acc on u.UserId=acc.UserId
                                        where u.UserId=@userId");

            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    list = db.DbConnecttion.Query<UserIndexModel>(sql, Engineer.ggg, new { userId = userId }).ToList();
                    if (list.Count > 0)
                    {
                        model = list[0];
                    }
                }

            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(UserDal), "GetAdminUserIndexModel", Engineer.ggg, new { userId = userId }, ex);
            }
            return model;
        }

        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public UserInfo GetUserByLoginName(string loginName)
        {
            UserInfo userInfo = null;
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    userInfo = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.UserName, OperationMethod.Equal, loginName)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(typeof(UserDal), ex);//(typeof(UserDal), "",Engineer.ggg, new { loginName = loginName }, ex);
            }
            return userInfo;

        }
        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserInfo GetUserById(int userId)
        {
            UserInfo userInfo = null;
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    userInfo = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.UserId, OperationMethod.Equal, userId)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(typeof(UserDal), ex);//(typeof(UserDal), "",Engineer.ggg, new { loginName = loginName }, ex);
            }
            return userInfo;

        }




        #region 注释
        ///// <summary>
        ///// 获取三层推荐人用户ID
        ///// sj
        ///// </summary>
        ///// <param name="userId">当前用户ID</param>
        ///// <returns>三个推荐人用户ID</returns>
        //public Dictionary<string, int> GetTreeUserId(int userId)
        //{
        //    int parentId = GetParentIdByUserId(userId);
        //    Dictionary<string, int> userIdDic = new Dictionary<string, int>();
        //    try
        //    {
        //        //第一层推荐人
        //        using (var db = BaseDal.ReadOnlyWineGameConn())
        //        {
        //            user = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.UserId, OperationMethod.Equal, parentId)).FirstOrDefault();
        //            if (user != null)
        //                userIdDic[EnumUserThree.OneUserId.ToString()] = user.ParentId;
        //            else
        //                user = new UserInfo();


        //        }
        //        //第二层推荐人
        //        using (var db = BaseDal.ReadOnlyWineGameConn())
        //        {
        //            user = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.UserId, OperationMethod.Equal, user.ParentId)).FirstOrDefault();
        //            if (user != null)
        //                userIdDic[EnumUserThree.TwoUserId.ToString()] = user.ParentId;
        //            else
        //                user = new UserInfo();
        //        }
        //        //第三层推荐人
        //        using (var db = BaseDal.ReadOnlyWineGameConn())
        //        {
        //            user = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.UserId, OperationMethod.Equal, user.ParentId)).FirstOrDefault();
        //            if (user != null)
        //                userIdDic[EnumUserThree.ThreeUserId.ToString()] = user.ParentId;
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserDal), "GetTreeUserId", Engineer.ccc, new { userId = userId }, ex);
        //    }
        //    return userIdDic;
        //}

        /// <summary>
        /// 获取等级信息
        /// author:menggd
        /// Date:2016年12月11日16:50:28
        /// </summary>
        /// <returns></returns>
        //        public IList<LevelInfo> GetLevelInfos()
        //        {
        //            IList<LevelInfo> list = new List<LevelInfo>();
        //            try
        //            {
        //                using (var db = ReadOnlyWineGameConn())
        //                {

        //                    list = db.Query<LevelInfo>(SqlQuery<LevelInfo>.Builder(db));
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //                LogHelper.WriteLog(typeof(UserDal), "GetLevelInfos", Engineer.ggg, null, ex);
        //            }

        //            return list;

        //        }

        //        /// <summary>
        //        /// 用户注册
        //        /// Author：孟国栋
        //        /// Date：2016年12月11日17:12:03
        //        /// </summary>
        //        /// <param name="model">用户信息实体</param>
        //        /// <returns></returns>
        //        public int Register(UserInfo model)
        //        {

        //            int userId = 0;
        //            try
        //            {
        //                using (var db = ReadOnlyWineGameConn())
        //                {
        //                    string sql = @"insert into UserInfo (UserName, Pwd, CreateTime, UserStatues, IsDelete,ParentId, UserType
        //                                           , LevelId,DownCount,  MobilePhone,IsTeam,IsReturn,IsTestAccount ,AuthenticationTime
        //                                            ,BecomeTeamTime,BestLevelTime ,IsActivate,IsQueue)
        //                                  values (@UserName,@Pwd,@CreateTime,@UserStatues,@IsDelete,@ParentId,@UserType,@LevelId,@DownCount
        //                                            , @MobilePhone,@IsTeam,@IsReturn,@IsTestAccount,@AuthenticationTime,@BecomeTeamTime
        //                                        ,@BestLevelTime,@IsActivate,@IsQueue);
        //                                  select @@IDENTITY;";
        //                    userId = ConvertHelper.ToInt32(db.DbConnecttion.ExecuteScalar(sql, model));
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //                LogHelper.WriteLog(typeof(UserDal), "Register", Engineer.ggg, model, ex);
        //            }
        //            return userId;


        //        }

        /// <summary>
        /// 根据用户ID获取父级ID
        /// Author：menggd
        /// Date：2016年12月11日18:40:53
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserInfo GetParentIdByUserId(int parentId)
        {
            UserInfo user = null;
            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    user = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.UserId, OperationMethod.Equal, parentId).AndWhere(x=>x.IsActivation,OperationMethod.Equal,1)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(UserDal), "GetParentIdByUserId", Engineer.ggg, new { parentId = parentId }, ex);
                // throw;
            }

            return user;

        }

        //        /// <summary>
        //        /// 分页查询用户表
        //        /// </summary>
        //        /// <param name="pageIndex">查询页数</param>
        //        /// <param name="pageSize">每页条数</param>
        //        /// <returns></returns>
        //        public IList<UserInfo> PageUserList(int pageIndex, int pageSize)
        //        {
        //            IList<UserInfo> userList = new List<UserInfo>();
        //            try
        //            {
        //                using (var db = ReadOnlyWineGameConn())
        //                {
        //                    userList = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db)).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        //                }
        //                if (userList.Count <= 0)
        //                    LogHelper.WriteInfo(typeof(UserDal), string.Format("PageUserList错误！作者：{0}========={1}=======", Engineer.ccc, DateTime.Now));
        //                return userList;
        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.WriteInfo(typeof(UserDal), string.Format("PageUserList错误！作者：{0}========={1}======={2}", Engineer.ccc, DateTime.Now, ex));
        //                return userList;
        //            }

        //        }

        //        /// <summary>
        //        /// 获取所有用户总数
        //        /// </summary>
        //        /// <returns>用户总数</returns>
        //        public int UserCount()
        //        {
        //            int userCount = 0;
        //            IList<UserInfo> userList = new List<UserInfo>();
        //            try
        //            {
        //                using (var db = ReadOnlyWineGameConn())
        //                {
        //                    userList = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db));

        //                }
        //                if (userList.Count <= 0)
        //                    LogHelper.WriteInfo(typeof(UserDal), string.Format("UserCount错误！作者：{0}========={1}======", Engineer.ccc, DateTime.Now));
        //                else
        //                    userCount = userList.Count;
        //                return userCount;
        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.WriteInfo(typeof(UserDal), string.Format("UserCount错误！作者：{0}========={1}======={2}", Engineer.ccc, DateTime.Now, ex));
        //                return userCount;
        //            }


        //        }
        ///// <summary>
        ///// 修改用户信息
        ///// author:m
        ///// Date:2016年12月17日14:10:39
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>

        //public bool UpdateUser(UserInfo model)
        //{
        //    bool result = false;
        //    try
        //    {
        //        string sql = @"update UserInfo set RealName=@realName,BankCardNo=@bankCardNo,IDCard=@IDCard,BankName=@bankName,DeliveryAddress=@DeliveryAddress,AuthenticationTime=@time where UserId=@userId;";
        //        using (var db = WriteWineGameDbBase())
        //        {
        //            int resultInt = db.DbConnecttion.Execute(sql, Engineer.ggg, new { realName = model.RealName, bankCardNo = model.BankCardNo, IDCard = model.IDCard, bankName = model.BankName, DeliveryAddress = model.DeliveryAddress, time = model.AuthenticationTime, userId = model.UserId });
        //            result = resultInt > 0 ? true : false;

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserDal), "UpdateUser", Engineer.ggg, model, ex);
        //    }

        //    return result;
        //} 
        #endregion

        /// <summary>
        /// 根据用户ID获取用户信息
        /// Author：m
        /// Date：2016年12月17日15:09:09
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public UserInfo GetUserInfoById(int userId)
        {
            UserInfo userInfo = null;
            try
            {
                using (var db = WriteSanNongDunDbBase())
                {
                    userInfo = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.UserId, OperationMethod.Equal, userId)).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(UserDal), "GetUserInfoById", Engineer.ggg, new { userId = userId }, ex);
            }
            return userInfo;

        }

        #region 注释
        //        /// <summary>
        //        /// 获取用户下作坊人数
        //        /// </summary>
        //        /// <param name="userId">用户id</param>
        //        /// <returns></returns>
        //        public int UserWineCount(int userId)
        //        {
        //            IList<UserInfo> userList = new List<UserInfo>();
        //            int count = 0;
        //            try
        //            {
        //                if (userId == 1)
        //                {
        //                    return 0;
        //                }
        //                using (var db = ReadOnlyWineGameConn())
        //                {
        //                    string sql = @"select userId, UserName, Pwd, RealName, u.CreateTime, UserStatues, IsDelete, ParentId, UserType
        //                                       , u.LevelId, DownCount, BankCardNo, IDCard, AuthenticationTime, BankName, MobilePhone, BecomeTeamTime
        //                                       , IsTeam, IsReturn, BestLevelTime, BestLevel, IsTestAccount, IsActivate
        //                                  from userInfo u,LevelInfo lv
        //                                  where u.levelid = lv.LevelId and u.ParentId=@userId and lv.LevelType=3";
        //                    userList = db.DbConnecttion.Query<UserInfo>(sql, Engineer.ccc, new { userId = userId }).ToList();

        //                }
        //                if (userList.Count > 0)
        //                    count = userList.Count;
        //                return count;
        //            }
        //            catch (Exception ex)
        //            {

        //                LogHelper.WriteLog(typeof(UserDal), "UserWineCount", Engineer.ccc, new { userId = userId }, ex);
        //            }

        //            return count;
        //        }


        /// <summary>
        /// 修改密码
        /// Author：m
        /// Date：2016年12月21日14:01:18
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public bool UpdateUserPwd(UserInfo model)
        //{
        //    bool result = false;
        //    try
        //    {
        //        using (var db = WriteWineGameDbBase())
        //        {

        //            var userInfo = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.UserId, OperationMethod.Equal, model.UserId)).FirstOrDefault();

        //            if (userInfo != null)
        //            {
        //                userInfo.LoginPWD = model.LoginPWD;
        //                result = db.Update<UserInfo>(userInfo);
        //            }

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog(typeof(UserDal), "UpdateUserPwd", Engineer.ggg, model, ex);

        //    }

        //    return result;
        //}

        //        /// <summary>
        //        /// 获取待激活用户列表
        //        /// Author：m
        //        /// Date：2016年12月23日00:50:16
        //        /// </summary>
        //        /// <param name="userName"></param>
        //        /// <param name="pageSize"></param>
        //        /// <param name="pageIndex"></param>
        //        /// <returns></returns>
        //        public Page<UserInfo> GetActivateUser(string userName, int pageSize, int pageIndex)
        //        {
        //            Page<UserInfo> page = null;
        //            string strWhere = "";

        //            if (!string.IsNullOrEmpty(userName))
        //            {
        //                strWhere = " and UserName like @userName";
        //            }

        //            string sql = string.Format(@"select userId, 
        //                                 UserName,
        //                                 RealName,
        //                                 CreateTime, 
        //                                 UserStatues,
        //                                 ParentId,
        //                                 UserType,
        //                                 MobilePhone,
        //                                 IsTestAccount,
        //                                 IsActivate
        //                                 from (select ROW_NUMBER() over (order by CreateTime desc)as num ,* from UserInfo where IsDelete=0 and IsTestAccount=0 and IsActivate=0 {0}) as t
        //                                 
        //                                 where t.num between {1} and {2}", strWhere, pageSize * (pageIndex - 1) + 1, pageSize * pageIndex);

        //            string sqlCount = string.Format(@"select Count(1)
        //                                 from (select ROW_NUMBER() over (order by CreateTime desc)as num ,* from UserInfo where IsDelete=0 and IsTestAccount=0 and IsActivate=0 {0}) as t",
        //                                 strWhere);
        //            try
        //            {
        //                using (var db = ReadOnlyWineGameConn())
        //                {

        //                    var pageData = db.DbConnecttion.Query<UserInfo>(sql, Engineer.ggg, new { userName = userName }).ToList();
        //                    if (pageData.Count > 0)
        //                    {
        //                        page.Data = pageData;
        //                    }

        //                    int totalCount = (int)db.DbConnecttion.ExecuteScalar(sql, new { userName = userName });

        //                    page.TotalCount = totalCount;

        //                }

        //            }
        //            catch (Exception ex)
        //            {

        //                LogHelper.WriteLog(typeof(UserDal), "GetActivateUser", Engineer.ggg, new { userName = userName, pageIndex = pageIndex, pageSize = pageSize }, ex);
        //            }
        //            return page;
        //        }

        ///// <summary>
        ///// 激活用户
        ///// Author：m
        ///// Date：2016年12月23日11:41:38
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public bool ActivateUser(UserInfo model)
        //{
        //    bool result = false;

        //    try
        //    {

        //        using (var db = WriteWineGameDbBase())
        //        {
        //            UserInfo userEntity = new UserInfo();

        //            var user = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(o => o.UserId, OperationMethod.Equal, model.UserId));
        //            if (user != null && user.Count > 0)
        //            {
        //                userEntity = user.FirstOrDefault();
        //            }

        //            userEntity.IsActivate = model.IsActivate;
        //            userEntity.LevelId = model.LevelId;

        //            result = db.Update<UserInfo>(userEntity);


        //        }


        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserDal), "ActivateUser", Engineer.ggg, model, ex);
        //    }

        //    return result;

        //}

        ///// <summary>
        ///// 用户下级成员
        ///// </summary>
        ///// <param name="parentId"></param>
        ///// <returns></returns>
        //public List<UserInfo> UserLowerList(int parentId)
        //{
        //    List<UserInfo> list = new List<UserInfo>();
        //    try
        //    {
        //        using (var db = ReadOnlyWineGameConn())
        //        {
        //            string sql = "select userId, UserName, RealName, CreateTime, UserStatues, IsDelete, ParentId, UserType, LevelName, LevelId, DownCount, MobilePhone, BecomeTeamTime, IsTeam, IsTestAccount, IsActivate, IsQueue, DeliveryAddress from userInfo(nolock) where ParentId=@parentId  and isDelete=0";

        //            list = db.DbConnecttion.Query<UserInfo>(sql, Engineer.ggg, new { parentId = parentId }).ToList();

        //           // list = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.ParentId, OperationMethod.Equal, parentId)).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog(typeof(UserDal), "UserLowerList", Engineer.ccc, parentId, ex);
        //    }
        //    return list;
        //}


        /// <summary>
        /// 分页用户下级成员
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>

        //public Page<UserInfo> UserLowerTeamPageList(int parentId, int pageIndex, int pageSize)
        //{
        //    Page<UserInfo> page = new Page<UserInfo>();
        //    List<UserInfo> list = new List<UserInfo>();
        //    try
        //    {
        //        using (var db = ReadOnlyWineGameConn())
        //        {
        //            list = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.ParentId, OperationMethod.Equal, parentId)).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        //            page.Data = list;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog(typeof(UserDal), "UserLowerList", Engineer.ccc, parentId, ex);
        //    }
        //    return page;
        //}


        //        /// <summary>
        //        /// 分页查询没有排队的用户
        //        /// </summary>
        //        /// <param name="pageIndex">查询页数</param>
        //        /// <param name="pageSize">每页条数</param>
        //        /// <returns></returns>
        //        public IList<UserInfo> UserNoQueueList(int pageIndex, int pageSize)
        //        {

        //            List<UserInfo> list = new List<UserInfo>();

        //            //  select *
        //            //from (
        //            //        select ROW_NUMBER()over(order by userInfo.userId)as num,*
        //            //        from  UserInfo  
        //            //        where [UserInfo].isqueue=0 and UserInfo.UserType=1 and UserInfo.IsActivate=1
        //            //    ) as t
        //            //where t.num between (0) and (10)
        //            int a = pageSize * (pageIndex - 1) + 1;
        //            int b = pageSize * pageIndex;
        //            string sql = string.Format
        //                (
        //                      @"select *
        //                        from (
        //                                 select ROW_NUMBER()over(order by userInfo.userId)as num,*
        //                                   from  UserInfo  
        //                                    where [UserInfo].isqueue=0 and UserInfo.UserType=1 and UserInfo.IsActivate=1 and UserInfo.LevelId=3 
        //                             ) as t
        //                      where t.num between {0} and {1}"
        //                , pageSize * (pageIndex - 1) + 1, pageSize * pageIndex);
        //            try
        //            {
        //                using (var db = ReadOnlyWineGameConn())
        //                {

        //                    list = db.DbConnecttion.Query<UserInfo>(sql, Engineer.ccc).ToList();

        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.WriteLog(typeof(UserDal), "UserNoQueueList", Engineer.ccc, null, ex);
        //            }
        //            return list;

        //        }

        //        /// <summary>
        //        /// 获取没有排队的用户总数
        //        /// </summary>
        //        /// <returns>用户总数</returns>
        //        public int UserNoQueueCount()
        //        {
        //            int userCount = 0;
        //            try
        //            {
        //                string sql = @"select COUNT(1)
        //                               from  UserInfo  
        //                               where [UserInfo].isqueue=0 and UserInfo.UserType=1 and UserInfo.IsActivate=1 and UserInfo.LevelId=3 ";
        //                using (var db = ReadOnlyWineGameConn())
        //                {
        //                    userCount = Convert.ToInt32(db.DbConnecttion.ExecuteScalar(sql, Engineer.ccc));
        //                }
        //                return userCount;
        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.WriteInfo(typeof(UserDal), string.Format("UserCount错误！作者：{0}========={1}======={2}", Engineer.ccc, DateTime.Now, ex));
        //                return userCount;
        //            }
        //        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        //public bool UpdateUserInfo(UserInfo user)
        //{
        //    bool result = false;
        //    try
        //    {
        //        using (var db = WriteWineGameDbBase())
        //        {
        //            var userList = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.UserId, OperationMethod.Equal, user.UserId));
        //            if (userList != null && userList.Count > 0)
        //            {
        //                result = db.Update(user);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog(typeof(UserDal), "UpdateUserInfo", Engineer.ccc, user, ex);
        //    }

        //    return result;
        //}






        ///// <summary>
        ///// 获取母体用户
        ///// </summary>
        ///// <returns></returns>
        //public UserInfo GetMotherUser()
        //{
        //    UserInfo user = null;
        //    try
        //    {
        //        using (var db = ReadOnlyWineGameConn())
        //        {
        //            user = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.UserType, OperationMethod.Equal, (int)UserType.Mother)).FirstOrDefault();

        //        }


        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserDal), "GetMotherUser", Engineer.ggg, null, ex);
        //        // throw;
        //    }

        //    return user;
        //}


        ///// <summary>
        ///// 获取激活总人数
        ///// </summary>
        ///// <returns></returns>
        //public int GetActiveTotal()
        //{
        //    int result = 0;
        //    string sql = "select COUNT(1) from UserInfo where UserType=1 and IsDelete=0 and IsTestAccount=0 and IsActivate=1";

        //    try
        //    {
        //        using (var db = ReadOnlyWineGameConn())
        //        {
        //            result = ConvertHelper.ToInt32(db.DbConnecttion.ExecuteScalar(sql, null));
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserDal), "GetActiveTotal", Engineer.ggg, null, ex);
        //    }
        //    return result;
        //}
        ///// <summary>
        ///// 获取所有未激活总人数
        ///// </summary>
        ///// <returns></returns>
        //public int GetNotActiveTotal()
        //{
        //    int result = 0;
        //    string sql = "select COUNT(1) from UserInfo where UserType=1 and IsDelete=0 and IsTestAccount=0 and IsActivate=0";

        //    try
        //    {
        //        using (var db = ReadOnlyWineGameConn())
        //        {
        //            result = ConvertHelper.ToInt32(db.DbConnecttion.ExecuteScalar(sql, null));
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserDal), "GetNotActiveTotal", Engineer.ggg, null, ex);
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 查询今日新增人数
        ///// </summary>
        ///// <returns></returns>
        //public int GetUserCountInday()
        //{
        //    int result = 0;
        //    string inday = DateTime.Now.ToString("yyyyMMdd");
        //    string sql = string.Format("select count(0) from UserInfo where CONVERT(varchar(100), createtime, 112)='{0}'", inday);

        //    try
        //    {
        //        using (var db = ReadOnlyWineGameConn())
        //        {
        //            result = ConvertHelper.ToInt32(db.DbConnecttion.ExecuteScalar(sql));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog(typeof(UserDal), "GetUserCountInday", Engineer.ggg, new { sql = sql }, ex);
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 根据日期查询总人数
        ///// </summary>
        ///// <param name="startTime"></param>
        ///// <param name="endTime"></param>
        ///// <returns></returns>
        //public int GetUserCountTotal(string startTime, string endTime)
        //{
        //    string strWhere = "";
        //    string sql = "";
        //    int result = 0;
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(startTime))
        //        {
        //            startTime = string.Join(string.Empty, startTime.Split('-'));
        //            strWhere = string.Format(" and  cast(CONVERT(varchar(100), createtime, 112) AS int)>={0}", startTime);
        //        }

        //        if (!string.IsNullOrWhiteSpace(endTime))
        //        {
        //            endTime = string.Join(string.Empty, endTime.Split('-'));
        //            strWhere = strWhere + string.Format(" AND cast(CONVERT(varchar(100), createtime, 112) AS int)<={0}", endTime);
        //        }

        //        sql = "select count(0) from UserInfo where 1=1 " + strWhere;
        //        using (var db = ReadOnlyWineGameConn())
        //        {
        //            result = ConvertHelper.ToInt32(db.DbConnecttion.ExecuteScalar(sql));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog(typeof(UserDal), "GetUserCountTotal", Engineer.ggg, new { startTime = startTime, endTime = endTime, sql = sql }, ex);
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 根据用户ID统计充值金额
        ///// Author：ccc
        ///// </summary>
        //public decimal StatisticsAmounteByUserId(int userId)
        //{
        //    decimal result = 0;
        //    string sql = "select SUM(w.ExchangeCount) from WineCoinExchange(nolock) w where UserId=@userId and w.Type=1";

        //    try
        //    {
        //        using (var db = ReadOnlyWineGameConn())
        //        {
        //            result = ConvertHelper.ToDecimal(db.DbConnecttion.ExecuteScalar(sql, new { userId = userId }));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteLog(typeof(UserDal), "StatisticsAmounteByUserId", Engineer.ccc, new { userId = userId }, ex);
        //    }
        //    return result;
        //}


        ///// <summary>
        ///// 根据用户ID获取下面总人数
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public int GetTotalPerson(int userId)
        //{
        //    int result = 0;

        //    try
        //    {

        //        string sql = "select * from UserAchievement(nolock)  where   userId =@userId";

        //        using (var db = ReadOnlyWineGameConn())
        //        {
        //            var model = db.DbConnecttion.Query<UserAchievement>(sql, Engineer.ggg, new { userId=userId}).FirstOrDefault();

        //            if (model!=null)
        //            {
        //                result = model.TotalPeople;
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserDal), "GetTotalPerson", Engineer.ggg, new { userId=userId},ex);
        //    }
        //    return result;
        //}
        /// <summary>
        /// 根据用户ID获取下面总业绩
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public decimal GetTotalAmount(int userId)
        //{
        //    decimal result = 0;

        //    try
        //    {

        //        string sql = "select * from UserAchievement(nolock)  where userId =@userId";

        //        using (var db = ReadOnlyWineGameConn())
        //        {
        //            var model = db.DbConnecttion.Query<UserAchievement>(sql, Engineer.ggg, new { userId = userId }).FirstOrDefault();

        //            if (model != null)
        //            {
        //                result = model.TotalAchievement;
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserDal), "GetTotalAmount", Engineer.ggg, new { userId = userId }, ex);
        //    }
        //    return result;
        //}

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        //public List<UserInfo> GetAllUser()
        //{
        //    List<UserInfo> list = new List<UserInfo>();
        //    try
        //    {
        //        string sql = "select userId,userName from UserInfo where UserType=1 and IsDelete=0 and IsTestAccount=0 ";

        //        using (var db = ReadOnlyWineGameConn())
        //        {
        //            list = db.DbConnecttion.Query<UserInfo>(sql,Engineer.ggg).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        LogHelper.WriteLog(typeof(UserDal), "GetAllUser",Engineer.ggg,null,ex);
        //    }

        //    return list;

        //} 
        #endregion
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        public int InserUser(UserInfo userEntity)
        {
            int userId = 0;
            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    string sql = @"insert  into UserInfo ( UserName, Pwd, Phone, ParentId, LeftId, RightId, IsActivation, CreateTime, TeamType, [level], UserStatus, UserType,TeamParentId,PayPwd)
                                            values (@UserName,@Pwd, @Phone, @ParentId, @LeftId, @RightId, @IsActivation, @CreateTime, @TeamType, @level, @UserStatus, @UserType,@TeamParentId,@PayPwd)
                                              select @@IDENTITY;";
                    userId = ConvertHelper.ToInt32(db.DbConnecttion.ExecuteScalar(sql, userEntity));
                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(UserDal), "Register", Engineer.ggg, userEntity, ex);
            }
            return userId;
        }



        public bool UpdateUserInfo(UserInfo userInfo)
        {
            bool isTrue = false;
            try
            {
                using (var db = BaseDal.WriteSanNongDunDbBase())
                {
                    isTrue = db.Update<UserInfo>(userInfo);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(UserDal), "UpdateUserInfo", Engineer.ggg, userInfo, ex);
            }
            return isTrue;
        }

        /// <summary>
        /// 获取总收益金额
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int GetSumMoney(DateTime? begin, DateTime? end) 
        {
            int count= 0;
            int sum = 0;
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
                        string sql = string.Format(@"select ISNULL(SUM([level]),0) from UserInfo where{0}",strWhere);
                     count= db.DbConnecttion.ExecuteScalar<int>(sql, Engineer.ggg, new { begin = begin, end = end });
                     sum = count * 330;

                    }
                }
                catch (Exception ex)
                {

                    LogHelper.WriteLog(typeof(UserDal), "GetSumMoney", Engineer.ggg, null, ex);
                }

            return sum;
        }

        /// <summary>
        /// 获取所有已激活的用户
        /// </summary>
        /// <returns></returns>
        public List<UserInfo> GetAllActivationUser()
        {
            List<UserInfo> model = new List<UserInfo>();
            try
            {

                using (var db = ReadOnlySanNongDunConn())
                {

                    model = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(o => o.IsActivation, OperationMethod.Equal, 1).AndWhere(o => o.UserType, OperationMethod.Equal, 1).AndWhere(o => o.UserStatus, OperationMethod.Equal, 1)).ToList();

                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(UserDal), "GetAllActivationUser", Engineer.ggg, null, ex);
            }

            return model;
        }
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public List<UserInfo> GetAllUser()
        {
            List<UserInfo> model = new List<UserInfo>();
            try
            {

                using (var db = ReadOnlySanNongDunConn())
                {

                    model = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(o => o.UserType, OperationMethod.Equal, 1).AndWhere(x => x.IsActivation, OperationMethod.Equal, 1).AndWhere(x => x.Level,OperationMethod.GreaterOrEqual, 1)).ToList();

                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(UserDal), "GetAllUser", Engineer.ggg, null, ex);
            }

            return model;
        }
        

        /// <summary>
        /// 向下获取左区
        /// </summary>
        /// <param name="leftId"></param>
        /// <returns></returns>
        public UserInfo GetUserByLeftId(string leftId)
        {
            UserInfo userInfo = null;
            List<UserInfo> list = new List<UserInfo>();
            try
            {
                using(var db=BaseDal.ReadOnlySanNongDunConn())
                {
                   list= db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.TeamParentId, OperationMethod.Equal, leftId)).ToList();
                   if (list != null && list.Count > 0)
                   {
                       userInfo = list[0];
                   }
                }
            }
            catch (Exception ex)
            {
               LogHelper.WriteLog(typeof(UserDal), "GetUserLowTeamId", Engineer.ggg, null, ex);
            }
            return userInfo;
        }

        /// <summary>
        /// 向上获取  左区用户
        /// </summary>
        /// <param name="leftId"></param>
        /// <returns></returns>
        public UserInfo GetLeftUserByTeamParentId(string teamParentId)
        {
            UserInfo userInfo = null;
            List<UserInfo> list = new List<UserInfo>();
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    //list = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.LeftId, OperationMethod.Equal, teamParentId).AndWhere(x=>x.IsActivation,OperationMethod.Equal,1)).ToList();
                    list = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.LeftId, OperationMethod.Equal, teamParentId)).ToList();
                    if (list != null && list.Count > 0)
                    {
                        userInfo = list[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(UserDal), "GetLeftUserByTeamParentId", Engineer.ggg, null, ex);
            }
            return userInfo;
        }

        /// <summary>
        /// 向下获取右区
        /// </summary>
        /// <param name="rightId"></param>
        /// <returns></returns>
        public UserInfo GetUserByRightId(string rightId)
        {
            UserInfo userInfo = null;
            List<UserInfo> list = new List<UserInfo>();
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    list = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.TeamParentId, OperationMethod.Equal, rightId)).ToList();
                    if (list != null && list.Count > 0)
                    {
                        userInfo = list[0];
                    }
                }
            }
            catch (Exception ex)
            {
               LogHelper.WriteLog(typeof(UserDal), "GetUserLowTeamId", Engineer.ggg, null, ex);
            }
            return userInfo;
        }

        /// <summary>
        /// 向上获取右区
        /// </summary>
        /// <param name="rightId"></param>
        /// <returns></returns>
        public UserInfo GetRightUserByTeamParentId(string teamParentId)
        {
            UserInfo userInfo = null;
            List<UserInfo> list = new List<UserInfo>();
            try
            {
                using (var db = BaseDal.ReadOnlySanNongDunConn())
                {
                    //list = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.RightId, OperationMethod.Equal, teamParentId).AndWhere(x => x.IsActivation, OperationMethod.Equal, 1)).ToList();
                    list = db.Query<UserInfo>(SqlQuery<UserInfo>.Builder(db).AndWhere(x => x.RightId, OperationMethod.Equal, teamParentId)).ToList();
                    if (list != null && list.Count > 0)
                    {
                        userInfo = list[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(UserDal), "GetRightUserByTeamParentId", Engineer.ggg, null, ex);
            }
            return userInfo;
        }
        /// <summary>
        /// 获取每日返EP服务
        /// </summary>
        /// <returns></returns>
        public List<UserAccInfoModel> GetEpAllUserInfo()
        {
            List<UserAccInfoModel> list = new List<UserAccInfoModel>();
            string sql = @"select u.YesTodayIsLogin,u.RightId,u.LeftId,u.TeamParentId,u.[level],acc.* from AccountInfo acc
                            inner join UserInfo u on acc.UserId=u.UserId
                            where u.UserStatus=1 and IsActivation=1 and acc.TotalAssets>0";

            using(var db= BaseDal.ReadOnlySanNongDunConn())
            {
                list = db.DbConnecttion.Query<UserAccInfoModel>(sql, Engineer.ccc).ToList<UserAccInfoModel>();  
            }
            return list;
        }


        public List<UserAccInfoModel> GetUserAndAcc(string parentId="")
        {
            string sqlWhere = "";
            if (!string.IsNullOrEmpty(parentId))
            {
                sqlWhere = string.Format("  and u.UserId={0}", parentId);
            }
            

            List<UserAccInfoModel> list = new List<UserAccInfoModel>();
            string sql = string.Format(@"select u.UserId, u.UserName, u.Pwd, u.PayPwd, u.IdCard, u.BankName, u.BankNumber
, u.Phone, u.ParentId, u.LeftId, u.RightId, u.TeamParentId, u.IsActivation
, u.RealName, u.CreateTime, u.TeamTime, u.TeamType
, u.[level], u.UserStatus, u.UserType, u.TodayIsLogin, u.YesTodayIsLogin 
AccountId, Sorce, GreenCount, LeftAchievement, RightAchievement, LeftCount, RightCount, HongBao, StaticsRelease,  FreezeGreen, GreenTotal

from AccountInfo acc
inner join UserInfo u on acc.UserId=u.UserId
where u.UserStatus=1 and IsActivation=1 and u.UserType=1 {0}", sqlWhere);

            using (var db = BaseDal.ReadOnlySanNongDunConn())
            {
                list = db.DbConnecttion.Query<UserAccInfoModel>(sql, Engineer.ccc).ToList<UserAccInfoModel>();
            }
            return list;
        }



        public List<int> AdminGetUserIdList(string userName)
        {
            List<int> list = new List<int>();

            string sql =string.Format( @"select UserId from UserInfo u
                          
                     where UserName like '%{0}%' ",userName);

            using (var db = BaseDal.ReadOnlySanNongDunConn())
            {
                list = db.DbConnecttion.Query<int>(sql, Engineer.ccc).ToList<int>();
            }
            return list;

        }
    }
}
