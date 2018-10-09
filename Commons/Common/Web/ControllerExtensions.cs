using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Web
{
    /// <summary>
    /// 要获取服务上下文属性,Controller必须继承BaseController基类
    /// 登录是否继承BaseController
    /// 创建人：
    /// 创建时间：2014-10-21
    /// </summary>
    public static class ControllerExtensions
    {
        public static ServiceContext GetServiceContext(this System.Web.Mvc.ControllerBase controllerBase)
        {
            var controller = controllerBase as BaseController;
            if (controller == null)
                throw new Exception("要获取服务上下文属性,Controller必须继承BaseController基类");

            return controller._ServiceContext;
        }

     


    }
    
}
