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

            if (Os.FileExists(this.optionsFilePath))
            {

            }
            else
            {
                this.optionsFilePath = this.getGlobalConfigFilePath();

                if (Os.FileExists(this.optionsFilePath))
                {
                    this.loadConfigFile();
                }
                else
                {
                    if (!Os.DirectoryExists(this.getGlobalConfigFileDirectory()))
                    {
                        Os.createDirectory(this.getGlobalConfigFileDirectory());
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
                string inputJSON = Os.readAllText(this.optionsFilePath);
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
                Os.writeAllText(this.optionsFilePath, outputJSON);
            }
            catch (Exception ex)
            {
                Program.log.write("saveConfigFile: " + ex.Message);
            }
        }

        public String getPortableConfigFilePath()
        {
            return Os.combine(
                Os.getDirectoryName(Application.ExecutablePath),
                this.configFileName
            );

        }

        private string getGlobalConfigFileDirectory()
        {
			string folderPath = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
			string configDirectory = Os.combine(
				folderPath,
                this.configFileDirectory
            );

			return configDirectory;
        }

        private string getGlobalConfigFilePath()
        {
            return Os.combine(
                this.getGlobalConfigFileDirectory(),
                this.configFileName
            );
        }
    }
}
