 using System;
namespace NewsPublish.Model.Response
{
    public class BannerModel
    {
        public BannerModel()
        {
        }
        public int Id { get; set; }
        public string Image
        {
            get;
            set;
        }
        public string Url
        {
            get;
            set;
        }

        public string Remark
        {
            get;
            set;
        }

    }
}
