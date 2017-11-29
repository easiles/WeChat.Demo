using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Demo.Models
{
    public class UserListModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public UserOpenIdData data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string next_openid { get; set; }
    }
    public class UserOpenIdData
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> openid { get; set; }
    }


}