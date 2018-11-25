using System;
using System.Collections.Generic;
using System.Linq;
using NewsPublish.Model.Entity;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;

namespace NewsPublish.Service
{
    /// <summary>
    /// Banner service.
    /// </summary>
    public class BannerService
    {
        private  Db _db;
        public BannerService(Db db)
        {
            _db = db;
        }
        /// <summary>
        /// Adds the banner.
        /// </summary>
        /// <returns>The banner.</returns>
        /// <param name="banner">Banner.</param>
        public ResponseModel AddBanner(AddBanner banner){

            var ba = new Banner { AddTime = DateTime.Now, Image = banner.Image, Url = banner.Url, Remark = banner.Remark };
            _db.Banner.Add(ba);
            int i = _db.SaveChanges();
            if (i > 0)
                return new ResponseModel { code = 200, result = "Banner添加成功" };
            return new ResponseModel { code = 0, result = "Banner添加失败" };
        }
/// <summary>
/// Gets the banner list.
/// </summary>
/// <returns>The banner list.</returns>
        public ResponseModel GetBannerList(){
            var banners = _db.Banner.ToList().OrderByDescending(c => c.AddTime);
            var response =new ResponseModel();
            response.code = 200;
            response.result = "Banner集合获取成功";
            response.data = new List<BannerModel>();
            foreach (var baner in banners)
            {
                response.data.Add(new BannerModel
                {
                    Id = baner.Id,
                    Image = baner.Image,
                    Url = baner.Url,
                    Remark = baner.Remark
                }); 
            }
            return response;
        }
        /// <summary>
        /// Deletes the banner.
        /// </summary>
        /// <returns>The banner.</returns>
        /// <param name="bannerId">Banner identifier.</param>
        public ResponseModel DeleteBanner(int bannerId){
            var banner = _db.Banner.Find(bannerId);
            if (banner == null)
                return new ResponseModel() { code = 0, result = "Banner不存在" };
            _db.Banner.Remove(banner);
            int i = _db.SaveChanges();
            if (i > 0)
                return new ResponseModel { code = 200, result = "Banner删除成功" };
            return new ResponseModel { code = 0, result = "Banner删除失败" };
        }
    }
}
