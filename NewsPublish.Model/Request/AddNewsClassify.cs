using System;
namespace NewsPublish.Model.Request
{
    public class AddNewsClassify
    {
        public AddNewsClassify()
        {
        }

        public string Name
        {
            get;
            set;
        }
        public int Sort
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
