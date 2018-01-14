using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Diagram;

namespace Plugin
{
    public class DropPlugin : IDropPlugin
    {
        private static long counter = 0;

        public string Name
        {
            get
            {
                return "Drop Plugin";
            }
        }

        public string Version
        {
            get
            {
                return "1.0";
            }
        }

        private string location = null;

        public void SetLocation(string location)
        {
            this.location = location;
        }

        private Log log = null;

        public void SetLog(Log log)
        {
            this.log = log;
        }

        public bool DropAction(DiagramView diagramview)
        {
            log.Write("Do Something in Drop Plugin:" + (counter++).ToString());

            return false;
        }
    }
}
