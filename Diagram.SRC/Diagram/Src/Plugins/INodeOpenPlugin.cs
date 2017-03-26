using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram 
{
    public interface INodeOpenPlugin : IDiagramPlugin  //UID0290945800
    {
        bool ClickOnNodeAction(Diagram diagram, DiagramView diagramview, Node node);
    }
}
