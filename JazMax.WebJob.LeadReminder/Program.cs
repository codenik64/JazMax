using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.WebJob.LeadReminder
{
    class Program
    {
        static void Main(string[] args)
        {
            LeadReminderCore core = new LeadReminderCore();
            core.DoWork();
        }
    }
}
