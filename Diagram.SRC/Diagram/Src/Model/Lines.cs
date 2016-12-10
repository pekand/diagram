using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    /// <summary>
    /// collection of nodes</summary>
    public class Lines : List<Line>
    {
        /*************************************************************************************************************************/
        // CONSTRUCTORS

        public Lines()
        {
        }

        public Lines(int capacity) : base(capacity)
        {
        }

        public Lines(Lines collection) : base(collection)
        {
        }

        /*************************************************************************************************************************/
        // SETTERS AND GETTERS

        public void copy(Lines lines)
        {
            this.Clear();

            foreach (Line line in lines)
            {
                this.Add(line.clone());
            }

        }
    }
}
