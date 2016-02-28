using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Text.RegularExpressions;

namespace Diagram
{
    // map node structure for copy paste operation
    public struct MappedNode
    {
        public int oldId;
        public Node newNode;
    }

    public class Diagram
    {
        public Main main = null;                 // reference to main form

        public Layers layers = new Layers();

        public List<DiagramView> DiagramViews = new List<DiagramView>(); // all views forms to diagram

        // RESOURCES
        public Font FontDefault = null;          // default font

        // ATTRIBUTES File
        public bool NewFile = true;              // flag for new unsaved file without name
        public bool SavedFile = true;            // flag for saved diagram with name
        public string FileName = "";             // path to diagram file

        // ATRIBUTES OBJECTS
        public int maxid = 0;                    // last used node id

        // ATTRIBUTES ENCRYPTION
        public bool encrypted = false;           // flag for encrypted file
        public string password = "";             // password for encrypted file
		private byte[] salt = null;              // salt

        // UNDO
        public Undo undo = null;                        // undo operations repository

        // ATTRIBUTES TextForm
        public List<TextForm> TextWindows = new List<TextForm>();   // opened text textforms for this diagram

        // ATTRIBUTES OPTIONS
        public Options options = new Options();  // diagram options saved to xml file

        public Diagram(Main main)
        {
            this.main = main;
            this.FontDefault = new Font("Open Sans", 10);
            this.undo = new Undo(this);
        }

        /*************************************************************************************************************************/

        // FILE IS NEW - check if file is empty
        public bool isNew()
        {
            return (this.FileName == "" && this.NewFile && this.SavedFile);
        }

        // FILE IS NEW - check if file is empty
        public bool isReadOnly()
        {
            return this.options.readOnly;
        }

        // FILE OPEN Open xml file. If file is invalid return false
        public bool OpenFile(string FileName)
        {
            if (Os.FileExists(FileName))
            {
                Os.setCurrentDirectory(Os.getFileDirectory(FileName));

                this.CloseFile();
                this.FileName = FileName;
                this.NewFile = false;
                this.SavedFile = true;

                string xml = Os.getFileContent(FileName);

                if (xml == null) {
                    this.CloseFile();
                    return false;
                }

                bool opened = false;
                if (xml.Trim() == "")
                {
                    opened = true; // count empty file as valid new diagram
                }
                else
                {
                    opened = this.LoadXML(xml);
                }

                this.SetTitle();

                return opened;

            }

            return false;
        }

