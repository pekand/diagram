using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    /// <summary>
    /// collection of nodes for drawing polygons. 
    /// Polygons is defined by nodes and lines making circle</summary>
    public class Polygon
    {
        public ColorType color = new ColorType();
        public Nodes nodes = new Nodes();
        public Lines lines = new Lines();
    }
}
