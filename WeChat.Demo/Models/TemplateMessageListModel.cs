using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Demo.Models
{
    public class TemplateMessageListModel
    {
        /// <summary>
        /// 
        /// </summary>
        public List<TemplateMessageInfo> template_list { get; set; }
    }

    public class TemplateMessageInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 寰呭姙浠诲姟鎻愰啋
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string primary_industry { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deputy_industry { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string example { get; set; }
    }
}