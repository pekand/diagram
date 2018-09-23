using System;
using System.Collections.Generic;

namespace Diagram
{
    /// <summary>
    /// global program parmeters for all instances </summary>
    public class ProgramOptions //UID0014460148
    {
        /*************************************************************************************************************************/

        // NOT SYNCHRONIZED PARAMETERS

        /// <summary>
        /// license</summary>
        public String license = "GPLv3";

        /// <summary>
        /// author</summary>
        public String author = "Andrej Pekar";

        /// <summary>
        /// contact email</summary>
        public String email = "infinite.diagram@gmail.com";

        /// <summary>
        /// home page url</summary>
        public String home_page = "https://www.infinite-diagram.com";

        /// <summary>
        /// local server ip address fo messaging beetwen runing instances</summary>
        public String server_default_ip = "127.0.0.1";

        /*************************************************************************************************************************/

        // SYNCHRONIZED PARAMETERS

        /// <summary>
        /// proxy uri</summary>
        public String proxy_uri = "";

        /// <summary>
        /// proxy auth username</summary>
        public String proxy_username = "";

        /// <summary>
        /// proxy auth password</summary>
        public String proxy_password = "";

#if DEBUG
        /// <summary>
        /// debug local messaging server port</summary>
        public long server_default_port = 13001;
#else
        /// <summary>
        /// release local messaging server port</summary>
        public long server_default_port = 13000;
#endif

#if MONO
        /// <summary>
        /// command for open editor on line position</summary>
        public String texteditor = "'subl %FILENAME%:%LINE%'";
#else
        /// <summary>
        /// command for open editor on line position</summary>
        public String texteditor = "subl \"%FILENAME%\":%LINE%";
#endif

        /// <summary>
        /// recently opened files</summary>
        public List<String> recentFiles = new List<String>();

        /*************************************************************************************************************************/

        /// <summary>
        /// load configuration</summary>
        public void LoadParams(ConfigFile configFile)
        {

            this.proxy_uri = configFile.Get("proxy_uri");
            this.proxy_username = configFile.Get("proxy_username");
            this.proxy_password = configFile.Get("proxy_password");
            this.server_default_port = Converter.ToInt(configFile.Get("server_default_port"));
            this.texteditor = configFile.Get("texteditor");
            IList<String> recentFiles = Serialization.XElementToList(configFile.GetElement("recentFiles"));

            if (recentFiles != null) { 
                this.recentFiles.Clear();
                foreach (String path in recentFiles) {
                    this.recentFiles.Add(path);
                }
            }

            this.RemoveOldRecentFiles();
        }

        /*************************************************************************************************************************/

        /// <summary>
        /// save configuration </summary>
        public void SaveParams(ConfigFile configFile)
        {

            configFile.Set("proxy_uri", this.proxy_uri);
            configFile.Set("proxy_username", this.proxy_username);
            configFile.Set("proxy_password", this.proxy_password);
            configFile.Set("server_default_port", this.server_default_port.ToString());
            configFile.Set("texteditor", this.texteditor);
            configFile.SetElement("recentFiles", Serialization.ListToXElement("recentFiles", this.recentFiles));

            this.RemoveOldRecentFiles();
        }

        /*************************************************************************************************************************/
        // Recent files

        /// <summary>
        /// add path to recent files</summary>
        public void AddRecentFile(String path)
        {
            if (Os.FileExists(path))
            {
                if (!this.recentFiles.Contains(path))
                {
                    this.recentFiles.Add(path);                                        
                }
            }
        }

        /// <summary>
        /// remove old not existing diagrams from recent files</summary>
        public void RemoveOldRecentFiles()
        {
            List<String> newRecentFiles = new List<String>();

            foreach (String path in this.recentFiles)
            {
                if(Os.FileExists(path))
                {
                    newRecentFiles.Add(path);
                }
            }
            this.recentFiles = newRecentFiles;
        }
    }
}
