using System;
namespace NewsPublish.Model.Request
{
    public class AddNews
    {
        public AddNews()
        {
        }

  
        public int NewsClassifyId
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Image
        {
            get;
            set;
        }
        public string Contents
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
