using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Diagram;

namespace Plugin
{
    public class FirstPlugin : INodeOpenPlugin
    {
        #region IPlugin Members 

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

        public bool ClickOnNodeAction(Diagram.Diagram diagram, Node node)
        {
            MessageBox.Show("Do Something in First Plugin");

            return true;
        }

        #endregion
    }
}
