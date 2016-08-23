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
        public static string GetWebPageTitle(
            string url, 
            string proxy_uri = "",
            string proxy_password = "",
            string proxy_username = "",
            int level = 0
            )
        {
            string page = Network.GetWebPage(
                url,
                proxy_uri,
                proxy_password,
                proxy_username,
                level = 0,
                null,
                false
            );

            string title = "";
            try {
                title = Regex.Match(
                        page,
                        "<title>(.*?)</title>",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline
                    ).Groups[1].Value;

            }
            catch (Exception ex)
            {
                Program.log.write("get link name error: " + ex.Message);
            }

            return (title.Trim() == "")? url : title.Trim();
        }

        public static bool ConfirmAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static void AcceptAllCertifications()
        {
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ConfirmAllCertifications);
        }

        /// <summary>
        /// download https page and parse title from it </summary>
        public static string GetWebPage(
            string url,
            string proxy_uri = "",
            string proxy_password = "",
            string proxy_username = "",
            int level = 0,
            CookieContainer cookieContainer = null,
            bool skiphttps = false
            )
        {

            Program.log.write("get title from: " + url);

            if (skiphttps)
            {
                url = url.Replace("https:", "http:");
            }

            string page = "";

            try
            {
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = false;
                request.UseDefaultCredentials = true;
                request.Timeout = 2000;

                if (proxy_uri != "" ||
                    proxy_password != "" ||
                    proxy_username != ""
                    )
                {
                    // set proxy credentials
                    WebProxy myProxy = new WebProxy();
                    if (proxy_uri != "")
                    {
                        Uri newUri = new Uri(proxy_uri);
                        myProxy.Address = newUri;
                    }

                    if (proxy_password != "" ||
                        proxy_username != ""
                        )
                    {
                        myProxy.Credentials = new NetworkCredential(
                            proxy_username,
                            proxy_password
                        );
                    }
                    request.Proxy = myProxy;
                }
                else
                {
                    request.Proxy = WebRequest.GetSystemWebProxy();
                }

                if (cookieContainer == null)
                {
                    cookieContainer = new CookieContainer();
                }

                if (cookieContainer != null)
                {
                    request.CookieContainer = cookieContainer;
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if ((int)response.StatusCode >= 300 && (int)response.StatusCode <= 399)
                {
                    string uriString = response.Headers["Location"];

                    if (level < 10)
                    {
                        return Network.GetWebPage(
                                uriString,
                                proxy_uri,
                                proxy_password,
                                proxy_username,
                                level + 1,
                                cookieContainer,
                                skiphttps
                            );
                    }
                }

                Stream resStream = response.GetResponseStream();

                MemoryStream memoryStream = new MemoryStream();
                resStream.CopyTo(memoryStream);

                // read stream with utf8
                memoryStream.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(memoryStream);
                page = reader.ReadToEnd();

                string encoding = Regex.Match(
                    page,
                    "<meta.*?charset=['\"]?(?<Encoding>[^\"']+)['\"]?",
                    RegexOptions.IgnoreCase
                ).Groups["Encoding"].Value;

                // try redirect 
                if (level < 10)
                {
                    string redirect = Regex.Match(
                    page,
                    "<meta.*?http-equiv=\"refresh\".*?(CONTENT|content)=[\"']\\d;\\s?(URL|url)=(?<url>.*?)([\"']\\s*\\/?>)",
                    RegexOptions.IgnoreCase
                    ).Groups["url"].Value;

                    if (redirect.Trim() != "")
                    {
                        Uri result = null;
                        Uri.TryCreate(new Uri(url), redirect, out result);
                        return Network.GetWebPage(
                            result.ToString(),
                            proxy_uri,
                            proxy_password,
                            proxy_username,
                            level + 1,
                            cookieContainer,
                            skiphttps
                        );
                    }
                }

                // use different encoding
                if (encoding.Trim() != "" && encoding.ToLower() != "utf-8")
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    StreamReader reader2 = new StreamReader(memoryStream, System.Text.Encoding.GetEncoding(encoding));
                    page = reader2.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Program.log.write("get link name error: " + ex.Message);
            }

            return page;
        }

        /// <summary>
        /// check if url start on http or https </summary>
        public static bool isURL(String url) 
		{
			return (Regex.IsMatch(url, @"^(http|https)://[^ ]*$"));
		}

        /// <summary>
        /// check if url start on https </summary>
        public static bool isHttpsURL(String url)
        {
            return (Regex.IsMatch(url, @"^(https)://[^ ]*$"));
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
