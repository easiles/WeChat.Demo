
namespace WeChat.Demo.Models
{
    /// <summary>
    /// 接受微信的承载实体
    /// </summary>
    public class WeChatMessageModel
    {
        #region Base
        /// <summary>
        /// 消息接收方微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 消息发送方微信号
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 信息类型 地理位置:location,文本消息:text,消息类型:image
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        public string MediaId { get; set; }

        #endregion

        #region 文本
        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content { get; set; }

        #endregion 

        #region 地理位置
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }

        #endregion

        #region 图片
        /// <summary>
        /// 图片链接，开发者可以用HTTP GET获取
        /// </summary>
        public string PicUrl { get; set; }
        #endregion 

        #region 事件
        /// <summary>
        /// 事件类型，subscribe(订阅/扫描带参数二维码订阅)、unsubscribe(取消订阅)、CLICK(自定义菜单点击事件) 、SCAN（已关注的状态下扫描带参数二维码）
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 事件KEY值
        /// </summary>
        public string EventKey { get; set; }

        #endregion
        /// <summary>
        /// 二维码的ticket，可以用来换取二维码
        /// </summary>
        public string Ticket { get; set; }

        #region 语音
        /// <summary>
        /// 语音格式，如amr，speex等
        /// </summary>
        public string Format { get; set; }

        #endregion

        #region  链接

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url { get; set; }

        #endregion

    }
}