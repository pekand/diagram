using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Security;

#if !MONO
using Microsoft.Win32;
#endif

namespace Diagram
{
    /// <summary>    
    /// Processing global options files
    /// Start server
    /// Processing command line arguments.
    /// Create mainform
    /// </summary>
    public class Main
    {

        /*************************************************************************************************************************/
        // OPTONS

        /// <summary>
        /// Global program options</summary>
        public ProgramOptions options = null;

        /// <summary>
        /// managing file with global program options</summary>
        private OptionsFile optionsFile = null;

        /// <summary>
        /// keyboard shorcut mapping</summary>
        public KeyMap keyMap = null;

        /// <summary>
        /// open directori with global configuration</summary>
        public void OpenConfigDir()
        {
            this.optionsFile.ShowDirectoryWithConfiguration();
        }

        /*************************************************************************************************************************/
        // Plugins

        /// <summary>
        /// load plugins</summary>
        public string pluginsDirectoryName = "plugins";
        public Plugins plugins = null;

        /*************************************************************************************************************************/
        // SERVER

        /// <summary>
        /// local messsaging server for communication between running program instances</summary>
        private Server server = null;


        /*************************************************************************************************************************/
        // MAIN APPLICATION

        // command line arguments
        private string[] args = null;

        /// <summary>
        /// form for catching messages from local server</summary>
        public MainForm mainform = null;

        /// <summary>
        /// parse command line arguments and open forms</summary>
        public Main()
        {
            // inicialize program
            options = new ProgramOptions();
            optionsFile = new OptionsFile(options);

            // load external plugins
            plugins = new Plugins();

            // executable location directory
            string pluginsLocalDirectory = Os.Combine(Os.GetCurrentApplicationDirectory(), this.pluginsDirectoryName);
            if (Os.DirectoryExists(pluginsLocalDirectory))
            {
                plugins.LoadPlugins(pluginsLocalDirectory);
            }

            string pluginsGlobalDirectory = Os.Combine(optionsFile.GetFullGlobalConfigFilePath(), this.pluginsDirectoryName);
            if (Os.DirectoryExists(pluginsLocalDirectory))
            {
                plugins.LoadPlugins(pluginsLocalDirectory);
            }

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

#if !MONO
            SystemEvents.PowerModeChanged += OnPowerChange;
#endif

// check if this program instance created server
            if (server.mainProcess)
            {
                this.mainform = new MainForm(this);
            }
        }

