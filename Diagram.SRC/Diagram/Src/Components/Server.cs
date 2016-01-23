using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace Diagram
{
    public class Server
    {

        // parent
        public Main main = null;

        public bool serverAlreadyExist = false;
		public bool serverCurrent = false; // is true when server runing in this process, false if server already run in other process

        private volatile bool _shouldStop = false;
        private TcpListener tcpListener;
        private Thread listenThread;

        public Server(Main main)
        {
            this.main = main;

            try
            {
                if (!SendMessage("ping"))
                {
                    Int32 port = main.options.server_default_port;
                    IPAddress localAddr = IPAddress.Parse(main.options.server_default_ip);

                    this.tcpListener = new TcpListener(localAddr, port);
                    this.listenThread = new Thread(new ThreadStart(ListenForClients));
                    this.listenThread.Start();
					this.serverCurrent = true;
                    Program.log.write("Server: start on " + main.options.server_default_ip + ":" + main.options.server_default_port);
                }
                else
                {
					Program.log.write("Server: already exist");
                    serverAlreadyExist = true;
                }
            }
            catch (Exception ex)
            {
                Program.log.write("Server: "+ex.Message);
            }
        }

        public void ListenForClients()
        {
            try
            {
                this.tcpListener.Start();

                while (!_shouldStop)
                {
                    //blocks until a client has connected to the server
                    TcpClient client = this.tcpListener.AcceptTcpClient();

                    //create a thread to handle communication
                    //with connected client
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    clientThread.Start(client);
                }

                Program.log.write("Server: close");
            }
            catch (Exception ex)
            {
				Program.log.write("Server: error: " + ex.Message);
            }
        }

        public void RequestStop()
        {
           _shouldStop = true;
           SendMessage("close");
        }

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

                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();
                string result = this.ParseMessage(encoder.GetString(message, 0, bytesRead));
                try
                {
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(result);
                    clientStream.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    Program.log.write("Server: HandleClientComm: client refuse connecton: error: " + ex.Message);
                }
            }

            tcpClient.Close();
        }

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

        public string ParseMessage(String Messsage)
        {
            // send message
			Program.log.write("Server: ParseMessage: "+Messsage);

            if (Messsage == "ping") // check if server is live
            {
                return "ping";
            }
            else
            if (Messsage == "close")
            {
                main.mainform.Invoke(new Action(() => main.mainform.ExitApplication()));
                return "close";
            }
            else
            {

                Match match = Regex.Match(Messsage, @"open:(.*)", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    string FileName = match.Groups[1].Value;
                    main.mainform.Invoke(new Action(() => main.mainform.OpenDiagram(FileName)));

                    return "ok";
                }

                return "error:bed request";
            }
        }
    }
}
