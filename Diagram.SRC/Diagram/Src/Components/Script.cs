using System;
using System.Windows.Forms;

using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System.IO;

/*
    ! | eval | evaluate
    script executed by F9
    evaluate selected nodes or all nodes globaly

    !#1 | eval#1 | evaluate#1
    script evaluated by priority

    $ | script | macro
    evaluate node only by double click

    @id
    get node by diagram.getNoteBySciptId
*/

/*
    #Tools

    F.log('text') # write to console F12
    F.show(string message) # show message dialog
    F.setClipboard() # set clipboard content to value after script finish
    F.getClipboard() # get clipboard content before script run
    F.get('scriptId') # get node by scriptId
    F.id(1) # get node by id
    F.layer() #current layer id
    F.create(100, 100, "name", [layerId]) #create node id
    F.connect(nodeA, nodeB)
    F.remove(node)
    F.delete(node)
    F.go(node) #go to node position
    F.go(x, y, [layerId])
    F.position() # return current position in diagram
    F.refresh() # refres diagram views
    F.val('123') # convert str to int
*/

/*
# example of python script:
#
# create circle from nodes in current layer

import clr
import math
clr.AddReference('Diagram')
from Diagram import Position

a = (2 * math.pi) / 100
l = DiagramView.currentLayer.id
prev = None
for i in range(100):
    x = int(500 * math.cos(a*i))
    y = int(500 * math.sin(a*i))
    rec = Diagram.createNode(Position(x, y), "", l)
    rec.transparent = True
    if prev != None:
        Diagram.Connect(rec, prev)
    prev = rec
DiagramView.Invalidate();

*/

/*
# example of python script:
#
# create circle from nodes in current layer

import clr
import math

a = (2 * math.pi) / 100
l = F.layer().id
prev = None
for i in range(100):
    x = int(500 * math.cos(a*i))
    y = int(500 * math.sin(a*i))
    rec = F.create(x, y, "", l)
    rec.transparent = True
    if prev != None:
        F.connect(rec, prev)
    prev = rec
F.refresh();

*/

/*
    # short example of python script using Tools:
    #
    # create circle from nodes in current layer

    import math

    a = (2 * math.pi) / 100
    l = F.layer()
    prev = None
    for i in range(100):
        x = int(500 * math.cos(a*i))
        y = int(500 * math.sin(a*i))
        rec = F.create(x, y, "", l)
        rec.transparent = True
        if prev != None:
            F.connect(rec, prev)
        prev = rec
    F.refresh()
*/

namespace Diagram
{

    /// <summary>
    ///  Tools advalible from script
    /// </summary>
    /// <example>
    /// Tools.log('message')
    /// Tools.ShowMessage('message')
    /// Tools.setClipboard('text')
    /// a = Tools.getClipboard()
    /// </example>
    public class Tools
    {
        private Script script = null;

        public string clipboard = "";

        public Tools(Script script)
        {
            this.script = script;
        }

        public void log(String text)
        {
            Program.log.write(text);
        }

        public void show(string message)
        {
            MessageBox.Show(message);
        }

        public void setClipboard(String clipboard)
        {
            this.clipboard = clipboard;
        }

        public String getClipboard()
        {
            return this.clipboard;
        }

        public Node get(string nodeScriptId)
        {
            return this.script.diagram.getNodeByScriptID(nodeScriptId);
        }

        public Node id(int id)
        {
            return this.script.diagram.GetNodeByID(id);
        }

        public Position position(int x, int y)
        {
            return new Position(x, y);
        }

        public Layer layer()
        {
            return this.script.diagramView.currentLayer;
        }

        public Node create(Position p, string name = "", int layer = -1)
        {
            if (layer < 0)
            {
                layer = this.layer().id;
            }

            return this.script.diagram.createNode(p, name, layer);
        }

        public Node create(int x, int y, string name = "", int layer = -1)
        {
            return this.script.diagram.createNode(new Position(x,y), name, layer);
        }

        public Line connect(Node a, Node b)
        {
            return this.script.diagram.Connect(a, b);
        }

        public void remove(Node n)
        {
            this.script.diagram.DeleteNode(n);
        }

        public void delete(Node n)
        {
            this.script.diagram.DeleteNode(n);
        }

        public void go(Node n)
        {
            this.script.diagramView.goToNode(n);
        }

