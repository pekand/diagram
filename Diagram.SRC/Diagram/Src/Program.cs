using System;
using System.Windows.Forms;
using System.Reflection;

// [VERSION]
[assembly: AssemblyVersion("0.5.0.1")]

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
    static class Program
    {
        /// <summary>
        /// debuging console for loging messages</summary>
        public static Log log = new Log();

        /// <summary>
        /// create main class which oppening forms</summary>
        public static Main main = null;

        [STAThread]
        static void Main()
        {
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
