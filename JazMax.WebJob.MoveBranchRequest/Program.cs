using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.WebJob.MoveBranchRequest
{
    class Program
    {
        static void Main(string[] args)
        {
            MoveBranchCore o = new MoveBranchCore();
            o.DoWork();
        }
    }
}
