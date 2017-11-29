using System;

namespace WeChat.Demo.Common
{
    public class CommonHelper
    {

        #region unix/datatime 时间转换
        /// <summary>
        /// unix时间转换为datetime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeToTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// datetime转换为unixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        #endregion

        #region 生成随机字符
        /// <summary>
        /// 生成随机字符
        /// </summary>
        /// <param name="iLength">生成字符串的长度</param>
        /// <returns>返回随机字符串</returns>
        public static string GetRandCode(int iLength)
        {
            string sCode = "";
            if (iLength == 0)
            {
                iLength = 4;
            }
            string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] arr = codeSerial.Split(',');
            int randValue = -1;
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < iLength; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);
                sCode += arr[randValue];
            }
            return sCode;
        }
        #endregion

    }
}