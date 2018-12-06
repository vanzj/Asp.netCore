using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        public NewsController(NewsService newsService, IHostingEnvironment hosting)
        {
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
        public JsonResult GetNews(int pagesize, int pageIndex, int classifyId, string keyword)
        {
            List<Expression<Func<News, bool>>> wheres = new List<Expression<Func<News, bool>>>();
            if (classifyId > 0)
            {
                wheres.Add(c => c.NewsClassifyId == classifyId);

            }
            if (!string.IsNullOrEmpty(keyword))
            {
                wheres.Add(c => c.Title == keyword);
            }

            int total = 0;
            var newses = _newsService.NewsPageQuery(pagesize, pageIndex, out total, wheres);
            return Json(new { total = total, data = newses.data });
        }

        public ActionResult NewsAdd()
        {
            var newsclassifies = _newsService.GetNewsClassifyList();
            return View(newsclassifies);
        }

        [HttpPost]
      
        public async Task<JsonResult> NewsAdd(AddNews addNews ,IFormCollection collection)
        {
            if (string.IsNullOrEmpty(addNews.Title))
                return Json(new ResponseModel() { code = 0, result = "标题不能为空" });
            if (addNews.NewsClassifyId < 0)
                return Json(new ResponseModel() { code = 0, result = "新闻类别不能为空" });
            var files = collection.Files;
            if (files.Count > 0)
            {



                var webRootPath = _hosting.WebRootPath;
                string relativeDirPath = @"/NewsPic";
                string absolutePath = webRootPath + relativeDirPath;

                //图片

                string[] fileTypes = new string[] { ".gif", ".jpg", ".jpeg", ".png", ".bmp" };
                string extension = Path.GetExtension(files[0].FileName);
                if (fileTypes.Contains(extension.ToLower()))
                {
                    if (!Directory.Exists(absolutePath))
                    {
                        Directory.CreateDirectory(absolutePath);
                    }
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    var filePath = absolutePath + "/" + fileName;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await files[0].CopyToAsync(stream);
                    }
                    //数据库操作
                    addNews.Image = "/NewsPic/" + fileName;

                    return Json(_newsService.AddNews(addNews));
                }

                return Json(new ResponseModel { code = 0, result = "图片格式有误" });
            }

            return Json(new ResponseModel { code = 0, result = "请上传文件" });
          
     
        }

        public   JsonResult NewsDel(int id)
        {
            return Json(_newsService.DeleteOneNews(id));
        }

        #region Classify
        public ActionResult NewsClassify()
        {
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
                return Json(new ResponseModel() { code = 0, result = "标题不能为空" });

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
    #endregion
}
