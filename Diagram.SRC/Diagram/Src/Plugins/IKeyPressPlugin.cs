using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diagram 
{
    public interface IKeyPressPlugin : IDiagramPlugin  //UID0290945802
    {
        bool KeyPressAction(Diagram diagram, DiagramView diagramview, Keys keyData);
    }
}
