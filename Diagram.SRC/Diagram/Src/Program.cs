using System;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

// [VERSION]
[assembly: AssemblyVersion("0.5.0.19")]

/*! \mainpage Infinite diagram
 *
 * \section intro_sec Introduction
 *
 * Program for creating diagrams
 *
 */
namespace Diagram
{
    /// <summary>
    /// Application entry point</summary>
    public static class Program
    {
        /// <summary>
        /// debuging console for loging messages</summary>
        public static Log log = new Log();

        /// <summary>
        /// create main class which oppening forms</summary>
        private static Main main = null;

        /*************************************************************************************************************************/
        // TOOLS

        /// <summary>
        /// get current app version</summary>
        public static string GetVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }

        /// get current app executable path</summary>
        public static string GetLocation()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().Location;
        }

        /*************************************************************************************************************************/
        // MAIN APPLICATION START

        [STAThread]
        private static void Main()
        {
            Program.log.write("Start application: " + GetLocation());

            Program.log.write("Version : " + GetVersion());
#if DEBUG
            Program.log.write("Debug mode");
#else
            Program.log.write("Production mode");
#endif
            // aplication default settings
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

// prevent catch global exception in debug mode
#if !DEBUG
            try
            {
#endif
                main = new Main();
                if (main.mainform != null) {
                    Application.Run(main.mainform);
                }
                Application.Exit();
#if !DEBUG
            // catch all exception globaly in release mode and prevent application crash
            }
            catch (Exception e) // global exception handling
            {
                log.write("Application crash: message:" + e.Message);
                log.saveLogToFile();

                MessageBox.Show("Application crash: message:" + e.Message);

                System.Environment.Exit(1); //close application with error code 1
            }
#endif
        }
    }
}
