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
    public class FindUidPlugin : INodeOpenPlugin
    {
        #region IPlugin Members 

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

        public bool isUid(string text)
        {
            Match matchUid = (new Regex(@"^UID\w{10}$")).Match(text);

            if (matchUid.Success)
            {
                return true;
            }

            return false;
        }

        public void openFileOnPosition(string file, int pos = 0)
        {
            Os.OpenFileOnPosition(file, pos);
        }

        public bool ClickOnNodeAction(Diagram.Diagram diagram, Node node)
        {
            if (diagram.FileName !="" && this.isUid(node.link.Trim())) {
                string uid = node.link.Trim();
                if (Os.FileExists(diagram.FileName)) {
                    string diagramDirectory = Os.GetFileDirectory(diagram.FileName);

                    try {
                        foreach (string file in Directory.EnumerateFiles(diagramDirectory, "*.cs", SearchOption.AllDirectories))
                        {
                            int pos = 1;
                            foreach (string line in File.ReadAllLines(file))
                            {
                                if (line.Contains(uid)) {

                                    this.openFileOnPosition(file, pos);
                                    return true;
                                }
                                pos++;
                            }
                        }
                    } catch (Exception ex) {

                    }
                }
            }

            return false;
        }

        #endregion
    }
}
