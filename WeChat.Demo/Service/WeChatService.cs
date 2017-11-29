using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Xml;
using WeChat.Demo.Common;
using WeChat.Demo.Models;

namespace WeChat.Demo.Service
{
    public class WeChatService
    {
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();

        #region Token验证
        /// <summary>
        /// 微信签名验证
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="echostr"></param>
        /// <returns></returns>
        public string VerifyToken(string signature, string timestamp, string nonce, string echostr)
        {
            var result = string.Empty;
            var token = WeChatConsts.Token;

            if (string.IsNullOrEmpty(signature) ||
                string.IsNullOrEmpty(timestamp) ||
                string.IsNullOrEmpty(nonce) ||
                string.IsNullOrEmpty(echostr))
            {
                return result;
            }

            string[] arrTmp = { token, timestamp, nonce };

            Array.Sort(arrTmp);     //字典排序

            string tmpStr = string.Join("", arrTmp);

            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1").ToLower();
            if (tmpStr == echostr)
            {
                result = echostr;
            }
            return result;
        }

        #endregion

        #region 验证JsApi权限配置
        /// <summary>
        /// 获取JsApi权限配置的数组/四个参数
        /// </summary>
        /// <param name="appid">应用id</param>
        /// <param name="appsecret">密钥</param>
        /// <returns>json格式的四个参数</returns>
        //public string GetJsApiInfo(string appid, string appsecret, string accessToken)
        //{
        //    string timestamp = CommonHelper.ConvertDateTimeInt(DateTime.Now).ToString();//生成签名的时间戳
        //    string nonceStr = CommonHelper.GetRandCode(16);//生成签名的随机串
        //    string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString();//当前的地址
        //    string jsapi_ticket = "";
        //    //ticket 缓存7200秒
        //    if (System.Web.HttpContext.Current.Session["jsapi_ticket"] == null)
        //    {
        //        jsapi_ticket = HttpHelper.HttpGet("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + accessToken + "&type=jsapi", "");
        //        System.Web.HttpContext.Current.Session["jsapi_ticket"] = jsapi_ticket;
        //        System.Web.HttpContext.Current.Session.Timeout = 7200;
        //    }
        //    else
        //    {
        //        jsapi_ticket = System.Web.HttpContext.Current.Session["jsapi_ticket"].ToString();
        //    }
        //    Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(jsapi_ticket);
        //    jsapi_ticket = respDic["ticket"].ToString();//获取ticket
        //    string[] ArrayList = { "jsapi_ticket=" + jsapi_ticket, "timestamp=" + timestamp, "noncestr=" + nonceStr, "url=" + url };
        //    Array.Sort(ArrayList);
        //    string signature = string.Join("&", ArrayList);
        //    signature = FormsAuthentication.HashPasswordForStoringInConfigFile(signature, "SHA1").ToLower();
        //    return "{\"appId\":\"" + appid + "\", \"timestamp\":" + timestamp + ",\"nonceStr\":\"" + nonceStr + "\",\"signature\":\"" + signature + "\"}";
        //}
        #endregion

