﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    class Job
        #thread
        doJob()
*/

namespace Diagram
{
    /// <summary>
    /// repository for thread related functions</summary>
    public class Job
    {
        /// <summary>
        /// run task in thread </summary>
        /// <example> 
        /// This of use doJob method
        /// <code>
        /// Job.doJob(
        ///    new DoWorkEventHandler(
        ///        delegate (object o, DoWorkEventArgs args)
        ///        {
        ///            // run in new thread
        ///        }
        ///    ),
        ///    new RunWorkerCompletedEventHandler(
        ///        delegate (object o, RunWorkerCompletedEventArgs args)
        ///        {
        ///            // complete
        ///        }
        ///    )
        /// );
        /// </code>
        /// </example>
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
