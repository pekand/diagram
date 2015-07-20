using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Diagram
{
    class Network
    {

        
        // NODE Get title from url // URL Title
        public static string GetWebPageTitle(string url)
        {
            WebClient x = new WebClient();
            string title = url;
            try
            {

                x.Encoding = System.Text.Encoding.UTF8;
                string source = x.DownloadString(url);
                string encoding = Regex.Match(source, "<meta.*?charset=['\"]?(?<Encoding>[^\"']+)['\"]?", RegexOptions.IgnoreCase).Groups["Encoding"].Value;
                if (encoding.Trim() != "" && encoding != "utf-8")
                {
                    x.Encoding = System.Text.Encoding.GetEncoding(encoding);
                    source = x.DownloadString(url);
                }
                title = WebUtility.HtmlDecode(Regex.Match(source, "<title\\b[^>]*>\\s*(?<Title>[\\s\\S]*?)</title>", RegexOptions.IgnoreCase).Groups["Title"].Value);
                if (title.Trim() == "") title = url;
            }
            catch (Exception ex)
            {
                Program.log.write("get web page title error: " + ex.Message);
            }
            return title;
        }
		
		public static bool isURL(String url) 
		{
			//return Uri.IsWellFormedUriString(url, UriKind.Absolute);
			//return (Regex.IsMatch(url, @"(((([a-z]|\d|-|.||~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'()*+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]).(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]).(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]).(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|.||~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))).)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))).?)(:\d*)?)(/((([a-z]|\d|-|.||~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'()*+,;=]|:|@)+(/(([a-z]|\d|-|.||~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'()*+,;=]|:|@)))?)?(\?((([a-z]|\d|-|.||~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'()*+,;=]|:|@)|[\uE000-\uF8FF]|/|\?)*)?(#((([a-z]|\d|-|.||~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'()*+,;=]|:|@)|/|\?)*)?$"));
			return (Regex.IsMatch(url, @"(http|https)://.*"));
		}

        public static bool isHttpsURL(String url)
        {
            return (Regex.IsMatch(url, @"(https)://.*"));
        }

		public static void runUrl(String url)
        {
			url = url.Replace(" ", "%20");
			System.Diagnostics.Process.Start(url);
		}
		
    }
}
