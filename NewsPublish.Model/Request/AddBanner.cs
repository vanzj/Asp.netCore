using System;
namespace NewsPublish.Model.Request
{
    public class AddBanner
    {
        public AddBanner()
        {
        }
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
