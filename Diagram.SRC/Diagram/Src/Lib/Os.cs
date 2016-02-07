using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

#if !MONO
using Shell32;
#endif

namespace Diagram
{
    class Os
    {
#if !MONO

        /// <summary>
        /// get path from lnk file in windows  </summary>
        public static string GetShortcutTargetFile(string shortcutFilename)
        {
            string pathOnly = Os.getDirectoryName(shortcutFilename);
            string filenameOnly = Os.getFileName(shortcutFilename);

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

        /// <summary>
        ///get icon from lnk file in windows  </summary>
        public static string GetShortcutIcon(string shortcutFilename)
        {
            string pathOnly = Os.getDirectoryName(shortcutFilename);
            string filenameOnly = Os.getFileName(shortcutFilename);

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

        /// <summary>
        /// open path in system if exist  </summary>
        public static void openPathInSystem(string path)
        {
            if (Os.FileExists(path))       // OPEN FILE
            {
                try
                {
                    string parent_diectory = Os.getFileDirectory(path);
                    System.Diagnostics.Process.Start(parent_diectory);
                }
                catch (Exception ex) { Program.log.write("openPathInSystem open file: error:" + ex.Message); }
            }
            else if (Os.DirectoryExists(path))  // OPEN DIRECTORY
            {
                try
                {
                    System.Diagnostics.Process.Start(path);
                }
                catch (Exception ex) { Program.log.write("openPathInSystem open directory: error:"+ex.Message); }
            }
        }

        /// <summary>
        /// check if diagramPath file path has good extension  </summary>
        public static bool isDiagram(string diagramPath)
        {
            diagramPath = normalizePath(diagramPath);
            if (Os.FileExists(diagramPath) && Path.GetExtension(diagramPath).ToLower() == ".diagram")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// open diagram file in current runing application with system call command </summary>
        public static void openDiagram(string diagramPath)
        {
            try
            {

                string currentApp = System.Reflection.Assembly.GetExecutingAssembly().Location;
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

        /// <summary>
        /// open directory in system</summary>
        public static void openDirectory(string path)
        {
            try
            {
                Process.Start(path);
            }
            catch (Exception ex)
            {
                Program.log.write("open directory: " + path + ": error: " + ex.Message);
            }
        }

        /// <summary>
        /// get parent directory of FileName path </summary>
        public static string getFileDirectory(string FileName)
        {
            if (FileName.Trim().Length > 0 && Os.FileExists(FileName))
            {
                return new FileInfo(FileName).Directory.FullName;
            }
            return null;
        }

        /// <summary>
        /// run command in system and wait for output </summary>
        public static void runCommand(string cmd, string workdir = null)
        {
            try
            {

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

#if !MONO
                string[] parts = Patterns.splitCommand(cmd);
                startInfo.FileName = parts[0];
                startInfo.Arguments = parts[1];

                /*startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/c " + "\"" + cmd + "\"";*/
#else
				startInfo.FileName = "/bin/bash";
				startInfo.Arguments = "-c \"" + cmd + "\"";
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

        /// <summary>
        /// run command in system and discard output </summary>
        public static void runCommandAndExit(string cmd)
        {

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
#if MONO
			startInfo.FileName = "/bin/bash";
			startInfo.Arguments =  "-c \"" + cmd + "\"";
#else
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + "\"" + cmd + "\"";
#endif

            process.StartInfo = startInfo;
            process.Start();
        }

        /// <summary>
        /// get string from clipboard </summary>
        public static string getTextFormClipboard()
        {
            DataObject retrievedData = (DataObject)Clipboard.GetDataObject();
            string clipboard = "";
            if (retrievedData != null && retrievedData.GetDataPresent(DataFormats.Text))  // [PASTE] [TEXT] insert text
            {
                clipboard = retrievedData.GetData(DataFormats.Text) as string;
            }

            return clipboard;
        }

        /// <summary>
        /// run application in current os </summary>
        public static void runProcess(string path)
        {
            path = normalizePath(path);
            System.Diagnostics.Process.Start(path);
        }

        /// <summary>
        /// find line number with first search string occurrence </summary>
        public static int fndLineNumber(string fileName, string search)
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

        /// <summary>
        /// meke filePath relative to currentPath. 
        /// If is set inCurrentDir path is converted to relative only 
        /// if currentPath is parent of filePath</summary>
		public static string makeRelative(string filePath, string currentPath, bool inCurrentDir = true)
		{
            filePath = filePath.Trim();
			currentPath = currentPath.Trim();

			if (currentPath == "") 
			{
				return filePath;
			} 

			if (!Os.FileExists(filePath) && !Os.DirectoryExists(filePath)) 
			{
				return filePath;
			}

			filePath = Os.getFullPath(filePath);

			if (Os.FileExists(currentPath)) 
			{
				currentPath = Os.getDirectoryName(currentPath);
			}

			if (!Os.DirectoryExists(currentPath))
			{
				return filePath;
			}

			currentPath = Os.getFullPath(currentPath);

			Uri pathUri = new Uri(filePath);
			// Folders must end in a slash
			if (!currentPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				currentPath += Path.DirectorySeparatorChar;
			}

			int pos = filePath.ToLower().IndexOf(currentPath.ToLower());
			if( inCurrentDir && pos != 0) // skip files outside of currentPath
			{
				return filePath;
			}

			Uri folderUri = new Uri(currentPath);
			return Uri.UnescapeDataString(
                folderUri.MakeRelativeUri(pathUri)
                .ToString()
                .Replace('/', Path.DirectorySeparatorChar)
            );
		}

        /// <summary>
        /// check if file exist independent on os </summary>
        public static bool FileExists(string path)
        {
            return File.Exists(normalizePath(path));
        }

        /// <summary>
        /// check if directory exist independent on os </summary>
        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(normalizePath(path));
        }

        /// <summary>
        /// check if directory or file exist independent on os </summary>
        public static bool Exists(string path)
        {
            return FileExists(path) || DirectoryExists(path);
        }

        /// <summary>
        /// get current running application executable directory </summary>
        public static string getCurrentApplicationDirectory()
        {
            string currentApp = System.Reflection.Assembly.GetExecutingAssembly().Location;
            return Os.getFileDirectory(currentApp); 
        }

        /// <summary>
        /// add slash before slash and quote </summary>
        public static string escape(string text)
        {
            return text.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        /// <summary>
        /// convert slash dependent on current os </summary>
        public static string normalizePath(string path)
        {

#if MONO
            return path.Replace("\\","/");
#else
            return path.Replace("/","\\");
#endif
        }

        /// <summary>
        /// normalize path and get full path from relative path </summary>
        public static string normalizedFullPath(string path)
        {
            return Path.GetFullPath(normalizePath(path));
        }
        
        /// <summary>
        /// convert win path slash to linux type slash </summary>
        public static string toBackslash(string text)
        {
            return text.Replace("\\", "/");
        }

        /// <summary>
        /// get path separator dependent on os </summary>
        public static string getSeparator()
        {
            return Path.DirectorySeparatorChar.ToString();
        }

        /// <summary>
        /// get file extension</summary>
        public static string getExtension(string file)
        {
            string ext = "";
            if (file != "" && Os.FileExists(file))
            {
                ext = Path.GetExtension(file).ToLower();
            }

            return ext;
        }

        /// <summary>
        /// check if path is directory</summary>
        public static bool isDirectory(string path)
        {
            return Os.DirectoryExists(path);
        }

        /// <summary>
        /// check if path is file</summary>
        public static bool isFile(string path)
        {
            return Os.FileExists(path);
        }

        /// <summary>
        /// Scans a folder and all of its subfolders recursively, and updates the List of files
        /// </summary>
        /// <param name="path">Full path for scened directory</param>
        /// <param name="files">out - file list</param>
        /// <param name="directories">out - directories list</param>
        public static void search(string path, List<string> files, List<string> directories)
        {
            try
            {
                foreach (string f in Directory.GetFiles(path))
                {
                    files.Add(f);
                }

                foreach (string d in Directory.GetDirectories(path))
                {
                    directories.Add(d);
                    search(d, files, directories);
                }

            }
            catch (System.Exception ex)
            {
                Program.log.write(ex.Message);
            }
        }

        /// <summary>
        /// get file name or directory name from path</summary>
        public static string getFileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// get file name or directory name from path</summary>
        public static string getFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// get file name or directory name from path</summary>
        public static string getDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// set current directory</summary>
        public static void setCurrentDirectory(string path)
        {
            Directory.SetCurrentDirectory(path);
        }

        /// <summary>
        /// get full path</summary>
        public static string getFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        /// <summary>
        /// get temporary directory</summary>
        public static string getTempPath()
        {
            return Path.GetTempPath();
        }

        /// <summary>
        /// concat path and subdir</summary>
        public static string combine(string path, string subdir)
        {
            return Path.Combine(path, subdir);
        }

        /// <summary>
        /// create directory</summary>
        public static bool createDirectory(string path)
        {
			try{
				Directory.CreateDirectory(path);

				return true;
			}
			catch (Exception e) 
			{
				Program.log.write("os.createDirectory fail: " + path + ": " + e.ToString());
			}
			return false;
        }

        /// <summary>
        /// write string content to file</summary>
        public static void writeAllText(string path, string data)
        {
            File.WriteAllText(path, data);
        }

        /// <summary>
        /// write string content to file</summary>
        public static string readAllText(string path)
        {
            return File.ReadAllText(path);
        }

        /// <summary>
        /// create empty file</summary>
        public static void createEmptyFile(string path)
        {
            File.Create(path).Dispose();
        }

        /// <summary>
        /// write string content to file</summary>
        public static void writeAllBytes(string path, byte[] data)
        {
            File.WriteAllBytes(path, data);
        }

        /// <summary>
        /// write string content to file</summary>
        public static byte[] readAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        /// <summary>
        /// get file content as string</summary>
        public static string getFileContent(string file)
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(file, Encoding.UTF8))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch (System.IO.IOException ex)
            {
                Program.log.write(ex.Message);
            }

            return null;
        }
    }
}
