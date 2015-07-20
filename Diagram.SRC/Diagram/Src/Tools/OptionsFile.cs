using System;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace Diagram
{
    public class OptionsFile
    {

        public String configFileDirectory = "Diagram"; // meno adresaru pre ukladanie konfiguracie
        public String configFileName = "diagram.cfg";  

        public String diagramFileExtension = "";

        public String optionsFilePath = "";
        public String configUserDirectory = ""; 


        public OptionsFile()
        {
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);

            if (File.Exists(appPath + configFileName))
            {
            }
        }

        public void saveOptionsFile()
        {
        }

        public void loadOptions()
        {
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            String configInLocalDirectoryPath = this.optionsFilePath = Path.Combine(appPath, configFileName);
            string localPath = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)).FullName;
            configUserDirectory = Path.Combine(localPath, configFileDirectory);
            String configUserDirectoryPath = Path.Combine(configUserDirectory, configFileName);

            if (File.Exists(configInLocalDirectoryPath))
            {
                this.optionsFilePath = Path.Combine(appPath, configFileName);
            }
            else 
            { 
                //check ci je v standardnom adresari
                try
                {
                    getOptions();

                }
                catch (Exception ex)
                {
                    Program.log.write(ex.Message);
                }
            }
            openOptionsFile(this.optionsFilePath);
        }

        public void openOptionsFile(String FileName)
        {
            if (File.Exists(FileName)) 
            {
                try
                {
                    string xml;
                    using (StreamReader streamReader = new StreamReader(FileName, Encoding.UTF8))
                    {
                        xml = streamReader.ReadToEnd();
                    }
                    this.setOptions(xml);

                }
                catch (System.IO.IOException ex)
                {
                    Program.log.write(ex.Message);
                }
            }
        }

        public void saveOptionsFile(String FileName)
        {
            try
            {
                getOptions();

            }
            catch (Exception ex)
            {
                Program.log.write(ex.Message);
            }
        }

        public void setOptions(String xmlOptions)
        {

            XmlReaderSettings xws = new XmlReaderSettings();
            xws.CheckCharacters = false;

            try
            {
                using (XmlReader xr = XmlReader.Create(new StringReader(xmlOptions), xws))
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
                                        /*if (el.Name.ToString() == "shiftx")
                                        {
                                            this.shiftx = Int32.Parse(el.Value);
                                        }*/

                                    }
                                    catch (Exception ex)
                                    {
                                        Program.log.write(ex.Message);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.log.write(ex.Message);
            }
        }


        public String getOptions()
        {
            XElement root = new XElement("diagram");
            try
            {
                // Options
                XElement option = new XElement("option");
                //option.Add(new XElement("shiftx", this.shiftx));

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
            catch (Exception e)
            {
                Program.log.write(e.Message);
            }


            return "";
        }

       
    }
}
