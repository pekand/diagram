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
using System.Security;

/*
 
*/

namespace Diagram
{
    // map node structure for copy paste operation
    public struct MappedNode
    {
        public int oldId;
        public Node newNode;
    }

    public class Diagram //UID2487098516
    {
        private Main main = null;                 // reference to main form

        /*************************************************************************************************************************/
        // COOLECTIONS

        public Layers layers = new Layers();
        public List<DiagramView> DiagramViews = new List<DiagramView>(); // all views forms to diagram
        public List<TextForm> TextWindows = new List<TextForm>();   // opened text textforms for this diagram

        /*************************************************************************************************************************/
        // ATTRIBUTES OPTIONS

        public Options options = new Options();  // diagram options saved to xml file        

        /*************************************************************************************************************************/
        // FILE

        public bool NewFile = true;              // flag for new unsaved file without name
        public bool SavedFile = true;            // flag for saved diagram with name
        public string FileName = "";             // path to diagram file        

        /*************************************************************************************************************************/
        // ATTRIBUTES ENCRYPTION

        private bool encrypted = false;           // flag for encrypted file
        private bool locked = false;              // flag for encrypted file
        private SecureString password = null;     // password for encrypted file
        private string passwordHash = null;
        private byte[] salt = null;               // salt

        /*************************************************************************************************************************/
        // UNDO

        public UndoOperations undoOperations = null;  // undo operations repository        

        /*************************************************************************************************************************/
        // RESOURCES

        public Font FontDefault = null; // default font

        /*************************************************************************************************************************/
        // CONSTRUCTORS

        public Diagram(Main main)
        {
            this.main = main;
            this.FontDefault = new Font("Open Sans", 10);
            this.undoOperations = new UndoOperations(this);
        }

        /*************************************************************************************************************************/
        // FILE OPERATIONS

