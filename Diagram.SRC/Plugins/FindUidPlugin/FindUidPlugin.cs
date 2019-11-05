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
    public class FindUidPlugin : INodeOpenPlugin, IOpenDiagramPlugin //UID0290845813
    {
        public string Name
        {
            get
            {
                return "Find uid in current diagram directory";
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

        public bool IsUid(string text)
        {
            Match matchUid = (new Regex(@"^UID\d{10}$")).Match(text);

            if (matchUid.Success)
            {
                return true;
            }

            Match matchString = (new Regex(@"^~[^ ]*$")).Match(text);

            if (matchString.Success)
            {
                return true;
            }

            return false;
        }

        public string GetUid(string text)
        {
            text = text.Trim();

            Match matchUid = (new Regex(@"^UID\d{10}$")).Match(text);

            if (matchUid.Success)
            {
                return text;
            }

            Match matchString = (new Regex(@"^~([^ ]*)$")).Match(text);

            if (matchString.Success)
            {
                return matchString.Groups[1].Value;
            }

            return null;
        }

        public void OpenFileOnPosition(string file, long pos = 0)
        {
            Os.OpenFileOnPosition(file, pos);
        }

        public bool ClickOnNodeAction(Diagram.Diagram diagram, DiagramView diagramview, Node node)
        {
            if (diagram.FileName !="" && this.IsUid(node.link)) {
                string uid = this.GetUid(node.link);
                if (Os.FileExists(diagram.FileName)) {
                    string diagramDirectory = Os.GetFileDirectory(diagram.FileName);

                    foreach (string file in Directory.EnumerateFiles(diagramDirectory, "*.*", SearchOption.AllDirectories))
                    {
                        try
                        {

                            // skip self
                            if (file == diagram.FileName)
                            {
                                continue;
                            }

                            long pos = 1;
                            foreach (string line in File.ReadAllLines(file))
                            {
                                if (line.Contains(uid))
                                {

                                    this.OpenFileOnPosition(file, pos);
                                    return true;
                                }
                                pos++;
                            }

                        }
                        catch (Exception ex)
                        {
                            Program.log.Write("FindUidPlugin: " + ex.Message);
                        }
                    }
                }
            }

            return false;
        }

        public void OpenDiagramAction(Diagram.Diagram diagram)
        {
            this.log.Write("Diagram is changed");
        }
    }
}
