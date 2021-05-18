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

namespace JazMax.Win.LeadCommunication
{
    public partial class LeadCom : ServiceBase
    {
        Thread m_thread = null;
        public LeadCom()
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
            Debug.WriteLine("JazMax Lead Commincation has stopped");
            m_thread.Abort();
        }

        public void ThreadProc()
        {
            int waitTime = 1000; // 1 second
            try
            {
                while (true)
                {
                    JazMax.Core.Leads.Services.LeadCommunication o = new Core.Leads.Services.LeadCommunication();
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
