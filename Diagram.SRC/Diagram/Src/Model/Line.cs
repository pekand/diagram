	namespace Diagram
{
    /// <summary>
    /// Line between two nodes in diagram</summary>
    public class Line
    {
        public int start = 0; // node id 
        public int end = 0; // node id 
        public Node startNode = null; // linked start node for quick access
        public Node endNode = null;
        public bool arrow = false; // node is rendered as arrow
    }
}
