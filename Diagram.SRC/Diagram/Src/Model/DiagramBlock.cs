using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{

    // container for part of diagram
    public class DiagramBlock
    {
        public Nodes nodes = new Nodes();
        public Lines lines = new Lines();

        public DiagramBlock(Nodes nodes = null, Lines lines = null)
        {
            this.nodes = nodes;
            this.lines = lines;
        }
    }
}
