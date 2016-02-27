using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Diagram
{
    class Network
    {

        /// <summary>
        /// download https page and parse title from it </summary>
        public static string GetWebPageTitle(string url, int level = 0)
        {
            string title = url;

            

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.MaximumAutomaticRedirections = 3;
                request.UseDefaultCredentials = true;
                if (Program.main.options.proxy_password != "" && Program.main.options.proxy_username != "")
                {
                    // set proxy credentials
                    WebProxy myProxy = new WebProxy();
                    Uri newUri = new Uri(Program.main.options.proxy_uri);
                    myProxy.Address = newUri;
                    myProxy.Credentials = new NetworkCredential(
                        Program.main.options.proxy_username, 
                        Program.main.options.proxy_password
                    );
                    request.Proxy = myProxy;
                }
                else
                {
                    request.Proxy = WebRequest.GetSystemWebProxy();
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();

                MemoryStream memoryStream = new MemoryStream();
                resStream.CopyTo(memoryStream);

                // read stream with utf8
                memoryStream.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(memoryStream);
                string page = reader.ReadToEnd();

                string encoding = Regex.Match(
                    page,
                    "<meta.*?charset=['\"]?(?<Encoding>[^\"']+)['\"]?",
                    RegexOptions.IgnoreCase
                ).Groups["Encoding"].Value;

                // try redirect 
                if (level < 3) {
                    string redirect = Regex.Match(
                    page,
                    "<meta.*?http-equiv=\"refresh\".*?(CONTENT|content)=[\"']\\d;\\s?(URL|url)=(?<url>.*?)([\"']\\s*\\/?>)",
                    RegexOptions.IgnoreCase
                    ).Groups["url"].Value;

                    if (redirect.Trim() != "") {
                        Uri result = null;
                        Uri.TryCreate(new Uri(url), redirect, out result);
                        return GetWebPageTitle(result.ToString(), level + 1);
                    }
                }

                // use different encoding
                if (encoding.Trim() != "" && encoding.ToLower() != "utf-8")
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    StreamReader reader2 = new StreamReader(memoryStream, System.Text.Encoding.GetEncoding(encoding));
                    page = reader2.ReadToEnd();
                }

                title = Regex.Match(
                        page,
                        "<title>(?<Title>.*?)</title>",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline
                    ).Groups["Title"].Value;

                title = WebUtility.HtmlDecode(title.Trim());
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
