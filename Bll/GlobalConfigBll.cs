using Dal;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    public class GlobalConfigBll
    {
        GlobalConfigDal _dal = new GlobalConfigDal();
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public string GetValueByConfigName(string configName)
        {
            return _dal.GetValueByConfigName(configName);
        }
        /// <summary>
        /// 修改配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool UpdateConfig(GlobalConfig config)
        {
            return _dal.UpdateGlobalConfig(config);
        }
    }
}
