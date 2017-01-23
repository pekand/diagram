using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace Diagram
{
    /// <summary>
    /// message server between running processies
    /// </summary>
    public class Server
    {
        /*************************************************************************************************************************/
        
        public Main main = null; // parent

        public bool mainProcess = false; // is true when server runing in this process, false if server already run in other process

        private volatile bool _shouldStop = false; // signal for server loop to stop
        private TcpListener tcpListener; // server
        private Thread listenThread; // thread for server loop

        /*************************************************************************************************************************/
        // SERVER LOOP

        public Server(Main main)
        {
            this.main = main;

            this.mainProcess = false;

            try
            {
                if (!SendMessage("ping")) // check if server exists
                {
                    Int32 port = main.options.server_default_port;
                    IPAddress localAddr = IPAddress.Parse(main.options.server_default_ip);

                    this.tcpListener = new TcpListener(localAddr, port);
                    this.listenThread = new Thread(new ThreadStart(ListenForClients)); // start thread with server
                    this.listenThread.Start();
					this.mainProcess = true;
                    Program.log.write("Server: start on " + main.options.server_default_ip + ":" + main.options.server_default_port);
                }
                else
                {
                    this.mainProcess = false;
                    Program.log.write("Server: already exist");                    
                }
            }
            catch (Exception ex)
            {
                Program.log.write("Server: "+ex.Message);
            }
        }

        // start server loop
        public void ListenForClients()
        {
            try
            {
                this.tcpListener.Start();

                while (!_shouldStop) // wait for signal to end server
                {
                    //blocks until a client has connected to the server
                    TcpClient client = this.tcpListener.AcceptTcpClient(); // wait for message from client

                    //create a thread to handle communication
                    //with connected client
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm)); // process message from client in thread
                    clientThread.Start(client);
                }

                Program.log.write("Server: close");
            }
            catch (Exception ex)
            {
				Program.log.write("Server: error: " + ex.Message);
            }
        }

        // process message catched from server
        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }
                
                ASCIIEncoding encoder = new ASCIIEncoding();

                // process catchet messages
                this.ParseMessage(
                    encoder.GetString(message, 0, bytesRead)
                );
            }

            tcpClient.Close();
        }

        /*************************************************************************************************************************/
        // MESSAGES

        // send message to server
        public bool SendMessage(String Messsage)
        {
			Program.log.write("Server: SendMessage: "+Messsage);

            try
            {
                TcpClient client = new TcpClient();

                IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(main.options.server_default_ip), main.options.server_default_port);

                client.Connect(serverEndPoint);

                NetworkStream clientStream = client.GetStream();

                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes(Messsage);

                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();
                return true;
            }
            catch (Exception ex)
            {
				Program.log.write("Server: SendMessage("+Messsage+"): error: " + ex.Message);
                return false;
            }
        }

        // parde message from server
        public void ParseMessage(String Messsage)
        {
            // send message
			Program.log.write("Server: ParseMessage: "+Messsage);

            if (Messsage == "ping") // check if server is live
            {
                return;
            }
            else
            if (Messsage == "close")
            {
                main.mainform.Invoke(new Action(() => main.mainform.ExitApplication()));
                return;
            }
            else
            {

                Match match = Regex.Match(Messsage, @"open:(.*)", RegexOptions.IgnoreCase); //d36c6402df
                if (match.Success)
                {
                    string FileName = match.Groups[1].Value;
                    main.mainform.Invoke(new Action(() => main.mainform.OpenDiagram(FileName)));

                    return;
                }

                return;
            }
        }

        // send close message to server
        public void RequestStop()
        {
            _shouldStop = true;
            SendMessage("close");
        }
    }
}
