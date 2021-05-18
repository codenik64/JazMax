
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

namespace JazMax.Win.NewMessenger
{
    public partial class Scheduler : ServiceBase
    {

        Thread ThreadService = null;
        public Scheduler()
        {
            InitializeComponent();
            
        }

        protected override void OnStart(string[] args)
        {
            ThreadService = new Thread(new ThreadStart(ThreadProc));
            ThreadService.Start();

            ServiceLog.CoreLog("Messenger Executed Succesfully");
        }

        protected override void OnStop()
        {
            ServiceLog.CoreLog("Service Aborted");
        }

        public void ThreadProc()
        {
            int waitTime = 1000; // 1 second
            try
            {
                
                do
                {
                    ServiceLog.DoWork();
                    Thread.Sleep(waitTime);
                    ServiceLog.CoreLog("Service is Doing Work");
                }
                while (true);

            }
            catch (ThreadAbortException e)
            {
                Debug.WriteLine(e.Message);
                ServiceLog.CoreLog(e.Message);
            }
        }
    }
}
