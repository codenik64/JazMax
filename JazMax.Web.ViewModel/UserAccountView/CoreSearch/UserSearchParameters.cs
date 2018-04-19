using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.UserAccountView.CoreSearch
{
    public class UserSearchParameters
    {
        public int CoreUserId { get; set; }
        public string byEmail { get; set; }
        public string byFirstName { get; set; }
        public string byLastName { get; set; }
        public string byUserType { get; set; }
    }
}
