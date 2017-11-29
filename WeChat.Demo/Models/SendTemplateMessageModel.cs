using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Demo.Models
{
    public class SendTemplateMessageModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string touser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Miniprogram miniprogram { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }
    }

    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public keyword first { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public keyword keyword1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public keyword keyword2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public keyword keyword3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public keyword remark { get; set; }
    }
    public class Miniprogram
    {
        /// <summary>
        /// 
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pagepath { get; set; }
    }

    public class First
    {
        /// <summary>
        /// 恭喜你购买成功！
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string color { get; set; }
    }

    public class keyword
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

}