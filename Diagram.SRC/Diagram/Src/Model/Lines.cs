using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    public class Lines : List<Line>
    {
        public Lines()
        {
        }

        public Lines(int capacity) : base(capacity)
        {
        }

        public Lines(Lines collection) : base(collection)
        {
        }
    }
}
