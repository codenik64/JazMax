using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Test.Leads
{
    class Program
    {
        static void Main(string[] args)
        {
            JazMax.Core.Leads.Logic.LeadHelper helper = new Core.Leads.Logic.LeadHelper(2);

            var a = helper.GetLead();

        }
    }
}