        /// <summary>
        /// process comand line arguments</summary>
        public void ParseCommandLineArguments(string[] args) // [PARSE] [COMMAND LINE]
        {

            // options - create new file with given name if not exist
            bool CommandLineCreateIfNotExistFile = false;

            bool ShowCommandLineHelp = false;

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
                if (arg == "-h" || arg == "--help" || arg == "/?")
                {
                    ShowCommandLineHelp = true;
                }
                else if(arg == "-e")
                {
                    CommandLineCreateIfNotExistFile = true;
                }
                else
                {
                    // [COMAND LINE] [OPEN] check if argument is diagram file
                    if (Os.GetExtension(arg).ToLower() == ".diagram")
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
            if (ShowCommandLineHelp)
            {
                String help =
                "diagram -h --help /?  >> show this help\n" +
                "diagram -e {filename} >> create file if not exist\n" +
                "diagram {filepath} {filepath} >> open existing file\n";
                MessageBox.Show(help, "Command line parameters");
            }
            else
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
                            Os.CreateEmptyFile(file);
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
                if (server.mainProcess)
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

            this.optionsFile.SaveConfigFile();
            Application.Exit();
            Application.ExitThread();
            Environment.Exit(0);
        }

        /*************************************************************************************************************************/
        // DIAGRAMS

        /// <summary>
        /// all opened diagrams models</summary>
        private List<Diagram> Diagrams = new List<Diagram>();

        /// <summary>
        /// add diagram to list of all diagrams</summary>
        public void AddDiagram(Diagram diagram)
        {
            this.Diagrams.Add(diagram);
        }

        /// <summary>
        /// remove diagram from list of all diagrams</summary>
        public void RemoveDiagram(Diagram diagram)
        {
            this.Diagrams.Remove(diagram);
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
                if (!server.mainProcess)
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
                    diagram.OpenDiagramView();
                }
            }
            // open existing diagram file
            else
            {
                if (passwordForm != null) // prevent open diagram if another diagram triing open 
                {
                    return;
                }

                if (Os.FileExists(FilePath))
                {
                    FilePath = Os.GetFullPath(FilePath);

                    // if server already exist in system, send him message whitch open diagram file
                    if (!server.mainProcess)
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
                                    Program.log.write("OpenDiagram: diagramView: setFocus");

                                    if (!diagram.DiagramViews[0].Visible)
                                    {
                                        diagram.DiagramViews[0].Show();
                                    }

                                    Program.log.write("bring focus");
                                    Media.BringToFront(diagram.DiagramViews[0]);
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
                                if (diagram.OpenFile(FilePath))
                                {
                                    this.options.AddRecentFile(FilePath);
                                    Diagrams.Add(diagram);
                                    // open diagram view on diagram model
                                    diagram.OpenDiagramView();
                                }
                            }
                        }
                    }
                }
            }
        }

        /*************************************************************************************************************************/
        // VIEWS

        /// <summary>
        /// all opened form views to diagrams models</summary>
        private List<DiagramView> DiagramViews = new List<DiagramView>();

        /// <summary>
        /// add diagram view to list of all views</summary>
        public void AddDiagramView(DiagramView view)
        {
            this.DiagramViews.Add(view);
        }

        /// <summary>
        /// remove diagram view from list of all diagram views</summary>
        public void RemoveDiagramView(DiagramView view)
        {
            this.DiagramViews.Remove(view);
        }

        /// <summary>
        /// hide diagram views except diagramView</summary>
        public void SwitchViews(DiagramView diagramView = null)
        {
            bool someIsHidden = false;
            foreach (DiagramView view in DiagramViews)
            {
                if (!view.Visible)
                {
                    someIsHidden = true;
                    break;
                }
            }

            if (someIsHidden)
            {
                ShowViews();
            }
            else
            {
                HideViews(diagramView);
            }
        }

        /// <summary>
        /// show views if last visible view is closed</summary>
        public void ShowIfIsLastViews(DiagramView diagramView = null)
        {
            bool someIsVisible = false;
            foreach (DiagramView view in DiagramViews)
            {
                if (view.Visible && diagramView != view)
                {
                    someIsVisible = true;
                    break;
                }
            }

            if (!someIsVisible)
            {
                ShowViews();
            }
        }

        /// <summary>
        /// show diagram views</summary>
        public void ShowViews()
        {
            foreach (DiagramView view in DiagramViews)
            {
                view.Show();
            }
        }

        /// <summary>
        /// hide diagram views</summary>
        public void HideViews(DiagramView diagramView = null)
        {
            foreach (DiagramView view in DiagramViews)
            {
                if (view != diagramView) {
                    view.Hide();
                }
            }
        }

        /*************************************************************************************************************************/
        // DIAGRAM EDIT FORMS

        /// <summary>
        /// all opened node edit forms for all diagrams models</summary>
        private List<TextForm> TextWindows = new List<TextForm>();

        /// <summary>
        /// add text form to list of all text forms</summary>
        public void AddTextWindow(TextForm textWindows)
        {
            this.TextWindows.Add(textWindows);
        }

        /// <summary>
        /// remove text form from list of all text forms</summary>
        public void RemoveTextWindow(TextForm textWindows)
        {
            this.TextWindows.Remove(textWindows);
        }

        /*************************************************************************************************************************/
        // PASSWORD FORMS

        /// <summary>
        ///input form for password</summary>
        private PasswordForm passwordForm = null;

        /// <summary>
        /// input form for new password</summary>
        private NewPasswordForm newPasswordForm = null;

        /// <summary>
        /// input form for change old password</summary>
        private ChangePasswordForm changePasswordForm = null;

        /// <summary>
        /// show dialog for password for diagram unlock</summary>
        public string GetPassword(string subtitle = "")
        {
            string password = null;

            if (this.passwordForm == null)
            {
                this.passwordForm = new PasswordForm(this);
            }

            this.passwordForm.Text = Translations.password + " - " + subtitle;
            this.passwordForm.Clear();
            this.passwordForm.ShowDialog();
            if (!this.passwordForm.cancled)
            {
                password = this.passwordForm.GetPassword();
                this.passwordForm.Clear();
            }

            this.passwordForm = null;

            return password;
        }

        /// <summary>
        /// show dialog for new password for diagram</summary>
        public string GetNewPassword()
        {
            string password = null;

            if (this.newPasswordForm == null)
            {
                this.newPasswordForm = new NewPasswordForm(this);
            }

            this.newPasswordForm.Clear();
            this.newPasswordForm.ShowDialog();
            if (!this.newPasswordForm.cancled)
            {
                password = this.newPasswordForm.GetPassword();
                this.newPasswordForm.Clear();
            }

            this.newPasswordForm = null;

            return password;
        }

        /// <summary>
        /// show dialog for change password for diagram</summary>
        public string ChangePassword(SecureString currentPassword)
        {
            string password = null;

            if (this.changePasswordForm == null)
            {
                this.changePasswordForm = new ChangePasswordForm(this);
            }

            this.changePasswordForm.Clear();
            this.changePasswordForm.oldpassword = currentPassword;
            this.changePasswordForm.ShowDialog();
            if (!this.changePasswordForm.cancled)
            {
                password = this.changePasswordForm.GetPassword();
                this.changePasswordForm.Clear();
            }

            this.changePasswordForm = null;

            return password;
        }

        /*************************************************************************************************************************/
        // LOCK DIAGRAM
