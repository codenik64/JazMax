using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.ChangeLog
{
    public class EditLogView
    {
        [Display(Name = "User")]
        public string Name { get; set; }
        public string TableName { get; set; }
        public int TableKey { get; set; }
        [Display(Name = "Value Before")]
        public string ValueBefore { get; set; }
        [Display(Name = "Value After")]
        public string ValueAfter { get; set; }
        [Display(Name = "Change Date")]
        public DateTime ChangeDate { get; set; }
        [Display(Name = "Field")]
        public string TableColumn { get; set; }
        public int CoreUserId { get; set; }
    }
}
