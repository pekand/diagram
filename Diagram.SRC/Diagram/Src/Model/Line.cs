using System.Drawing;

namespace Diagram
{
    /// <summary>
    /// Line between two nodes in diagram</summary>
    public class Line
    {
        /*************************************************************************************************************************/
        // POSITION

        public int start = 0; // node id 
        public int end = 0; // node id 

        public Node startNode = null; // linked start node for quick access
        public Node endNode = null; // linked end node for quick access

        /*************************************************************************************************************************/
        // STYLES

        public bool arrow = false; // node is rendered as arrow
        public ColorType color = new ColorType(); // line color
        public int width = 1; // line width

        /*************************************************************************************************************************/
        // LAYER

        public int layer = 0; // layer parent node id

        /*************************************************************************************************************************/
        // CONSTRUCTORS

        public Line()
        {
        }

        public Line(Line line)
        {
            this.set(line);
        }

        /*************************************************************************************************************************/
        // SETTERS AND GETTERS

        public void set(Line line)
        {
            this.start = line.start;
            this.end = line.end;
            this.startNode = line.startNode;
            this.endNode = line.endNode;
            this.arrow = line.arrow;
            this.color.set(line.color);
            this.width = line.width;
            this.layer = line.layer;
        }

        /// <summary>
        /// clone line to new line</summary>
        public Line clone()
        {
            return new Line(this);
        }
    }
}
