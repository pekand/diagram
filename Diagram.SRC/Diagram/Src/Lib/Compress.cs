using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Diagram
{
    /// <summary>
    /// directory structure for zip file in directory to string</summary>
    public class EDirectory
    {
        public string name = "";
    }

    /// <summary>
    /// file structure for zip file in directory to string</summary>
    public class EFile
    {
        public string name = "";
        public string data = "";
    }

    /// <summary>
    /// repository for compression related functions</summary>
    public class Compress
    {
        /*************************************************************************************************************************/
        // ZIP STRING

        /// <summary>
        /// gZip utf8 string to base64</summary>
        public static string Zip(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }

                
                return Convert.ToBase64String(mso.ToArray());
            }
        }

        /// <summary>
        /// gUnzip base64 strng to utf8</summary>
        public static string Unzip(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        /*************************************************************************************************************************/
        // COMPRESS DIRECTORY

        /// <summary>
        /// compress directory with files to string</summary>
        public static string compress(string path)
        {
            if (!Os.Exists(path)) {
                return "";
            }

            path = Os.normalizedFullPath(path);

            List<EDirectory> directories = new List<EDirectory>();
            List<EFile> files = new List<EFile>();

            if (Os.isFile(path)) {
                EFile eFile = new EFile();
                eFile.name = Os.getFileName(path);
                eFile.data = Convert.ToBase64String(
                    Os.readAllBytes(path)
                );
                files.Add(eFile);
            }

            if (Os.isDirectory(path))
            {
                List<string> filePaths = new List<string>();
                List<string> directoryPaths = new List<string>();

                Os.search(path, filePaths, directoryPaths);

                int pathLength = path.Length + 1;

                foreach (string dirPath in directoryPaths)
                {
                    EDirectory eDirectory = new EDirectory();
                    eDirectory.name = dirPath.Substring(pathLength);
                    directories.Add(eDirectory);
                }

                foreach (string filePath in filePaths)
                {
                    EFile eFile = new EFile();
                    eFile.name = filePath.Substring(pathLength);
                    eFile.data = Convert.ToBase64String(
                        File.ReadAllBytes(filePath)
                    );
                    files.Add(eFile);
                }
            }

            XElement xRoot = new XElement("archive");
            xRoot.Add(new XElement("version", "1"));

            XElement xDirectories = new XElement("directories");

            foreach (EDirectory directory in directories)
            {
                XElement xDirectory = new XElement("directory");

                xDirectory.Add(
                    new XElement(
                        "name",
                        directory.name
                    )
                );

                xDirectories.Add(xDirectory);
            }

            xRoot.Add(xDirectories);

            XElement xFiles = new XElement("files");

            foreach (EFile file in files) {
                XElement xFile = new XElement("file");

                xFile.Add(
                    new XElement(
                        "name",
                        file.name
                    )
                );

                xFile.Add(
                    new XElement(
                        "data",
                        file.data
                    )
                );
                xFiles.Add(xFile);
            }

            xRoot.Add(xFiles);

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.OmitXmlDeclaration = true;
            xws.CheckCharacters = false;
            xws.Indent = true;

            using (XmlWriter xw = XmlWriter.Create(sb, xws))
            {
                xRoot.WriteTo(xw);
            }

            return Zip(sb.ToString());
        }

        /// <summary>
        /// decompress string with directory structure to path</summary>
        public static void decompress(string compressedData, string destinationPath)
        {
            if (!Os.DirectoryExists(destinationPath))
            {
                return;
            }

            destinationPath = Os.normalizedFullPath(destinationPath);

            string xml = Unzip(compressedData);

            XmlReaderSettings xws = new XmlReaderSettings();
            xws.CheckCharacters = false;

            string version = "";
            List<EDirectory> directories = new List<EDirectory>();
            List<EFile> files = new List<EFile>();

            try
            {
                using (XmlReader xr = XmlReader.Create(new StringReader(xml), xws))
                {

                    XElement xRoot = XElement.Load(xr);
                    if (xRoot.Name.ToString() == "archive") {
                        foreach (XElement xEl in xRoot.Elements())
                        {
                            if (xEl.Name.ToString() == "version")
                            {
                                version = xEl.Value;
                            }

                            if (xEl.Name.ToString() == "directories")
                            {
                                foreach (XElement xDirectory in xEl.Descendants())
                                {
                                    if (xDirectory.Name.ToString() == "directory")
                                    {

                                        string name = "";

                                        foreach (XElement xData in xDirectory.Descendants())
                                        {
                                            if (xData.Name.ToString() == "name")
                                            {
                                                name = xData.Value;
                                            }
                                        }

                                        if (name.Trim() != "")
                                        {
                                            EDirectory eDirectory = new EDirectory();
                                            eDirectory.name = name;
                                            directories.Add(eDirectory);
                                        }
                                    }
                                }
                            }

                            if (xEl.Name.ToString() == "files")
                            {
                                foreach (XElement xFile in xEl.Descendants())
                                {
                                    if (xFile.Name.ToString() == "file")
                                    {
                                        string name = "";
                                        string data = "";

                                        foreach (XElement xData in xFile.Descendants())
                                        {
                                            if (xData.Name.ToString() == "name")
                                            {
                                                name = xData.Value;
                                            }

                                            if (xData.Name.ToString() == "data")
                                            {
                                                data = xData.Value;
                                            }
                                        }

                                        if (name.Trim() != "" && data.Trim() != "")
                                        {
                                            EFile eFile = new EFile();
                                            eFile.name = name;
                                            eFile.data = data;
                                            files.Add(eFile);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.log.write("decompress file xml error: " + ex.Message);
            }

            foreach (EDirectory directory in directories)
            {
                string newDirPath = Os.combine(destinationPath, directory.name);
                if (!Os.Exists(newDirPath))
                {
                    Os.createDirectory(newDirPath);
                }
            }

            foreach (EFile file in files)
            {
                string newFilePath = Os.combine(destinationPath, file.name);
                if (!Os.Exists(newFilePath)) {
                    Os.writeAllBytes(
                        newFilePath,
                        Convert.FromBase64String(
                            file.data
                        )
                    );
                }
            }

            // process dirrectories create to path

            // process files create to path 
        }
    }
}