        // FILE LOAD XML. If file is invalid return false
        public bool LoadXML(string xml)
        {

            XmlReaderSettings xws = new XmlReaderSettings();
            xws.CheckCharacters = false;

            string version = "";
            string salt = "";
            string encrypted = "";

            try
            {
                using (XmlReader xr = XmlReader.Create(new StringReader(xml), xws))
                {

                    XElement root = XElement.Load(xr);
                    foreach (XElement diagram in root.Elements())
                    {
                        if (diagram.Name.ToString() == "version")
                        {
                            version = diagram.Value;
                        }

                        if (diagram.Name.ToString() == "salt")
                        {
                            salt = diagram.Value;
                        }

                        if (diagram.Name.ToString() == "encrypted")
                        {
                            encrypted = diagram.Value;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(main.translations.fileHasWrongFormat);
                Program.log.write("load xml error: " + ex.Message);
                this.CloseFile();
            }

            if (version == "1" && salt != "" && encrypted != "")
            {
                // prevent open multiple files with password in one time
                if (main.passwordForm != null)
                {
                    this.CloseFile();
                    return false;
                }

                bool error = false;
                do
                {
                    error = false;

                    if (main.passwordForm == null)
                    {
                        main.passwordForm = new PasswordForm(main);
                    }

                    main.passwordForm.Clear();
                    main.passwordForm.ShowDialog();
                    if (!main.passwordForm.cancled)
                    {
                        try
                        {
                            this.password = main.passwordForm.GetPassword();
                            this.salt = Encrypt.SetSalt(salt);
                            this.LoadInnerXML(Encrypt.DecryptStringAES(encrypted, this.password, this.salt));
                        }
                        catch(Exception e)
                        {
                            // probably invalid password
                            Program.log.write("LoadXML: Password or file is invalid: " + e.Message);
                            error = true;
                        }
                    }
                    else
                    {
                        main.passwordForm.CloseForm();
                        this.CloseFile();
                        return false;
                    }

                } while (error);

                main.passwordForm.CloseForm();
                return true;
            }
            else
            {
                return LoadInnerXML(xml);
            }
        }

        // FILE LOAD XML inner part of diagram file. If file is invalid return false
        public bool LoadInnerXML(string xml)
        {
            string FontDefaultString = TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(this.FontDefault);

            XmlReaderSettings xws = new XmlReaderSettings();
            xws.CheckCharacters = false;

            Nodes nodes = new Nodes();
            Lines lines = new Lines();

            try
            {
                using (XmlReader xr = XmlReader.Create(new StringReader(xml), xws))
                {

                    XElement root = XElement.Load(xr);
                    foreach (XElement diagram in root.Elements())
                    {
                        if (diagram.HasElements)
                        {

                            if (diagram.Name.ToString() == "option")
                            {
                                foreach (XElement el in diagram.Descendants())
                                {
                                    try
                                    {
                                        if (el.Name.ToString() == "shiftx")
                                        {
                                            this.options.homePosition.x = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "shifty")
                                        {
                                            this.options.homePosition.y = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "homelayer")
                                        {
                                            options.homeLayer = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "endlayer")
                                        {
                                            options.endLayer = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "endPositionx")
                                        {
                                            this.options.endPosition.x = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "endPositiony")
                                        {
                                            this.options.endPosition.y = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "startShiftX")
                                        {
                                            options.homePosition.x = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "startShiftY")
                                        {
                                            options.homePosition.y = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "diagramreadonly")
                                        {
                                            this.options.readOnly = bool.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "grid")
                                        {
                                            this.options.grid = bool.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "borders")
                                        {
                                            this.options.borders = bool.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "defaultfont")
                                        {
                                            if (el.Attribute("type").Value == "font")
                                            {
                                                this.FontDefault = Fonts.XmlToFont(el);
                                            }
                                            else
                                            {
                                                if (FontDefaultString != el.Value)
                                                {
                                                    this.FontDefault = (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(el.Value);
                                                }
                                            }
                                        }

                                        if (el.Name.ToString() == "coordinates")
                                        {
                                            this.options.coordinates = bool.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "firstLayereShift.x")
                                        {
                                            this.options.firstLayereShift.x = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "firstLayereShift.y")
                                        {
                                            this.options.firstLayereShift.y = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "window.position.restore")
                                        {
                                            this.options.restoreWindow = bool.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "window.position.x")
                                        {
                                            this.options.Left = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "window.position.y")
                                        {
                                            this.options.Top = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "window.position.width")
                                        {
                                            this.options.Width = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "window.position.height")
                                        {
                                            this.options.Height = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "window.state")
                                        {
                                            this.options.WindowState = Int32.Parse(el.Value);
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Program.log.write("load xml diagram options: " + ex.Message);
                                    }
                                }
                            }

                            if (diagram.Name.ToString() == "rectangles")
                            {
                                foreach (XElement block in diagram.Descendants())
                                {

                                    if (block.Name.ToString() == "rectangle")
                                    {
                                        Node R = new Node();
                                        R.font = this.FontDefault;

                                        foreach (XElement el in block.Descendants())
                                        {
                                            try
                                            {
                                                if (el.Name.ToString() == "id")
                                                {
                                                    R.id = Int32.Parse(el.Value);
                                                    maxid = (R.id > maxid) ? R.id : maxid;
                                                }

                                                if (el.Name.ToString() == "font")
                                                {
                                                    if (el.Attribute("type").Value == "font")
                                                    {
                                                        R.font = Fonts.XmlToFont(el);
                                                    }
                                                    else
                                                    {
                                                        if (FontDefaultString != el.Value)
                                                        {
                                                            R.font = (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(el.Value);
                                                        }
                                                    }
                                                }

                                                if (el.Name.ToString() == "fontcolor")
                                                {
                                                    R.fontcolor.set(el.Value.ToString());
                                                }

                                                if (el.Name.ToString() == "text")
                                                {
                                                    R.name = el.Value;
                                                }


                                                if (el.Name.ToString() == "note")
                                                {
                                                    R.note = el.Value;
                                                }


                                                if (el.Name.ToString() == "link")
                                                {
                                                    R.link = el.Value;
                                                }

                                                if (el.Name.ToString() == "scriptid")
                                                {
                                                    R.scriptid = el.Value;
                                                }

                                                if (el.Name.ToString() == "shortcut")
                                                {
                                                    R.shortcut = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "attachment")
                                                {
                                                    R.attachment = el.Value;
                                                }

                                                if (el.Name.ToString() == "layer")
                                                {
                                                    R.layer = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "haslayer")
                                                {
                                                    R.haslayer = bool.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "layershiftx")
                                                {
                                                    R.layerShift.x = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "layershifty")
                                                {
                                                    R.layerShift.y = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "x")
                                                {
                                                    R.position.x = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "y")
                                                {
                                                    R.position.y = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "width")
                                                {
                                                    R.width = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "height")
                                                {
                                                    R.height = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "color")
                                                {
                                                    R.color.set(el.Value.ToString());
                                                }

                                                if (el.Name.ToString() == "transparent")
                                                {
                                                    R.transparent = bool.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "embeddedimage")
                                                {
                                                    R.embeddedimage = bool.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "imagedata")
                                                {
                                                    R.image = new Bitmap(new MemoryStream(Convert.FromBase64String(el.Value)));
                                                    R.height = R.image.Height;
                                                    R.width = R.image.Width;
                                                    R.isimage = true;
                                                }

                                                if (el.Name.ToString() == "image")
                                                {
                                                    R.imagepath = el.Value.ToString();
                                                    R.loadImage();
                                                }


                                                if (el.Name.ToString() == "timecreate")
                                                {
                                                    R.timecreate = el.Value;
                                                }


                                                if (el.Name.ToString() == "timemodify")
                                                {
                                                    R.timemodify = el.Value;
                                                }

                                                if (el.Name.ToString() == "protect")
                                                {
                                                    R.protect = bool.Parse(el.Value);
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                Program.log.write("load xml nodes error: " + ex.Message);
                                            }
                                        }
                                        nodes.Add(R);
                                    }
                                }
                            }

                            if (diagram.Name.ToString() == "lines")
                            {
                                foreach (XElement block in diagram.Descendants())
                                {
                                    if (block.Name.ToString() == "line")
                                    {
                                        Line L = new Line();
                                        L.layer = -1; // for identification unset layers

                                        foreach (XElement el in block.Descendants())
                                        {
                                            try
                                            {
                                                if (el.Name.ToString() == "start")
                                                {
                                                    L.start = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "end")
                                                {
                                                    L.end = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "arrow")
                                                {
                                                    L.arrow = el.Value == "1" ? true : false;
                                                }

                                                if (el.Name.ToString() == "color")
                                                {
                                                    L.color.set(el.Value.ToString());
                                                }

                                                if (el.Name.ToString() == "width")
                                                {
                                                    L.width = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "layer")
                                                {
                                                    L.layer = Int32.Parse(el.Value);
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                Program.log.write("load xml lines error: " + ex.Message);
                                            }
                                        }

                                        lines.Add(L);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(main.translations.fileHasWrongFormat);
                Program.log.write("load xml error: " + ex.Message);
                this.CloseFile();
                return false;
            }

            int newWidth = 0;
            int newHeight = 0;

            Nodes nodesReordered = new Nodes(); // order nodes parent first (layer must exist when sub node is created)
            this.nodesReorderNodes(0, null, nodes, nodesReordered);

            foreach (Node rec in nodesReordered) // Loop through List with foreach
            {
                if (!rec.isimage)
                {
                    SizeF s = rec.measure();
                    newWidth = (int)s.Width;
                    newHeight = (int)s.Height;

                    // font change correction > center node
                    if (rec.width != 0 && newWidth != rec.width) {
                        rec.position.x += (rec.width - newWidth) / 2;
                    }

                    if (rec.height != 0 && newHeight != rec.height) {
                       rec.position.y += (rec.height - newHeight) / 2;
                    }

                    rec.resize();

                }

                this.layers.addNode(rec);
            }

            this.layers.buildTree();

            foreach (Line line in lines)
            {
                this.Connect(
                    this.layers.getNode(line.start),
                    this.layers.getNode(line.end),
                    line.arrow,
                    line.color,
                    line.width
                );
            }

            return true;
        }

        // FILE Save - save diagram
        public bool save()
        {
            if (this.FileName != "" && Os.FileExists(this.FileName))
            {
                this.SaveXMLFile(this.FileName);
                this.NewFile = false;
                this.SavedFile = true;
                this.SetTitle();

                return true;
            }

            return false;
        }

        // FILE SAVEAS - save diagram as
        public void saveas(String FileName)
        {
            this.SaveXMLFile(FileName);
            this.FileName = FileName;
            this.SavedFile = true;
            this.NewFile = false;

            this.SetTitle();
        }

        // FILE SAVE save xml file or encrypted file
        public void SaveXMLFile(string FileName)
        {
            string diagraxml = "";

            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(FileName);
                diagraxml = this.SaveInnerXMLFile();
                if (this.password == "")
                {
                    file.Write(diagraxml);
                }
                else
                {
                    XElement root = new XElement("diagram");
                    try
                    {
                        root.Add(new XElement("version", "1"));

                        // encrypted file is saved allways as different string
                        this.salt = Encrypt.CreateSalt(14);

                        root.Add(new XElement("encrypted", Encrypt.EncryptStringAES(diagraxml, this.password, this.salt)));
                        root.Add(new XElement("salt", Encrypt.GetSalt(this.salt)));

                        StringBuilder sb = new StringBuilder();
                        XmlWriterSettings xws = new XmlWriterSettings();
                        xws.OmitXmlDeclaration = true;
                        xws.CheckCharacters = false;
                        xws.Indent = true;

                        using (XmlWriter xw = XmlWriter.Create(sb, xws))
                        {
                            root.WriteTo(xw);
                        }

                        file.Write(sb.ToString());
                    }
                    catch (Exception ex)
                    {
                        Program.log.write("save file error: " + ex.Message);
                    }
                }

                file.Close();

            }
            catch (System.IO.IOException ex)
            {
                Program.log.write("save file io error: " + ex.Message);
                MessageBox.Show(main.translations.fileIsLocked);
                this.CloseFile();
            }
            catch (Exception ex)
            {
                Program.log.write("save file error: " + ex.Message);
            }
        }

        // FILE SAVE XML create xml from current diagram file state
        public string SaveInnerXMLFile()
        {
            bool checkpoint = false;
            XElement root = new XElement("diagram");
            try
            {
                // Options
                XElement option = new XElement("option");
                option.Add(new XElement("shiftx", this.options.homePosition.x));
                option.Add(new XElement("shifty", this.options.homePosition.y));
                option.Add(new XElement("endPositionx", this.options.endPosition.x));
                option.Add(new XElement("endPositiony", this.options.endPosition.y));
                option.Add(new XElement("firstLayereShift.x", this.options.firstLayereShift.x));
                option.Add(new XElement("firstLayereShift.y", this.options.firstLayereShift.y));
                option.Add(new XElement("homelayer", this.options.homeLayer));
                option.Add(new XElement("endlayer", this.options.endLayer));
                option.Add(new XElement("diagramreadonly", this.options.readOnly));
                option.Add(new XElement("grid", this.options.grid));
                option.Add(new XElement("borders", this.options.borders));
                option.Add(Fonts.FontToXml(this.FontDefault, "defaultfont"));
                option.Add(new XElement("coordinates", this.options.coordinates));
                option.Add(new XElement("window.position.restore", this.options.restoreWindow));
                option.Add(new XElement("window.position.x", this.options.Left));
                option.Add(new XElement("window.position.y", this.options.Top));
                option.Add(new XElement("window.position.width", this.options.Width));
                option.Add(new XElement("window.position.height", this.options.Height));
                option.Add(new XElement("window.state", this.options.WindowState));

                // Rectangles
                XElement rectangles = new XElement("rectangles");
                foreach (Node rec in this.getAllNodes())
                {
                    XElement rectangle = new XElement("rectangle");
                    rectangle.Add(new XElement("id", rec.id));
                    if (!Fonts.compare(this.FontDefault, rec.font))
                    {
                        rectangle.Add(Fonts.FontToXml(rec.font));
                    }
                    rectangle.Add(new XElement("fontcolor", rec.fontcolor));
                    if (rec.name != "") rectangle.Add(new XElement("text", rec.name));
                    if (rec.note != "") rectangle.Add(new XElement("note", rec.note));
                    if (rec.link != "") rectangle.Add(new XElement("link", rec.link));
                    if (rec.scriptid != "") rectangle.Add(new XElement("scriptid", rec.scriptid));
                    if (rec.shortcut != 0) rectangle.Add(new XElement("shortcut", rec.shortcut));
                    if (rec.attachment != "") rectangle.Add(new XElement("attachment", rec.attachment));

                    rectangle.Add(new XElement("layer", rec.layer));

                    if (rec.haslayer)
                    {
                        rectangle.Add(new XElement("haslayer", rec.haslayer));
                        rectangle.Add(new XElement("layershiftx", rec.layerShift.x));
                        rectangle.Add(new XElement("layershifty", rec.layerShift.y));
                    }

                    rectangle.Add(new XElement("x", rec.position.x));
                    rectangle.Add(new XElement("y", rec.position.y));
                    rectangle.Add(new XElement("width", rec.width));
                    rectangle.Add(new XElement("height", rec.height));
                    rectangle.Add(new XElement("color", rec.color));
                    if (rec.transparent) rectangle.Add(new XElement("transparent", rec.transparent));
                    if (rec.embeddedimage) rectangle.Add(new XElement("embeddedimage", rec.embeddedimage));

                    if (rec.embeddedimage && rec.image != null) // image is inserted directly to file
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Bitmap));
                        rectangle.Add(new XElement("imagedata", Convert.ToBase64String((byte[])converter.ConvertTo(rec.image, typeof(byte[])))));
                    }
                    else if (rec.imagepath != "")
                    {
                        rectangle.Add(new XElement("image", rec.imagepath));
                    }

                    if (rec.protect) rectangle.Add(new XElement("protect", rec.protect));

                    rectangle.Add(new XElement("timecreate", rec.timecreate));
                    rectangle.Add(new XElement("timemodify", rec.timemodify));

                    rectangles.Add(rectangle);
                }

                // Lines
                XElement lines = new XElement("lines");
                foreach (Line lin in this.getAllLines())
                {
                    XElement line = new XElement("line");
                    line.Add(new XElement("start", lin.start));
                    line.Add(new XElement("end", lin.end));
                    line.Add(new XElement("arrow", (lin.arrow) ? "1" : "0"));
                    line.Add(new XElement("color", lin.color));
                    if (lin.width != 1) line.Add(new XElement("width", lin.width));
                    line.Add(new XElement("layer", lin.layer));
                    lines.Add(line);
                }

                root.Add(option);
                root.Add(rectangles);
                root.Add(lines);

                checkpoint = true;
            }
            catch (Exception ex)
            {
                Program.log.write("save xml error: " + ex.Message);
            }

            if (checkpoint)
            {
                try
                {

                    StringBuilder sb = new StringBuilder();
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.OmitXmlDeclaration = true;
                    xws.CheckCharacters = false;
                    xws.Indent = true;

                    using (XmlWriter xw = XmlWriter.Create(sb, xws))
                    {
                        root.WriteTo(xw);
                    }

                    return sb.ToString();

                }
                catch (Exception ex)
                {
                    Program.log.write("write xml to file error: " + ex.Message);
                }

            }

            return "";
        }

        // FILE UNSAVE Subor sa zmenil treba ho ulozit
        public void unsave(string type, Node node)
        {
            Nodes nodes = new Nodes();
            nodes.Add(node);
            this.unsave(type, nodes);
        }

        public void unsave(string type, Line line)
        {
            Lines lines = new Lines();
            lines.Add(line);
            this.unsave(type, null, lines);
        }

        public void unsave(string type, Node node, Line line)
        {
            Nodes nodes = new Nodes();
            nodes.Add(node);
            Lines lines = new Lines();
            lines.Add(line);
            this.unsave(type, nodes, lines);
        }

        public void unsave(string type, Nodes nodes = null, Lines lines = null)
        {
            this.undo.add(type, nodes, lines);
            this.unsave();
        }

        public void unsave()
        {
            this.SavedFile = false;
            this.SetTitle();

            this.InvalidateDiagram();
        }


        // FILE CLOSE - Vycisti  nastavenie do  východzieho tavu a prekresli obrazovku
        public void CloseFile()
        {
            // Prednadstavenie atributov
            this.maxid = 0;
            this.NewFile = true;
            this.SavedFile = true;
            this.FileName = "";

            // clear nodes and lines lists
            this.layers.clear();

            this.options.readOnly = false;
            this.options.grid = true;
            this.options.coordinates = false;

            this.TextWindows.Clear();
        }

        /*************************************************************************************************************************/

        public Nodes getAllNodes()
        {
            return this.layers.getAllNodes();
        }

        public Lines getAllLines()
        {
            return this.layers.getAllLines();
        }

        // NODE find node by id
        public Node GetNodeByID(int id)
        {
            return this.layers.getNode(id);
        }

        // NODE Najdenie nody podla scriptid
        public Node getNodeByScriptID(string id)
        {
            Regex regex = new Regex(@"^\s*@(\w+){1}\s*$");
            Match match = null;

            foreach (Node rec in this.getAllNodes()) // Loop through List with foreach
            {
                match = regex.Match(rec.link);
                if (match.Success && match.Groups[1].Value == id) return rec;
            }

            return null;
        }

        // NODE delete all nodes which is not in layer history
        public bool canDeleteNode(Node node)
        {
            // sub node is viewed
            foreach (DiagramView view in this.DiagramViews)
            {
                if (view.isNodeInLayerHistory(node))
                {
                    return false;
                }
            }

            return true;
        }

        // NODE delete node
        public void DeleteNode(Node rec)
        {
            if (rec != null && !this.options.readOnly)
            {
                foreach (DiagramView DiagramView in this.DiagramViews) //remove node from selected nodes in views
                {
                    if (DiagramView.selectedNodes.Count() > 0)
                    {
                        for (int i = DiagramView.selectedNodes.Count() - 1; i >= 0; i--)
                        {
                            if (DiagramView.selectedNodes[i] == rec)
                            {
                                DiagramView.selectedNodes.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }

                if (this.TextWindows.Count() > 0) // close text edit to node
                {
                    for (int i = this.TextWindows.Count() - 1; i >= 0; i--)
                    {
                        if (this.TextWindows[i].node == rec)
                        {
                            this.TextWindows[i].Close();
                            break;
                        }
                    }
                }

                this.undo.add("delete", rec);
                this.layers.removeNode(rec);
                this.unsave();
            }
        }

        // NODE delete multiple nodes and set undo operation
        public void DeleteNodes(Nodes nodes)
        {
            bool canRefresh = false;

            Nodes toDeleteNodes = new Nodes();
            Lines toDeleteLines = new Lines();

            this.layers.getAllNodesAndLines(nodes, ref toDeleteNodes, ref toDeleteLines);

            foreach (Node node in nodes)
            {
                if (this.canDeleteNode(node))
                {
                    canRefresh = true;
                }
            }

            if (canRefresh)
            {
                this.undo.add("delete", toDeleteNodes, toDeleteLines);

                foreach (Node node in toDeleteNodes.Reverse<Node>()) // remove lines to node
                {
                    this.DeleteNode(node);
                }

                this.InvalidateDiagram();
                this.unsave();
            }
        }

        // NODE Editovanie vlastnosti nody
        public TextForm EditNode(Node rec)
        {
            bool found = false;
            for (int i = TextWindows.Count() - 1; i >= 0; i--) // Loop through List with foreach
            {
                if (TextWindows[i].node == rec)
                {
                    TextWindows[i].SetFocus();
                    TextWindows[i].Focus();
                    found = true;
                    return TextWindows[i];
                }
            }

            if (!found) {
                TextForm textf = new TextForm(main);
                textf.setDiagram(this);
                textf.node = rec;
                string[] lines = rec.name.Split(Environment.NewLine.ToCharArray()).ToArray();
                if(lines.Count()>0)
                    textf.Text = lines[0];

                this.TextWindows.Add(textf);
                main.TextWindows.Add(textf);
                textf.Show();
                textf.SetFocus();
                return textf;
            }
            return null;
        }

        // NODE Editovanie vlastnosti nody
        public void EditNodeClose(Node rec)
        {
            for (int i = TextWindows.Count() - 1; i >= 0; i--) // Loop through List with foreach
            {
                if (TextWindows[i].node == rec)
                {
                    TextWindows.RemoveAt(i);
                }
            }
        }

        // NODE Create Rectangle on point
        public Node createNode(
            Position position,
            string name = "",
            int layer = 0,
            ColorType color = null,
            Font font = null
        ) {
            if (!this.options.readOnly)
            {
                var rec = new Node();
                if (font == null)
                {
                    rec.font = this.FontDefault;
                }
                else
                {
                    rec.font = font;
                }

                rec.id = ++maxid;
                rec.layer = layer;

                rec.setName(name);
                rec.note = "";
                rec.link = "";

                rec.position.set(position);

                if (color != null)
                {
                    rec.color.set(color);
                }
                else
                {
                    rec.color.set(Media.getColor(this.options.colorNode));
                }

                DateTime dt = DateTime.Now;
                rec.timecreate = String.Format("{0:yyyy-M-d HH:mm:ss}", dt);
                rec.timemodify = rec.timecreate;

                this.layers.addNode(rec);

                return rec;
            }

            return null;
        }

        // NODE Create Rectangle on point
        public Node createNode(
            Node node
        )
        {
            if (!this.options.readOnly)
            {
                node.id = ++maxid;

                DateTime dt = DateTime.Now;
                node.timecreate = String.Format("{0:yyyy-M-d HH:mm:ss}", dt);
                node.timemodify = node.timecreate;

                Layer layer = this.layers.addNode(node);

                if (layer != null)
                {
                    return node;
                }
            }

            return null;
        }

        // NODE HASLINE check if line exist between two nodes
        public bool hasConnection(Node a, Node b)
        {
            Line line = this.layers.getLine(a, b);
            return line != null;
        }

        public Line getLine(Node a, Node b)
        {
            return this.layers.getLine(a, b);
        }

        // NODE CONNECT connect two nodes
        public Line Connect(Node a, Node b)
        {
            if (!this.options.readOnly && a != null && b != null)
            {
                Line line = this.layers.getLine(a, b);

                if (line == null)
                {

                    // calculate line layer from node layers
                    int layer = 0;
                    if (a.layer == b.layer) // nodes are in same layer
                    {
                        layer = a.layer;
                    }
                    else
                    if (a.layer == b.id) // b is perent of a
                    {
                        layer = a.layer;
                    }
                    else
                    if (b.layer == a.id) // a is perent of b
                    {
                        layer = b.layer;
                    }
                    else
                    {
                        return null; // invalid connection (nodes are not related or in same layer)
                    }

                    line = new Line();
                    line.start = a.id;
                    line.end = b.id;
                    line.startNode = this.GetNodeByID(line.start);
                    line.endNode = this.GetNodeByID(line.end);
                    line.layer = layer;
                    this.layers.addLine(line);

                    return line;
                }
            }

            return null;
        }

        // NODE DISCONNECT remove connection between two nodes
        public void Disconnect(Node a, Node b)
        {
            if (!this.options.readOnly && a != null && b != null)
            {
                Line line = this.layers.getLine(a, b);

                if (line != null)
                {
                    this.layers.removeLine(line);
                }
            }
        }

        // NODE CONNECT connect two nodes and add arrow or set color
        public Line Connect(Node a, Node b, bool arrow = false, ColorType color = null, int width = 1)
        {
            Line line = this.Connect(a, b);

            if (line != null)
            {
                line.arrow = arrow;
                if (color != null)
                {
                    line.color.set(color);
                }
                
                line.width = width;
            }

            return line;
        }

        // NODES ALIGN to column
        public void AlignToColumn(Nodes Nodes)
        {
            if (Nodes.Count() > 0)
            {
                int miny = Nodes[0].position.y;
                int topx = Nodes[0].position.x;
                foreach (Node rec in Nodes)
                {
                    if (rec.position.y <= miny)
                    {
                        miny = rec.position.y;
                        topx = rec.position.x + rec.width / 2; ;
                    }
                }

                foreach (Node rec in Nodes)
                {
                    rec.position.x = topx - rec.width / 2;
                }
            }
        }

        // NODES ALIGN to line
        public void AlignToLine(Nodes Nodes)
        {
            if (Nodes.Count() > 0)
            {
                int minx = Nodes[0].position.x;
                int topy = Nodes[0].position.y;
                foreach (Node rec in Nodes)
                {
                    if (rec.position.x <= minx)
                    {
                        minx = rec.position.x;
                        topy = rec.position.y + rec.height / 2;
                    }
                }

                foreach (Node rec in Nodes)
                {
                    rec.position.y = topy - rec.height / 2;
                }
            }
        }

        // NODES ALIGN compact
        // align node to left and create constant space between nodes
        public void AlignCompact(Nodes nodes)
        {
            if (nodes.Count() > 0)
            {
                int minx = nodes[0].position.x;
                int miny = nodes[0].position.y;
                foreach (Node rec in nodes)
                {
                    if (rec.position.x <= minx) // find top left element
                    {
                        minx = rec.position.x;
                    }

                    if (rec.position.y <= miny) // find most top element
                    {
                        miny = rec.position.y;
                    }
                }

                foreach (Node rec in nodes) // align to left
                {
                    rec.position.x = minx;
                }

                // sort elements by y coordinate
                nodes.OrderByPositionY();

                int posy = miny;
                foreach (Node rec in nodes) // zmensit medzeru medzi objektami
                {
                    rec.position.y = posy;
                    posy = posy + rec.height + 10;
                }
            }
        }

        // NODES ALIGN compact
        // align node to left and create constant space between nodes
        public void AlignCompactLine(Nodes nodes)
        {
            if (nodes.Count() > 0)
            {
                int minx = nodes[0].position.x;
                int miny = nodes[0].position.y;
                foreach (Node rec in nodes)
                {
                    if (rec.position.x <= minx) // find top left element
                    {
                        minx = rec.position.x;
                    }

                    if (rec.position.y <= miny) // find most top element
                    {
                        miny = rec.position.y;
                    }
                }

                foreach (Node rec in nodes) // align to top
                {
                    rec.position.y = miny;
                }

                // sort elements by y coordinate
                nodes.OrderByPositionX();

                int posx = minx;
                foreach (Node rec in nodes) // zmensit medzeru medzi objektami
                {
                    rec.position.x = posx;
                    posx = posx + rec.width + 10;
                }
            }
        }

        // NODES ALIGN left
        public void AlignRight(Nodes Nodes)
        {
            if (Nodes.Count() > 0)
            {
                int maxx = Nodes[0].position.x + Nodes[0].width;
                foreach (Node rec in Nodes)
                {
                    if (rec.position.x + rec.width >= maxx)
                    {
                        maxx = rec.position.x + rec.width;
                    }
                }

                foreach (Node rec in Nodes)
                {
                    rec.position.x = maxx - rec.width;
                }
                this.unsave();
                this.InvalidateDiagram();
            }
        }

        // NODES ALIGN left
        public void AlignLeft(Nodes Nodes)
        {
            if (Nodes.Count() > 0)
            {
                int minx = Nodes[0].position.x;
                foreach (Node rec in Nodes)
                {
                    if (rec.position.x <= minx)
                    {
                        minx = rec.position.x;
                    }
                }

                foreach (Node rec in Nodes)
                {
                    rec.position.x = minx;
                }
            }
        }

        // NODES remove shortcut
        public void RemoveShortcut(Node node)
        {
            if (node.shortcut > 0) node.shortcut = 0;
        }

        // NODE Reset font to default font for all nodes
        public void ResetFont()
        {
            foreach (Node rec in this.getAllNodes()) // Loop through List with foreach
            {
                rec.font = this.FontDefault;
                rec.resize();
            }

            this.unsave();
            this.InvalidateDiagram();
        }

        // NODE Reset font to default font for group of nodes
        public void ResetFont(Nodes Nodes)
        {
            if (Nodes.Count>0) {
                foreach (Node rec in Nodes) // Loop through List with foreach
                {
                    rec.font = this.FontDefault;
                    rec.resize();
                }
                this.unsave();
                this.InvalidateDiagram();
            }
        }

        // NODE Najdenie nody podla pozicie myši
        public Node findNodeInPosition(Position position, int layer)
        {
            foreach (Node node in this.layers.getLayer(layer).nodes) // Loop through List with foreach
            {
                if (layer == node.layer || layer == node.id)
                {
                    if
                    (
                        node.position.x <= position.x && position.x <= node.position.x + node.width &&
                        node.position.y <= position.y && position.y <= node.position.y + node.height
                    )
                    {
                        return node;
                    }
                }
            }

            return null;
        }

        // NODE set image
        public void setImage(Node rec, string file)
        {
            rec.isimage = true;
            rec.imagepath = Os.makeRelative(file, this.FileName);
            rec.image = new Bitmap(rec.imagepath);
            string ext = Os.getExtension(file);
            if (ext != ".ico") rec.image.MakeTransparent(Color.White);
            rec.height = rec.image.Height;
            rec.width = rec.image.Width;
        }

        // NODE remove image
        public void removeImage(Node rec)
        {
            rec.isimage = false;
            rec.imagepath = "";
            rec.image = null;
            rec.embeddedimage = false;
            rec.resize();
        }

        // NODE set image embedded
        public void setImageEmbedded(Node rec)
        {
            if (rec.isimage)
            {
                rec.embeddedimage = true;
            }
        }

        /*************************************************************************************************************************/

        // LAYER MOVE posunie rekurzivne layer a jeho nody OBSOLATE
        public void MoveLayer(Node rec, Position vector)
        {
            if (rec != null)
            {
                Nodes nodes = this.layers.getLayer(rec.id).nodes;
                foreach (Node node in nodes) // Loop through List with foreach
                {
                    if (node.layer == rec.id)
                    {
                        node.position.add(vector);

                        if (node.haslayer)
                        {
                            MoveLayer(node, vector);
                        }
                    }
                }
            }
        }

        /*************************************************************************************************************************/

        // DIAGRAM VIEW open new view on diagram
        public DiagramView openDiagramView(DiagramView parent = null)
        {
            DiagramView diagramview = new DiagramView(main, this, parent);
            diagramview.setDiagram(this);
            this.DiagramViews.Add(diagramview);
            main.DiagramViews.Add(diagramview);
			this.SetTitle();
            diagramview.Show();
            return diagramview;
        }

        // DIAGRAM VIEW invalidate all opened views
        public void InvalidateDiagram()
        {
            foreach (DiagramView DiagramView in this.DiagramViews)
            {
                if (DiagramView.Visible == true)
                {
                    DiagramView.Invalidate();
                }
            }
        }

        // DIAGRAM close diagram
        public void CloseView(DiagramView view)
        {
            this.DiagramViews.Remove(view);
            main.DiagramViews.Remove(view);

            foreach (DiagramView diagramView in this.DiagramViews) {
                if (diagramView.parentView == view) {
                    diagramView.parentView = null;
                }
            }

            this.CloseDiagram();
        }

        // DIAGRAM close diagram
        public void CloseDiagram()
        {
            bool canclose = true;

            if (this.DiagramViews.Count > 0 || TextWindows.Count > 0)
            {
                canclose = false;
            }

            if (canclose)
            {
                this.CloseFile();
                main.Diagrams.Remove(this);
                main.CloseEmptyApplication();
            }
        }

        // DIAGRAM VIEWS change title
        public void SetTitle()
        {
            foreach (DiagramView DiagramView in this.DiagramViews)
            {
                DiagramView.SetTitle();
            }
        }

        // DIAGRAM undo
        public void doUndo()
        {
            if (this.undo.doUndo())
            {
                this.unsave();
            }
        }

        // DIAGRAM redo
        public void doRedo()
        {
            if (this.undo.doRedo())
            {
                this.unsave();
            }
        }

        // DIAGRAM refresh - refresh items depends on external resources like images
        public void refresh()
        {
            foreach (Node node in this.layers.getAllNodes())
            {
                
                if (node.isimage && !node.embeddedimage)
                {
                    node.loadImage();
                }
                else
                {
                    node.resize();
                }
            }

            this.InvalidateDiagram();
        }

        /*************************************************************************************************************************/

        // CLIPBOARD PASTE paste part of diagram from clipboard                                   // CLIPBOARD
        public Nodes AddDiagramPart(string DiagramXml, Position position, int layer)
        {
            Nodes NewNodes = new Nodes();
            Lines NewLines = new Lines();

            XmlReaderSettings xws = new XmlReaderSettings();
            xws.CheckCharacters = false;

            string xml = DiagramXml;

            try
            {
                using (XmlReader xr = XmlReader.Create(new StringReader(xml), xws))
                {

                    XElement root = XElement.Load(xr);
                    foreach (XElement diagram in root.Elements())
                    {
                        if (diagram.HasElements)
                        {

                            if (diagram.Name.ToString() == "rectangles")
                            {
                                foreach (XElement block in diagram.Descendants())
                                {

                                    if (block.Name.ToString() == "rectangle")
                                    {
                                        Node R = new Node();
                                        R.font = this.FontDefault;

                                        foreach (XElement el in block.Descendants())
                                        {
                                            try
                                            {
                                                if (el.Name.ToString() == "id")
                                                {
                                                    R.id = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "text")
                                                {
                                                    R.name = el.Value;
                                                }


                                                if (el.Name.ToString() == "note")
                                                {
                                                    R.note = el.Value;
                                                }

                                                if (el.Name.ToString() == "x")
                                                {
                                                    R.position.x = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "y")
                                                {
                                                    R.position.y = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "color")
                                                {
                                                    R.color.set(el.Value.ToString());
                                                }


                                                if (el.Name.ToString() == "timecreate")
                                                {
                                                    R.timecreate = el.Value;
                                                }


                                                if (el.Name.ToString() == "timemodify")
                                                {
                                                    R.timemodify = el.Value;
                                                }


                                                if (el.Name.ToString() == "font")
                                                {
                                                    R.font = Fonts.XmlToFont(el);
                                                }


                                                if (el.Name.ToString() == "fontcolor")
                                                {
                                                    R.fontcolor.set(el.Value.ToString());
                                                }

                                                if (el.Name.ToString() == "link")
                                                {
                                                    R.link = el.Value;
                                                }

                                                if (el.Name.ToString() == "shortcut")
                                                {
                                                    R.shortcut = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "transparent")
                                                {
                                                    R.transparent = bool.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "embeddedimage")
                                                {
                                                    R.embeddedimage = bool.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "imagedata")
                                                {
                                                    R.image = new Bitmap(new MemoryStream(Convert.FromBase64String(el.Value)));
                                                    R.height = R.image.Height;
                                                    R.width = R.image.Width;
                                                    R.isimage = true;
                                                }

                                                if (el.Name.ToString() == "image")
                                                {
                                                    R.imagepath = el.Value.ToString();
                                                    if (Os.FileExists(R.imagepath))
                                                    {
                                                        try
                                                        {
                                                            string ext = "";
                                                            ext = Os.getExtension(R.imagepath).ToLower();

                                                            if (ext == ".jpg" || ext == ".png" || ext == ".ico" || ext == ".bmp") // skratenie cesty k suboru
                                                            {
                                                                R.image = new Bitmap(R.imagepath);
                                                                if (ext != ".ico") R.image.MakeTransparent(Color.White);
                                                                R.height = R.image.Height;
                                                                R.width = R.image.Width;
                                                                R.isimage = true;
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Program.log.write("load image from xml error: " + ex.Message);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        R.imagepath = "";
                                                    }
                                                }

                                                if (el.Name.ToString() == "timecreate")
                                                {
                                                    R.timecreate = el.Value;
                                                }


                                                if (el.Name.ToString() == "timemodify")
                                                {
                                                    R.timemodify = el.Value;
                                                }

                                                if (el.Name.ToString() == "attachment")
                                                {
                                                    R.attachment = el.Value;
                                                }

                                                if (el.Name.ToString() == "layer")
                                                {
                                                    R.layer = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "protect")
                                                {
                                                    R.protect = bool.Parse(el.Value);
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                Program.log.write(main.translations.dataHasWrongStructure + ": error: " + ex.Message);
                                            }
                                        }

                                        NewNodes.Add(R);
                                    }
                                }
                            }

                            if (diagram.Name.ToString() == "lines")
                            {
                                foreach (XElement block in diagram.Descendants())
                                {
                                    if (block.Name.ToString() == "line")
                                    {
                                        Line L = new Line();
                                        foreach (XElement el in block.Descendants())
                                        {
                                            try
                                            {
                                                if (el.Name.ToString() == "start")
                                                {
                                                    L.start = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "end")
                                                {
                                                    L.end = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "arrow")
                                                {
                                                    L.arrow = el.Value == "1" ? true : false;
                                                }

                                                if (el.Name.ToString() == "color")
                                                {
                                                    L.color.set(el.Value.ToString());
                                                }

                                                if (el.Name.ToString() == "width")
                                                {
                                                    L.width = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "layer")
                                                {
                                                    L.layer = Int32.Parse(el.Value);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Program.log.write(main.translations.dataHasWrongStructure + ": error: " + ex.Message);
                                            }
                                        }
                                        NewLines.Add(L);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.log.write(main.translations.dataHasWrongStructure + ": error: " + ex.Message);
            }


            List<MappedNode> maps = new List<MappedNode>();

            Nodes NewReorderedNodes = new Nodes(); // order nodes parent first (layer must exist when sub node is created)
            this.nodesReorderNodes(0, null, NewNodes, NewReorderedNodes);

            int layerParent = 0;

            MappedNode mappedNode;
            Nodes createdNodes = new Nodes();
            Node newNode = null;
            int oldId = 0;
            foreach (Node rec in NewReorderedNodes)
            {
                layerParent = 0;
                if (rec.layer == 0)
                {
                    layerParent = layer;
                }
                else
                {
                    foreach (MappedNode mapednode in maps)
                    {
                        if (rec.layer == mapednode.oldId)
                        {
                            layerParent = mapednode.newNode.id;
                            break;
                        }
                    }
                }

                rec.layer = layerParent;
                rec.position.add(position);
                rec.resize();

                oldId = rec.id;
                newNode = this.createNode(rec); ;

                if (newNode != null) {
                    mappedNode = new MappedNode();
                    mappedNode.oldId = oldId;
                    mappedNode.newNode = newNode;
                    createdNodes.Add(newNode);
                    maps.Add(mappedNode);
                }
            }

            // fix layers and shortcuts
            foreach (Node rec in NewNodes)
            {
                if (rec.shortcut != 0)
                {
                    foreach (MappedNode mapednode in maps)
                    {
                        if (rec.shortcut == mapednode.oldId)
                        {
                            rec.shortcut = mapednode.newNode.id;
                            break;
                        }
                    }
                }
            }

            Lines createdLines = new Lines();
            Line newLine = null;
            foreach (Line line in NewLines)
            {
                foreach (MappedNode mapbegin in maps)
                {
                    if (line.start == mapbegin.oldId)
                    {
                        foreach (MappedNode mapend in maps)
                        {
                            if (line.end == mapend.oldId)
                            {
                                newLine = this.Connect(
                                    mapbegin.newNode,
                                    mapend.newNode,
                                    line.arrow,
                                    line.color,
                                    line.width
                                );

                                if (newLine != null)
                                {
                                    createdLines.Add(newLine);
                                }
                            }
                        }
                    }
                }
            }

            this.unsave("create", createdNodes, createdLines);

            return NewNodes;
        }

        // CLIPBOARD Get all layers nodes
        private void nodesReorderNodes(int layer, Node parent, Nodes nodesIn, Nodes nodesOut)
        {
            foreach (Node node in nodesIn)
            {
                if (node.layer == layer)
                {
                    if (parent != null) {
                        parent.haslayer = true;
                    }

                    nodesOut.Add(node);

                    nodesReorderNodes(node.id, node, nodesIn, nodesOut);
                }
            }
        }

        // CLIPBOARD Get all layers nodes
        public void getLayerNodes(Node node, Nodes nodes)
        {
            if (node.haslayer) {
                foreach(Node subnode in this.layers.getLayer(node.id).nodes) {
                    nodes.Add(subnode);

                    if (subnode.haslayer) {
                        getLayerNodes(subnode, nodes);
                    }
                }
            }
        }

        // CLIPBOARD COPY copy part of diagram to text xml string
        public string GetDiagramPart(Nodes nodes)
        {
            string copyxml = "";

            if (nodes.Count() > 0)
            {
                XElement root = new XElement("diagram");
                XElement rectangles = new XElement("rectangles");
                XElement lines = new XElement("lines");

                int minx = nodes[0].position.x;
                int miny = nodes[0].position.y;
                int minid = nodes[0].id;

                Nodes subnodes = new Nodes();

                foreach (Node node in nodes)
                {
                    getLayerNodes(node, subnodes);
                }

                foreach (Node node in subnodes)
                {
                    nodes.Add(node);
                }

                foreach (Node node in nodes)
                {
                    if (node.position.x < minx) minx = node.position.x;
                    if (node.position.y < miny) miny = node.position.y;
                    if (node.id < minid) minid = node.id;
                }

                foreach (Node rec in nodes)
                {
                    XElement rectangle = new XElement("rectangle");
                    rectangle.Add(new XElement("id", rec.id - minid + 1));
                    rectangle.Add(new XElement("x", rec.position.x - minx));
                    rectangle.Add(new XElement("y", rec.position.y - miny));
                    rectangle.Add(new XElement("text", rec.name));
                    rectangle.Add(new XElement("note", rec.note));
                    rectangle.Add(new XElement("color", rec.color));
                    rectangle.Add(Fonts.FontToXml(rec.font));
                    rectangle.Add(new XElement("fontcolor", rec.fontcolor));
                    if (rec.link != "") rectangle.Add(new XElement("link", rec.link));
                    if (rec.shortcut != 0 && rec.shortcut - minid + 1 > 0) rectangle.Add(new XElement("shortcut", rec.shortcut + 1));
                    rectangle.Add(new XElement("transparent", rec.transparent));

                    if (rec.embeddedimage) rectangle.Add(new XElement("embeddedimage", rec.embeddedimage));

                    if (rec.embeddedimage && rec.image != null)
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Bitmap));
                        rectangle.Add(new XElement("imagedata", Convert.ToBase64String((byte[])converter.ConvertTo(rec.image, typeof(byte[])))));
                    }
                    else if (rec.imagepath != "")
                    {
                        rectangle.Add(new XElement("image", rec.imagepath));
                    }

                    rectangle.Add(new XElement("timecreate", rec.timecreate));
                    rectangle.Add(new XElement("timemodify", rec.timemodify));
                    rectangle.Add(new XElement("attachment", rec.attachment));
                    if (rec.layer != 0 && rec.layer - minid + 1 > 0)  rectangle.Add(new XElement("layer", rec.layer - minid + 1));
                    if (rec.protect) rectangle.Add(new XElement("protect", rec.protect));

                    rectangles.Add(rectangle);
                }

                foreach (Line li in this.getAllLines())
                {
                    foreach (Node recstart in nodes)
                    {
                        if (li.start == recstart.id)
                        {
                            foreach (Node recend in nodes)
                            {
                                if (li.end == recend.id)
                                {
                                    XElement line = new XElement("line");
                                    line.Add(new XElement("start", li.start - minid + 1));
                                    line.Add(new XElement("end", li.end - minid + 1));
                                    line.Add(new XElement("arrow", (li.arrow) ? "1" : "0"));
                                    line.Add(new XElement("color", li.color));
                                    if (li.width != 1) line.Add(new XElement("width", li.width));
                                    if (li.layer - minid +1 > 0) {
                                        line.Add(new XElement("layer", li.layer - minid + 1));
                                    }
                                    lines.Add(line);
                                }
                            }
                        }
                    }
                }

                root.Add(rectangles);
                root.Add(lines);
                copyxml = root.ToString();
            }

            return copyxml;
        }
    }
}
