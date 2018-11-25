using System;
namespace NewsPublish.Model.Request
{
    public class AddComment
    {
        public AddComment()
        {
        }

        public int Id
        {
            get;
            set;
        }
        public int NewsId
        {
            get;
            set;
        }
        public string Contents
        {
            get;
            set;
        }
        public DateTime AddTime
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
