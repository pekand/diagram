using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        delegate void SetTextCallback(string text);

        public void addLine(string line)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(addLine);
                this.Invoke(d, new object[] { line });
            }
            else
            {
                this.textBox2.Text += line + "\n";
            }

             
        }

        public  void RunServer(String response,string ip,Int32 port)
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                //Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse(ip);

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop. 
                while (true)
                {
                   this.addLine("Waiting for a connection...");

                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    this.addLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client. 
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        this.addLine("Received: " + data);


                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        this.addLine("Sent: " + data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                this.addLine("SocketException: " + e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        var worker = new BackgroundWorker();
        worker.WorkerSupportsCancellation = true;
        worker.DoWork += (senders, es) =>
        {
            this.RunServer(textBox1.Text, "127.0.0.1", Int32.Parse(this.textBox3.Text));
        };
        worker.RunWorkerAsync();

            
        }
    }
}
