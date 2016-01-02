using System.Drawing;

namespace Diagram
{
    /// <summary>
    /// Node in diagram</summary>
    public class Node
    {
        public int id = 0; // node unique id

        public bool selected = false; // node is selected by mouse

        public Font font = null; // node name font
        public Color fontcolor = System.Drawing.Color.Black; // node name ext color

        public string text = ""; // node name
        public string note = ""; // node note
        public string link = ""; // node link to external source

        public int shortcut = 0; // node id whitch is linked with this node

        public Position position = new Position(); // node position in canvas

        public int width = 0; // node size counted from current font
        public int height = 0;

        public int layer = 0; // cislo vrstvy v ktorej sa noda nachádza
        public bool haslayer = false;
        public Position layerShift = new Position(); // zaciatocna pozícia vrstvy

        public Color color = System.Drawing.ColorTranslator.FromHtml("#FFFFB8"); // node color
        public bool transparent = false; // node is transparent, color is turn off

        public bool isimage = false; // show node as image instead of text
        public bool embeddedimage = false; // image is imported to xml file as string
        public string imagepath = ""; // path to node image
        public Bitmap image = null; // loaded image
        public int iwidth = 0; //image size
        public int iheight = 0;

        public string attachment = ""; // compressed file attachment

        public string timecreate = ""; // node creation time
        public string timemodify = "";// node modification time

        public bool visible = true;

        //Script
        public string scriptid = ""; // node text id for in script search

        /// <summary>
        /// node copy from another node to current node</summary>
        public void copyNode(Node node, bool skipPosition = false, bool skipSize = false) 
        {
            this.font = node.font;
            this.fontcolor = node.fontcolor;
            this.text = node.text;
            this.note = node.note;
            this.link = node.link;
            this.shortcut = node.shortcut;

            if (!skipPosition)
            {
                this.position = node.position;
            }

            if (!skipSize)
            {
                this.width = node.width;
                this.height = node.height;
            }

            this.color = node.color;
            this.transparent = node.transparent;
            this.isimage = node.isimage;
            this.embeddedimage = node.embeddedimage;
            this.imagepath = node.imagepath;
            this.image = node.image;
            this.iwidth = node.iwidth;
            this.iheight = node.iheight;
        }
    }
}
