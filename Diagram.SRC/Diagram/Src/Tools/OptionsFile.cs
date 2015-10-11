using System;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Diagram
{
    public class OptionsFile
    {

        public String configFileDirectory = "Diagram"; // meno adresaru pre ukladanie konfiguracie

#if DEBUG
        public String configFileName = "diagram.debug.json";
#else
        public String configFileName = "diagram.json";  
#endif


        public String optionsFilePath = "";

        ProgramOptions parameters = null;

        public OptionsFile(ProgramOptions parameters)
        {
            this.parameters = parameters;

            this.optionsFilePath = this.getPortableConfigFilePath();

            if (File.Exists(this.optionsFilePath))
            {

            }
            else 
            {
                this.optionsFilePath = this.getGlobalConfigFilePath();

                if (File.Exists(this.optionsFilePath))
                {
                    this.loadConfigFile();
                }
                else
                {
                    if (!Directory.Exists(this.getGlobalConfigFileDirectory()))
                    {
                        Directory.CreateDirectory(this.getGlobalConfigFileDirectory());
                    }

                    this.saveConfigFile();
                }
            }


        }

        private void loadConfigFile()
        {
            try
            {
                Program.log.write("loadConfigFile: path:" + this.optionsFilePath);
                string inputJSON = File.ReadAllText(this.optionsFilePath);
                this.parameters = JsonConvert.DeserializeObject<ProgramOptions>(inputJSON);
            }
            catch (Exception ex)
            {
                Program.log.write("loadConfigFile: " + ex.Message);
            }
        }

        private void saveConfigFile()
        {
            try
            {
                string outputJSON = JsonConvert.SerializeObject(this.parameters);
                File.WriteAllText(this.optionsFilePath, outputJSON);
            }
            catch (Exception ex)
            {
                Program.log.write("saveConfigFile: " + ex.Message);
            }
        }

        public String getPortableConfigFilePath() 
        {
            return Path.Combine(
                Path.GetDirectoryName(Application.ExecutablePath),
                this.configFileName
            );

        }

        private string getGlobalConfigFileDirectory()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                this.configFileDirectory
            );
        }

        private string getGlobalConfigFilePath()
        {
            return Path.Combine(
                this.getGlobalConfigFileDirectory(),
                this.configFileName
            );
        }
    }
}
