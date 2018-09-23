namespace Diagram
{
    /// <summary>
    /// collection of nodes for drawing polygons. 
    /// Polygons is defined by nodes and lines making circle</summary>
    public class Polygon //UID2534425093
    {
        public long layer = 0;
        public ColorType color = new ColorType("#000000");
        public Nodes nodes = new Nodes();

        /*************************************************************************************************************************/
        // CONSTRUCTORS

        public Polygon()
        {
        }

        public Polygon(Polygon polygon)
        {
            this.set(polygon);
        }

        /*************************************************************************************************************************/
        // SETTERS AND GETTERS

        public void set(Polygon polygon)
        {
            this.layer = polygon.layer;
            this.color = polygon.color;

            foreach (Node node in polygon.nodes) {
                this.nodes.Add(node.clone());
            }
        }

        /// <summary>
        /// clone line to new line</summary>
        public Polygon clone()
        {
            return new Polygon(this);
        }
    }
}
