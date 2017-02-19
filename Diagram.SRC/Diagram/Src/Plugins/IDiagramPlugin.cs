using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    public interface IDiagramPlugin
    {
        string Name { get; }
        string Version { get; }
    }
}
