using Bll;
using Common;
using DataModel;
using EveryDayEpService;
using SndApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SanNongDunWeb.Controllers
{
    public class TestController : Controller
    {
        UserBll _userBll=new UserBll();
        AccountInfo _accountBll = new AccountInfo();
        //
        // GET: /Test/
        TestBll t = new TestBll();
        public ActionResult Index()
        {
            ReturnModel modela = new ReturnModel();
            ReturnModel modelb = new ReturnModel();
            ReturnModel modelc = new ReturnModel();
            try
            {
                string url = PubConstant.WineGameWebApi + "api/userinfo/getuserep?username=15910685563";

                modela = HttpClientHelper.GetResponse<ReturnModel>(url);

                string urlb = PubConstant.WineGameWebApi + "api/userinfo/UpdateUserEp?username=15910685563&ep=500&plusType=2";

                modelb = HttpClientHelper.GetResponse<ReturnModel>(urlb);

                string urlc = PubConstant.WineGameWebApi + "api/userinfo/PlusActivationCard?username=15910685563&count=1";

                modelc = HttpClientHelper.GetResponse<ReturnModel>(urlc);
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(ReturnModel), "InserUser", Engineer.ggg, modela, ex);
            }

            return View(modela);
        }

        //public ActionResult IndexShow() 
        //{
        //    t.IndexShow();
        //    return View();
        //}

    }
}
