using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Diagram
{
    class Network
    {

        /// <summary>
        /// download http page and parse title from it </summary>
        public static string GetWebPageTitle(string url)
        {
            string title = url;

            try
            {
                WebClient x = new WebClient();
                x.Encoding = System.Text.Encoding.UTF8;
                string source = x.DownloadString(url);

                string encoding = Regex.Match(
                    source, 
                    "<meta.*?charset=['\"]?(?<Encoding>[^\"']+)['\"]?", 
                    RegexOptions.IgnoreCase
                ).Groups["Encoding"].Value;

                if (encoding.Trim() != "" && encoding != "utf-8")
                {
                    x.Encoding = System.Text.Encoding.GetEncoding(encoding);
                    source = x.DownloadString(url);
                }

                title = WebUtility.HtmlDecode(
                    Regex.Match(
                        source, 
                        "<title\\b[^>]*>\\s*(?<Title>[\\s\\S]*?)</title>", 
                        RegexOptions.IgnoreCase
                    ).Groups["Title"].Value
                );

                if (title.Trim() == "") title = url;
            }
            catch (Exception ex)
            {
                Program.log.write("get web page title error: " + ex.Message);
            }
            return title;
        }


        /// <summary>
        /// download https page and parse title from it </summary>
        public static string GetSecuredWebPageTitle(string url)
        {
            string title = "";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(resStream);
                string page = reader.ReadToEnd();

                title = WebUtility.HtmlDecode(
                    Regex.Match(
                        page, 
                        "<title\\b[^>]*>\\s*(?<Title>[\\s\\S]*?)</title>", 
                        RegexOptions.IgnoreCase
                    ).Groups["Title"].Value
                );

                if (title.Trim() == "") title = url;

                
            }
            catch (Exception ex)
            {
                Program.log.write("get link name error: " + ex.Message);
            }

            return title;
        }

        /// <summary>
        /// check if url start on http or https </summary>
        public static bool isURL(String url) 
		{
			return (Regex.IsMatch(url, @"(http|https)://.*"));
		}

        /// <summary>
        /// check if url start on https </summary>
        public static bool isHttpsURL(String url)
        {
            return (Regex.IsMatch(url, @"(https)://.*"));
        }

        /// <summary>
        /// open url in os default browser </summary>
        public static void openUrl(String url)
        {
			url = url.Replace(" ", "%20");
			System.Diagnostics.Process.Start(url);
		}
		
    }
}
