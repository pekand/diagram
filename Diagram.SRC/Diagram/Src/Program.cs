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
        private static ProgramSetup aplicationSetup = new ProgramSetup(); //setup application
        public static Log log = new Log(); // debuging console for displaiing messages
        public static Main main = new Main();
        
        [STAThread]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void Main()
        {
            if (main.mainform != null) {
                Application.Run(main.mainform);
            }
            Application.Exit();
        }
    }
}
