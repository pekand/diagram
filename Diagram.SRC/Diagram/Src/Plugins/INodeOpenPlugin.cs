using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram 
{
    public interface INodeOpenPlugin : IDiagramPlugin
    {
        bool ClickOnNodeAction(Diagram diagram, Node node);
    }
}
