using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace Winwink.DesktopWallPaper
{
    public class BingPicture
    {
        /// <summary>
        /// 图片链接地址 如https://cn.bing.com/az/hprichbg/rb/SiberianJay_ZH-CN8167378429_1920x1080.jpg
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// 图片名字 如地址为https://cn.bing.com/az/hprichbg/rb/SiberianJay_ZH-CN8167378429_1920x1080.jpg，图片名为SiberianJay
        /// </summary>
        public string PictureName { get; set; }

        public string GetPictureOfToday()
        {
            var siteUrl = "https://cn.bing.com/";
            WebRequest request = WebRequest.Create(siteUrl);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));
            var webContent = reader.ReadToEnd();
            var url = GetPictureUrl(webContent);
            var name = GetPictureName(url);

            //PictureUrl = siteUrl +　url;
            PictureUrl = url;

            PictureName = name;
            return url;
        }

        //2017-12-10~2019-03-07
        //private string GetPictureUrl(string content)
        //{
        //    var url = "";
        //    MatchCollection TitleMatchs = Regex.Matches(content, "url:\\s*\"([^.\\\\]+.jpg)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        //    foreach (Match NextMatch in TitleMatchs)
        //    {
        //        url = NextMatch.Groups[1].Value;
        //    }
        //    return url;
        //}

        //2017-12-10~2019-03-07
        //private string GetPictureName(string url)
        //{
        //    var split1 = url.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
        //    var fileName = split1.Last();

        //    var split2 = fileName.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
        //    var pictureName = split2[0];
        //    return pictureName;
        //}

        private string GetPictureUrl(string content)
        {
            var url = "";
            MatchCollection TitleMatchs = Regex.Matches(content, "href=\\\"(/th\\?id=[^\"]+)\\\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            foreach (Match NextMatch in TitleMatchs)
            {
                url = NextMatch.Groups[1].Value;
            }
            url = "https://s.cn.bing.net" + url;
            return url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">eg. https://s.cn.bing.net/th?id=OHR.BagpipeOpera_ZH-CN9506207351_1920x1080.jpg&rf=NorthMale_1920x1080.jpg&pid=hp </param>
        /// <returns></returns>
        private string GetPictureName(string url)
        {
            url = url.Replace("&amp;", "&");
             var value = GetParamValue(url, "rf");
            return value;
        }

        private string GetParamValue(string url, string key)
        {
            var list = url.Split(new string[] { "&", "?" }, StringSplitOptions.RemoveEmptyEntries);
            Hashtable map = new Hashtable();
            foreach(string keyValueStr in list)
            {
                var splist = keyValueStr.Split(new string[] { "=" }, StringSplitOptions.None);
                if(splist.Length>=2)
                {
                    if(key==splist[0])
                    {
                        return splist[1];
                    }
                }
            }
            return "";
        }

    }
}
