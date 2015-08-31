using System;
using System.Windows.Forms;
using System.Security.Permissions;

/*! \mainpage Infinite diagram
 *
 * \section intro_sec Introduction
 *
 * Program for creating diagrams
 *
 */
namespace Diagram
{
    static class Program
    {
        private static ProgramInit aplicationSetup = new ProgramInit(); //setup application
        public static Log log = new Log(); // debuging console for displaiing messages
        public static Main main = null;

        [STAThread]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void Main()
        {
            try
            {
                main = new Main();
                Application.Run(main.mainform);
                Application.Exit();

            }
            catch (Exception e) {
                log.write("Application crash: message:" + e.Message);
                log.saveLogToFile();
                System.Environment.Exit(1);
            }
        }
    }
}
