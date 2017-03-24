using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Diagram;

namespace Plugin
{
    public class FirstPlugin : INodeOpenPlugin, IKeyPressPlugin, IOpenDiagramPlugin //UID0290845814
    {
        #region IPlugin Members 

        private static int counter = 0;

        public string Name
        {
            get
            {
                return "First Plugin";
            }
        }

        public string Version
        {
            get
            {
                return "1.0";
            }
        }

        private Log log = null;

        public void setLog(Log log)
        {
            this.log = log;
        }

        public bool ClickOnNodeAction(Diagram.Diagram diagram, Node node)
        {
            log.Write("Do Something in First Plugin:" + (counter++).ToString());

            return true;
        }

        public bool KeyPressAction(Diagram.Diagram diagram, String key)
        {
            log.Write("Do Something in First Plugin:" + (counter++).ToString());

            return true;
        }

        public void OpenDiagramAction(Diagram.Diagram diagram)
        {
            log.Write("Open diagram action fired from first plugin");
        }

        #endregion
    }
}
