namespace Diagram
{
    /// <summary>
    /// container for manipulation with part of diagram</summary> 
    public class DiagramBlock //UID6305074892
    {
        public Nodes nodes = new Nodes();
        public Lines lines = new Lines();
        public Polygons polygons = new Polygons();

        public DiagramBlock(Nodes nodes = null, Lines lines = null, Polygons polygons = null)
        {
            this.nodes = nodes;
            this.lines = lines;
            this.polygons = polygons;
        }
    }
}
