using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common.Web
{
    /// <summary>
    /// 验证是否登录，登录后自动跳转到/acount/login页面，返回参数returnUrl上一个页面地址
    /// 
    /// 创建时间：2014-10-21
    /// </summary>
    public class LoginCheckedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool invalidRequestUrl = true;
            //var returnUrl = filterContext.HttpContext.Request.RawUrl;
            //获取全路径
            int port = filterContext.HttpContext.Request.Url.Port;//url的端口号
            var returnUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;//完整url地址
            returnUrl = returnUrl.Replace(":" + port.ToString(), "");//将端口号移除
            //获取BaseController的服务上下文
            var serviceContext = filterContext.Controller.GetServiceContext();
            //判定BaseController服务上下文和当前用户是否为空
            if (serviceContext != null && serviceContext.SND_CurrentUser != null)
            {
                //用户已登录，此处可以编写相应的业务逻辑
                invalidRequestUrl = false;
                //SendLoginRecordMsg(serviceContext, returnUrl);
            }
            if (invalidRequestUrl)
            {
                //未登录状态，跳转登录页
                //去除跳转地址栏双“//”问题 
                if (returnUrl.Substring(0, returnUrl.IndexOf(".") + 1) == PubConstant.ShowBaseUrl.TrimEnd('/').Substring(0, PubConstant.ShowBaseUrl.TrimEnd('/').IndexOf(".") + 1))
                {
                    string LoginUrl = PubConstant.ShowBaseUrl.TrimEnd('/');
                    filterContext.Result = new RedirectResult(LoginUrl +
                        new UrlHelper(filterContext.RequestContext).RouteUrl("Default", new { action = "index", controller = "login", returnUrl = returnUrl }));

                }
                else if (returnUrl.Substring(0, returnUrl.IndexOf(".") + 1) == PubConstant.ManagementBaseUrl.TrimEnd('/').Substring(0, PubConstant.ManagementBaseUrl.TrimEnd('/').IndexOf(".") + 1))
                {
                    string LoginUrl = PubConstant.ManagementBaseUrl.TrimEnd('/');
                    filterContext.Result = new RedirectResult(LoginUrl +
                        new UrlHelper(filterContext.RequestContext).RouteUrl("Default", new { action = "index", controller = "login", returnUrl = returnUrl }));

                }
               
            }
        }
       


    }
    ///// <summary>
    ///// 市场后台验证是否登录
    ///// </summary>
    //public class MarketLoginCheckedAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        bool invalidRequestUrl = true;
          
    //        //获取全路径
    //        int port = filterContext.HttpContext.Request.Url.Port;//url的端口号
    //        var returnUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;//完整url地址
    //        returnUrl = returnUrl.Replace(":" + port.ToString(), "");//将端口号移除
    //        //获取BaseController的服务上下文
    //        var serviceContext = filterContext.Controller.GetMarketServiceContext();
    //        //判定BaseController服务上下文和当前用户是否为空
    //        if (serviceContext != null && serviceContext.MarketCurrentUser != null)
    //        {
    //            //用户已登录，此处可以编写相应的业务逻辑
    //            invalidRequestUrl = false;
              
    //        }
    //        if (invalidRequestUrl)
    //        {
    //            //未登录状态，跳转登录页
    //                string LoginUrl = PubConstant.MarketBaseUrl.TrimEnd('/');
    //                filterContext.Result = new RedirectResult(LoginUrl +
    //                    new UrlHelper(filterContext.RequestContext).RouteUrl("Default", new { action = "index", controller = "login", returnUrl = returnUrl }));

    //        }
    //    }
 
    //}

    }

