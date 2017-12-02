using System;

namespace Diagram
{
    /// <summary>    
    /// </summary>
    public class OptionsFile //UID8285897740
    {
        /*************************************************************************************************************************/

        public String configFileDirectory = "Diagram"; // name of directory for save global configuration file

#if DEBUG
        // global configuration file name in debug mode
        public String configFileName = "diagram.debug.xml";
#else
        // global configuration file name
        public String configFileName = "diagram.xml";
#endif

        // Example: C:\Users\user_name\AppData\Roaming\Diagram\diagram.json
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

            // use local config file
            this.optionsFilePath = Os.Combine(Os.GetCurrentApplicationDirectory(), this.configFileName);

            // use global config file if local version not exist
            if (!Os.FileExists(this.optionsFilePath))
            {
                this.optionsFilePath = Os.Combine(
                    this.GetGlobalConfigDirectory(),
                    this.configFileName
                );
            }

            // open config file if exist
            if (Os.FileExists(this.optionsFilePath))
            {
                this.LoadConfigFile();
            }
            else
            {
                string globalConfigDirectory = Os.Combine(
                    Os.GetApplicationsDirectory(),
                    this.configFileDirectory
                );

                // create global config directory if not exist
                if (!Os.DirectoryExists(globalConfigDirectory))
                {
                    Os.CreateDirectory(globalConfigDirectory);
                }

                // if config file dosn't exist create one with default values
                this.SaveConfigFile();
            }
        }

        /// <summary>
        /// load global config file from json file</summary>
        private void LoadConfigFile()
        {
            try
            {
                Program.log.Write("loadConfigFile: path:" + this.optionsFilePath);
                string inputJSON = Os.ReadAllText(this.optionsFilePath);

                ConfigFile configFile = new ConfigFile("configuration");
                configFile.OpenFile(this.optionsFilePath);

                this.parameters.LoadParams(configFile);
            }
            catch (Exception ex)
            {
                Program.log.Write("loadConfigFile: " + ex.Message);
            }
        }

        /// <summary>
        /// save global config file as json</summary>
        public void SaveConfigFile()
        {
            try
            {
                ConfigFile configFile = new ConfigFile("configuration");
                this.parameters.SaveParams(configFile);               
                configFile.SaveFile(this.optionsFilePath);
            }
            catch (Exception ex)
            {
                Program.log.Write("saveConfigFile: " + ex.Message);
            }
        }

        /*************************************************************************************************************************/

        /// <summary>
        /// open directory with configuration file</summary> 
        public void ShowDirectoryWithConfiguration()
        {
            Os.ShowDirectoryInExternalApplication(Os.GetDirectoryName(optionsFilePath));
        }

        /// <summary>
        /// open directory with configuration file</summary> 
        public string  GetGlobalConfigDirectory()
        {
            return Os.Combine(
                Os.GetApplicationsDirectory(),
                this.configFileDirectory
            );
        }
    }
}
