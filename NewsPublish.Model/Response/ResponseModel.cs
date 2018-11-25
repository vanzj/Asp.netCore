using System;
namespace NewsPublish.Model.Response
{
    public class ResponseModel
    {
        public ResponseModel()
        {
        }
        public int code
        {
            get;
            set;
        }
        public string result
        {
            get;
            set;
        }
        public dynamic data { get; set; }
    }
}
