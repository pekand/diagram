using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Diagram;
using System.Text.RegularExpressions;
using System.IO;

namespace Plugin
{
    public class CreateDirectoryPlugin : IPopupPlugin
    {
        public string Name
        {
            get
            {
                return "Create directory from diagram";
            }
        }

        public string Version
        {
            get
            {
                return "1.0";
            }
        }

        private string location = null;

        public void SetLocation(string location)
        {
            this.location = location;
        }

        private Log log = null;

        public void SetLog(Log log)
        {
            this.log = log;
        }

        public void OpenFileOnPosition(string file, long pos = 0)
        {
            Os.OpenFileOnPosition(file, pos);
        }

        public void CreateDirectoryItem_Click(object sender, EventArgs e, Diagram.DiagramView diagramview)
        {
            string DiagramPath = diagramview.diagram.FileName.Trim();

            if (DiagramPath == "" && !Os.FileExists(DiagramPath)) {
                return;
            }

            string NewDirectoryPath = Os.Combine(Os.GetFileDirectory(DiagramPath), "test");

            Os.CreateDirectory(DiagramPath);

            Node newrec = diagramview.CreateNode(diagramview.shift.Add(diagramview.Width,diagramview.Height));

            newrec.setName("test");
            newrec.link = NewDirectoryPath;
            diagramview.diagram.Unsave("create", newrec, diagramview.shift, diagramview.currentLayer.id);

        }

        public void PopupAddItemsAction(Diagram.DiagramView diagramview, System.Windows.Forms.ToolStripMenuItem pluginsItem)
        {
            System.Windows.Forms.ToolStripMenuItem createDirectoryItem = new System.Windows.Forms.ToolStripMenuItem();
            createDirectoryItem.Name = "editItem";
            createDirectoryItem.Text = "CreateDirectory";
            createDirectoryItem.Click += new System.EventHandler((sender, e) => this.CreateDirectoryItem_Click(sender, e, diagramview));

            pluginsItem.DropDownItems.Add(createDirectoryItem);
        }

        public void PopupOpenAction(Diagram.DiagramView diagramview, System.Windows.Forms.ToolStripMenuItem pluginsItem)
        {

        }
    }
}
