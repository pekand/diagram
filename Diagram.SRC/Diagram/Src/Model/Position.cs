using System;

namespace Diagram
{
    public class Position
    {
        public int x = 0;
        public int y = 0;

        public Position(int x = 0, int y = 0)
        { 
            this.x = x;
            this.y = y;
        }

        public double distance(Position b)
        {
            return Math.Sqrt((b.x - this.x) * (b.x - this.x) + (b.y - this.y) * (b.y - this.y));
        }

        public Position copy(Position b)
        {
            this.x = b.x;
            this.y = b.y;
            return this;
        }

        public Position convertTostandard()
        {
            return new Position(this.x, -this.y);
        }

        public override string ToString()
        {
            return "[" + x.ToString() +  "," + y.ToString() + "]";
        }

    }
}
