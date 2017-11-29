using System;
using System.IO;
using System.Web;

namespace WeChat.Demo.Common
{
    public class LogHelper
    {

        /// <summary>
        /// 记录bug，以便调试
        /// </summary>
        public static bool Log(string str)
        {
            try
            {
                string logPath = HttpContext.Current.Server.MapPath("/err_log/");
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                FileStream fileStream = new FileStream(HttpContext.Current.Server.MapPath("/err_log//xiejun_" + DateTime.Now.ToLongDateString() + "_.txt"), FileMode.Append);
                StreamWriter streamWriter = new StreamWriter(fileStream);
                //开始写入
                streamWriter.WriteLine(str);
                //清空缓冲区
                streamWriter.Flush();
                //关闭流
                streamWriter.Close();
                fileStream.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}