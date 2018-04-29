﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.PropertyManagement
{
    public class PropertyListingAgentsView
    {
        public int PropertyListingAgentsId { get; set; } // PropertyListingAgentsId (Primary key)
        public int PropertyListingId { get; set; } // PropertyListingId
        public int AgentId { get; set; } // AgentId
        public bool IsActive { get; set; } // IsActive
    }
}
