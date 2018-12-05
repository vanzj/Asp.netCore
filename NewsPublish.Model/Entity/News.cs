using System;
using System.Collections.Generic;

namespace NewsPublish.Model.Entity
{
    public class News

    {


        public News() => NewsComments = new HashSet<NewsComment>();
        public int Id
        {
            get;
            set;
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
        public DateTime PublishDate
        {
            get;
            set;
        }
        public string Remark
        {
            get;
            set;
        }
        public virtual NewsClassify NewClassify
        {
            get;
            set;
        }
        public virtual ICollection<NewsComment> NewsComments
        {
            get;
            set;
        }
    }
}
