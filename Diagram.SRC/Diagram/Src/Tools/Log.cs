using System;

namespace Diagram
{
    /// <summary>
    /// Class for catch log informations.
    /// This informations can be show in console form </summary>
    public class Log //UID8455348623
    {
        /// <summary>
        /// form for displaying errors </summary>
        private Console console = null;

        /// <summary>
        /// All messages saved in log </summary>
        private string log = "";

        /// <summary>
        /// Get message from program and save it to log.
        /// If console windows is updated then update window</summary>
        /// <param name="message">Message witch will by saved in log</param>
        public void Write(string message)
        {
            log = message + "\n" + log;

            /// If console window is displayes then actualize data
            if (this.console != null)
            {
                this.console.Invoke(new Action(() => this.console.RefreshWindow()));
            }
        }

        /// <summary> 
        /// Get all text in log.</summary>
        /// <returns>String width complete log</returns>
        public string GetText()
        {
            return log;
        }

        /// <summary>
        /// clear log test</summary>
        public void ClearLog()
        {
            log = "";
            this.Write("Log clear");
        }

        /// <summary>
        /// save  current log to file
        /// for crash log</summary>
        public void SaveLogToFile(string logPath = "")
        {
            if (logPath == "") {
                string tempDir = Os.GetTempPath();
                string tempFile = "infinite-diagram-" + DateTime.Now.ToString("MM-dd-yyyy-HH-MM-ss") + ".log";
                logPath = Os.Combine(tempDir, tempFile);
            }

            Os.WriteAllText(logPath, this.log);
        }

        /// <summary> 
        /// use console form for view errors</summary>
        public void SetConsole(Console console)
        {
            this.console = console;
        }
    }
}

