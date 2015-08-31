using System.Drawing;

namespace Diagram
{
    // NODE Obdlznik - noda
    public class Node
    {
        public int id = 0; // jednoznačný identifikátor nody

        public bool selected = false; // či je noda vybraná

        public Font font = null;
        public Color fontcolor = System.Drawing.Color.Black;

        public string text = ""; // názov nody
        public string note = ""; // poznámka v node
        public string link = ""; // odkaz v node

        public int shortcut = 0; // číslo nody na ktorú odkazuje daná noda

        public Position position = new Position(); // pozícia nody

        public int width = 0; // velkost nody (po otvorení súboru sa prerátava)
        public int height = 0;

        public int layer = 0; // cislo vrstvy v ktorej sa noda nachádza
        public bool haslayer = false;
        public int layershiftx = 0; // zaciatocna pozícia vrstvy
        public int layershifty = 0;

        public Color color = System.Drawing.ColorTranslator.FromHtml("#FFFFB8");
        public bool transparent = false;

        public bool isimage = false; // má sa zobraziť obrázok namiesto textu nody
        public bool embeddedimage = false; // obrázok je vlozený v súbore
        public string imagepath = ""; // noda má externý obrázok ktorý je nalinkovaný
        public Bitmap image = null;
        public int iwidth = 0; //velkost obrazku
        public int iheight = 0;

        public string timecreate = ""; // cas vytvorenia nody
        public string timemodify = "";// cas modifikovania nody

        //Script
        public string scriptid = ""; // oznacenie

        public void copyNode(Node node, bool skipPosition = false, bool skipSize = false) {
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
