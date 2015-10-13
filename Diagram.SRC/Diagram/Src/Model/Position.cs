using System;

namespace Diagram
{

    /// <summary>
    /// Point position in canvas</summary>
    public class Position
    {
        public int x = 0;
        public int y = 0;

        /// <summary>
        /// Constructor</summary>
        public Position(int x = 0, int y = 0)
        { 
            this.x = x;
            this.y = y;
        }

        // <summary>
        /// Count distance between two points</summary>
        public double distance(Position b)
        {
            return Math.Sqrt((b.x - this.x) * (b.x - this.x) + (b.y - this.y) * (b.y - this.y));
        }

        // <summary>
        /// Copy position to current position</summary>
        public Position copy(Position b)
        {
            this.x = b.x;
            this.y = b.y;
            return this;
        }

        // <summary>
        /// Convert position to cartesian coordinate</summary>
        public Position convertTostandard()
        {
            return new Position(this.x, -this.y);
        }

        // <summary>
        /// Convert position to string</summary>
        public override string ToString()
        {
            return "[" + x.ToString() +  "," + y.ToString() + "]";
        }

    }
}
