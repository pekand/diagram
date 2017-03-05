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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void log(string line)
        {
            this.textBox2.Text += line + "\r\n";
        }

        public  void Connect(String message,string ip,Int32 port)
        {
            try
            {
                // Create a TcpClient. 
                // Note, for this client to work you need to have a TcpServer  
                // connected to the same address as specified by the server, port 
                // combination.
                //Int32 port = 13000;
                
                TcpClient client = new TcpClient(ip, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing. 
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                log("Sent: " + message);

                // Receive the TcpServer.response. 

                /* // Buffer to store the response bytes.
                 data = new Byte[256];

                 // String to store the response ASCII representation.
                 String responseData = String.Empty;

                 // Read the first batch of the TcpServer response bytes.
                 Int32 bytes = stream.Read(data, 0, data.Length);
                 responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                 log("Received: " + responseData);*/

                try
                {
                    // get response from server
                    stream.ReadTimeout = 1000;
                    byte[] resp = new byte[2048];
                    var memStream = new MemoryStream();
                    int bytesread = stream.Read(resp, 0, resp.Length);
                    while (bytesread > 0)
                    {
                        memStream.Write(resp, 0, bytesread);
                        bytesread = stream.Read(resp, 0, resp.Length);
                    }
                    string response = Encoding.UTF8.GetString(memStream.ToArray());
                    log("Server: SendMessage(" + message + "): response: " + response);
                }
                catch (Exception ex)
                {
                    log("Server: SendMessage(" + message + "): no response from server: " + ex.Message);
                }

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                log("ArgumentNullException:: " + e);
            }
            catch (SocketException e)
            {
                log("SocketException: " + e);
            }
        }

        // send message to server
        public bool SendMessage(String Messsage, string ip, Int32 port)
        {
            log("Server: SendMessage: " + Messsage);

            try
            {
                // connect to server
                TcpClient client = new TcpClient();
                //client.SendTimeout = 1000;
                IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                client.Connect(serverEndPoint);
                NetworkStream clientStream = client.GetStream();

                // prepare message for server
                /*ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes(Messsage);

                // send message
                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();

                try
                {
                    // get response from server
                    clientStream.ReadTimeout = 1000;
                    byte[] resp = new byte[2048];
                    var memStream = new MemoryStream();
                    int bytesread = clientStream.Read(resp, 0, resp.Length);
                    while (bytesread > 0)
                    {
                        memStream.Write(resp, 0, bytesread);
                        bytesread = clientStream.Read(resp, 0, resp.Length);
                    }
                    string response = Encoding.UTF8.GetString(memStream.ToArray());
                    log("Server: SendMessage(" + Messsage + "): response: " + response);
                }
                catch (Exception ex)
                {
                    log("Server: SendMessage(" + Messsage + "): no response from server: " + ex.Message);
                }*/


                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(Messsage);

                //---send the text---
                log("Sending : " + Messsage);
                clientStream.Write(bytesToSend, 0, bytesToSend.Length);

                //---read back the text---
                byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                int bytesRead = clientStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                log("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));

                return true;
            }
            catch (Exception ex)
            {
                log("Server: SendMessage(" + Messsage + "): error: " + ex.Message);
            }

            return false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage(textBox1.Text,"127.0.0.1",Int32.Parse(this.textBox3.Text));
        }
    }
}
