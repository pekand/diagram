using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

#if !MONO
using Shell32;
#endif

namespace Diagram
{
    class Os
    {
#if !MONO
        public static string GetShortcutTargetFile(string shortcutFilename)
        {
            string pathOnly = System.IO.Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = System.IO.Path.GetFileName(shortcutFilename);

            Shell shell = new Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return string.Empty;
        }

        public static string GetShortcutIcon(string shortcutFilename)
        {
            string pathOnly = System.IO.Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = System.IO.Path.GetFileName(shortcutFilename);

            Shell shell = new Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                string iconlocation = "";
                link.GetIconLocation(out iconlocation);
                return iconlocation;
            }

            return string.Empty;
        }
#endif

        public static void openPathInSystem(String path)
        {
            if (File.Exists(path))       // OPEN FILE
            {
                try
                {
                    string parent_diectory = new FileInfo(path).Directory.FullName;
                    System.Diagnostics.Process.Start(parent_diectory);
                }
                catch (Exception ex) { Program.log.write(ex.Message); }
            }
            else if (Directory.Exists(path))  // OPEN DIRECTORY
            {
                try
                {
                    System.Diagnostics.Process.Start(path);
                }
                catch (Exception ex) { Program.log.write(ex.Message); }
            }
        }

        public static bool isDiagram(String diagramPath)
        {
            diagramPath = normalizePath(diagramPath);
            if (File.Exists(diagramPath) && Path.GetExtension(diagramPath).ToLower() == ".diagram")
            {
                return true;
            }

            return false;
        }

        public static String escape(String text)
        {
            return text.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        public static void openDiagram(String diagramPath)
        {
            try
            {

                String currentApp = System.Reflection.Assembly.GetExecutingAssembly().Location;
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = currentApp;
                startInfo.Arguments = "\"" + escape(diagramPath) + "\"";
                Program.log.write("diagram: openlink: open diagram: " + currentApp + "\"" + escape(diagramPath) + "\"");
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Program.log.write(ex.Message);
            }
        }

        public static String getFileDirectory(String FileName)
        {
            if (FileName.Trim().Length > 0 && File.Exists(FileName))
            {
                return new FileInfo(FileName).Directory.FullName;
            }
            return null;
        }

        public static void runCommand(String cmd, string workdir = null)
        {
            try
            {

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
#if !MONO
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/c " + "\"" + cmd + "\"";
#else
				startInfo.FileName = "/bin/bash";
                startInfo.Arguments = "-c " + cmd;
#endif
                startInfo.WorkingDirectory = workdir;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                process.StartInfo = startInfo;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                Program.log.write("output: " + output);
                Program.log.write("error: " + error);
            }
            catch (Exception ex)
            {
                Program.log.write("exception: " + ex.Message);
            }
        }

        public static void runCommandAndExit(String cmd)
        {

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
#if MONO
            startInfo.FileName = "/bin/bash";
            startInfo.Arguments = "-c " + cmd;
#else
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + "\"" + cmd + "\"";
#endif

            process.StartInfo = startInfo;
            process.Start();
        }

        public static String getTextFormClipboard()
        {
            DataObject retrievedData = (DataObject)Clipboard.GetDataObject();
            String clipboard = "";
            if (retrievedData != null && retrievedData.GetDataPresent(DataFormats.Text))  // [PASTE] [TEXT] insert text
            {
                clipboard = retrievedData.GetData(DataFormats.Text) as string;
            }

            return clipboard;
        }

        public static void runProcess(String path)
        {
            path = normalizePath(path);
            System.Diagnostics.Process.Start(path);
        }

        public static int fndLineNumber(String fileName, String search)
        {
            int pos = 0;
            string line;
            using (StreamReader file = new StreamReader(fileName))
            {
                while ((line = file.ReadLine()) != null)
                {
                    pos++;
                    if (line.Contains(search))
                    {
                        return pos;
                    }
                }
            }

            return pos;
        }

		public static string makeRelative(String filePath, String currentPath, bool inCurrentDir = true)
		{
#if !MONO
            filePath = filePath.ToLower();
            currentPath = currentPath.ToLower();
#endif
            filePath = filePath.Trim();
			currentPath = currentPath.Trim();

			if (currentPath == "") 
			{
				return filePath;
			} 

			if (!File.Exists(filePath) && !Directory.Exists(filePath)) 
			{
				return filePath;
			}

			filePath = Path.GetFullPath(filePath);

			if (File.Exists(currentPath)) 
			{
				currentPath = Path.GetDirectoryName(currentPath);
			}

			if (!Directory.Exists(currentPath))
			{
				return filePath;
			}

			currentPath = Path.GetFullPath(currentPath);

			Uri pathUri = new Uri(filePath);
			// Folders must end in a slash
			if (!currentPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				currentPath += Path.DirectorySeparatorChar;
			}

			int pos = filePath.ToLower().IndexOf(currentPath.ToLower());
			if( inCurrentDir &&  pos != 0) // skip files outside of currentPath
			{
				return filePath;
			}

			Uri folderUri = new Uri(currentPath);
			return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
		}

        public static string normalizePath(string path)
        {

#if MONO
            return path.Replace("\\","/");
#else
            return path.Replace("/","\\");
#endif

        }

        public static string normalizedFullPath(string path)
        {
            return Path.GetFullPath(normalizePath(path));
        }

        public static bool FileExists(string path)
        {
            return File.Exists(normalizePath(path));
        }

        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(normalizePath(path));
        }

        public static string getCurrentApplicationDirectory()
        {
            String currentApp = System.Reflection.Assembly.GetExecutingAssembly().Location;
            return Os.getFileDirectory(currentApp); 
        }


        public static string toBackslash(string text)
        {
            return text.Replace("\\", "/");
        }

        public static string getSeparator()
        {
            return Path.DirectorySeparatorChar.ToString();
        }

    }
}
