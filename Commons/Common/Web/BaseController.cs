using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common.Web
{
    [LoginChecked]
    public class BaseController : Controller
    {
        /// <summary>
        /// 服务上下文，用于获取当前登录用户信息
        /// </summary>
        public ServiceContext _ServiceContext = new ServiceContext();
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResultBase;
            if (viewResult != null)
            {
                //viewResult.ViewData["_CurrentUser"] = _ServiceContext.CurrentUser;
                viewResult.ViewData["_CurrentUser"] = _ServiceContext.SND_CurrentUser;


            }
            base.OnActionExecuted(filterContext);
        }
    }
}
