using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Test.Lead
{
    class Program
    {
        static void Main(string[] args)
        {
            JazMax.Core.Leads.Services.DataExtractor o = new Core.Leads.Services.DataExtractor();
            o.DoWork();
        }
    }
}
