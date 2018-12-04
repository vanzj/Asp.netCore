using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;
using NewsPublish.Service;
using Microsoft.AspNetCore.Hosting;
using System.IO;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewsPublish.Web.Areas.admin.Controllers
{ 
    [Area("admin")]
    public class BannerController : Controller
    {
        private BannerService _bannerService;
        private IHostingEnvironment _hosting;


        public BannerController ( BannerService bannerService, IHostingEnvironment hosting)

        {
            _bannerService = bannerService;
            _hosting = hosting;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var banner = _bannerService.GetBannerList();

            return View(banner);
        }
        public IActionResult BannerAdd(){
            return View();
        }
        [HttpPost]
        public async Task<JsonResult>AddBanner(AddBanner addBanner,IFormCollection collection){
            var files = collection.Files;
            if(files.Count>0){

              
                var webRootPath = _hosting.WebRootPath;
                string relativeDirPath = @"/BannerPic";
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
                    var filePath = absolutePath + "\\" + fileName;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await files[0].CopyToAsync(stream);
                    }
                    //数据库操作
                    addBanner.Image = "/BannerPic/" + fileName;

                    return Json(_bannerService.AddBanner(addBanner));
                }

                return Json(new ResponseModel { code = 0, result = "图片格式有误" });
            }
       
            return Json(new ResponseModel { code = 0, result = "请上传文件" });
        } 

        public JsonResult DeleteBanner(int BannerId){
            if (BannerId < 0)
                return Json(new ResponseModel { code = 0, result = "参数有误" });
            return Json(_bannerService.DeleteBanner(BannerId));

        }
    }
}