        #region 消息处理

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public string MessageService(string xml)
        {
            string message;
            try
            {
                var model = RequestMessage(xml);
                return ResponseMessage(model);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

        /// <summary>
        /// 请求消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public WeChatMessageModel RequestMessage(string message)
        {
            var model = new WeChatMessageModel();

            if (string.IsNullOrEmpty(message))
            {
                return null;
            }

            var requestDocXml = new XmlDocument();
            requestDocXml.LoadXml(message);

            var rootElement = requestDocXml.DocumentElement;
            if (rootElement != null)
            {
                var toUserNameNode = rootElement.SelectSingleNode("ToUserName");
                if (toUserNameNode != null)
                    model.ToUserName = toUserNameNode.InnerText;

                var fromUserNameNode = rootElement.SelectSingleNode("FromUserName");
                if (fromUserNameNode != null)
                    model.FromUserName = fromUserNameNode.InnerText;

                var createTimeNode = rootElement.SelectSingleNode("CreateTime");
                if (createTimeNode != null)
                    model.CreateTime = createTimeNode.InnerText;

                var mediaIdNode = rootElement.SelectSingleNode("MediaId");
                if (mediaIdNode != null)
                    model.MediaId = mediaIdNode.InnerText;

                var msgTypeNode = rootElement.SelectSingleNode("MsgType");
                if (msgTypeNode != null)
                    model.MsgType = msgTypeNode.InnerText;

                switch (model.MsgType)
                {
                    case "text"://文本
                        var contentNode = rootElement.SelectSingleNode("Content");
                        if (contentNode != null)
                            model.Content = contentNode.InnerText;
                        break;
                    case "image"://图片
                        var picUrlNode = rootElement.SelectSingleNode("PicUrl");
                        if (picUrlNode != null)
                            model.PicUrl = picUrlNode.InnerText;
                        break;
                    case "voice"://语音
                        var formatNode = rootElement.SelectSingleNode("Format");
                        if (formatNode != null)
                            model.Format = formatNode.InnerText;
                        break;
                    case "location"://地理位置
                        var locationXNode = rootElement.SelectSingleNode("Location_X");
                        if (locationXNode != null)
                            model.Location_X = locationXNode.InnerText;

                        var locationYNode = rootElement.SelectSingleNode("Location_Y");
                        if (locationYNode != null)
                            model.Location_X = locationYNode.InnerText;

                        var scaleNode = rootElement.SelectSingleNode("Scale");
                        if (scaleNode != null)
                            model.Scale = scaleNode.InnerText;

                        var labelNode = rootElement.SelectSingleNode("Label");
                        if (labelNode != null)
                            model.Label = labelNode.InnerText;
                        break;

                    case "link"://链接
                        var titleNode = rootElement.SelectSingleNode("Title");
                        if (titleNode != null)
                            model.Title = titleNode.InnerText;
                        var descriptionNode = rootElement.SelectSingleNode("Description");
                        if (descriptionNode != null)
                            model.Description = descriptionNode.InnerText;
                        var urlNode = rootElement.SelectSingleNode("Url");
                        if (urlNode != null)
                            model.Url = urlNode.InnerText;
                        break;

                    case "event"://事件
                        var xmlNode1 = rootElement.SelectSingleNode("Event");
                        if (xmlNode1 != null)
                            model.Event = xmlNode1.InnerText;
                        if (model.Event != "TEMPLATESENDJOBFINISH")//关注类型
                        {
                            var node1 = rootElement.SelectSingleNode("EventKey");
                            if (node1 != null)
                                model.EventKey = node1.InnerText;
                        }
                        break;
                }
            }
            return model;
        }


        /// <summary>
        /// 相应消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ResponseMessage(WeChatMessageModel model)
        {
            var result = string.Empty;

            switch (model.MsgType)
            {
                case "text"://文本回复
                    result = ResponseText(model.FromUserName, model.ToUserName, string.Format("收到文本：{0}", model.Content));
                    break;
                case "image"://图片回复
                    result = ResponseText(model.FromUserName, model.ToUserName, string.Format("收到图片：{0}", model.PicUrl));
                    break;
                case "voice"://语音回复
                    result = ResponseText(model.FromUserName, model.ToUserName, "收到语音");
                    break;
                case "video"://视频回复
                    result = ResponseText(model.FromUserName, model.ToUserName, "收到视频");
                    break;
                case "music"://音乐回复
                    result = ResponseText(model.FromUserName, model.ToUserName, "收到音乐");
                    break;
                case "news"://图文回复
                    result = ResponseText(model.FromUserName, model.ToUserName, "收到图文");
                    break;
                case "link"://链接回复
                    result = ResponseText(model.FromUserName, model.ToUserName, string.Format("收到：{0}",model.Title));
                    break;
                case "location"://地理位置回复
                    result = ResponseText(model.FromUserName, model.ToUserName, string.Format("收到地理位置：{0}",model.Label));
                    break;
                case "event":
                    switch (model.Event)
                    {
                        case "subscribe":
                            result = ResponseText(model.FromUserName, model.ToUserName, "欢迎关注测试公众号");
                            break;
                        case "SCAN":
                            break;
                    }
                    break;
                default://默认回复
                    result = ResponseText(model.FromUserName, model.ToUserName, "未能解析出你要回复的值");
                    break;
            }
            return result;
        }


        #region 回复方式
        /// <summary>
        /// 回复文本
        /// </summary>
        /// <param name="fromUserName">发送给谁(openid)</param>
        /// <param name="toUserName">来自谁(公众账号ID)</param>
        /// <param name="content">回复类型文本</param>
        /// <returns>拼凑的XML</returns>
        public string ResponseText(string fromUserName, string toUserName, string content)
        {
            var responseMessage = WeChatMessageConsts.ResponseTextMessage;
            var response = string.Format(responseMessage, fromUserName, toUserName, CommonHelper.ConvertDateTimeInt(DateTime.Now), content);
            return response;
        }

        /// <summary>
        /// 图片消息处理
        /// </summary>
        /// <param name="fromUserName"></param>
        /// <param name="toUserName"></param>
        /// <param name="icUrl"></param>
        /// <returns></returns>
        public string ResponseImage(string fromUserName, string toUserName, string icUrl)
        {
            var responseMessage = WeChatMessageConsts.ResponseImageMessage;
            var response = string.Format(responseMessage, fromUserName, toUserName, CommonHelper.ConvertDateTimeInt(DateTime.Now), icUrl);
            return response;
        }

        #endregion
        #endregion 

        #region 菜单

        /// <summary>
        /// 创建菜单
        /// 文档：https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141013
        /// </summary>
        /// <returns></returns>
        public string CreateMenu(string filePath, string accessToken)
        {
            var result = string.Empty;

            var url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", accessToken);
            var menuJson = FileHelper.GetFileContent(filePath);


            if (string.IsNullOrEmpty(menuJson))
            {
                result = "获取_createMenu.txt为空";
                return result;
            }
            try
            {
                var response = HttpHelper.HttpPost(url, menuJson);
                result = response;
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("创建菜单异常: Message：{0}, InnerException：{1}, StackTrace:{2}", ex.Message, ex.InnerException, ex.StackTrace));
            }


            return result;
        }

        /// <summary>
        /// 获取菜单列表
        /// 文档：https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141014
        /// </summary>
        /// <returns></returns>
        public string GetMenuList(string accessToken)
        {

            var result = string.Empty;
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", accessToken);

            try
            {
                var response = HttpHelper.HttpGet(url);

                // 解析
                result = response;
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("获取菜单列表异常：Message：{0}, InnerException：{1}, StackTrace:{2}", ex.Message, ex.InnerException, ex.StackTrace));
            }

            return result;
        }

