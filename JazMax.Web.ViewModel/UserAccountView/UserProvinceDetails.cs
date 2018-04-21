using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.UserAccountView
{
    public class UserProvinceDetails
    {
        public int ProvinceId { get; set; }
        public string Pronvince { get; set; }
        [Display(Name = "Personal Assistant")]
        public string PAInfo { get; set; }
        public int PACoreUserId { get; set; }
    }
}
