using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    /// <summary>
    /// collection of layers</summary>
    public class Layers
    {
        private int maxid = 0;                    // last used node id

        private Dictionary<int, Node> allNodes = new Dictionary<int, Node>();

        private List<Layer> layers = new List<Layer>();

        /*************************************************************************************************************************/
        // CONSTRUCTORS

        public Layers()
        {
            this.createLayer();
        }

        public Layer createLayer(Node parent = null)
        {
            Layer layer = getLayer((parent == null) ? 0 : parent.id);

            // create new layer if not exist
            if (layer == null)
            {
                Layer parentLayer = null;

                if (parent != null)
                {
                    parentLayer = this.getLayer(parent.layer);
                }

                layer = new Layer(parent, parentLayer);
                this.layers.Add(layer);
            }

            return layer;
        }

        /// <summary>
        /// Add referencies to layers to parents for fast parents search
        /// Is called after load diagram from file or clipboard
        /// </summary>
        public void setLayersParentsReferences()
        {
            foreach (Layer l in this.layers)
            {
                if (l.id != 0)
                {
                    foreach (Layer p in this.layers)
                    {
                        if (l.parentNode.layer == p.id)
                        {
                            l.parentLayer = p;
                            break;
                        }
                    }
                }
            }
        }

        /*************************************************************************************************************************/
        // SELECT ITEM

        public Node getNode(int id)
        {
            if (this.allNodes.ContainsKey(id)) {
                return this.allNodes[id];
            }

            return null;
        }

        public Line getLine(Node start, Node end)
        {
            foreach (Layer l in this.layers)
            {
                foreach (Line line in l.lines)
                {
                    if ((line.start == start.id && line.end == end.id) || (line.start == end.id && line.end == start.id))
                        return line;
                }
            }

            return null;
        }

        public Line getLine(int startId, int endId)
        {
            Node start = getNode(startId);

            if (start == null)
            {
                return null;
            }

            Node end = getNode(endId);

            if (end == null)
            {
                return null;
            }

            return getLine(start, end);
        }

        public bool hasLayer(int id = 0)
        {
            foreach (Layer l in this.layers)
            {
                if (l.id == id)
                {
                    return true;
                }
            }

            return false;
        }

        public Layer getLayer(int id = 0)
        {
            foreach (Layer l in this.layers)
            {
                if (l.id == id)
                {
                    return l;
                }
            }

            return null;
        }

        public Layer getLayer(Node node)
        {
            if (node.layer == 0) {
                return getLayer(0);
            }

            foreach (Layer l in this.layers)
            {

                if (l.parentNode != null && l.parentNode.id == node.layer)
                {
                    return l;
                }

            }

            return null;
        }

        public Nodes getAllNodes()
        {
            Nodes nodes = new Nodes();

            foreach (Node n in this.allNodes.Values)
            {
                nodes.Add(n);
            }

            return nodes;
        }

        public Nodes getAllNodes(Node node)
        {
            Nodes nodes = new Nodes();

            if (node.haslayer)
            {
                Layer layer = this.getLayer(node.id);

                foreach (Node subNode in layer.nodes)
                {
                    nodes.Add(subNode);

                    if (subNode.haslayer)
                    {
                        Nodes subNodes = this.getAllNodes(subNode);

                        foreach (Node subNode2 in subNodes)
                        {
                            nodes.Add(subNode2);
                        }
                    }
                }
            }

            return nodes;
        }

        public Lines getAllLines()
        {
            Lines lines = new Lines();

            foreach (Layer l in this.layers)
            {
                lines.AddRange(l.lines);
            }

            return lines;
        }

        // all nodes contain nodes and all sublayer nodes, allLines contain all node lines and all sublayer lines
        public void getAllNodesAndLines(Nodes nodes, ref Nodes allNodes, ref Lines allLines)
        {
            foreach (Node node in nodes)
            {
                // add node itself to output
                allNodes.Add(node);

                if (node.haslayer)
                {
                    Layer layer = this.getLayer(node.id);
                    getAllNodesAndLines(layer.nodes, ref allNodes, ref allLines);
                }

                Lines lines = getAllLinesFromNode(node);
                foreach (Line line in lines)
                {
                    bool found = false;

                    foreach (Line subline in allLines)
                    {
                        if (line.start == subline.start && line.end == subline.end)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        allLines.Add(line);
                    }
                }
            }
        }

        // get all lines for all children nodes
        public Lines getAllSubNodeLines(Node node)
        {
            Lines lines = new Lines();

            if (node.haslayer)
            {
                Layer layer = this.getLayer(node.id);

                foreach (Line line in layer.lines)
                {
                    lines.Add(line);
                }

                foreach (Node subNode in layer.nodes)
                {
                    if (node.haslayer)
                    {
                        Lines sublines = this.getAllSubNodeLines(subNode);

                        foreach (Line line in sublines)
                        {
                            lines.Add(line);
                        }
                    }
                }
            }

            return lines;
        }

        public Lines getAllLinesFromNode(Node node)
        {
            Lines lines = new Lines();

            Layer layer = this.getLayer(node);

            if (layer != null)
            {
                foreach (Line line in layer.lines)
                {
                    if (line.start == node.id || line.end == node.id)
                    {
                        lines.Add(line);
                    }
                }
            }

            return lines;
        }

        /// <summary>
        /// Search for string in all nodes </summary>        
        public Nodes searchInAllNodes(string searchFor)
        {
            Nodes nodes = new Nodes();

            foreach (Layer l in this.layers)
            {
                foreach (Node node in l.nodes)
                {
                    if (node.Contain(searchFor))
                    {
                        nodes.Add(node);
                    }
                }
            }

            return nodes;
        }

        /*************************************************************************************************************************/
        // ADD ITEM

        public Node createNode(Node node)
        {

            DateTime dt = DateTime.Now;
            node.timecreate = String.Format("{0:yyyy-M-d HH:mm:ss}", dt);
            node.timemodify = node.timecreate;

            Layer layer = this.addNode(node);

            if (layer != null)
            {
                return node;
            }

            return null;
        }

        public Layer addNode(Node node)
        {
            // prevent duplicate id
            if (node.id == 0)
            {
                node.id = ++maxid; // new node
            }
            else
            {

                Node nodeById = this.getNode(node.id); 

                if (nodeById != null)
                {
                    node.id = ++maxid; // already exist node with this id
                }
                else if (maxid < node.id)
                {
                    maxid = node.id; // node not exist but has id bigger then current max id
                }
            }

            Layer layer = this.getLayer(node.layer);

            if (layer == null)
            {
                Node parentNode = this.getNode(node.layer);
                layer = this.createLayer(parentNode);
                parentNode.haslayer = true;
            }

            this.allNodes.Add(node.id, node);
            layer.nodes.Add(node);

            return layer;
        }

        public Layer addLine(Line line)
        {
            Layer layer = this.getLayer(line.layer);

            if (layer == null)
            {
                layer = this.createLayer(this.getNode(line.layer));
            }

            layer.lines.Add(line);

            return layer;
        }

        /*************************************************************************************************************************/
        // REMOVE ITEM

        public void removeNode(int id)
        {
            Node node = this.getNode(id);
            if (node != null)
            {
                this.removeNode(node);
            }
        }

        public void removeNode(Node node)
        {
            this.allNodes.Remove(node.id);

            Layer layer = getLayer(node.layer);

            foreach (Node n in layer.nodes)
            {
                if (n.shortcut == node.id) // remove shortcut to node
                {
                    n.shortcut = 0;
                }
            }

            foreach (Line l in layer.lines.Reverse<Line>()) // remove lines to node
            {
                if (l.start == node.id || l.end == node.id)
                {
                    this.removeLine(l);
                }
            }

            if (node.haslayer) // remove nodes in node layer
            {
                removeLayer(node.id);
            }

            if (layer != null) // remove node from node layer
            {
                layer.nodes.Remove(node);
            }

        }

        public Layer removeLine(Line line)
        {
            Layer layer = this.getLayer(line.layer);

            layer.lines.Remove(line);

            return layer;
        }

        public Layer removeLine(int startId, int endId)
        {
            Line line = getLine(startId, endId);

            if (line != null)
            {
                return this.removeLine(line);
            }

            return null;
        }

        // LAYER remove layer and all sub layers
        public void removeLayer(int layerId)
        {
            Layer layer = this.getLayer(layerId);

            if (layer != null)
            {
                foreach (Node n in layer.nodes)
                {
                    if (n.haslayer)
                    {
                        removeLayer(n.id);
                    }

                    layers.Remove(layer);
                }
            }
        }

        public void clear()
        {
            this.maxid = 0;

            foreach (Layer l in this.layers)
            {
                l.lines.Clear();
                l.nodes.Clear();
            }

            this.allNodes.Clear();
            this.layers.Clear();
            this.createLayer();
        }

        /*************************************************************************************************************************/
        // MOVE ITEM

        // MOVE move node from layer to other layer
        public void moveNode(Node node, int layer)
        {
            Layer outLayer = getLayer(layer);
            Layer inLayer = getLayer(node.layer);

            if (outLayer != null && inLayer != null)
            {
                outLayer.nodes.Add(node);
                inLayer.nodes.Remove(node);

                node.layer = layer;
            }
        }

        // NODE move nodes to foreground
        public void moveToForeground(Node node)
        {
            Layer layer = getLayer(node);
            if (layer != null)
            {
                var item = node;
                layer.nodes.Remove(item);
                layer.nodes.Insert(layer.nodes.Count(), item);
            }
        }

        // NODE move nodes to background
        public void moveToBackground(Node node)
        {
            Layer layer = getLayer(node);
            if (layer != null)
            {
                var item = node;
                layer.nodes.Remove(node);
                layer.nodes.Insert(0, item);
            }
        }

    }
}
