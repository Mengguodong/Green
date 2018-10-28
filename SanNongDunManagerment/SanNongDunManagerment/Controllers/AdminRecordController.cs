using Bll;
using Common.Web;
using DataModel;
using DataModel.LiuViewModel;
using DataModel.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;






namespace SanNongDunManagerment.Controllers
{
    public class AdminRecordController : BaseController
    {
        //
        // GET: /Record/

        HongBaoLogBll _HongBaoLogBll = new HongBaoLogBll();
        ScoreLogBll _ScoreLogBll = new ScoreLogBll();
        StatusLogBll _StatusLogBll = new StatusLogBll();
        UserBll _UserBll = new UserBll();
        int pageSize = 15;

        // GET: /Record/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="type">1:静态,2:直推</param>
        /// <returns></returns>
        public ActionResult AdminStatusLogIndex(string userName="",int pageIndex = 1, int type = 1)
        {
            UserInfo user = new UserInfo();

            if (!string.IsNullOrEmpty(userName))
            {
                user = _UserBll.GetUserInfoByUserName(userName);
            }
            int userId = 0;
            if (user != null)
            {
                userId = user.UserId;
            }
            PagedList<StatusViewModel> pageList = null;

            Page<StatusViewModel> pageView = new Page<StatusViewModel>();
            pageView.Data = new List<StatusViewModel>();
            string titleName = "静态记录";
            Page<StatusLog> page = new Page<StatusLog>();


            page = _StatusLogBll.AdminGetStatusLog(userId, pageIndex, pageSize, type);
                if (page.Data != null)
                {
                    foreach (var item in page.Data)
                    {
                        StatusViewModel viewModel = new StatusViewModel();
                        viewModel.CreateTime = item.CreateTime;
                        viewModel.LogCount = item.LogCount;
                        viewModel.LogType = item.LogType;
                        viewModel.ReUserId = item.ReUserId;
                        viewModel.StatusLogId = item.StatusLogId;
                        viewModel.UserId = item.UserId;


                        UserInfo userInfo = _UserBll.GetUserInfoById(item.UserId);
                        viewModel.UserName = userInfo.UserName;
                        if (type == 2)
                        {
                            UserInfo userRe = _UserBll.GetUserInfoById(item.ReUserId);
                            viewModel.ReUserName = userRe.UserName;
                        }

                        pageView.Data.Add(viewModel);
                    }
                //}
                pageView.PageIndex = page.PageIndex;
                pageView.PageSize = page.PageSize;
                pageView.PageYe = page.PageYe;
                pageView.TotalCount = page.TotalCount;
                pageList = new PagedList<StatusViewModel>(pageView.Data, page.PageIndex, page.PageSize, page.TotalCount);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AdminStatusLogIndex", pageList);
            }
            //if (type == 2)
            //{
            //    titleName = "推荐记录";
            //}
            //ViewBag.titleName = titleName;
          ViewData["type"] = type;
            return View(pageList);



        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="type">1:自己红包,2:业绩红包</param>
        /// <returns></returns>
        public ActionResult AdminHongBaoLogIndex(string userName = "", int pageIndex = 1, int type = 1)
        {
            UserInfo user = new UserInfo();

            if (!string.IsNullOrEmpty(userName))
            {
                user = _UserBll.GetUserInfoByUserName(userName);
            }
            int userId = 0;
            if (user != null)
            {
                userId = user.UserId;
            }
            PagedList<HongBaoViewModel> pageList = null;

            Page<HongBaoViewModel> pageView = new Page<HongBaoViewModel>();
            pageView.Data = new List<HongBaoViewModel>();
            string titleName = "红包记录";
            Page<HongBaoLog> page = new Page<HongBaoLog>();
          

            page = _HongBaoLogBll.AdminGetHongBaoLog(userId, pageIndex, pageSize, type);
                if (page.Data != null)
                {
                    foreach (var item in page.Data)
                    {
                        HongBaoViewModel viewModel = new HongBaoViewModel();
                        viewModel.CreateTime = item.CreateTime;
                        viewModel.HongBaoCount = item.HongBaoCount;
                        viewModel.LogCount = item.LogCount;
                        viewModel.LogId = item.LogId;
                        viewModel.LogType = item.LogType;
                        viewModel.ReUserId = item.ReUserId;
                        viewModel.UserId = item.UserId;

                        UserInfo userInfo = _UserBll.GetUserInfoById(item.UserId);
                        viewModel.UserName = userInfo.UserName;

                        if (type == 2)
                        {
                            UserInfo userRe = _UserBll.GetUserInfoById(item.ReUserId);
                            viewModel.ReUserName = userRe.UserName;
                        }

                        pageView.Data.Add(viewModel);
                    }
                
                pageView.PageIndex = page.PageIndex;
                pageView.PageSize = page.PageSize;
                pageView.PageYe = page.PageYe;
                pageView.TotalCount = page.TotalCount;

                pageList = new PagedList<HongBaoViewModel>(pageView.Data, page.PageIndex, page.PageSize, page.TotalCount);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AdminHongBaoLogIndex", pageList);
            }
            //if (type == 2)
            //{
            //    titleName = "业绩红包";
            //}
            //ViewBag.titleName = titleName;
            ViewData["type"] = type;
            return View(pageList);
        }

        public ActionResult AdminScoreLogIndex(string userName="", int pageIndex = 1)
        {
            UserInfo user = new UserInfo();
         
            if (!string.IsNullOrEmpty(userName))
            {
                user = _UserBll.GetUserInfoByUserName(userName);
            }
            int userId = 0;
            if (user != null) 
            {
                userId = user.UserId;
            }

            PagedList<ScoreViewModel> pageList = null;

            Page<ScoreViewModel> pageView = new Page<ScoreViewModel>();
            pageView.Data = new List<ScoreViewModel>();

            Page<ScoreLog> page = new Page<ScoreLog>();

            page = _ScoreLogBll.AdminGetScoreLog(userId, pageIndex, pageSize);
                if (page.Data != null)
                {
                    foreach (var item in page.Data)
                    {
                        ScoreViewModel viewModel = new ScoreViewModel();
                        viewModel.CreateTime = item.CreateTime;
                        viewModel.LogCount = item.LogCount;
                        viewModel.Remark = item.Remark;
                        viewModel.ScoreLogId = item.ScoreLogId;

                        viewModel.UserId = item.UserId;


                        UserInfo userInfo = _UserBll.GetUserInfoById(item.UserId);
                        viewModel.UserName = userInfo.UserName;


                        pageView.Data.Add(viewModel);
                    }
                
                pageView.PageIndex = page.PageIndex;
                pageView.PageSize = page.PageSize;
                pageView.PageYe = page.PageYe;
                pageView.TotalCount = page.TotalCount;

                pageList = new PagedList<ScoreViewModel>(pageView.Data, page.PageIndex, page.PageSize, page.TotalCount);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AdminScoreLogIndex", pageList);
            }


            return View(pageList);

        }
      

    }
}
