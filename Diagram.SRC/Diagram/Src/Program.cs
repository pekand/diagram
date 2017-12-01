using System;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

// [VERSION]
[assembly: AssemblyVersion("0.5.0.43")]

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
    public static class Program //UID2573216529
    {
        /// <summary>
        /// debuging console for loging messages</summary>
        public static Log log = new Log();

        /// <summary>
        /// create main class which oppening forms</summary>
        private static Main main = null;

        /// <summary>
        /// Process global unhandled global exceptions</summary>
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Program.log.Write("Fatal error: " + e.ExceptionObject.ToString());
            log.SaveLogToFile();

            MessageBox.Show(e.ExceptionObject.ToString());

            Environment.Exit(1);
        }

        /*************************************************************************************************************************/
        // MAIN APPLICATION START        

        [STAThread]
        private static void Main() //UID4670767500
        {

#if !DEBUG
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
#endif

            Program.log.Write("Start application: " + Os.GetThisAssemblyLocation());

            Program.log.Write("Version : " + Os.GetThisAssemblyVersion());
#if DEBUG
            Program.log.Write("Debug mode");
#else
            Program.log.Write("Production mode");
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
            if (main.mainform != null)
            {
                Application.Run(main.mainform);
            }

#if !DEBUG
            // catch all exception globaly in release mode and prevent application crash
            }
            catch (Exception e) // global exception handling
            {
                log.Write("Application crash: message:" + e.Message);
                log.SaveLogToFile();

                MessageBox.Show("Application crash: message:" + e.Message);

                System.Environment.Exit(1); //close application with error code 1
            }
#endif
        }
    }
}
