using System;
namespace NewsPublish.Model.Request
{
    public class AddComment
    {
        public AddComment()
        {
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
       
       
    }
}
