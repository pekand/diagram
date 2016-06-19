using System;
using System.IO;

namespace Diagram
{
    /// Class for catch log informations.
    /// This informations can be show in console form
    class Log
    {
        /// form for displaying errors
        private Console console = null;

        /// All messages saved in log
        private string log = "";

        /// <param name="message">Message witch will by saved in log</param>
        /// Get message from program and save it to log.
        /// If console windows is updated then update window
        public void write(string message)
        {
            log = message + "\n" + log;

            /// If console window is displayes then actualize data
            if (this.console != null)
            {
                this.console.Invoke(new Action(() => this.console.refreshWindow()));
            }
        }

        ///  Get all text in log.
        /// <returns>String width complete log</returns>
        public string getText()
        {
            return log;
        }

        /// clear data in log
        public void clearLog()
        {
            log = "";
            this.write("Log clear");
        }

        // save  current log to file
        // for example crash log
        public void saveLogToFile(string logPath = "")
        {
            if (logPath == "") {
                string tempDir = Os.getTempPath();
                string tempFile = "infinite-diagram-crash-log.txt";
                logPath = Os.combine(tempDir, tempFile);
            }

            Os.writeAllText(logPath, this.log);
        }

        ///  use console for view errors
        public void setConsole(Console console)
        {
            this.console = console;
        }
    }
}

