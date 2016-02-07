using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    public class UndoOperation
    {
        public string type = "";
        
        public Nodes nodes = new Nodes();
        public Lines lines = new Lines();

        public UndoOperation(string type, Nodes nodes = null, Lines lines = null)
        {
            this.type = type;

            if (nodes != null)
            {
                foreach (Node node in nodes)
                {
                    this.nodes.Add(new Node(node));
                }
            }

            if (lines != null)
            {
                foreach (Line line in lines)
                {
                    this.lines.Add(new Line(line));
                }
            }
        }
    }

    public class Undo
    {
        public Diagram diagram = null;             // diagram assigned to current undo

        public Stack<UndoOperation> operations = new Stack<UndoOperation>();
        public Stack<UndoOperation> reverseOperations = new Stack<UndoOperation>();

        public Undo(Diagram diagram)
        {
            this.diagram = diagram;
        }

        public void add(string type, Node node)
        {
            Nodes nodes = new Nodes();
            if (node != null)
            {
                nodes.Add(new Node(node));
            }
            this.add(type, nodes);
        }

        public void add(string type, Line line)
        {
            Lines lines = new Lines();
            if (line != null)
            {
                lines.Add(new Line(line));
            }
            this.add(type, null, lines);
        }

        public void add(string type, Node node, Line line)
        {
            Nodes nodes = new Nodes();
            if (node != null)
            {
                nodes.Add(new Node(node));
            }

            Lines lines = new Lines();
            if (line != null)
            {
                lines.Add(new Line(line));
            }
            this.add(type, nodes, lines);
        }

        public void add(string type, Nodes nodes = null, Lines lines = null)
        {
            operations.Push(new UndoOperation(type, (nodes != null) ? new Nodes(nodes) : null, (lines != null) ? new Lines(lines) : null));

            // forgot undo operation
            if (reverseOperations.Count() > 0)
            {
                reverseOperations.Clear();
            }
        }

        private void doUndoDelete(UndoOperation operation)
        {
            if (operation.nodes != null)
            {
                foreach (Node node in operation.nodes)
                {
                    this.diagram.layers.addNode(node);
                }
            }

            if (operation.lines != null)
            {
                foreach (Line line in operation.lines)
                {
                    line.startNode = this.diagram.GetNodeByID(line.start);
                    line.endNode = this.diagram.GetNodeByID(line.end);
                    this.diagram.layers.addLine(line);
                }
            }

        }

        private void doUndoCreate(UndoOperation operation)
        {
            if (operation.lines != null)
            {
                foreach (Line line in operation.lines)
                {
                    this.diagram.layers.removeLine(line);
                }
            }

            if (operation.nodes != null)
            {
                foreach (Node node in operation.nodes)
                {
                    this.diagram.layers.removeNode(node.id);
                }
            }
        }

        private void doUndoEdit(UndoOperation operation)
        {
            if (operation.lines != null)
            {
                foreach (Line lineOld in operation.lines)
                {
                    lineOld.startNode = this.diagram.GetNodeByID(lineOld.start);
                    lineOld.endNode = this.diagram.GetNodeByID(lineOld.end);
                    Line line = this.diagram.layers.getLine(lineOld.startNode, lineOld.endNode);

                    if (line != null)
                    {
                        line.set(lineOld);
                    }
                }
            }

            if (operation.nodes != null)
            {
                foreach (Node nodeOld in operation.nodes)
                {
                    Node node = this.diagram.layers.getNode(nodeOld.id);

                    if (node != null)
                    {
                        node.set(nodeOld);
                    }
                }
            }
        }

        public bool doUndo()
        {
            if (operations.Count() > 0)
            {
                UndoOperation operation = operations.Pop();

                if (operation.type == "delete")
                {
                    this.doUndoDelete(operation);
                }

                if (operation.type == "create")
                {
                    this.doUndoCreate(operation);
                }

                if (operation.type == "edit")
                {
                    this.doUndoEdit(operation);
                }

                reverseOperations.Push(operation);

                return true;
            }

            return false;
        }

        public bool doRedo()
        {
            if (operations.Count() > 0)
            {
                UndoOperation operation = reverseOperations.Pop();

                operations.Push(operation);

                return true;
            }

            return false;
        }
    }
}
