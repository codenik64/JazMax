using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.WebJob.LeadCommunication
{
    class Program
    {
        static void Main(string[] args)
        {
            LeadComCore com = new LeadComCore();
            com.DoWork();
        }
    }
}