        // open file. If file is invalid return false
        public bool OpenFile(string FileName)
        {
            if (Os.FileExists(FileName))
            {
                Os.SetCurrentDirectory(Os.GetFileDirectory(FileName));

                this.CloseFile();
                this.FileName = FileName;
                this.NewFile = false;
                this.SavedFile = true;

                string xml = Os.GetFileContent(FileName);

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
        
        // save diagram
        public bool Save()
        {
            if (this.FileName != "" && Os.FileExists(this.FileName))
            {
                this.SaveXMLFile(this.FileName);
                this.NewFile = false;
                this.SavedFile = true;
                this.undoOperations.rememberSave();
                this.SetTitle();

                return true;
            }

            return false;
        }

        // save diagram as
        public void Saveas(String FileName)
        {
            this.SaveXMLFile(FileName);
            this.FileName = FileName;
            this.SavedFile = true;
            this.NewFile = false;

            this.SetTitle();
        }

        // set default options for file like new file 
        public void CloseFile()
        {
            // Prednadstavenie atributov
            this.NewFile = true;
            this.SavedFile = true;
            this.FileName = "";

            // clear nodes and lines lists
            this.layers.Clear();

            this.options.readOnly = false;
            this.options.grid = true;
            this.options.coordinates = false;

            this.TextWindows.Clear();
        }

        /*************************************************************************************************************************/
        // XML OPERATIONS

        // XML SAVE file or encrypted file
        public void SaveXMLFile(string FileName)
        {
            string diagraxml = this.SaveInnerXMLFile();

            // encrypt data if password is set
            try
            {
                if (this.password != null)
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
                        XmlWriterSettings xws = new XmlWriterSettings
                        {
                            OmitXmlDeclaration = true,
                            CheckCharacters = false,
                            Indent = true
                        };

                        using (XmlWriter xw = XmlWriter.Create(sb, xws))
                        {
                            root.WriteTo(xw);
                        }

                        diagraxml = sb.ToString();
                    }
                    catch (Exception ex)
                    {
                        Program.log.Write("save file error: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.log.Write("save file error: " + ex.Message);
            }

            // save data to file
            try
            {
                if (diagraxml != "") { // prevent acidentaly loos data
                    System.IO.StreamWriter file = new System.IO.StreamWriter(FileName);
                    file.Write(diagraxml);
                    file.Close();
                }

            }
            catch (System.IO.IOException ex)
            {
                Program.log.Write("save file io error: " + ex.Message);
                MessageBox.Show(Translations.fileIsLocked);
                this.CloseFile();
            }
            catch (Exception ex)
            {
                Program.log.Write("save file error: " + ex.Message);
            }
        }

        // XML SAVE create xml from current diagram file state
        public string SaveInnerXMLFile()
        {
            bool checkpoint = false;
            XElement root = new XElement("diagram");
            try
            {
                // [options] [config]
                XElement option = new XElement("option");
                option.Add(new XElement("shiftx", this.options.homePosition.x));
                option.Add(new XElement("shifty", this.options.homePosition.y));
                option.Add(new XElement("endPositionx", this.options.endPosition.x));
                option.Add(new XElement("endPositiony", this.options.endPosition.y));
                option.Add(new XElement("firstLayereShift.x", this.options.firstLayereShift.x));
                option.Add(new XElement("firstLayereShift.y", this.options.firstLayereShift.y));
                if (this.options.openLayerInNewView) option.Add(new XElement("openLayerInNewView", this.options.openLayerInNewView));
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

                if (this.options.icon != "")
                {
                    option.Add(new XElement("icon", this.options.icon));
                }

                if (this.options.backgroundImage != null)
                {
                    option.Add(new XElement(
                        "backgroundImage", 
                        Media.ImageToString(this.options.backgroundImage)
                        )
                    );
                }

                // Rectangles
                XElement rectangles = new XElement("rectangles");
                foreach (Node rec in this.GetAllNodes())
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
                    if (rec.mark) rectangle.Add(new XElement("mark", rec.mark));
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
                        rectangle.Add(new XElement("imagedata", Media.BitmapToString(rec.image)));
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
                foreach (Line lin in this.GetAllLines())
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
                Program.log.Write("save xml error: " + ex.Message);
            }

            if (checkpoint)
            {
                try
                {

                    StringBuilder sb = new StringBuilder();
                    XmlWriterSettings xws = new XmlWriterSettings { 
                        OmitXmlDeclaration = true,
                        CheckCharacters = false,
                        Indent = true
                    };

                    using (XmlWriter xw = XmlWriter.Create(sb, xws))
                    {
                        root.WriteTo(xw);
                    }

                    return sb.ToString();

                }
                catch (Exception ex)
                {
                    Program.log.Write("write xml to file error: " + ex.Message);
                }

            }

            return "";
        }

        // XML LOAD If file is invalid return false
        public bool LoadXML(string xml)
        {

            XmlReaderSettings xws = new XmlReaderSettings
            {
                CheckCharacters = false
            };

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
                MessageBox.Show(Translations.fileHasWrongFormat);
                Program.log.Write("load xml error: " + ex.Message);
                this.CloseFile();
            }

            if (version == "1" && salt != "" && encrypted != "")
            {
                bool error = false;
                do
                {
                    error = false;

                    string password = this.main.GetPassword(Os.GetFileNameWithoutExtension(this.FileName));
                    if (password != null)
                    {
                        try
                        {
                            this.salt = Encrypt.SetSalt(salt);
                            this.LoadInnerXML(Encrypt.DecryptStringAES(encrypted, password, this.salt));
                            this.encrypted = true;
                            this.SetPassword(password);
                        }
                        catch (Exception e)
                        {
                            // probably invalid password
                            Program.log.Write("LoadXML: Password or file is invalid: " + e.Message);
                            error = true;
                        }
                    }
                    else
                    {
                        // password dialog is cancled
                        this.CloseFile();
                        return false;
                    }

                } while (error);

                return true;
            }
            else
            {
                return LoadInnerXML(xml);
            }
        }

        // XML LOAD inner part of diagram file. If file is invalid return false
        public bool LoadInnerXML(string xml)
        {
            string FontDefaultString = TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(this.FontDefault);

            XmlReaderSettings xws = new XmlReaderSettings {
                CheckCharacters = false
            };

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

                            if (diagram.Name.ToString() == "option") // [options] [config]
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

                                        if (el.Name.ToString() == "openLayerInNewView")
                                        {
                                            this.options.openLayerInNewView = bool.Parse(el.Value);
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

                                        if (el.Name.ToString() == "icon")
                                        {
                                            this.options.icon = el.Value;
                                        }

                                        if (el.Name.ToString() == "backgroundImage")
                                        {
                                            this.options.backgroundImage = Media.StringToImage(el.Value);
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Program.log.Write("load xml diagram options: " + ex.Message);
                                    }
                                }
                            }

                            if (diagram.Name.ToString() == "rectangles")
                            {
                                foreach (XElement block in diagram.Descendants())
                                {

                                    if (block.Name.ToString() == "rectangle")
                                    {
                                        Node R = new Node
                                        {
                                            font = this.FontDefault
                                        };

                                        foreach (XElement el in block.Descendants())
                                        {
                                            try
                                            {
                                                if (el.Name.ToString() == "id")
                                                {
                                                    R.id = Int32.Parse(el.Value);
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
                                                    R.fontcolor.Set(el.Value.ToString());
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

                                                if (el.Name.ToString() == "mark")
                                                {
                                                    R.mark = bool.Parse(el.Value);
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
                                                    R.color.Set(el.Value.ToString());
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
                                                    R.image = Media.StringToBitmap(el.Value);
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
                                                Program.log.Write("load xml nodes error: " + ex.Message);
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
                                        Line L = new Line {
                                            layer = -1 // for identification unset layers
                                        };

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
                                                    L.color.Set(el.Value.ToString());
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
                                                Program.log.Write("load xml lines error: " + ex.Message);
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
                MessageBox.Show(Translations.fileHasWrongFormat);
                Program.log.Write("load xml error: " + ex.Message);
                this.CloseFile();
                return false;
            }

            int newWidth = 0;
            int newHeight = 0;

            Nodes nodesReordered = new Nodes(); // order nodes parent first (layer must exist when sub node is created)
            this.NodesReorderNodes(0, null, nodes, nodesReordered);

            foreach (Node rec in nodesReordered) // Loop through List with foreach
            {
                if (!rec.isimage)
                {
                    SizeF s = rec.measure();
                    newWidth = (int)s.Width;
                    newHeight = (int)s.Height;

                    // font change correction > center node
                    if (rec.width != 0 && newWidth != rec.width)
                    {
                        rec.position.x += (rec.width - newWidth) / 2;
                    }

                    if (rec.height != 0 && newHeight != rec.height)
                    {
                        rec.position.y += (rec.height - newHeight) / 2;
                    }

                    rec.resize();

                }

                this.layers.AddNode(rec);
            }

            this.layers.SetLayersParentsReferences();

            foreach (Line line in lines)
            {
                this.Connect(
                    this.layers.GetNode(line.start),
                    this.layers.GetNode(line.end),
                    line.arrow,
                    line.color,
                    line.width
                );
            }

            return true;
        }

        /*************************************************************************************************************************/
        // STATES

        // check if file is empty
        public bool IsNew()
        {
            return (this.FileName == "" && this.NewFile && this.SavedFile);
        }

        // check if file is empty
        public bool IsReadOnly()
        {
            return this.options.readOnly;
        }

        /*************************************************************************************************************************/
        // UNSAVE

        public void Unsave(string type, Node node, Position position = null, int layer = 0)
        {
            Nodes nodes = new Nodes();
            nodes.Add(node);
            this.Unsave(type, nodes, null, position, layer);
        }

        public void Unsave(string type, Line line, Position position = null, int layer = 0)
        {
            Lines lines = new Lines();
            lines.Add(line);
            this.Unsave(type, null, lines, position, layer);
        }

        public void Unsave(string type, Node node, Line line, Position position = null, int layer = 0)
        {
            Nodes nodes = new Nodes();
            nodes.Add(node);
            Lines lines = new Lines();
            lines.Add(line);
            this.Unsave(type, nodes, lines, position, layer);
        }

        public void Unsave(string type, Nodes nodes = null, Lines lines = null, Position position = null, int layer = 0)
        {
            this.undoOperations.rememberSave();
            this.undoOperations.add(type, nodes, lines, position, layer);
            this.Unsave();
        }

        public void Unsave()
        {
            this.SavedFile = false;
            this.SetTitle();

            this.InvalidateDiagram();
        }

        public void Restoresave()
        {
            this.SavedFile = true;
            this.SetTitle();

            this.InvalidateDiagram();
        }

        /*************************************************************************************************************************/
        // UNDO

        // undo
        public void DoUndo(DiagramView view = null)
        {
            this.undoOperations.doUndo(view);
        }

        // redo
        public void DoRedo(DiagramView view = null)
        {
            this.undoOperations.doRedo(view);
        }

        /*************************************************************************************************************************/
        // NODES SELECT

        // NODE find node by id
        public Node GetNodeByID(int id)
        {
            return this.layers.GetNode(id);
        }

        // NODE find node by link
        public Node GetNodeByScriptID(string id)
        {
            Regex regex = new Regex(@"^\s*@(\w+){1}\s*$");
            Match match = null;

            foreach (Node rec in this.GetAllNodes()) // Loop through List with foreach
            {
                match = regex.Match(rec.link);
                if (match.Success && match.Groups[1].Value == id) return rec;
            }

            return null;
        }

        public Nodes GetAllNodes()
        {
            return this.layers.GetAllNodes();
        }

        public Lines GetAllLines()
        {
            return this.layers.GetAllLines();
        }

        // NODE Najdenie nody podla pozicie myši
        public Node FindNodeInPosition(Position position, int layer, Node skipNode = null)
        {
            foreach (Node node in this.layers.GetLayer(layer).nodes.Reverse<Node>()) // Loop through List with foreach
            {
                if (layer == node.layer || layer == node.id)
                {
                    if
                    (
                        node.position.x <= position.x && position.x <= node.position.x + node.width &&
                        node.position.y <= position.y && position.y <= node.position.y + node.height &&
                        (skipNode == null || skipNode.id != node.id)
                    )
                    {
                        return node;
                    }
                }
            }

            return null;
        }

        /*************************************************************************************************************************/
        // NODES DELETE

        // NODE delete all nodes which is not in layer history
        public bool CanDeleteNode(Node node)
        {
            // sub node is viewed
            foreach (DiagramView view in this.DiagramViews)
            {
                if (view.IsNodeInLayerHistory(node))
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

                this.layers.RemoveNode(rec);
            }
        }

        // NODE delete multiple nodes and set undo operation
        public void DeleteNodes(Nodes nodes, Position position = null, int layer = 0)
        {
            bool canDelete = false;

            Nodes toDeleteNodes = new Nodes();
            Lines toDeleteLines = new Lines();

            this.layers.GetAllNodesAndLines(nodes, ref toDeleteNodes, ref toDeleteLines);

            foreach (Node node in nodes)
            {
                if (this.CanDeleteNode(node))
                {
                    canDelete = true;
                }
            }

            if (canDelete)
            {
                this.undoOperations.add("delete", toDeleteNodes, toDeleteLines, position, layer);

                foreach (Node node in toDeleteNodes.Reverse<Node>()) // remove lines to node
                {
                    this.DeleteNode(node);
                }

                this.InvalidateDiagram();
                this.Unsave();
            }
        }

        /*************************************************************************************************************************/
        // NODES EDIT

        // NODE Editovanie vlastnosti nody
        public TextForm EditNode(Node rec)
        {
            bool found = false;
            for (int i = TextWindows.Count() - 1; i >= 0; i--) // Loop through List with foreach
            {
                if (TextWindows[i].node == rec)
                {
                    Media.BringToFront(TextWindows[i]);
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
                main.AddTextWindow(textf);
                textf.Show();
                Media.BringToFront(textf);
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

        /*************************************************************************************************************************/
        // NODES CREATE

        // NODE Create Rectangle on point
        public Node CreateNode(
            Position position,
            string name = "",
            int layer = 0,
            ColorType color = null,
            Font font = null
        ) {
            if (this.options.readOnly)
            {
                return null;
            }

            var rec = new Node();
            if (font == null)
            {
                rec.font = this.FontDefault;
            }
            else
            {
                rec.font = font;
            }

            rec.layer = layer;

            rec.setName(name);
            rec.note = "";
            rec.link = "";

            rec.position.Set(position);

            if (color != null)
            {
                rec.color.Set(color);
            }
            else
            {
                rec.color.Set(Media.GetColor(this.options.colorNode));
            }

            return this.layers.CreateNode(rec);
        }

        // NODE add node to diagram (create new id and layer if not exist) 
        public Node CreateNode(Node node)
        {
            if (!this.options.readOnly)
            {
                return this.layers.CreateNode(node);
            }

            return null;
        }

        /*************************************************************************************************************************/
        // LINES SELECT

        // LINE HASLINE check if line exist between two nodes
        public bool HasConnection(Node a, Node b)
        {
            Line line = this.layers.GetLine(a, b);
            return line != null;
        }

        public Line GetLine(Node a, Node b)
        {
            return this.layers.GetLine(a, b);
        }

        public Line GetLine(int a, int b)
        {
            return this.layers.GetLine(a, b);
        }

        // LINE CONNECT connect two nodes
        public Line Connect(Node a, Node b)
        {
            if (!this.options.readOnly && a != null && b != null)
            {
                Line line = this.layers.GetLine(a, b);

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

                    line = new Line {
                        start = a.id,
                        end = b.id,
                        startNode = this.GetNodeByID(a.id),
                        endNode = this.GetNodeByID(b.id),
                        layer = layer
                    };

                    this.layers.AddLine(line);

                    return line;
                }
            }

            return null;
        }

        // LINE DISCONNECT remove connection between two nodes
        public void Disconnect(Node a, Node b)
        {
            if (!this.options.readOnly && a != null && b != null)
            {
                Line line = this.layers.GetLine(a, b);

                if (line != null)
                {
                    this.layers.RemoveLine(line);
                }
            }
        }

        // LINE CONNECT connect two nodes and add arrow or set color
        public Line Connect(Node a, Node b, bool arrow = false, ColorType color = null, int width = 1)
        {
            Line line = this.Connect(a, b);

            if (line != null)
            {
                line.arrow = arrow;
                if (color != null)
                {
                    line.color.Set(color);
                }
                
                line.width = width;
            }

            return line;
        }

        /*************************************************************************************************************************/
        // ALIGN

        // align to line
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

        // align node to top element and create constant space between nodes
        public void AlignCompact(Nodes nodes)
        {
            if (nodes.Count() > 0)
            {
                int minx = nodes[0].position.x;
                int miny = nodes[0].position.y;
                foreach (Node rec in nodes)
                {
                    if (rec.position.y <= miny) // find most top element
                    {
                        miny = rec.position.y;
                        minx = rec.position.x;
                    }
                }

                foreach (Node rec in nodes) // align to left
                {
                    rec.position.x = minx;
                }

                // sort elements by y coordinate
                nodes.OrderByPositionY();

                int posy = miny;
                foreach (Node rec in nodes) // change space between nodes
                {
                    rec.position.y = posy;
                    posy = posy + rec.height + 10;
                }
            }
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

        // align node to most left node and create constant space between nodes
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

        // align left
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
            }
        }

        // align left
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

        // align node to top element and create constant space between nodes and sort items
        public void SortNodes(Nodes nodes)
        {
            if (nodes.Count() > 0)
            {
                int minx = nodes[0].position.x;
                int miny = nodes[0].position.y;
                foreach (Node rec in nodes)
                {
                    if (rec.position.y <= miny) // find most top element
                    {
                        minx = rec.position.x;
                        miny = rec.position.y;
                    }
                }

                foreach (Node rec in nodes) // align to left
                {
                    rec.position.x = minx;
                }

                nodes.OrderByPositionY();

                // check if nodes are ordered by name already
                bool alreadyAsc = true;
                for (int i = 0; i < nodes.Count - 1; i++)
                {
                    if (String.Compare(nodes[i + 1].name, nodes[i].name) < 0)
                    {
                        alreadyAsc = false;
                        break;
                    }
                }

                if (alreadyAsc)
                {
                    nodes.OrderByNameDesc();
                }
                else
                {
                    nodes.OrderByNameAsc();
                }

                int posy = miny;
                foreach (Node rec in nodes) // change space between nodes
                {
                    rec.position.y = posy;
                    posy = posy + rec.height + 10;
                }
            }
        }

        // split node to lines
        public Nodes SplitNode(Nodes nodes)
        {
            if (nodes.Count() > 0)
            {
                Nodes newNodes = new Nodes();

                int minx = nodes[0].position.x;
                int miny = nodes[0].position.y;
                foreach (Node rec in nodes)
                {
                    if (rec.position.y <= miny) // find most top element
                    {
                        minx = rec.position.x;
                        miny = rec.position.y;
                    }
                }

                foreach (Node node in nodes) // create new nodes
                {
                    string[] lines = node.name.Split(new string[] { "\n" }, StringSplitOptions.None);

                    int posy = node.position.y + node.height + 10;

                    foreach (String line in lines)
                    {
                        if (line.Trim() != "")
                        {
                            Node newNode = this.CreateNode(new Node(node)); // duplicate content of old node
                            newNode.setName(line);
                            newNode.position.y = posy;
                            posy = posy + newNode.height + 10;
                            newNodes.Add(newNode);
                        }
                    }
                }

                return newNodes;
            }

            return null;
        }

        /*************************************************************************************************************************/
        // SHORTCUTS

        // remove shortcut
        public void RemoveShortcut(Node node)
        {
            if (node.shortcut > 0) node.shortcut = 0;
        }

        // remove mark
        public void RemoveMark(Node node)
        {
            if (node.mark) node.mark = false;
        }

        /*************************************************************************************************************************/
        // NODE FONTS

        // NODE Reset font to default font for group of nodes
        public void ResetFont(Nodes nodes, Position position = null, int layer = 0)
        {
            if (nodes.Count>0) {
                this.undoOperations.add("edit", nodes, null, position, layer);
                foreach (Node rec in nodes) // Loop through List with foreach
                {
                    rec.font = this.FontDefault;
                    rec.resize();
                }
                this.Unsave();
                this.InvalidateDiagram();
            }
        }

        /*************************************************************************************************************************/
        // IMAGE

        // set image
        public void SetImage(Node rec, string file)
        {
            try
            {
                rec.isimage = true;
                rec.image = Media.GetImage(file);
                rec.imagepath = Os.MakeRelative(file, this.FileName);
                string ext = Os.GetExtension(file);
                if (ext != ".ico") rec.image.MakeTransparent(Color.White);
                rec.height = rec.image.Height;
                rec.width = rec.image.Width;
            }
            catch(Exception e)
            {
                Program.log.Write("setImage: " + e.Message);
            }
        }

        // remove image
        public void RemoveImage(Node rec)
        {
            rec.isimage = false;
            rec.imagepath = "";
            rec.image = null;
            rec.embeddedimage = false;
            rec.resize();
        }

        // set image embedded
        public void SetImageEmbedded(Node rec)
        {
            if (rec.isimage)
            {
                rec.embeddedimage = true;
            }
        }

        /*************************************************************************************************************************/
        // LAYERS

        // LAYER MOVE posunie rekurzivne layer a jeho nody OBSOLATE
        public void MoveLayer(Node rec, Position vector)
        {
            if (rec != null)
            {
                Nodes nodes = this.layers.GetLayer(rec.id).nodes;
                foreach (Node node in nodes) // Loop through List with foreach
                {
                    if (node.layer == rec.id)
                    {
                        node.position.Add(vector);

                        if (node.haslayer)
                        {
                            MoveLayer(node, vector);
                        }
                    }
                }
            }
        }

        /*************************************************************************************************************************/
        // VIEW

        // open new view on diagram
        public DiagramView OpenDiagramView(DiagramView parent = null, Layer layer = null)
        {
            DiagramView diagramview = new DiagramView(main, this, parent);
            diagramview.SetDiagram(this);
            this.DiagramViews.Add(diagramview);
            main.AddDiagramView(diagramview);
			this.SetTitle();
            diagramview.Show();
            if (layer != null)
            {
                diagramview.GoToLayer(layer.id);
            }
            return diagramview;
        }

        // invalidate all opened views
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

        // close view
        public void CloseView(DiagramView view)
        {
            this.DiagramViews.Remove(view);
            main.RemoveDiagramView(view);

            foreach (DiagramView diagramView in this.DiagramViews) {
                if (diagramView.parentView == view) {
                    diagramView.parentView = null;
                }
            }

            this.CloseDiagram();
        }

        // close diagram
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
                main.RemoveDiagram(this);
                main.CloseEmptyApplication();
            }
        }

        // change title
        public void SetTitle()
        {
            foreach (DiagramView DiagramView in this.DiagramViews)
            {
                DiagramView.SetTitle();
            }
        }

        // refresh - refresh items depends on external resources like images
        public void RefreshAll()
        {
            foreach (Node node in this.layers.GetAllNodes())
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

        // refresh nodes- refresh items depends on external resources like images or hyperlinks names
        public void RefreshNodes(Nodes nodes)
        {
            foreach (Node node in nodes)
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

        // refresh background image after background image change
        public void RefreshBackgroundImages()
        {
            foreach (DiagramView DiagramView in this.DiagramViews)
            {
                DiagramView.RefreshBackgroundImage();
            }
        }

        /*************************************************************************************************************************/
        // CLIPBOARD

        // paste part of diagram from clipboard                                   
        public DiagramBlock AddDiagramPart(string DiagramXml, Position position, int layer)
        {
            Nodes NewNodes = new Nodes();
            Lines NewLines = new Lines();

            XmlReaderSettings xws = new XmlReaderSettings
            {
                CheckCharacters = false
            };

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
                                        Node R = new Node
                                        {
                                            font = this.FontDefault
                                        };

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
                                                    R.color.Set(el.Value.ToString());
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
                                                    R.fontcolor.Set(el.Value.ToString());
                                                }

                                                if (el.Name.ToString() == "link")
                                                {
                                                    R.link = el.Value;
                                                }

                                                if (el.Name.ToString() == "shortcut")
                                                {
                                                    R.shortcut = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "mark")
                                                {
                                                    R.mark = bool.Parse(el.Value);
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
                                                            ext = Os.GetExtension(R.imagepath).ToLower();

                                                            if (ext == ".jpg" || ext == ".png" || ext == ".ico" || ext == ".bmp") // skratenie cesty k suboru
                                                            {
                                                                R.image = Media.GetImage(R.imagepath);
                                                                if (ext != ".ico") R.image.MakeTransparent(Color.White);
                                                                R.height = R.image.Height;
                                                                R.width = R.image.Width;
                                                                R.isimage = true;
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Program.log.Write("load image from xml error: " + ex.Message);
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
                                                Program.log.Write("Data has wrong structure. : error: " + ex.Message);
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
                                                    L.color.Set(el.Value.ToString());
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
                                                Program.log.Write("Data has wrong structure. : error: " + ex.Message);
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
                Program.log.Write("Data has wrong structure. : error: " + ex.Message);
            }


            List<MappedNode> maps = new List<MappedNode>();

            Nodes NewReorderedNodes = new Nodes(); // order nodes parent first (layer must exist when sub node is created)
            this.NodesReorderNodes(0, null, NewNodes, NewReorderedNodes);

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
                rec.position.Add(position);
                rec.resize();

                oldId = rec.id;
                newNode = this.CreateNode(rec);

                if (newNode != null) {
                    mappedNode = new MappedNode {
                        oldId = oldId,
                        newNode = newNode
                    };
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

            return new DiagramBlock(NewNodes, createdLines);
        }

        // Get all layers nodes
        private void NodesReorderNodes(int layer, Node parent, Nodes nodesIn, Nodes nodesOut)
        {
            foreach (Node node in nodesIn)
            {
                if (node.layer == layer)
                {
                    if (parent != null) {
                        parent.haslayer = true;
                    }

                    nodesOut.Add(node);

                    NodesReorderNodes(node.id, node, nodesIn, nodesOut);
                }
            }
        }

        // Get all layers nodes
        public void GetLayerNodes(Node node, Nodes nodes)
        {
            if (node.haslayer) {
                foreach(Node subnode in this.layers.GetLayer(node.id).nodes) {
                    nodes.Add(subnode);

                    if (subnode.haslayer) {
                        GetLayerNodes(subnode, nodes);
                    }
                }
            }
        }

        // copy part of diagram to text xml string
        public string GetDiagramPart(Nodes nodes)
        {
            Nodes copy = new Nodes();
            foreach (Node node in nodes)
            {
                copy.Add(node);
            }

            string copyxml = "";

            if (copy.Count() > 0)
            {
                XElement root = new XElement("diagram");
                XElement rectangles = new XElement("rectangles");
                XElement lines = new XElement("lines");

                int minx = copy[0].position.x;
                int miny = copy[0].position.y;
                int minid = copy[0].id;

                foreach (Node node in copy)
                {
                    if (node.position.x < minx) minx = node.position.x;
                    if (node.position.y < miny) miny = node.position.y;
                    if (node.id < minid) minid = node.id;
                }

                Nodes subnodes = new Nodes();

                foreach (Node node in copy)
                {
                    GetLayerNodes(node, subnodes);
                }

                foreach (Node node in subnodes)
                {
                    copy.Add(node);
                }

                foreach (Node rec in copy)
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
                    if (rec.mark) rectangle.Add(new XElement("mark", rec.mark));
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

                foreach (Line li in this.GetAllLines())
                {
                    foreach (Node recstart in copy)
                    {
                        if (li.start == recstart.id)
                        {
                            foreach (Node recend in copy)
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

        public DiagramBlock GetPartOfDiagram(Nodes nodes)
        {
            Nodes allNodes = new Nodes();
            Lines lines = new Lines();

            foreach (Node node in nodes)
            {
                allNodes.Add(node);
            }

            if (allNodes.Count() > 0)
            {
                Nodes subnodes = new Nodes();

                foreach (Node node in allNodes)
                {
                    GetLayerNodes(node, subnodes);
                }

                foreach (Node node in subnodes)
                {
                    allNodes.Add(node);
                }

                foreach (Line li in this.GetAllLines())
                {
                    foreach (Node recstart in allNodes)
                    {
                        if (li.start == recstart.id)
                        {
                            foreach (Node recend in allNodes)
                            {
                                if (li.end == recend.id)
                                {
                                    lines.Add(li);
                                }
                            }
                        }
                    }
                }
            }

            return new DiagramBlock(allNodes, lines);
        }

        public DiagramBlock DuplicatePartOfDiagram(Nodes nodes, int layer = 0)
        {
            // get part of diagram for duplicate
            DiagramBlock diagramPart = this.GetPartOfDiagram(nodes);

            List<MappedNode> maps = new List<MappedNode>();

            Nodes duplicatedNodes = new Nodes();
            foreach (Node node in diagramPart.nodes)
            {
                duplicatedNodes.Add(node.clone());
            }

            // order nodes parent first (layer must exist when sub node is created)
            Nodes NewReorderedNodes = new Nodes(); 
            this.NodesReorderNodes(layer, null, duplicatedNodes, NewReorderedNodes);

            int layerParent = 0;

            MappedNode mappedNode;
            Nodes createdNodes = new Nodes();
            Node newNode = null;
            int oldId = 0;
            foreach (Node rec in NewReorderedNodes)
            {
                layerParent = layer;
                
                // find layer id for sub layer
                foreach (MappedNode mapednode in maps)
                {
                    if (rec.layer == mapednode.oldId)
                    {
                        layerParent = mapednode.newNode.id;
                        break;
                    }
                }


                rec.layer = layerParent;
                rec.resize();

                oldId = rec.id;
                newNode = this.CreateNode(rec);

                if (newNode != null)
                {
                    mappedNode = new MappedNode
                    {
                        oldId = oldId,
                        newNode = newNode
                    };

                    createdNodes.Add(newNode);
                    maps.Add(mappedNode);
                }
            }

            // fix layers and shortcuts
            foreach (Node rec in duplicatedNodes)
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
            foreach (Line line in diagramPart.lines)
            {
                foreach (MappedNode mapbegin in maps)
                {
                    if (line.start == mapbegin.oldId)
                    {
                        foreach (MappedNode mapend in maps)
                        {
                            if (line.end == mapend.oldId)
                            {
                                // create new line by connecting new nodes
                                newLine = this.Connect(
                                    mapbegin.newNode,
                                    mapend.newNode,
                                    line.arrow,
                                    line.color,
                                    line.width
                                );

                                if (newLine != null) // skip invalid lines (perent not exist)
                                {
                                    createdLines.Add(newLine);
                                }
                            }
                        }
                    }
                }
            }

            return new DiagramBlock(createdNodes, createdLines);
        }

        /*************************************************************************************************************************/
        // SECURITY

        // encrypt diagram 
        public bool SetPassword(string password = null)
        {
            string newPassword = null;

            if (password == null)
            {
                newPassword = this.main.GetNewPassword();
            }
            else
            {
                newPassword = password;
            }

            if (newPassword != null)
            {

                if (newPassword == "")
                {
                    this.encrypted = false;
                    this.password = null;
                    this.passwordHash = null;
                    return true;
                }

                if (newPassword != "" && this.password == null)
                {
                    this.encrypted = true;
                    this.password = Encrypt.ConvertToSecureString(newPassword);
                    this.passwordHash = Encrypt.CalculateSHAHash(newPassword);
                    return true;
                }

                if (newPassword != "" && this.password != null && !Encrypt.CompareSecureString(this.password, newPassword))
                {
                    this.encrypted = true;
                    this.password = Encrypt.ConvertToSecureString(newPassword);
                    this.passwordHash = Encrypt.CalculateSHAHash(newPassword);
                    return true;
                }
            }

            return false;
        }

        // change password
        public bool ChangePassword()
        {
            string newPassword = this.main.ChangePassword(this.password);
            if (newPassword != null)
            {
                return this.SetPassword(newPassword);
            }

            return false;
        }

        // check if password is set
        public bool IsEncrypted()
        {
            return this.encrypted;
        }

        // check if diagram is locked
        public bool IsLocked()
        {
            return this.locked;
        }

        // lock diagram - forgot password
        public void LockDiagram()
        {
            if (this.encrypted && !this.locked)
            {
                this.locked = true;
                this.password = null;
                this.InvalidateDiagram();
            }
        }

        // unlock diagram - prompt for new password
        public bool UnlockDiagram()
        {
            if (this.encrypted && this.locked)
            {
                while (true) // while password is not correct or cancel is pressed
                {
                    string password = this.main.GetPassword();

                    if (password != null && this.passwordHash == Encrypt.CalculateSHAHash(password))
                    {
                        this.SetPassword(password);
                        this.locked = false;
                        this.InvalidateDiagram();
                        return true;
                    }
                    else if (password == null)
                    {
                        this.locked = true;
                        this.CloseDiagram();
                        return false;
                    }
                }
            }

            return false;
        }
    }
}
