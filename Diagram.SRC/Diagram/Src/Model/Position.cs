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

        /// <summary>
        /// Constructor</summary>
        public Position(Position p)
        {
            this.x = p.x;
            this.y = p.y;
        }

        /// <summary>
        /// Clone</summary>
        public Position clone()
        {
            return new Position(this);
        }

        // <summary>
        /// set </summary>
        public Position set(int x, int y)
        {
            this.x = x;
            this.y = y;
            return this;
        }

        // <summary>
        /// set </summary>
        public Position set(Position p)
        {
            this.x = p.x;
            this.y = p.y;
            return this;
        }

        // <summary>
        /// Count distance between two points</summary>
        public double distance(Position b)
        {
            return Math.Sqrt((b.x - this.x) * (b.x - this.x) + (b.y - this.y) * (b.y - this.y));
        }

        // <summary>
        /// add vector</summary>
        public Position add(Position p)
        {
            this.x += p.x;
            this.y += p.y;
            return this;
        }

        // <summary>
        /// add vector</summary>
        public Position add(int x, int y)
        {
            this.x += x;
            this.y += y;
            return this;
        }

        public static Position operator +(Position a, Position b)
        {
            return new Position(a).add(b);
        }

        // <summary>
        /// subtract vector</summary>
        public Position subtract(Position p)
        {
            this.x += -p.x;
            this.y += -p.y;
            return this;
        }

        // <summary>
        /// subtract constant</summary>
        public Position subtract(int c)
        {
            this.x += -c;
            this.y += -c;
            return this;
        }

        public static Position operator -(Position a, Position b)
        {
            return new Position(a).subtract(b);
        }

        // <summary>
        /// subtract vector</summary>
        public Position invert()
        {
            this.x = -this.x;
            this.y = -this.y;
            return this;
        }

        // <summary>
        /// scale vector</summary>
        public Position scale(float scale)
        {
            this.x = (int)(this.x * scale);
            this.y = (int)(this.y * scale);
            return this;
        }

        // <summary>
        /// zoom vector</summary>
        public Position zoom(float scale)
        {
            this.x = (int)(this.x / scale);
            this.y = (int)(this.y / scale);
            return this;
        }

        public static Position operator *(Position a, int c)
        {
            return new Position(a).scale(c);
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
        public Position convertToStandard()
        {
            return new Position(this.x, -this.y);
        }

        // <summary>
        /// Convert position to string</summary>
        public override string ToString()
        {
            return "[" + x.ToString() +  "," + y.ToString() + "]";
        }

        // <summary>
        /// Convert position to cartesian coordinates</summary>
        public Position toCartesian()
        {
            this.y = -this.y;
            return this;
        }

        // <summary>
        /// Convert position to pc view coordinates</summary>
        public Position toView()
        {
            this.y = -this.y;
            return this;
        }

    }
}
