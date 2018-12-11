using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;
using NewsPublish.Service;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewsPublish.Web.Controllers
{
    public class NewsController : Controller
    {

        private NewsService _newsService;
        private CommentService _commentService;
        public NewsController(NewsService newsService, CommentService commentService)
        {
            _newsService = newsService;
            _commentService = commentService;
        }

        // GET: /<controller>/
        public IActionResult Classify(int id)
        {
            if (id < 0)
                 Response.Redirect("/Home/Index");
            var classify= _newsService.GetOneNewsClassify(id);
            ViewData["ClassifyName"] = "首页";
            ViewData["NewsList"] = new ResponseModel();
            ViewData["NewCommentNewsList"] = new ResponseModel();
            ViewData["Title"] = "首页";
            if (classify.code == 0)
            {
                Response.Redirect("/Home/Index");
            }
            else
            {
                ViewData["ClassifyName"] = classify.data.Name;
                var newsList = _newsService.GetNewList(c => c.NewsClassifyId == id, 6);
                ViewData["NewsList"] = newsList;
                var newCommentNewsList = _newsService.GetNewCommentNewsList(c => c.NewsClassifyId == id, 5);
                ViewData["NewCommentNewsList"] = newCommentNewsList;
                ViewData["Title"] = classify.data.Name;
            }

            return View(_newsService.GetNewsClassifyList());
        }
        // GET: /<controller>/
        public IActionResult Detail(int id)
        {
            if (id < 0)
                Response.Redirect("/Home/Index");
            var News = _newsService.GetOneNews(id);
            ViewData["ClassifyName"] = "首页";
            ViewData["News"] = News;
            ViewData["NewsTitle"] = "未找到";
              ViewData["CommentList"] = new ResponseModel();
            ViewData["RecommendNewsLis"] = new ResponseModel();
            if (News.code == 0)
            {
                Response.Redirect("/Home/Index");
            }
            else
            {
                ViewData["Title"] = News.data.Title;
                ViewData["NewsTitle"] = News.data.Title;
                var commentList = _commentService.GetCommentList(c => c.NewsId == id);
                ViewData["CommentList"] = commentList;
                var recommendNewsLis = _newsService.GetRecommendNewsLis(News.data.Id);
                ViewData["RecommendNewsLis"] = recommendNewsLis;
            }

            return View(_newsService.GetNewsClassifyList());
        }



        [HttpPost]
        public JsonResult GetSearchOneNews(string keyword)
        {
            return Json(_newsService.GetSearchOneNews(c => c.Title.Contains(keyword)&&c.NewClassify.Name == ViewData["ClassifyName"].ToString()));
        }

        public IActionResult Wrong()
        {
            ViewData["Title"] = "404";
            return View(_newsService.GetNewsClassifyList());
        }

        public JsonResult PostComment(AddComment addComment)
        {
            return Json(_commentService.AddComment(addComment));
        }


    }


}

