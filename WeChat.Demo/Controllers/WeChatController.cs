using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WeChat.Demo.Common;
using WeChat.Demo.Service;

namespace WeChat.Demo.Controllers
{
    public class WeChatController : Controller
    {
        private readonly  WeChatService _weChatService = new WeChatService();

        #region Token验证/消息处理

        public ActionResult WeChatProcessRequest()
        {
            string response;

            // Post 是消息处理
            // Get 是Token 验证
            if (Request.HttpMethod.ToLower() == "post")
            {
                var requestStream =Request.InputStream;
                byte[] requestByte = new byte[requestStream.Length];
                requestStream.Read(requestByte, 0, (int)requestStream.Length);
                var requestMessage = Encoding.UTF8.GetString(requestByte);
                response = _weChatService.MessageService(requestMessage);
            }
            else
            {
                // Token 验证
                var signature = Request.QueryString["signature"];
                var timestamp = Request.QueryString["timestamp"];
                var nonce = Request.QueryString["nonce"];
                var echoStr = Request.QueryString["echoStr"];
                response = _weChatService.VerifyToken(signature, timestamp, nonce, echoStr);
            }

            return Content(response);
        }


        #endregion

        #region 获取 AccessToken

        /// <summary>
        /// 获取 AccessToken
        /// 文档：https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421140183
        /// </summary>
        public string AccessToken
        {
            get
            {
                if (Session["AccessToken"] != null)
                {
                    return (string)Session["AccessToken"];
                }

                var jss = new JavaScriptSerializer();
                var response = HttpHelper.HttpGet(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}", WeChatConsts.GrantType, WeChatConsts.AppId, WeChatConsts.AppSecret));
                var respDic = (Dictionary<string, object>)jss.DeserializeObject(response);
                if (respDic["access_token"] == null)
                {
                    return null;
                }

                var accessToken = respDic["access_token"].ToString();
                Session["AccessToken"] = accessToken;
                Session.Timeout = 7200;

                return accessToken;
            }
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public string GetAccessToken(string appId, string appSecret)
        {
            if (Session["AccessToken"] != null)
            {
                return (string)Session["AccessToken"];
            }

            var jss = new JavaScriptSerializer();
            var response = HttpHelper.HttpGet(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}", WeChatConsts.GrantType, WeChatConsts.AppId, WeChatConsts.AppSecret));
            var respDic = (Dictionary<string, object>)jss.DeserializeObject(response);
            if (respDic["access_token"] == null)
            {
                return null;
            }

            var accessToken = respDic["access_token"].ToString();
            Session["AccessToken"] = accessToken;
            Session.Timeout = 7200;

            return accessToken;
        }

        #endregion

        #region 菜单
        
        /// <summary>
        /// 创建菜单
        /// 文档：https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141013
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateMenu()
        {
            var filePath = Server.MapPath("~/App_Data/Menu/_createMenu.txt");
            var result = _weChatService.CreateMenu(filePath, AccessToken);
            return Content(result);
        }

        /// <summary>
        /// 获取菜单列表
        /// 文档：https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141014
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMenuList()
        {
            var result = _weChatService.GetMenuList(AccessToken);
            return Content(result);
        }

        /// <summary>
        /// 删除菜单列表
        /// 文档：https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141015
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteMenu()
        {
            var result = _weChatService.DeleteMenu(AccessToken);
            return Content(result);
        }

        #endregion

        #region 发送模板消息

        public ActionResult SendTemplateMessage()
        {
            _weChatService.SendTemplateMessage(AccessToken);
            return Content("1");
        }


        #endregion 

        #region

        public ActionResult CreateQrcodeTicket()
        {
            var result = _weChatService.CreateQrcodeTicket(WeChatConsts.AccessToken);
            
            return Content(result);
        }

        public void GetQrcodeByTicket(string ticket)
        {
            _weChatService.GetQrcodeByTicket(ticket);
        }

        #endregion

        #region 授权登录


        public ActionResult GenerateAuthQrcode()
        {
            var redirectUrL = Server.UrlEncode(WeChatConsts.RedirectUrL);
            _weChatService.GenerateAuthQrcode(AccessToken, redirectUrL);

            return null;
        }

        #endregion
    }
}