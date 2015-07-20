using System;


namespace Diagram
{
    /// Class for catch log informations.
    /// This informations can be show in console form
    class Log
    {
        /// All messages saved in log
        string log = "";

        /// <param name="message">Message witch will by saved in log</param>
        /// Get message from program and save it to log.
        /// If console windows is updated then update window
        public void write(string message)
        {
            log = message + "\n" + log;

            /// If console window is displayes then actualize data
            if (Program.main!= null && Program.main.console != null)
            {
                Program.main.console.Invoke(new Action(() => Program.main.console.refreshWindow()));
            }
        }

        ///  Get all text in log.
        /// <returns>String width complete log</returns>
        public string getText()
        {
            return log;
        }

        ///  set all text in log.
        ///  Text is updated 
        public void setText(string text)
        {
            log = text;
        }

        /// clear data in log
        public void clearLog()
        {
            log = "";
            this.write("Log clear");
        }
    }
}

