using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JazMax.Win.LeadCore
{
    public partial class LeadCore : ServiceBase
    {
        Thread m_thread = null;
        public LeadCore()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            m_thread = new Thread(new ThreadStart(ThreadProc));
            m_thread.Start();
        }

        protected override void OnStop()
        {
            Debug.WriteLine("JazMax Lead Core Data Extractor Has Stopped");
            m_thread.Abort();
        }

        public void ThreadProc()
        {
            int waitTime = 1000; // 1 second
            try
            {
                while (true)
                {
                    JazMax.Core.Leads.Services.DataExtractor o = new Core.Leads.Services.DataExtractor();
                    o.DoWork();
                    Thread.Sleep(waitTime);
                }
            }
            catch (ThreadAbortException e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
