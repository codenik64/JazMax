using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.WebJob
{
    public class JobModel
    {
        public int JobId { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public string RecordsAffected { get; set; }
    }
}