#if !MONO

        /// <summary>
        /// lock encrypted diagrams if computer go to sleep or hibernation</summary>
        private void OnPowerChange(object s, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    break;
                case PowerModes.Suspend:
                    this.LockDiagrams();
                    break;
            }
        }

        /// <summary>
        /// forgot password if diagram is encrypted</summary>
        public void LockDiagrams()
        {
            foreach (Diagram diagram in Diagrams)
            {
                diagram.LockDiagram();
            }
        }

        /// <summary>
        /// prompt for password if diagram is encrypted</summary>
        public void UnlockDiagrams()
        {
            foreach (Diagram diagram in Diagrams)
            {
                diagram.UnlockDiagram();
            }
        }
#endif
        /*************************************************************************************************************************/
        // ABOUT FORM        

        /// <summary>
        /// about form for display basic informations about application</summary>
        private AboutForm aboutForm = null;

        /// <summary>
        /// show about</summary>
        public void ShowAbout()
        {
            if (this.aboutForm == null)
            {
                this.aboutForm = new AboutForm(this);
            }

            this.aboutForm.ShowDialog();

            this.aboutForm = null;
        }

        /*************************************************************************************************************************/
        // CONSOLE

        /// <summary>
        /// form for display logged messages</summary>
        private Console console = null;

        /// <summary>
        /// show error console</summary>
        public void ShowConsole()
        {
            if (this.console == null)
            {
                this.console = new Console(this);
                this.console.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CloseConsole);
                Program.log.setConsole(this.console);
            }

            this.console.Show();

            this.console = null;
        }

        /// <summary>
        /// clean after error console close</summary>
        private void CloseConsole(object sender, FormClosedEventArgs e)
        {
            Program.log.setConsole(null);
            this.console = null;
        }

    }
}
