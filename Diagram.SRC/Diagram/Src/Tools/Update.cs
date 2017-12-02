using System;
using System.ComponentModel;

namespace Diagram
{
    class Update
    {
        public static string updateRepositoryLocation = "https://infinite-diagram.com/install/";
        public static string architecture = "win64/";
        public static string advailableVersionFile = "current-version";

        public static void UpdateApplication(Main main)
        {
            Job.doJob(
                new DoWorkEventHandler(
                    delegate (object o, DoWorkEventArgs args)
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
                                    main.ExitApplication();
                                }
                            }
                        }
                    }
                ),
                new RunWorkerCompletedEventHandler(
                    delegate (object o, RunWorkerCompletedEventArgs args)
                    {
                    }
                )
            );
        }

        public static string GetAvailableVersion()
        {
            return Network.GetWebPage(updateRepositoryLocation + architecture + advailableVersionFile).Trim();
        }

        public static bool CheckCurrentVersion(string availableVersion)
        {
            if (availableVersion.Trim() == "") {
                return false;
            }

            long result = 0;

            try
            {
                string currentApplicationVersion = Os.GetThisAssemblyVersion();
                Version local = new Version(currentApplicationVersion);
                Version remote = new Version(availableVersion);
                result = remote.CompareTo(local);
            }
            catch (Exception e)
            {
                Program.log.Write("CheckCurrentVersion: " + e.Message);
            }

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
    }
}
