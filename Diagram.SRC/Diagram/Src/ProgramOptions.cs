using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Diagram
{
    /// <summary>
    /// global program parmeters for all instances </summary>
    public class ProgramOptions
    {
        /*************************************************************************************************************************/

        // NOT SYNCHRONIZED PARAMETERS

        [JsonIgnore]
        /// <summary>
        /// license</summary>
        public String license = "GPLv3";

        [JsonIgnore]
        /// <summary>
        /// author</summary>
        public String author = "Andrej Pekar";

        [JsonIgnore]
        /// <summary>
        /// contact email</summary>
        public String email = "infinite.diagram@gmail.com";

        [JsonIgnore]
        /// <summary>
        /// home page url</summary>
        public String home_page = "https://www.infinite-diagram.com";

        [JsonIgnore]
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
        public Int32 server_default_port = 13001;
#else
        /// <summary>
        /// release local messaging server port</summary>
        public Int32 server_default_port = 13000;
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
        public IList<String> recentFiles = new List<String>();

        /*************************************************************************************************************************/

        /// <summary>
        /// copy options from other instance</summary>
        public void SetParams(ProgramOptions options)
        {
            this.proxy_uri = options.proxy_uri;
            this.proxy_username = options.proxy_username;
            this.proxy_password = options.proxy_password;
            this.server_default_port = options.server_default_port;
            this.texteditor = options.texteditor;

            this.recentFiles.Clear();
            foreach (String path in options.recentFiles) {
                this.recentFiles.Add(path);
            }

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
            IList<String> newRecentFiles = new List<String>();

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
