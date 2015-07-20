using System;
using System.Windows.Forms;

using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System.IO;

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

        public string clipboard = "";

        public void log(String text)
        {
            Program.log.write(text);
        }

        public void ShowMessage(string message)
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

    }

    class Script
    {
        private ScriptEngine pyEngine = null;
        private dynamic pyScope = null;

        // ATTRIBUTES Diagram
        public Diagram diagram = null;       // diagram ktory je previazany z pohladom
        public DiagramView diagramView = null;       // diagram ktory je previazany z pohladom
        public Tools tools = new Tools();

        public string script = "";

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
        /// <param name="script">Script with pys=thon code</param>
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
