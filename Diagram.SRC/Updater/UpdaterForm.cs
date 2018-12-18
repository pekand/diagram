using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
    public partial class UpdaterForm : Form
    {
        // diagram mutex identificator
#if DEBUG
        private const string uid = "1b63a17a-3f78-428b-86d1-10e598a9f089";
#else
        private const string uid = "c3b82368-a7ff-4b23-a42d-19eecc9210df";
#endif

        string workspace = null;

        bool DownloadFinish = false;

        public UpdaterForm()
        {
            InitializeComponent();

            //get directory for downlod
            workspace = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(workspace);

            

            startDownload();
        }

        private void UpdaterForm_Load(object sender, EventArgs e)
        {


        }

        private void startDownload()
        {
            Thread thread = new Thread(() => {
                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                client.DownloadFileAsync(new Uri("https://dummyimage.com/600x400/000/fff"), Path.Combine(workspace, "out.txt"));
            });
            thread.Start();
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;                
                DownloadProgressBar.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate {
                DownloadFinish = true;
                ActionButton.Enabled = false;                
                if (CheckifDiagramRunning())
                {
                    ActionButton.Enabled = true;
                    ActionButton.Text = "Continue";
                } 
            });
        }

        bool CheckifDiagramRunning() {
            System.Threading.Mutex mutex = null;
            try
            {
                mutex = System.Threading.Mutex.OpenExisting(uid);                
                mutex.WaitOne();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void ActionButton_Click(object sender, EventArgs e)
        {
            if (!DownloadFinish) {
                this.Close();
            }
        }

        private void UpdaterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Directory.Delete(workspace, true);
        }
    }
}
