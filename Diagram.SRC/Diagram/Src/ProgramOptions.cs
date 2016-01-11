using System;
using Newtonsoft.Json;

namespace Diagram
{
    /// <summary>
    /// global program parmeters</summary>
    public class ProgramOptions
    {

        [JsonIgnore]
        /// <summary>
        /// home page url</summary>
        public String home_page = "https://www.infinite-diagram.com";

        [JsonIgnore]
        /// <summary>
        /// release note</summary>
        public String release_note = "ReleaseNote.html";

        /// <summary>
        /// local server ip address</summary>
        public String server_default_ip = "127.0.0.1";

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
        public String texteditor = "\"c:\\Program Files\\Sublime Text 3\\sublime_text.exe\" \"%FILENAME%\":%LINE%";
#endif

        }
}
