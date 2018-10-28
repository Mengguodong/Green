using Bll;
using DataModel.RequestModel;
using DataModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace SanNongDunManagerment.Controllers
{
    public class LogInfoTableController : Common.Web.BaseController
    {
        LogBll _logBll = new LogBll();
        //
        // GET: /LogInfoTable/

        public ActionResult Index(string userName,int adminLogType=1,int logType=1,int pageIndex=1)
        {
              string headerTitle="";
            int pageSize = 15;
            Page<UserLogInfoModel> page = new Page<UserLogInfoModel>();
            PagedList<UserLogInfoModel> pageList = null;
            page =_logBll.GetPageUserLog(userName, pageIndex, pageSize, adminLogType, logType);

            if (page.Data != null)
            {
                pageList = new PagedList<UserLogInfoModel>(page.Data, pageIndex, pageSize, page.TotalCount);


                ViewBag.TotalCount = page.TotalCount;
            }
            ViewData["logType"] = logType;

            if (Request.IsAjaxRequest())
            {
                if (logType == 3)
                {
                    return PartialView("_LogInfoStatic", pageList);
                }
                else 
                {
                    return PartialView("_LogInfo", pageList);
                }
                
            }
            switch (logType)
            {
                case 1:
                    headerTitle = "充值记录";
                    break;
                case 2:
                    headerTitle = "提现记录";
                    break;
                case 3:
                    headerTitle = "静态释放";
                    break;
                case 4:
                    headerTitle = "动态释放";
                    break;
                case 5:
                    headerTitle = "复投EP";
                    break;
            }

            ViewBag.HeaderTitle = headerTitle;
        

            return View(pageList);
        }

    }
}
