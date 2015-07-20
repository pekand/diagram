namespace Diagram
{
    // LINE  Spojovacia ciara
    public class Line
    {
        public int start = 0; // začiatočná noda spojnice
        public int end = 0; // koncová noda spojnice
        public Node startNode = null;
        public Node endNode = null;
        public bool arrow = false; // má sa spojnica zobraziť ako šípka
    }
}
