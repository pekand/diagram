using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    class Update
    {
        public static string updateRepositoryLocation = "https://infinite-diagram.com/install/";
        public static string architecture = "win64/";
        public static string advailableVersionFile = "current-version";

        public static bool UpdateApplication()
        {
            string availableVersion = Update.GetAvailableVersion();

            if (Update.CheckCurrentVersion(availableVersion))
            {
                // show dialog
                //run update
                UpdateForm updateForm = new UpdateForm();
                updateForm.ShowDialog();

                if (updateForm.CanUpdate())
                {
                    string installerPath = Update.downloadUpdate(availableVersion);
                    if (installerPath != null)
                    {
                        Os.RunCommandAndExit(installerPath, "/SILENT");
                        return true;
                    }
                }
            }

            return false;
        }

        public static string GetAvailableVersion()
        {
            return Network.GetWebPage(updateRepositoryLocation + architecture + advailableVersionFile).Trim();
        }

        public static bool CheckCurrentVersion(string availableVersion)
        {
            string currentApplicationVersion = Program.GetVersion();

            var local = new Version(currentApplicationVersion);
            var remote = new Version(availableVersion);

            var result = remote.CompareTo(local);
            if (result > 0) {
                return true;
            }

            return false;
        }

        public static string downloadUpdate(string availableVersion)
        {

            string setupFileName = "infinite-diagram-"+availableVersion+".exe";
            string updateTemporaryDirectory = Os.Combine(Os.GetTempPath(),"infinite-diagram-update");

            Os.CreateDirectory(updateTemporaryDirectory);

            string installerPath = Os.Combine(updateTemporaryDirectory, setupFileName);

            string installerUrl = updateRepositoryLocation + architecture + availableVersion + "/" + setupFileName;

            if (Network.DownloadFile(installerUrl, installerPath))
            {
                return installerPath;
            }

            return null;
        }

        public static void updateApplication(string advailableVersion)
        {

        }
    }
}
