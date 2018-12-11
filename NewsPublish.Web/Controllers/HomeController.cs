using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Service;
using NewsPublish.Web.Models;

namespace NewsPublish.Web.Controllers
{
    public class HomeController : Controller
    {
        private NewsService _newsService;
        private BannerService _bannerService;

        public HomeController(NewsService newsService,BannerService bannerService)
        {
            _newsService = newsService;
            _bannerService=bannerService;
        }




        public IActionResult Index()
        {
            ViewData["Title"] = "首页";
            return View(_newsService.GetNewsClassifyList());
        }

        [HttpGet]
        public JsonResult GetBanner()
        {
            return Json(_bannerService.GetBannerList());
        }
        [HttpGet]
        public JsonResult GetNewsCount()
        {
            return Json(_newsService.GetNewsCount(c=>true));
        }
        [HttpGet]
        public JsonResult GetNews()
        {
            return Json(_newsService.GetNewList(c => true, 6));
        }
        [HttpGet]
        public JsonResult GetOneNews(int id)
        {
            return Json(_newsService.GetOneNews(id));
        }
        [HttpGet]
        public JsonResult GetNewCommentNews()
        {
            return Json(_newsService.GetNewCommentNewsList(c => true, 5));
        }
        [HttpPost]
        public JsonResult GetSearchOneNews(string keyword)
        {
            return Json(_newsService.GetSearchOneNews(c => c.Title.Contains(keyword)));
        }

        public IActionResult Wrong()
        {
            ViewData["Title"] = "404";
            return View(_newsService.GetNewsClassifyList());
        }
    }
}