        public void go(int x, int y, int layer = -1)
        {
            if (layer >= 0)
            {
                this.script.diagramView.goToLayer(layer);
            }

            this.script.diagramView.goToPosition(new Position(x, y));
        }

        public Position position()
        {
            return this.script.diagramView.shift;
        }

        public void refresh()
        {
            this.script.diagramView.Invalidate();
        }

        public int val(string s)
        {
            int x = 0;

            if (Int32.TryParse(s, out x))
            {
                return x;
            }

            return 0;
        }

        public string val(int v)
        {
            return v.ToString();
        }
    }

    /// <example>
    /// try
    /// {
    ///
    ///     Script macro = new Script();
    ///     result = macro.runScript(this.SelectedNodes[0].text);
    /// }
    /// catch(Exception ex)
    /// {
    ///     Program.log.write("evaluation error: " + ex.Message);
    /// }
    /// </example>
    public class Script
    {
        private ScriptEngine pyEngine = null;
        private dynamic pyScope = null;

        // ATTRIBUTES Diagram
        public Diagram diagram = null;       // diagram ktory je previazany z pohladom
        public DiagramView diagramView = null;       // diagram ktory je previazany z pohladom
        public Tools tools = null;

        public string script = "";

        public Script()
        {
            this.tools = new Tools(this);
        }

        /// <summary>
        /// Set current diagram for context in script
        /// </summary>
        /// <param name="diagram"></param>
        public void setDiagram(Diagram diagram)
        {
            this.diagram = diagram;
        }

        /// <summary>
        /// Get diagram in current script context
        /// </summary>
        /// <returns>Diagram</returns>
        public Diagram getDiagram()
        {
            return this.diagram;
        }

        /// <summary>
        /// Set current diagramView for context in script
        /// </summary>
        /// <param name="diagramView"></param>
        public void setDiagramView(DiagramView diagramView)
        {
            this.diagramView = diagramView;
        }

        /// <summary>
        /// Get diagramView in current script context
        /// </summary>
        /// <returns></returns>
        public DiagramView getDiagramView()
        {
            return this.diagramView;
        }

        /// <summary>
        /// Set text to clipboard
        /// </summary>
        /// <param name="clipboard"></param>
        public void setClipboard(String clipboard)
        {
            this.tools.setClipboard(clipboard);
        }

        /// <summary>
        /// Get text from clipboard
        /// </summary>
        /// <returns></returns>
        public String getClipboard()
        {
            return this.tools.getClipboard();
        }

        /// <summary>
        /// Run python code in curent scope
        /// </summary>
        /// <param name="code">python code</param>
        /// <returns></returns>
        private dynamic CompileSourceAndExecute(String code)
        {
            ScriptSource source = pyEngine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);
            CompiledCode compiled = source.Compile(); // Executes in the scope of Python
            return compiled.Execute(pyScope);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="script">Script with python code</param>
        /// <example>
        /// import clr
        /// clr.AddReference('Diagram')
        /// from Diagram import Position
        /// DiagramView.CreateNode(Position(0,0))
        /// </example>
        /// <example>
        /// Tools.log("test")
        /// Tools.ShowMessage("test")
        /// Tools.setClipboard("test")
        /// clp = Tools.getClipboard()
        /// <returns>Return script string result</returns>
        public string runScript(String script)
        {
            if (pyEngine == null)
            {
                pyEngine = Python.CreateEngine();
                pyScope = pyEngine.CreateScope();

                /// add items to scope
                pyScope.Diagram = this.diagram;
                pyScope.Tools = this.tools;
                pyScope.F = this.tools;
                pyScope.DiagramView = this.diagramView;
            }

			string output = null;

            try
            {
                /// set streams
                MemoryStream ms = new MemoryStream();
                StreamWriter outputWr = new StreamWriter(ms);
                pyEngine.Runtime.IO.SetOutput(ms, outputWr);
                pyEngine.Runtime.IO.SetErrorOutput(ms, outputWr);

                /// execute script
                this.CompileSourceAndExecute(script);

                /// read script output
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms);
                output = sr.ReadToEnd();

                Program.log.write("Script: output:\n" + output);
            }
            catch (Exception ex)
            {
                Program.log.write("Script: error: "+ex.ToString());
            }

			return output;
        }
    }
}
