using System;
using System.Collections.Generic;

namespace NewsPublish.Model.Entity
{
    public class NewsClassify
    {
        public NewsClassify() => News = new HashSet<News>();
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Sort
        {
            get;
            set;
        }
        public string Remark
        {
            get;
            set;
        }
        public virtual ICollection<News> News
        {
            get;
            set;
        }
    }
}
