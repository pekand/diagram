using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq; // using
using System.Xml.Linq; // using
using System.Xml;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Text;

namespace Diagram
{
    public class Diagram
    {
        public Main main = null;

        public List<Node> Nodes = new List<Node>();          // zoznam nod
        public List<Line> Lines = new List<Line>();          // zoznam spojovacich ciar

        public List<DiagramView> DiagramViews = new List<DiagramView>(); // zoznam otvorenych pohladov do diagramou

        // ATTRIBUTES DRAW
        public int NodePadding = 10;              // CONST okraj stvorca
        public int EmptyNodePadding = 20;         // CONST okraj stvorca

        // RESOURCES
        public PrivateFontCollection fonts = null;
        public FontFamily family = null;
        public Font FontDefault = null;

        // ATTRIBUTES File
        public bool NewFile = true;              //súbor je nový ešte neuložený(nemá meno)
        public bool SavedFile = true;            //súbor bol uložený na disk(má svoje meno)
        public string FileName = "";             //názov otvoreného súboru

        // ATRIBUTES OBJECTS
        public int maxid = 0;                    // največšie vložené id objektu

        // ATTRIBUTES ENCRYPTION
        public bool encrypted = false;           // pri ukladaní súbor zašifrovať
        public string password = "";             // heslo
		private byte[] salt = null;


        // ATTRIBUTES TextForm
        public List<TextForm> TextWindows = new List<TextForm>();   // Zoznam otvorenych okien

        // ATTRIBUTES OPTIONS
        public Options options = new Options();

        public Diagram(Main main)
        {
            this.main = main;
			this.FontDefault = new Font("Open Sans", 10);
        }
        /*************************************************************************************************************************/

        // [FILE] IS NEW - check if file is empty
        public bool isNew()
        {
            return (this.FileName == "" && this.NewFile && this.SavedFile);
        }

        // [FILE] [OPEN] Otvorenie xml súboru
        public bool OpenFile(string FileName)
        {

            if (File.Exists(FileName))
            {
                Directory.SetCurrentDirectory(new FileInfo(FileName).DirectoryName);

                this.CloseFile();
                this.FileName = FileName;
                this.NewFile = false;
                this.SavedFile = true;
                if (new FileInfo(FileName).Length != 0)
                {

                    try
                    {
                        string xml;
                        using (StreamReader streamReader = new StreamReader(FileName, Encoding.UTF8))
                        {
                            xml = streamReader.ReadToEnd();
                        }
                        this.LoadXML(xml);

                    }
                    catch (System.IO.IOException ex)
                    {
                        Program.log.write(ex.Message);
                        MessageBox.Show(main.translations.fileIsLocked);
                        this.CloseFile();
                    }

                    this.SetTitle();

                    return true; // subor sa otvoril v poriadku
                }
            }

            return false; // subor sa nepodarilo otvorit
        }

