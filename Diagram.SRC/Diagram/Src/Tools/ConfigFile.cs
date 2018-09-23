using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace Diagram
{
    public class ConfigFile
    {

        XElement root ;

        public ConfigFile(String RootName)
        {
            this.root = new XElement(RootName);
        }

        public void SaveFile(String FilePath)
        {
            try
            {

                System.IO.StreamWriter file = new System.IO.StreamWriter(FilePath);
                string xml = this.Save();
                file.Write(xml);
                file.Close();

            }
            catch (Exception ex)
            {
                Program.log.Write("write xml to file error: " + ex.Message);
            }
        }

        public string Save()
        {

            try
            {

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

                return sb.ToString();

            }
            catch (Exception ex)
            {
                Program.log.Write("write xml to file error: " + ex.Message);
            }

            return null;

        }

        private void ProcessLoad(XElement element, String path)
        {
            foreach (XElement child in element.Elements())
            {

                string name = child.Name.ToString();

                if (child.Elements().Count() > 0)
                {
                    ProcessLoad(child, path+"/"+ name);
                    continue;
                }


                string value = child.Value;
                this.Set(path + "/" + name, value);

            }
        }

        public void OpenFile(String FilePath)
        {
            if (Os.FileExists(FilePath))
            {

                string xml = Os.GetFileContent(FilePath);

                this.Open(xml);
            }
        }

        public void Open(String xml)
        {
            XmlReaderSettings xws = new XmlReaderSettings
            {
                CheckCharacters = false
            };

            this.Clear();

            try
            {
                using (XmlReader xr = XmlReader.Create(new StringReader(xml), xws))
                {

                    root = XElement.Load(xr);
                }
            }
            catch (Exception ex)
            {
                Program.log.Write("load xml error: " + ex.Message);
            }
        }

        public void Set(String Name, String Value)
        {
            string[] parts = Name.Split('/');

            XElement element = new XElement(parts[parts.Length-1], Value);
            SetElement(Name, element);
        }        

       
        public void SetElement(String Name, XElement Value)
        {
            string[] parts = Name.Split('/');

            XElement currentElement = root;
            XElement child = null;
            long i = 0;
            for (i = 0; i < parts.Length - 1; i++)
            {
                child = currentElement.Element(parts[i]);

                if (child == null)
                {
                    break;
                }

                currentElement = child;
            }

            for (; i < parts.Length - 1; i++)
            {
                XElement partKey = new XElement(parts[i]);
                currentElement.Add(partKey);
                currentElement = partKey;
            }

            XElement node = currentElement.Element(Value.Name);
            if (node != null)
            {
                node.Remove();
            }
           
            currentElement.Add(Value);

        }

        public XElement GetElement(String Name)
        {
            string[] parts = Name.Split('/');

            XElement currentElement = root;
            XElement child = null;

            foreach (String part in parts)
            {
                child = currentElement.Element(part);
                if (child == null)
                {
                    return null;
                }

                currentElement = child;
            }

            return currentElement;
        }


        public string Get(String Name)
        {
            XElement element = GetElement(Name);

            if (element == null)
            {
                return "";
            }

            return element.Value;
        }

        public void Clear()
        {

        }
    }
}
