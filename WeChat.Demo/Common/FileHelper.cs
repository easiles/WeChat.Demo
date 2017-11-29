using System;
using System.IO;
using System.Text;

namespace WeChat.Demo.Common
{
    public class FileHelper
    {
        public static string GetFileContent(string filePath)
        {
            var result = new StringBuilder();

            try
            {
                // 创建一个 StreamReader 的实例来读取文件 
                // using 语句也能关闭 StreamReader
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;

                    // 从文件读取并显示行，直到文件的末尾 
                    while ((line = sr.ReadLine()) != null)
                    {
                        var temp = line.Replace("\r\n", " ");
                        result.Append(temp);
                    }
                }
            }
            catch (Exception ex)
            {
                
                
            }
            return result.ToString();
        }
    }
}