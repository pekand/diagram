using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Diagram
{
    /// <summary>
    /// Processing command line arguments. </summary>
    public class Main
    {

        // command line arguments
        string[] args = null;

        /// <summary>
        /// form for catching messages from local server</summary>
        public MainForm mainform = null;

        /// <summary>
        /// Global program options</summary>
        public ProgramOptions options = null;

        /// <summary>
        /// managing file with global program options</summary>
        public OptionsFile optionsFile = null;

        /// <summary>
        /// program translation strings</summary>
        public Translations translations = null;

        /// <summary>
        /// keyboard shorcut mapping</summary>
        public KeyMap keyMap = null;

        /// <summary>
        /// form for display logged messages</summary>
        public Console console = null;

        // ENCRYPTION FORM

        /// <summary>
        ///input form for password</summary>
        public PasswordForm passwordForm = null;

        /// <summary>
        /// input form for new password</summary>
        public NewPasswordForm newPasswordForm = null;

        /// <summary>
        /// input form for change old password</summary>
        public ChangePasswordForm changePasswordForm = null;

        /// <summary>
        /// about form for display basic informations about application</summary>
        public AboutForm aboutForm = null;

        /// <summary>
        /// local messsaging server for communication between running program instances</summary>
        public Server server = null;

        /// <summary>
        /// all opened diagrams models</summary>
        public List<Diagram> Diagrams = new List<Diagram>();

        /// <summary>
        /// all opened form views to diagrams models</summary>
        public List<DiagramView> DiagramViews = new List<DiagramView>();

        /// <summary>
        /// all opened node edit forms for all diagrams models</summary>
        public List<TextForm> TextWindows = new List<TextForm>();

        /// <summary>
        /// close application if not diagram view or node edit form is open </summary>
        public void CloseEmptyApplication()
        {
            Program.log.write("Program : CloseApplication");

            bool canclose = true;

            if (Diagrams.Count > 0 || DiagramViews.Count > 0 || TextWindows.Count > 0)
            {
                canclose = false;
            }

            if (canclose)
            {
                if (server.serverCurrent)
                {
                    server.RequestStop();
                }
                else
                {
                    ExitApplication();
                }
            }
        }


        /// <summary>
        /// force close application</summary>
        public void ExitApplication()
        {
            Program.log.write("Program : ExitApplication");

            if (mainform != null)
            {
                mainform.Close();
            }

            if (passwordForm != null)
            {
                passwordForm.Close();
            }

            if (newPasswordForm != null)
            {
                newPasswordForm.Close();
            }

            if (changePasswordForm != null)
            {
                changePasswordForm.Close();
            }

            if (console != null)
            {
                console.Close();
            }

            this.optionsFile.saveConfigFile();
            Application.Exit();
            Application.ExitThread();
            Environment.Exit(0);
        }

        /// <summary>
        /// open existing diagram or create new empty diagram
        /// Create diagram model and then open diagram view on this model</summary>
        public void OpenDiagram(String FilePath = "")
        {
            Program.log.write("Program : OpenDiagram: " + FilePath);

            // open new empty diagram
            if (FilePath == "")
            {
                // if server already exist in system, send him message whitch open empty diagram
                if (server.serverAlreadyExist)
                {
                    server.SendMessage("open:");
                }
                // open diagram in current program instance
                else
                {
                    // create new model
                    Diagram diagram = new Diagram(this);
                    Diagrams.Add(diagram);
                    // open diagram view on diagram model
                    diagram.openDiagramView();
                }
            }
            // open existing diagram file
            else
            {
                if (Os.FileExists(FilePath))
                {
                    FilePath = Os.getFullPath(FilePath);

                    // if server already exist in system, send him message whitch open diagram file
                    if (server.serverAlreadyExist)
                    {
                        server.SendMessage("open:" + FilePath);
                    }
                    // open diagram in current program instance
                    else
                    {
                        // check if file is already opened in current instance
                        bool alreadyOpen = false;

                        foreach (Diagram diagram in Diagrams)
                        {
                            if (diagram.FileName == FilePath)
                            {
                                // focus
                                if (diagram.DiagramViews.Count() > 0)
                                {
                                    Program.log.write("window get focus");
                                    diagram.DiagramViews[0].setFocus();
                                }
                                alreadyOpen = true;
                                break;
                            }
                        }

                        if (!alreadyOpen)
                        {
                            Diagram diagram = new Diagram(this);
                            lock (diagram)
                            {
                                // create new model
                                if (diagram.OpenFile(FilePath)) {
                                    Diagrams.Add(diagram);
                                    // open diagram view on diagram model
                                    diagram.openDiagramView();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// process comand line arguments</summary>
        public void ParseCommandLineArguments(string[] args) // [PARSE] [COMMAND LINE]
        {

            // options - create new file with given name if not exist
            bool CommandLineCreateIfNotExistFile = false;

            // list of diagram files names for open
            List<String> CommandLineOpen = new List<String>();

            String arg = "";
            for (int i = 0; i < args.Count(); i++)
            {

                //skip application name
                if (i == 0)
                {
                    continue;
                }

                // current processing argument
                arg = args[i];

                // [COMAND LINE] [CREATE]  oprions create new file with given name if not exist
                if (arg == "-e")
                {
                    CommandLineCreateIfNotExistFile = true;
                }
                else
                {
                    // [COMAND LINE] [OPEN] check if argument is diagram file
                    if (Os.getExtension(arg).ToLower() == ".diagram")
                    {
                        CommandLineOpen.Add(arg);
                    }
                    else
                    {
                        Program.log.write("bed commmand line argument: " + arg);
                    }
                }
            }

            // open diagram given as arguments
            if (CommandLineOpen.Count > 0)
            {
                for (int i = 0; i < CommandLineOpen.Count(); i++)
                {
                    string file = CommandLineOpen[i];

                    // tray create diagram file if command line option is set
                    if (CommandLineCreateIfNotExistFile && !Os.FileExists(file))
                    {
                        try
                        {
                            Os.createEmptyFile(file);
                        }
                        catch (Exception ex)
                        {
                            Program.log.write("create empty diagram file error: " + ex.Message);
                        }
                    }

                    if (Os.FileExists(file))
                    {
                        this.OpenDiagram(file);
                    }
                }

                // cose application if is not diagram model opened
                this.CloseEmptyApplication();
            }
            else
            {
                //open empty diagram
                this.OpenDiagram();
            }
        }

        /// <summary>
        /// parse command line arguments and open forms</summary>
        public Main()
        {
            // inicialize program
            translations = new Translations();
            options = new ProgramOptions();
            optionsFile = new OptionsFile(options);

            // create local server for comunication between local instances
            server = new Server(this);

            Program.log.write("Program: Main");

#if DEBUG
            Program.log.write("program: debug mode");
#else
			Program.log.write("program: release mode");
#endif

            // TODO: Load global options file and save it when application is closed
            // load options
            // options.loadOptions();


            // get command line arguments
            this.args = Environment.GetCommandLineArgs();
#if DEBUG
            // custom debug arguments
			if (this.args.Length == 1 && Os.FileExists("test.diagram")) // if not argument is added from system ad some testing arguments
            {
                // comand line arguments testing
                this.args = new string[] {
                    System.Reflection.Assembly.GetExecutingAssembly().Location
                    ,"test.diagram"
                };
            }
#endif
            // process comand line arguments
            this.ParseCommandLineArguments(this.args);

            // check if this program instance created server
            if (!server.serverAlreadyExist)
            {
                this.mainform = new MainForm(this);
            }
        }
    }
}
