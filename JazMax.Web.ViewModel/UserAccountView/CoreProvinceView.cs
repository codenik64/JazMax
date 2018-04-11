using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.UserAccountView
{
    public class CoreProvinceView
    {
        public int ProvinceId { get; set; }
        [DisplayName("Province")]
        public string ProvinceName { get; set; }

        [DisplayName("PA")]
        public string PAName { get; set; }


        public Nullable<bool> IsAssigned { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
