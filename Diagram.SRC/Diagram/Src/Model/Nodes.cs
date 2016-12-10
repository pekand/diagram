using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    /// <summary>
    /// collection of nodes</summary>
    public class Nodes : List<Node>
    {
        /*************************************************************************************************************************/
        // CONSTRUCTORS

        public Nodes()
        {
        }

        public Nodes(int capacity) : base(capacity)
        {
        }

        public Nodes(List<Node> collection) : base(collection)
        {
        }

        /*************************************************************************************************************************/
        // SETTERS AND GETTERS

        public void copy(Nodes nodes)
        {
            this.Clear();

            foreach (Node node in nodes)
            {
                this.Add(node.clone());
            }

        }

        /*************************************************************************************************************************/
        // SORT

        public void OrderByIdAsc()
        {
            this.Sort((x, y) => x.id.CompareTo(y.id));
        }

        public void OrderByNameAsc()
        {
            this.Sort((x, y) => string.Compare(x.name, y.name));
        }

        public void OrderByNameDesc()
        {
            this.Sort((x, y) => string.Compare(y.name, x.name));
        }

        public void OrderByLink()
        {
            this.Sort((x, y) => string.Compare(x.link, y.link));
        }

        public void OrderByPositionY()
        {
            this.Sort((a, b) => a.position.y.CompareTo(b.position.y));
        }

        public void OrderByPositionX()
        {
            this.Sort((a, b) => a.position.x.CompareTo(b.position.x));
        }
        
    }
}
