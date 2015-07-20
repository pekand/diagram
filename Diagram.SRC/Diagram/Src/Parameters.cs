using System;

namespace Diagram
{
    /*
     * parameters for aplication
     * global parmeters
     */

    public class Parameters
    {
        public string ApplicationVersion = "0.2.007 beta";
        public string ReleaseDate = "2015-06-22";
        public String home_page = "https://pekand.com";
        public String server_default_ip = "127.0.0.1";

#if DEBUG            
        public Int32 server_default_port = 13001;
#else
        public Int32 server_default_port = 13000;
#endif
        }
}
