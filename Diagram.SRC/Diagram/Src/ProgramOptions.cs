using System;
using Newtonsoft.Json;

namespace Diagram
{
    /*
     * parameters for aplication
     * global parmeters
     */

    public class ProgramOptions
    {
        [JsonIgnore]
        public string ApplicationVersion = "0.2.010";
        [JsonIgnore]
        public string ReleaseDate = "2015-08-26";
        [JsonIgnore]
        public String home_page = "https://pekand.com";
        public String server_default_ip = "127.0.0.1";
#if MONO
        public String texteditor = "'subl %FILENAME%:%LINE%'";
#else
        public String texteditor = "\"c:\\Program Files\\Sublime Text 3\\sublime_text.exe\" \"%FILENAME%\":%LINE%";
#endif

#if DEBUG
        public Int32 server_default_port = 13001;
#else
        public Int32 server_default_port = 13000;
#endif
        }
}
