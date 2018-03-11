using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.UserAccountView
{
    public class CoreUserTypeView
    {
        public int CoreUserTypeId { get; set; }

        [DisplayName("User Group Name")]
        public string UserTypeName { get; set; }
        public string UserRoleId { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
