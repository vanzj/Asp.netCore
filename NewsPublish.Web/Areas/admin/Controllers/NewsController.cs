using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Model.Entity;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;
using NewsPublish.Service;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewsPublish.Web.Areas.admin.Controllers
{
    [Area("admin")]
    public class NewsController : Controller
    {
        private NewsService _newsService;
        private IHostingEnvironment _hosting;

        public NewsController (NewsService newsService ,IHostingEnvironment hosting){
            _newsService = newsService;
            _hosting = hosting;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult News()
        {
            var newsclassifies = _newsService.GetNewsClassifyList();
            return View(newsclassifies);
        }

        [HttpGet]
        public JsonResult GetNews(int pagesize,int pageIndex,int classifyId   ,string keyword)
        {
            List<Expression<Func<News, bool>>> wheres = new List<Expression<Func<News, bool>>>();
            if (classifyId>0)
            {
                wheres.Add(c => c.NewsClassifyId == classifyId);

            }
            if (!string.IsNullOrEmpty(keyword))
            {
                wheres.Add(c => c.Title == keyword);
            }

            int total = 0;
            var newses = _newsService.NewsPageQuery(pagesize, pageIndex, out total, wheres);
            return Json(new{ total=total ,data= newses });
        }
        public ActionResult NewsClassify(){
            var newsClassifies = _newsService.GetNewsClassifyList();
            return View(newsClassifies);
        }
        public ActionResult NewsClassifyEdit(int Id)
        {
            var newsClassify = _newsService.GetOneNewsClassify(Id);
            return View(newsClassify);
        }
        public ActionResult NewsClassifyAdd()
        {          
            return View();
        }

     

        [HttpPost]
        public JsonResult NewsClassifyAdd(AddNewsClassify addNewsClassify)
        {
            if (string.IsNullOrWhiteSpace(addNewsClassify.Name))
                return Json( new ResponseModel() { code = 0, result = "标题不能为空" });

            return Json(_newsService.AddNewsClassify(addNewsClassify));
        }
        [HttpPost]
        public JsonResult NewsClassifyEdit(EditNewsClassify editNewsClassify)
        {
            if (string.IsNullOrWhiteSpace(editNewsClassify.Name))
                return Json(new ResponseModel() { code = 0, result = "标题不能为空" });

            return Json(_newsService.EditNewsClassify(editNewsClassify));
        }
    }
}
