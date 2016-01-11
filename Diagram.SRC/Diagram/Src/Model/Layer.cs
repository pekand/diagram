using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    public class Layer
    {
        public int id = 0; // parent node id, layer owner

        public Node parentNode = null;
        public Layer parentLayer = null; // up layer in layer hierarchy

        public Nodes nodes = new Nodes();          // all layer nodes
        public Lines lines = new Lines();          // all layer lines

        // LAYER construct - parentNode is node in upper layer - parentLayer is layer whitch has parentNode
        public Layer(Node parentNode = null, Layer parentLayer = null)
        {
            if (parentNode != null)
            {
                this.id = parentNode.id;
                this.parentNode = parentNode;
            }

            this.parentLayer = parentLayer;
        }
    }
}
