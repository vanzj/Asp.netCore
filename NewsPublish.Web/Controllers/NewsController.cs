using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Model.Response;
using NewsPublish.Service;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewsPublish.Web.Controllers
{
    public class NewsController : Controller
    {

        private NewsService _newsService;
       
        public NewsController(NewsService newsService)
        {
            _newsService = newsService;
        
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
    }
}
