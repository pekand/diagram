using System;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

/*
    class OptionsFile
        OptionsFile()
        loadConfigFile()
        saveConfigFile()
        getPortableConfigFilePath()
        getGlobalConfigFileDirectory()
        getGlobalConfigFilePath()
        openConfigDir()
        
*/

namespace Diagram
{
    /// <summary>    
    /// </summary>
    public class OptionsFile
    {
        /*************************************************************************************************************************/

        public String configFileDirectory = "Diagram"; // name of directory for save global configuration file

#if DEBUG
        // global configuration file name in debug mode
        public String configFileName = "diagram.debug.json";
#else
        // global configuration file name
        public String configFileName = "diagram.json";
#endif

        public String optionsFilePath = ""; // full path to global options json file

        public ProgramOptions parameters = null;

        /*************************************************************************************************************************/

        /// <summary>
        /// load global config file from portable file configuration or global file configuration
        /// </summary>
        /// <param name="parameters">reference to parameter object</param>
        public OptionsFile(ProgramOptions parameters)
        {
            this.parameters = parameters;

            this.optionsFilePath = this.getPortableConfigFilePath();

            // use global if portable version not exist
            if (!Os.FileExists(this.optionsFilePath))
            {
                this.optionsFilePath = this.getFullGlobalConfigFilePath();
            }

            if (Os.FileExists(this.optionsFilePath))
            {
                this.loadConfigFile();
            }
            else
            {
                // create global config directory if not exist
                if (!Os.DirectoryExists(this.getGlobalConfigFileDirectory()))
                {
                    Os.createDirectory(this.getGlobalConfigFileDirectory());
                }

                this.saveConfigFile();
            }
        }


        /// <summary>
        /// load global config file from json file</summary>
        private void loadConfigFile()
        {
            try
            {
                Program.log.write("loadConfigFile: path:" + this.optionsFilePath);
                string inputJSON = Os.readAllText(this.optionsFilePath);
                this.parameters.setParams(JsonConvert.DeserializeObject<ProgramOptions>(inputJSON));
            }
            catch (Exception ex)
            {
                Program.log.write("loadConfigFile: " + ex.Message);
            }
        }

        /// <summary>
        /// save global config file as json</summary>
        public void saveConfigFile()
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

        /*************************************************************************************************************************/

        /// <summary>
        /// get config file directory when diagram app is used in portable mode</summary> 
        public String getPortableConfigFilePath()
        {
            return Os.combine(
                Os.getDirectoryName(Application.ExecutablePath),
                this.configFileName
            );
        }

        /// <summary>
        /// get config file directory</summary> 
        private string getGlobalConfigFileDirectory()
        {
			string folderPath = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
			string configDirectory = Os.combine(
				folderPath,
                this.configFileDirectory
            );

			return configDirectory;
        }

        /// <summary>
        /// get full config file path</summary> 
        private string getFullGlobalConfigFilePath()
        {
            return Os.combine(
                this.getGlobalConfigFileDirectory(),
                this.configFileName
            );
        }

        /// <summary>
        /// open directory with configuration file</summary> 
        public void showDirectoryWithConfiguration()
        {
            Os.showDirectoryInExternalApplication(Os.getDirectoryName(optionsFilePath));
        }
    }
}
