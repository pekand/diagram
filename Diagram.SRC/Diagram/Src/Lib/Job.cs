using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    public class Job
    {
        /// <summary>
        /// run task in thread </summary>
        public static void doJob(DoWorkEventHandler doJob = null, RunWorkerCompletedEventHandler afterJob = null)
        {
            try
            {
                BackgroundWorker bw = new BackgroundWorker();

                bw.WorkerReportsProgress = true;

                bw.DoWork += doJob;

                bw.RunWorkerCompleted += afterJob;

                bw.RunWorkerAsync();

            }
            catch (Exception ex)
            {
                Program.log.write("get link name error: " + ex.Message);
            }
        }
    }
}