        /// <summary>
        /// 删除菜单列表
        /// 文档：https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141015
        /// </summary>
        /// <returns></returns>
        public string DeleteMenu(string accessToken)
        {
            var result = string.Empty;
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", accessToken);

            try
            {
                var response = HttpHelper.HttpGet(url);

                // 解析
                result = response;
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("删除菜单列表异常：Message：{0}, InnerException：{1}, StackTrace:{2}", ex.Message, ex.InnerException, ex.StackTrace));
            }

            return result;
        }

        #endregion 

        #region 二维码

        /// <summary>
        /// 创建二维码ticket
        /// </summary>
        /// <returns></returns>
        public string CreateQrcodeTicket(string accessToken)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}", accessToken);

            var json = "{\"expire_seconds\": 604800, \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": 123}}}";

            try
            {
                var response = HttpHelper.HttpPost(url, json);

                Dictionary<string, object> reDic = (Dictionary<string, object>)_serializer.DeserializeObject(response);
                return reDic.ContainsKey("ticket") ? reDic["ticket"].ToString() : reDic["errcode"].ToString();
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("创建二维码: Message：{0}, InnerException：{1}, StackTrace:{2}", ex.Message, ex.InnerException, ex.StackTrace));
                return null;
            }
            
            return null;
        }


        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <returns></returns>
        public void GetQrcodeByTicket(string ticket)
        {
            string url = string.Format("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}", ticket);

            WebRequest webreq = WebRequest.Create(url);
            //红色部分为文件URL地址，这里是一张图片
            WebResponse webres = webreq.GetResponse();
            Stream stream = webres.GetResponseStream();
            if (stream != null)
            {
                System.Drawing.Image.FromStream(stream);
                stream.Close();
            }
        }

        #endregion 

        #region 用户信息

        /// <summary>
        /// 获取用户列表
        /// 文档地址：https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421140840
        /// 当关注着超过1000,需要通过 next_openid 来获取
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public UserListModel GetUserAppIdList(string accessToken)
        {
            UserListModel result;
            var url =
                string.Format(
                    "https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}", accessToken);


            var response = HttpHelper.HttpGet(url);

            if (response == null)
            {
                return null;
            }

            try
            {
                result = _serializer.Deserialize<UserListModel>(response);
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;

        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="opemId"></param>
        /// <returns></returns>
        public UserInfoModel GetUserInfo(string accessToken, string opemId)
        {
            UserInfoModel result;
            var url =
                string.Format(
                    " https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", accessToken, opemId);


            var response = HttpHelper.HttpGet(url);

            if (response == null)
            {
                return null;
            }

            try
            {
                result = _serializer.Deserialize<UserInfoModel>(response);
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;

        }

        #endregion

        #region 发送模板消息

        /// <summary>
        /// 设置所属行业
        /// </summary>
        /// <param name="accessToken"></param>
        public void SetIndustry(string accessToken)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/template/api_set_industry?access_token={0}", accessToken);


            var json = "{\"industry_id1\": 1, \"industry_id2\": 2}";

           
            try
            {
                var response = HttpHelper.HttpPost(url, json);
                Dictionary<string, object> reDic = (Dictionary<string, object>)_serializer.DeserializeObject(response);
            }
            catch (Exception)
            {
                //TODO 记录日志   
            }
        
        }

        /// <summary>
        /// 获取模板消息列表
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public TemplateMessageListModel GetTemplateMessageList(string accessToken)
        {
            var result = new TemplateMessageListModel()
            {
                template_list = new List<TemplateMessageInfo>()
            };
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/template/get_all_private_template?access_token={0}",accessToken);

            

            try
            {
                var response = HttpHelper.HttpGet(url);
                result = _serializer.Deserialize<TemplateMessageListModel>(response);
            }
            catch (Exception ex)
            {
                
            }
            return result;
        }


        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="accessToken"></param>
        public void SendTemplateMessage(string accessToken)
        {
            var sendMessage = new SendTemplateMessageModel()
            {
                touser = "ozCXO05WuHEA51V6ZC4agkUgLxwY",
                template_id = "BYUiwQ4SE32zlvjiSIjX44JUP-r0vyR2cdAdKHBED1M",
                url = "https://www.baidu.com/",
                data = new Data()
                {
                    first = new keyword()
                    {
                        value = "代办事项测试"
                    },
                    keyword1 = new keyword() { value = "XXX" },
                    keyword2 = new keyword() { value = "11月19日"},
                    keyword3 = new keyword() { value = "微信公众号测试"},
                    remark = new keyword() { value = "一切正常"}
                }
            };

            var json = _serializer.Serialize(sendMessage);

            var url = string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", accessToken);

            try
            {
                var response = HttpHelper.HttpPost(url, json);
                //TODO 解析返回数据
            }
            catch (Exception ex)
            {
                // TODO 记录日志
            }
        
        }

        #endregion

        #region 微信网页授权

        /// <summary>
        /// 
        /// </summary>
        public void GenerateAuthQrcode(string accessToken, string redirectUrL)
        {
            var url =
                string.Format(
                    "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect", WeChatConsts.AppId, redirectUrL);


        }


        #endregion

    }
}