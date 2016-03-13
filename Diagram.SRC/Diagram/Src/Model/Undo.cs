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

        public int group = 0; // undo operations in some grop are undo in one step
        
        public Nodes nodes = new Nodes();
        public Lines lines = new Lines();

        public Position position = new Position(); // position in diagram when change occurred
        public int layer = 0;

        public UndoOperation(
            string type, 
            Nodes nodes = null, 
            Lines lines = null, 
            int group = 0, 
            Position position = null, 
            int layer = 0
        ) {
            this.type = type;
            this.group = group;
            this.position.set(position);
            this.layer = layer;

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
        public int group = 0; // if two operations is in same group then undo restore both operations

        public int saved = 0; // if is 0 then indicate saved
        public bool saveLost = false; // if save is in redo and redo is cleared then save position is lost
        public bool grouping = false; // if grouping is true and new undo is added then new undo is same group as previous undo

        public Diagram diagram = null;                // diagram assigned to current undo
        
        public Stack<UndoOperation> operations = new Stack<UndoOperation>();
        public Stack<UndoOperation> reverseOperations = new Stack<UndoOperation>();

        public Undo(Diagram diagram)
        {
            this.diagram = diagram;
        }

        public void add(string type, Node node, Position position = null, int layer = 0)
        {
            Nodes nodes = new Nodes();
            if (node != null)
            {
                nodes.Add(new Node(node));
            }
            this.add(type, nodes, null, position, layer);
        }

        public void add(string type, Line line, Position position = null, int layer = 0)
        {
            Lines lines = new Lines();
            if (line != null)
            {
                lines.Add(new Line(line));
            }
            this.add(type, null, lines, position, layer);
        }

        public void add(string type, Node node, Line line, Position position = null, int layer = 0)
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
            this.add(type, nodes, lines, position, layer);
        }

        public void add(string type, Nodes nodes = null, Lines lines = null, Position position = null, int layer = 0)
        {
            operations.Push(
                new UndoOperation(
                    type, 
                    (nodes != null) ? new Nodes(nodes) : null, 
                    (lines != null) ? new Lines(lines) : null, 
                    (grouping) ? group : 0, // add multiple operations into one undo group
                    position,
                    layer
                )
            );

            this.saved++;
            
            // forgot undo operation
            if (reverseOperations.Count() > 0)
            {
                if (this.saved > 0) // save is in redo but if redo is cleared theh save is lost
                {
                    this.saveLost = true;
                }
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
                    this.diagram.layers.removeLine(line.start, line.end);
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

        public int startGroup()
        {
            grouping = true;
            return ++this.group;
        }

        public int endGroup()
        {
            grouping = false;
            return ++this.group;
        }

        public bool isSame(string type, Nodes nodes, Lines lines)
        {
            
            if (operations.Count()>0)
            {

                UndoOperation operation = operations.First();

                if (operation.type == type)
                {

                    if (operation.nodes.Count() == 0 && nodes == null)
                    {
                    }
                    else if ((operation.nodes == null && nodes != null) || (operation.nodes != null && nodes == null))
                    {
                        return false;
                    }
                    else if (operation.nodes.Count() == nodes.Count())
                    {
                        for (int i = 0; i < nodes.Count(); i++)
                        {
                            if (operation.nodes[i].id != nodes[i].id)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }

                    if (operation.lines.Count() == 0 && lines == null)
                    {
                    }
                    else if ((operation.lines == null && lines != null) || (operation.lines != null && lines == null))
                    {
                        return false;
                    }
                    else if (operation.lines.Count() == lines.Count())
                    {
                        for (int i = 0; i < lines.Count(); i++)
                        {
                            if (operation.lines[i].start != lines[i].start || operation.lines[i].end != lines[i].end)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }

                    return true;
                }
            }

            return false;
        }

        public bool doUndo(DiagramView view = null)
        {

            if (operations.Count() == 0)
            {
                return false;
            }

            int group = 0;

            bool result = false;

            do
            {
                UndoOperation operation = operations.First();

                // first restore position where change occurred
                if (view != null && !view.isOnPosition(operation.position, operation.layer))
                {
                    view.goToShift(operation.position);
                    view.goToLayer(operation.layer);
                    view.Invalidate();
                    return false;
                }

                // process all operations in same group
                if (group != 0 && operation.group != group)
                {
                    group = 0;
                    break;
                }

                group = operation.group;

                if (operation.type == "delete")
                {
                    this.doUndoDelete(operation);
                    reverseOperations.Push(operation);
                }

                if (operation.type == "create")
                {
                    this.doUndoCreate(operation);
                    reverseOperations.Push(operation);
                }

                if (operation.type == "edit" || 
                    operation.type == "move" || 
                    operation.type == "changeLineColor" || 
                    operation.type == "changeLineWidth" || 
                    operation.type == "changeNodeColor"
                ) {
                    Nodes nodes = new Nodes();
                    foreach (Node node in operation.nodes)
                    {
                        nodes.Add(this.diagram.GetNodeByID(node.id));
                    }

                    Lines lines = new Lines();
                    foreach (Line line in operation.lines)
                    {
                        lines.Add(this.diagram.getLine(line.start, line.end));
                    }

                    UndoOperation roperation = new UndoOperation(
                        operation.type, 
                        nodes, 
                        lines,
                        operation.group, 
                        operation.position, 
                        operation.layer
                    );
                    reverseOperations.Push(roperation);
                    this.doUndoEdit(operation);
                }

                operations.Pop();
                result = true;
            } while (group != 0 && operations.Count() > 0);

            if (result) {
                this.saved--;
                if (!this.saveLost && this.saved == 0)
                {
                    this.diagram.restoresave();
                }
                else
                {
                    this.diagram.unsave();
                }
            }

            return result;
        }

        public bool doRedo(DiagramView view = null)
        {
            if (reverseOperations.Count() == 0)
            {
                return false;
            }

            int group = 0;
            bool result = false;

            do
            {
                UndoOperation operation = reverseOperations.First();

                // first restore position where change occurred
                if (view != null && !view.isOnPosition(operation.position, operation.layer))
                {
                    view.goToShift(operation.position);
                    view.goToLayer(operation.layer);
                    view.Invalidate();
                    return false;
                }

                // process all operations in same group
                if (group != 0 && operation.group != group)
                {
                    group = 0;
                    break;
                }

                group = operation.group;

                if (operation.type == "delete")
                {
                    this.doUndoCreate(operation);
                    operations.Push(operation);
                }

                if (operation.type == "create")
                {
                    this.doUndoDelete(operation);
                    operations.Push(operation);
                }

                if (operation.type == "edit" ||
                    operation.type == "move" ||
                    operation.type == "changeLineColor" ||
                    operation.type == "changeLineWidth" ||
                    operation.type == "changeNodeColor"
                )
                {
                    Nodes nodes = new Nodes();
                    foreach (Node node in operation.nodes)
                    {
                        nodes.Add(this.diagram.GetNodeByID(node.id));
                    }

                    Lines lines = new Lines();
                    foreach (Line line in operation.lines)
                    {
                        lines.Add(this.diagram.getLine(line.start, line.end));
                    }

                    UndoOperation roperation = new UndoOperation(
                        operation.type, 
                        nodes, 
                        lines, 
                        operation.group,
                        operation.position, 
                        operation.layer
                    );

                    operations.Push(roperation);
                    this.doUndoEdit(operation);
                }

                reverseOperations.Pop();
                result = true;

            } while (group != 0 && reverseOperations.Count() > 0);

            if (result)
            {
                this.saved++;
                if (!this.saveLost && this.saved == 0)
                {
                    this.diagram.restoresave();
                }
                else
                {
                    this.diagram.unsave();
                }
            }

            return result;
        }

        public void rememberSave()
        {
            saveLost = false;
            saved = 0;
        }

        public bool canUndo()
        {
            return this.operations.Count() > 0;
        }

        public bool canRedo()
        {
            return this.reverseOperations.Count() > 0;
        }
    }
}
