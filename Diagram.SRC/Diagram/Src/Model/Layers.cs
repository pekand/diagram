using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    public class Layers
    {
        public List<Layer> layers = new List<Layer>();

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

        public List<Node> getAllNodes()
        {
            List<Node> nodes = new List<Node>();

            foreach (Layer l in this.layers)
            {
                nodes.AddRange(l.nodes);
            }

            return nodes;
        }

        public List<Line> getAllLines()
        {
            List<Line> lines = new List<Line>();

            foreach (Layer l in this.layers)
            {
                lines.AddRange(l.lines);
            }

            return lines;
        }

        public Layer addNode(Node node)
        {
            Layer layer = this.getLayer(node.layer);

            if (layer == null)
            {
                Node parentNode = this.getNode(node.layer);
                layer = this.createLayer(parentNode);
                parentNode.haslayer = true;
            }

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

        public Node getNode(int id)
        {
            foreach (Layer l in this.layers)
            {
                foreach (Node node in l.nodes)
                {
                    if (node.id == id)
                        return node;
                }
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

        public void removeNode(Node node)
        {
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
            foreach (Layer l in this.layers)
            {
                l.lines.Clear();
                l.nodes.Clear();
            }

            layers.Clear();
            this.createLayer();
        }

        public void buildTree()
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
    }
}
