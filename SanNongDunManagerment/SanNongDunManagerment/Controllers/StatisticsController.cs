using Bll;
using Common.Web;
using DataModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanNongDunManagerment.Controllers
{
    public class StatisticsController : BaseController
    {
        StatisticsBll _bll = new StatisticsBll();
        //
        // GET: /Statistics/

        public ActionResult Index(DateTime? begin,DateTime? end)
        {

            StatisticsViewModel model = new StatisticsViewModel();


            model = _bll.GetStatisticsModel(begin,end);


            model.StartTime = begin;
            model.EndTime = end;
            return View(model);
        }

    }
}
