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

namespace SanNongDunWeb.Controllers
{
    public class RecordController : BaseController
    {
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

        public ActionResult StatusLogIndex(int pageIndex = 1, int type = 1)
        {

            Page<StatusViewModel> pageView = new Page<StatusViewModel>();
            pageView.Data = new List<StatusViewModel>();
            string titleName = "静态记录";
            Page<StatusLog> page = new Page<StatusLog>();
            UserInfo userinfo = Get();
            if (userinfo != null)
            {
                page = _StatusLogBll.GetStatusLog(userinfo.UserId, pageIndex, pageSize, type);
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
                    

                        UserInfo user = _UserBll.GetUserInfoById(item.UserId);
                        viewModel.UserName = user.UserName;
                        if (type == 2)
                        {
                            UserInfo userRe = _UserBll.GetUserInfoById(item.ReUserId);
                            viewModel.ReUserName = userRe.UserName;
                        }
                    
                        pageView.Data.Add(viewModel);
                    }
                }
                pageView.PageIndex = page.PageIndex;
                pageView.PageSize = page.PageSize;
                pageView.PageYe = page.PageYe;
                pageView.TotalCount = page.TotalCount;
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_StatusLogIndex", pageView);
            }
            if (type == 2)
            {
                titleName = "推荐记录";
            }
            ViewBag.titleName = titleName;
            ViewBag.type = type;
            return View(pageView);

           

        }

        public ActionResult HongBaoLogIndex(int pageIndex = 1, int type = 1)
        {
            Page<HongBaoViewModel> pageView = new Page<HongBaoViewModel>();
            pageView.Data =new List<HongBaoViewModel>();
            string titleName = "红包记录";
            Page<HongBaoLog> page = new Page<HongBaoLog>();
            UserInfo userinfo = Get();
            if (userinfo != null)
            {
                page = _HongBaoLogBll.GetHongBaoLog(userinfo.UserId, pageIndex, pageSize, type);
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

                       UserInfo user= _UserBll.GetUserInfoById(item.UserId);
                        viewModel.UserName = user.UserName;

                        if (type == 2)
                        {
                            UserInfo userRe = _UserBll.GetUserInfoById(item.ReUserId);
                            viewModel.ReUserName = userRe.UserName;
                        }
                       
                        pageView.Data.Add( viewModel);
                    }
                }
                pageView.PageIndex = page.PageIndex;
                pageView.PageSize = page.PageSize;
                pageView.PageYe = page.PageYe;
                pageView.TotalCount = page.TotalCount;
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_HongBaoLogIndex", pageView);
            }
            if (type == 2)
            {
                titleName = "业绩红包";
            }
            ViewBag.titleName = titleName;
            ViewBag.type = type;
            return View(pageView);
        }
        public ActionResult ScoreLogIndex(int pageIndex = 1)
        {
            Page<ScoreViewModel> pageView = new Page<ScoreViewModel>();
            pageView.Data = new List<ScoreViewModel>();
           
            Page<ScoreLog> page = new Page<ScoreLog>();
            UserInfo userinfo = Get();
            if (userinfo != null)
            {
                page = _ScoreLogBll.GetScoreLog(userinfo.UserId, pageIndex, pageSize);
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


                        UserInfo user = _UserBll.GetUserInfoById(item.UserId);
                        viewModel.UserName = user.UserName;
                       

                        pageView.Data.Add(viewModel);
                    }
                }
                pageView.PageIndex = page.PageIndex;
                pageView.PageSize = page.PageSize;
                pageView.PageYe = page.PageYe;
                pageView.TotalCount = page.TotalCount;
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ScoreLogIndex", pageView);
            }
           
          
          
            return View(pageView);

        }
        public UserInfo Get()
        {
            UserInfo userinfo = null;
            if (_ServiceContext != null && _ServiceContext.SND_CurrentUser != null && _ServiceContext.SND_CurrentUser.UserId > 0)
            {
                userinfo = _UserBll.GetUserInfoById(_ServiceContext.SND_CurrentUser.UserId);
            }

            return userinfo;
        }
    }
}
