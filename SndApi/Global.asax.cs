using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;

using SndApi.Service;

using System.Web.Routing;



namespace SndApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();

            //将WebApiConfig.Register(GlobalConfiguration.Configuration);改为：GlobalConfiguration.Configure(WebApiConfig.Register);

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            // 使api返回为json 
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear(); 
        }
    }
}