        // [FILE] [LOAD] [XML]
        public void LoadXML(string xml)
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
                            Program.log.write(e.Message);
                            error = true;
                        }
                    }
                    else
                    {
                        this.CloseFile();
                        return;
                    }

                    main.passwordForm.CloseForm();
                } while (error);
            }
            else
            {
                LoadInnerXML(xml);
            }

        }

        // [FILE] [LOAD] [XML] vnutorna cast
        public void LoadInnerXML(string xml)
        {
            string FontDefaultString = TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(this.FontDefault);

            XmlReaderSettings xws = new XmlReaderSettings();
            xws.CheckCharacters = false;

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

                                        if (el.Name.ToString() == "layer")
                                        {
                                            options.layer = Int32.Parse(el.Value);
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
                                                    R.fontcolor = System.Drawing.ColorTranslator.FromHtml(el.Value.ToString());
                                                }

                                                if (el.Name.ToString() == "text")
                                                {
                                                    R.text = el.Value;
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
                                                    R.layershiftx = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "layershifty")
                                                {
                                                    R.layershifty = Int32.Parse(el.Value);
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
                                                    R.color = System.Drawing.ColorTranslator.FromHtml(el.Value.ToString());
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
                                                    if (File.Exists(R.imagepath))
                                                    {
                                                        try
                                                        {
                                                            string ext = "";
                                                            ext = Path.GetExtension(R.imagepath).ToLower();


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

                                            }
                                            catch (Exception ex)
                                            {
                                                Program.log.write("load xml nodes error: " + ex.Message);
                                            }
                                        }
                                        this.Nodes.Add(R);
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
                                            }
                                            catch (Exception ex)
                                            {
                                                Program.log.write("load xml lines error: " + ex.Message);
                                            }
                                        }
                                        this.Lines.Add(L);
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
            }

            int newWidth = 0;
            int newHeight = 0;

            foreach (Node rec in this.Nodes) // Loop through List with foreach
            {
                if (!rec.isimage)
                {
                    SizeF s = this.MeasureStringWithMargin(rec.text, rec.font);
                    newWidth = (int)s.Width;
                    newHeight = (int)s.Height;

                    // font change correction > center node
                    if (rec.width != 0 && newWidth != rec.width) {
                        rec.position.x += (rec.width - newWidth) / 2;
                    }

                    if (rec.height != 0 && newHeight != rec.height) {
                       rec.position.y += (rec.height - newHeight) / 2;
                    }

                    rec.width = newWidth;
                    rec.height = newHeight;
                }
            }

            // check file integrity
            for (int i = this.Lines.Count() - 1; i >= 0; i--) // Loop through List with foreach
            {
                this.Lines[i].startNode = this.GetNodeByID(this.Lines[i].start);
                this.Lines[i].endNode = this.GetNodeByID(this.Lines[i].end);
                if (this.Lines[i].startNode == null || this.Lines[i].endNode == null)
                {
                    this.Lines.RemoveAt(i);
                }
            }
        }

        // [FILE] Save - Ulozit súbor
        public bool save()
        {
            if (this.FileName != "" && File.Exists(this.FileName))
            {
                this.SaveXMLFile(this.FileName);
                this.NewFile = false;
                this.SavedFile = true;
                this.SetTitle();

                return true;
            }

            return false;
        }

        // [FILE] SAVEAS - Uložiť súbor ako
        public void saveas(String FileName)
        {

            this.SaveXMLFile(FileName);
            this.FileName = FileName;
            this.SavedFile = true;
            this.NewFile = false;

            this.SetTitle();
        }

        // [FILE] [SAVE] Ulozenie xml súboru
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

        // [FILE] [SAVE] [XML]
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
                option.Add(new XElement("layer", this.options.layer));
                option.Add(new XElement("diagramreadonly", this.options.readOnly));
                option.Add(new XElement("grid", this.options.grid));
                option.Add(new XElement("borders", this.options.borders));
                option.Add(Fonts.FontToXml(this.FontDefault, "defaultfont"));
                option.Add(new XElement("coordinates", this.options.coordinates));
                option.Add(new XElement("window.position.x", this.options.Left));
                option.Add(new XElement("window.position.y", this.options.Top));
                option.Add(new XElement("window.position.width", this.options.Width));
                option.Add(new XElement("window.position.height", this.options.Height));
                option.Add(new XElement("window.state", this.options.WindowState));

                // Rectangles
                XElement rectangles = new XElement("rectangles");
                foreach (Node rec in this.Nodes)
                {
                    XElement rectangle = new XElement("rectangle");
                    rectangle.Add(new XElement("id", rec.id));
                    if (!Fonts.compare(this.FontDefault, rec.font))
                    {
                        rectangle.Add(Fonts.FontToXml(rec.font));
                    }
                    rectangle.Add(new XElement("fontcolor", System.Drawing.ColorTranslator.ToHtml(rec.fontcolor)));
                    if (rec.text != "") rectangle.Add(new XElement("text", rec.text));
                    if (rec.note != "") rectangle.Add(new XElement("note", rec.note));
                    if (rec.link != "") rectangle.Add(new XElement("link", rec.link));
                    if (rec.scriptid != "") rectangle.Add(new XElement("scriptid", rec.scriptid));
                    if (rec.shortcut != 0) rectangle.Add(new XElement("shortcut", rec.shortcut));

                    rectangle.Add(new XElement("layer", rec.layer));

                    if (rec.haslayer)
                    {
                        rectangle.Add(new XElement("haslayer", rec.haslayer));
                        rectangle.Add(new XElement("layershiftx", rec.layershiftx));
                        rectangle.Add(new XElement("layershifty", rec.layershifty));
                    }

                    rectangle.Add(new XElement("x", rec.position.x));
                    rectangle.Add(new XElement("y", rec.position.y));
                    rectangle.Add(new XElement("width", rec.width));
                    rectangle.Add(new XElement("height", rec.height));
                    rectangle.Add(new XElement("color", System.Drawing.ColorTranslator.ToHtml(rec.color)));
                    if (rec.transparent) rectangle.Add(new XElement("transparent", rec.transparent));
                    if (rec.embeddedimage) rectangle.Add(new XElement("embeddedimage", rec.embeddedimage));

                    if (rec.embeddedimage && rec.image != null) // ak je obrázok vložený priamo do súboru
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

                    rectangles.Add(rectangle);
                }

                // Lines
                XElement lines = new XElement("lines");
                foreach (Line lin in this.Lines)
                {
                    XElement line = new XElement("line");
                    line.Add(new XElement("start", lin.start));
                    line.Add(new XElement("end", lin.end));
                    line.Add(new XElement("arrow", (lin.arrow) ? "1" : "0"));
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

        // [FILE] UNSAVE Subor sa zmenil treba ho ulozit
        public void unsave()
        {
            this.SavedFile = false;
            this.SetTitle();
        }

        // [FILE] [CLOSE] - Vycisti  nastavenie do východzieho tavu a prekresli obrazovku
        public void CloseFile()
        {
            // Prednadstavenie atributov
            this.maxid = 0;
            this.NewFile = true;
            this.SavedFile = true;
            this.FileName = "";

            // Vycistenie zasobnikov
            this.Lines.Clear();
            this.Nodes.Clear();



            this.options.readOnly = false;
            this.options.grid = true;
            this.options.coordinates = false;

            this.TextWindows.Clear();
        }

        /*************************************************************************************************************************/

        // [NODE] Najdenie nody podla id
        public Node GetNodeByID(int id)
        {
            foreach (Node rec in this.Nodes) // Loop through List with foreach
            {
                if (rec.id == id) return rec;
            }
            return null;
        }

        // [NODE] Najdenie indexu v poli nody podla id
        public int GetIndexByID(int id)
        {
            for (int i = 0; i < this.Nodes.Count(); i++) // Loop through List with foreach
            {
                if (this.Nodes[i].id == id) return i;
            }
            return -1;
        }

        // [NODE] Najdenie nody podla scriptid
        public Node GetNodeByScriptID(string id)
        {
            foreach (Node rec in this.Nodes) // Loop through List with foreach
            {
                if (rec.scriptid == id) return rec;
            }
            return null;
        }

        // NODE delete all nodes which is not in layer history
        public bool canDeleteNode(Node rec)
        {
            if (!rec.haslayer)
            {
                foreach (DiagramView view in this.DiagramViews)
                {
                    if (view.isNodeInLayerHistory(rec))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // [NODE] Zmazanie nody
        public void DeleteNode(Node rec)
        {
            if (rec != null && !this.options.readOnly)
            {
                int id = rec.id;
                for (int i = this.Lines.Count() - 1; i >= 0; i--) // Loop through List with foreach
                {
                    if (this.Lines[i].start == rec.id || this.Lines[i].end == rec.id)
                    {
                        this.Lines.RemoveAt(i);
                    }
                }

                foreach (Node r in this.Nodes) //odstranenie odkazov na nodu
                {
                    if (r.shortcut == id)
                    {
                        r.shortcut = 0;
                    }
                }
                foreach (DiagramView DiagramView in this.DiagramViews) //odstranenie odkazov na nodu vo vybratych nodach vo vsetkych otvorenych pohladoch
                {
                    if (DiagramView.SelectedNodes.Count() > 0)
                    {
                        for (int i = DiagramView.SelectedNodes.Count() - 1; i >= 0; i--)
                        {
                            if (DiagramView.SelectedNodes[i] == rec)
                            {
                                DiagramView.SelectedNodes.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }

                if (this.TextWindows.Count() > 0)
                {
                    for (int i = this.TextWindows.Count() - 1; i >= 0; i--)
                    {
                        if (this.TextWindows[i].rec == rec)
                        {
                            this.TextWindows[i].Close();
                            break;
                        }
                    }
                }


                this.Nodes.Remove(rec);
                this.unsave();
            }
        }

        // NODE Editovanie vlastnosti nody
        public TextForm EditNode(Node rec)
        {
            bool found = false;
            for (int i = TextWindows.Count() - 1; i >= 0; i--) // Loop through List with foreach
            {
                if (TextWindows[i].rec == rec)
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
                textf.rec = rec;
                string[] lines = rec.text.Split(Environment.NewLine.ToCharArray()).ToArray();
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
                if (TextWindows[i].rec == rec)
                {
                    TextWindows.RemoveAt(i);
                }
            }
        }

        // NODE Create Rectangle on point
        public Node CreateNode(int x, int y, int layer, string text = "", string  color = null)
        {
            if (!this.options.readOnly)
            {
                var rec = new Node();
                rec.font = this.FontDefault;
                rec.text = text;
                rec.note = "";
                rec.link = "";
                rec.position.x = x;
                rec.position.y = y;
                SizeF s = this.MeasureStringWithMargin(text, rec.font);
                rec.id = ++maxid;
                rec.width = (int)s.Width;
                rec.height = (int)s.Height;
                rec.layer = layer;
                if (color != null) {
                    rec.color = Media.getColor(color);
                }
                DateTime dt = DateTime.Now;
                rec.timecreate =  String.Format("{0:yyyy-M-d HH:mm:ss}", dt);
                rec.timemodify =  rec.timecreate;
                this.Nodes.Add(rec);
                return rec;
            }
            else
            {
                return null;
            }
        }

        // NODE CONNECT Spojenie dvoch nod
        public void Connect(Node a, Node b, bool arrow = false)
        {
            if (!this.options.readOnly)
            {
                bool found = false;
                for (int i = this.Lines.Count() - 1; i >= 0; i--) // odstranenie spojenia
                {
                    if ((this.Lines[i].start == a.id && this.Lines[i].end == b.id) || (this.Lines[i].start == b.id && this.Lines[i].end == a.id))
                    {
                        this.Lines.RemoveAt(i); ;
                        this.InvalidateDiagram();
                        found = true;
                        this.unsave();
                        break;
                    }
                }
                if (!found) // vytvorenie spojenia
                {
                    Line L = new Line();
                    L.start = a.id;
                    L.end = b.id;
                    L.startNode = this.GetNodeByID(L.start);
                    L.endNode = this.GetNodeByID(L.end);
                    L.arrow = arrow;
                    this.Lines.Add(L);
                    this.unsave();
                    this.InvalidateDiagram();
                }
            }
        }

        // NODES ALIGN to column
        public void AlignToColumn(List<Node> Nodes)
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
        public void AlignToLine(List<Node> Nodes)
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
        public void AlignCompact(List<Node> Nodes)
        {
            // vyratanie ci je mensi profil na sirku alebo na vysku a potom ich zarovnat
            // po zarovnani by sa nemali prekrivat
            // ked su zarovnané pozmensovat medzery medzi nimi na nejaku konstantnu vzdialenost

            if (Nodes.Count() > 0)
            {
                int minx = Nodes[0].position.x;
                int miny = Nodes[0].position.y;
                foreach (Node rec in Nodes)
                {
                    if (rec.position.x <= minx) // najdi najlavejsi bod
                    {
                        minx = rec.position.x;
                    }

                    if (rec.position.y <= miny) // najdi najvrchnejsi bod
                    {
                        miny = rec.position.y;
                    }
                }

                foreach (Node rec in Nodes) // zarovnaj dolava
                {
                    rec.position.x = minx;
                }

                // zotriedenie prvkov podla y velkosti
                List<Node> SortedList = Nodes.OrderBy(o => o.position.y).ToList();

                int posy = miny;
                foreach (Node rec in SortedList) // zmensit medzeru medzi objektami
                {
                    rec.position.y = posy;
                    posy = posy + rec.height + 10;
                }
            }
        }

        // NODES ALIGN left
        public void AlignRight(List<Node> Nodes)
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
        public void AlignLeft(List<Node> Nodes)
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

        // NODE Zmeranie velosti textu a prida margin
        public SizeF MeasureStringWithMargin(string s, Font font)
        {
            SizeF result;
            if (s != "")
            {
                result = Fonts.MeasureString(s, font);
                result.Height += 2 * this.NodePadding;
                result.Width += 2 * this.NodePadding;
            }
            else
            {
                result = new SizeF(this.EmptyNodePadding, this.EmptyNodePadding);
            }

            return result;
        }

         // NODE Reset font to default font for all nodes
        public void ResetFont()
        {
            foreach (Node rec in this.Nodes) // Loop through List with foreach
            {
                rec.font = this.FontDefault;
                SizeF s = this.MeasureStringWithMargin(rec.text, rec.font);
                rec.width = (int)s.Width;
                rec.height = (int)s.Height;
            }

            this.unsave();
            this.InvalidateDiagram();
        }

        // NODE Reset font to default font for group of nodes
        public void ResetFont(List<Node> Nodes)
        {
            if (Nodes.Count>0) {
                foreach (Node rec in Nodes) // Loop through List with foreach
                {
                    rec.font = this.FontDefault;
                    SizeF s = this.MeasureStringWithMargin(rec.text, rec.font);
                    rec.width = (int)s.Width;
                    rec.height = (int)s.Height;
                }
                this.unsave();
                this.InvalidateDiagram();
            }
        }

        // NODE Najdenie nody podla pozicie myši
        public Node findNodeInPosition(int x, int y, int layer)
        {
            for (int i = this.Nodes.Count() - 1; i >= 0; i--) // Loop through List with foreach
            {
                if (layer == this.Nodes[i].layer || layer == this.Nodes[i].id)
                {
                    if
                    (
                        this.Nodes[i].position.x <= x && x <= this.Nodes[i].position.x + this.Nodes[i].width &&
                        this.Nodes[i].position.y <= y && y <= this.Nodes[i].position.y + this.Nodes[i].height
                    )
                    {
                        return this.Nodes[i];
                    }
                }
            }
            return null;
        }
        /*************************************************************************************************************************/

        // DIAGRAM VIEWopen new view on diagram
        public void openDiagramView()
        {
            DiagramView diagramview = new DiagramView(main, this);
            diagramview.setDiagram(this);
            this.DiagramViews.Add(diagramview);
            main.DiagramViews.Add(diagramview);
			this.SetTitle();
            diagramview.Show();
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
        /*************************************************************************************************************************/
    }
}
