using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common.Web
{
    /// <summary>
    /// 是否激活（用户未激活时不能访问首页以外的页面）
    /// </summary>
    public class IsActivateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool invalidRequestUrl = true;

            //获取BaseController的服务上下文
            var serviceContext = filterContext.Controller.GetServiceContext();

            if (serviceContext != null && serviceContext.CurrentUser != null && serviceContext.CurrentUser.IsActivate == 1)
            {
                //用户已激活
                invalidRequestUrl = false;
            }

            if (invalidRequestUrl)//用户未激活，跳转激活页面
            {
                //跳转到激活页面
                string ActivateUrl = PubConstant.ShowBaseUrl.TrimEnd('/');
                filterContext.Result = new RedirectResult(ActivateUrl +
                    new UrlHelper(filterContext.RequestContext).RouteUrl("Default", new { action = "activate", controller = "home" }));
            }
        }

 

    }
}
