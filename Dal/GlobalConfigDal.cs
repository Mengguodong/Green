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
    public class GlobalConfigDal : BaseDal
    {
        public string GetValue(string configKey)
        {
            string configValue = "";
            try
            {
                if (!string.IsNullOrEmpty(configKey))
                {
                    using (var db = ReadOnlySanNongDunConn())
                    {
                        configValue = db.Query<GlobalConfig>(SqlQuery<GlobalConfig>.Builder(db).AndWhere(o => o.ConfigKey, OperationMethod.Equal, configKey)).FirstOrDefault().ConfigValue;
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(GlobalConfigDal), "GetValue", Engineer.ccc, configKey, ex);
            }
            return configValue;
        }

        public GlobalConfig GetGlobalConfig(string configKey)
        {
            GlobalConfig gc = null;
            
            try
            {
                if (!string.IsNullOrEmpty(configKey))
                {
                    using (var db = ReadOnlySanNongDunConn())
                    {
                        gc = db.Query<GlobalConfig>(SqlQuery<GlobalConfig>.Builder(db).AndWhere(o => o.ConfigKey, OperationMethod.Equal, configKey)).FirstOrDefault();
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(GlobalConfigDal), "GetGlobalConfig", Engineer.ccc, gc, ex);
            }
            return gc;
        }



        /// <summary>
        /// 修改账户  
        /// sj
        /// </summary>
        /// <param name="customerAccount">修改账户实体</param>
        /// <returns></returns>
        public bool UpdateGlobalConfig(GlobalConfig gc)
        {

            bool result = false;
            try
            {
                if (gc != null)
                {
                    using (var db = BaseDal.WriteSanNongDunDbBase())
                    {
                        result = db.Update(gc);
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(GlobalConfigDal), "UpdateGlobalConfig", Engineer.ccc, gc, ex);
            }
            return result;
        }
        /// <summary>
        /// 根据配置名称获取值
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public string GetValueByConfigName(string configName)
        {
            string result = string.Empty;

            try
            {
                using (var db = ReadOnlySanNongDunConn())
                {
                    result = db.Query<GlobalConfig>(SqlQuery<GlobalConfig>.Builder(db).AndWhere(o => o.ConfigKey, OperationMethod.Equal, configName)).FirstOrDefault().ConfigValue;
                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(GlobalConfigDal), "GetValueByConfigName", Engineer.ggg, new { configName = configName }, ex);
            }

            return result;
        }
    }
}
