
namespace WeChat.Demo.Common
{
    public class WeChatMessageConsts
    {
        /*
         * 回复消息xml
         * 文档地址：https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421140543
         */

        /// <summary>
        /// 回复文本消息
        /// </summary>
        public static string ResponseTextMessage
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                        </xml>";
            }
        }

        /// <summary>
        /// 回复图片消息
        /// </summary>
        public static string ResponseImageMessage
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[image]]></MsgType>
                            <Image>
                            <MediaId><![CDATA[{3}]]></MediaId>
                            </Image>
                        </xml>";
            }
        }


    }
}