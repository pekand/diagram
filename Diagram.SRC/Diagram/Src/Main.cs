using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Diagram
{
    public class Main
    {
        public MainForm mainform = null; // forma na zachytvanie sprav z tcp serveru

        public Parameters parameters = new Parameters();
        public OptionsFile options = new OptionsFile();
        public Translations translations = new Translations(); // class with translations strings
        public Console console = null;

        // ENCRYPTION FORM
        public PasswordForm passwordForm = null;
        public NewPasswordForm newPasswordForm = null;
        public ChangePasswordForm changePasswordForm = null;

        // ABOUT FORM
        public AboutForm aboutForm = null;

        // SERVER
        public Server server = null;

        public List<Diagram> Diagrams = new List<Diagram>();              // zoznam otvorenych diagramov
        public List<DiagramView> DiagramViews = new List<DiagramView>();  // zoznam otvorenych pohladov do diagramou
        public List<TextForm> TextWindows = new List<TextForm>();                   // Zoznam otvorenych editacnych okien

        public void CloseApplication()
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

        //force close application
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

            Application.Exit();
            Application.ExitThread();
            Environment.Exit(0);
        }

        // open empty or new file and view on this file
        public void OpenDiagram(String FilePath = "")
        {
            Program.log.write("Program : OpenDiagram: " + FilePath);

            if (FilePath == "")
            {
                if (server.severExist)
                {
                    server.SendMessage("open:" + FilePath);
                }
                else
                {
                    Diagram diagram = new Diagram(this);
                    Diagrams.Add(diagram);
                    diagram.openDiagramView();
                }
            }
            else
            {
                if (File.Exists(FilePath))
                {
                    FilePath = Path.GetFullPath(FilePath);

                    if (server.severExist)
                    {
                        server.SendMessage("open:" + FilePath);
                    }
                    else
                    {
                        bool alreadyOpen = false;

                        foreach (Diagram diagram in Diagrams)
                        {
                            if (diagram.FileName == FilePath)
                            {
                                //focus
                                if (diagram.DiagramViews.Count() > 0)
                                {
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

                                diagram.OpenFile(FilePath);
                                Diagrams.Add(diagram);

                                diagram.openDiagramView();
                            }
                        }
                    }
                }
            }

        }

        public void GlobalExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            // example throw exception
            // throw new ApplicationException("Exception");
            Exception e = (Exception)args.ExceptionObject;
            Program.log.write("GlobalExceptionHandler: " + e.Message);
            MessageBox.Show("GlobalExceptionHandler: " + e.Message);
        }

        public void ParseCommandLineArguments(string[] args)
        {
            // [PARSE] [COMMAND LINE] prejdenie parametrov z konzoli
            bool CommandLineCreateIfNotExistFile = false;
            List<String> CommandLineOpen = new List<String>();

            String arg = "";
            for (int i = 0; i < args.Count(); i++)
            {
                if (i == 0) //skip application name
                {
                    continue;
                }

                arg = args[i];

                if (arg == "-e")  // [COMAND LINE] [CREATE] vytvori subor ak neexistuje
                {
                    CommandLineCreateIfNotExistFile = true; // create diagram if not exist
                }
                else
                {
                    // [COMAND LINE] [OPEN] skusit ci parameter nieje subor ktoy sa moze otvorit
                    if (Path.GetExtension(arg).ToLower() == ".diagram")
                    {
                        CommandLineOpen.Add(arg);
                    }
                    else
                    {
                        Program.log.write("bed commmand line argument: " + arg);
                    }
                }
            }

            if (CommandLineOpen.Count > 0) // Otvorenie súboru z parametru
            {
                for (int i = 0; i < CommandLineOpen.Count(); i++)
                {
                    string file = CommandLineOpen[i];

                    if (CommandLineCreateIfNotExistFile && !File.Exists(file))
                    {
                        try
                        {
                            File.Create(file).Dispose();
                        }
                        catch (Exception ex)
                        {
                            Program.log.write("create empty diagram file error: " + ex.Message);
                        }
                    }

                    if (File.Exists(file))
                    {
                        OpenDiagram(file);
                    }
                }

                CloseApplication(); //cose application if is not file opened
            }
            else
            {
                OpenDiagram();
            }
        }

        public Main()
        {

            server = new Server(this);


            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalExceptionHandler);

            Program.log.write("Program: Main (version:" + parameters.ApplicationVersion + ")");

#if DEBUG
            Program.log.write("program: debug mode");
#else
				Program.log.write("program: release mode");
#endif

            // load options
            //xxx options.loadOptions();

#if DEBUG
            string[] args = Environment.GetCommandLineArgs();

            // custom debug arguments
            if (args.Length == 1)
            { // comand line arguments testing
                args = new string[] { 
                    System.Reflection.Assembly.GetExecutingAssembly().Location
                    //,"file.diagram"
                };
            }

#else
				string[] args = Environment.GetCommandLineArgs();
#endif

            ParseCommandLineArguments(args);

            if (!server.severExist)
            {
                mainform = new MainForm(this);
            }
        }
    }
}
