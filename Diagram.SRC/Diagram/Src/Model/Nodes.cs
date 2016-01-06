using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    public class Nodes : List<Node>
    {
        public Nodes()
        {
        }

        public Nodes(int capacity) : base(capacity)
        {
        }

        public Nodes(List<Node> collection) : base(collection)
        {
        }

        public void OrderByLink()
        {
            this.Sort((x, y) => string.Compare(x.link, y.link));
        }

        public void OrderByPositionY()
        {
            this.Sort((x, y) => x.position.y < y.position.y);
        }
    }
}
