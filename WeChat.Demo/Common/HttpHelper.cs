using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WeChat.Demo.Common
{
    public class HttpHelper
    {
        
        ///// <summary>
        ///// HttpGet
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="headerDic"></param>
        ///// <returns></returns>
        //public static string HttpGet(string url, Dictionary<string, string> headerDic = null)
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
        //        wbRequest.Method = "GET";
        //        HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
        //        using (Stream responseStream = wbResponse.GetResponseStream())
        //        {
        //            if (responseStream != null)
        //            {
        //                using (var sReader = new StreamReader(responseStream))
        //                {
        //                    result = sReader.ReadToEnd();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Log(string.Format("Message：{0}, InnerException：{1}, StackTrace:{2}", ex.Message, ex.InnerException, ex.StackTrace));
        //        return null;
        //    }
        //    return result;
        //}

        /// <summary>
        /// HttpGet
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headerDic"></param>
        /// <returns></returns>
        public static string HttpGet(string url, Dictionary<string, string> headerDic = null)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
                wbRequest.Method = "GET";
                wbRequest.Method = "POST";
                wbRequest.ContentType = "application/x-www-form-urlencoded";
                if (headerDic != null && headerDic.Count > 0)
                {
                    foreach (var item in headerDic)
                    {
                        wbRequest.Headers.Add(item.Key, item.Value);
                    }
                }
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var sReader = new StreamReader(responseStream))
                        {
                            result = sReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("Message：{0}, InnerException：{1}, StackTrace:{2}", ex.Message, ex.InnerException, ex.StackTrace));
                return null;
            }
            return result;
        }

        /// <summary>
        /// HttpPost
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramData"></param>
        /// <param name="headerDic"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string paramData, Dictionary<string, string> headerDic = null)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest) WebRequest.Create(url);
                wbRequest.Method = "POST";
                wbRequest.ContentType = "application/x-www-form-urlencoded";
                wbRequest.ContentLength = Encoding.UTF8.GetByteCount(paramData);
                if (headerDic != null && headerDic.Count > 0)
                {
                    foreach (var item in headerDic)
                    {
                        wbRequest.Headers.Add(item.Key, item.Value);
                    }
                }
                using (Stream requestStream = wbRequest.GetRequestStream())
                {
                    using (StreamWriter swrite = new StreamWriter(requestStream))
                    {
                        swrite.Write(paramData);
                    }
                }
                HttpWebResponse wbResponse = (HttpWebResponse) wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var sread = new StreamReader(responseStream))
                        {
                            result = sread.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("Message：{0}, InnerException：{1}, StackTrace:{2}", ex.Message, ex.InnerException, ex.StackTrace));
                return null;
            }

            return result;
        }
    }
}