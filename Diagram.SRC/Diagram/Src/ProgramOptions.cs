using System;
using Newtonsoft.Json;

/*
    class ProgramOptions
        license
        author
        email
        home_page
        release_note
        server_default_ip
        proxy_uri
        proxy_username
        proxy_password
        server_default_port
        texteditor

        setParams()
*/

namespace Diagram
{
    /// <summary>
    /// global program parmeters</summary>
    public class ProgramOptions
    {
        [JsonIgnore]
        /// <summary>
        /// version</summary>
        public String license = "GPLv3";

        [JsonIgnore]
        /// <summary>
        /// version</summary>
        public String author = "Andrej Pekar";

        [JsonIgnore]
        /// <summary>
        /// version</summary>
        public String email = "infinite.diagram@gmail.com";

        [JsonIgnore]
        /// <summary>
        /// home page url</summary>
        public String home_page = "https://www.infinite-diagram.com";

        [JsonIgnore]
        /// <summary>
        /// release note</summary>
        public String release_note = "ReleaseNote.html";

        [JsonIgnore]
        /// <summary>
        /// local server ip address</summary>
        public String server_default_ip = "127.0.0.1";

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
        /// debug lolocal messaging server port</summary>
        public Int32 server_default_port = 13001;
#else
        /// <summary>
        /// release lolocal messaging server port</summary>
        public Int32 server_default_port = 13000;
#endif

#if MONO
        /// <summary>
        /// linux open on position command</summary>
        public String texteditor = "'subl %FILENAME%:%LINE%'";
#else
        /// <summary>
        /// windows open on position command</summary>
        public String texteditor = "subl \"%FILENAME%\":%LINE%";
#endif

        public void setParams(ProgramOptions options)
        {
            this.proxy_uri = options.proxy_uri;
            this.proxy_username = options.proxy_username;
            this.proxy_password = options.proxy_password;
            this.server_default_port = options.server_default_port;
            this.texteditor = options.texteditor;
        }

    }
}
