using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.UserAccountView
{
    public class BranchDetailsView
    {
        public CoreBranchView CoreBranchView { get; set; }
        public List<AgentDetailsView> AgentDetailsView { get; set; }
    }
}
