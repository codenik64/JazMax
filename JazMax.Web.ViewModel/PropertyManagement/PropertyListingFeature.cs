using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.PropertyManagement
{
    public class PropertyListingFeatureView
    {
        public int ProprtyListingFeaturesId { get; set; } // ProprtyListingFeaturesId (Primary key)
        public int PropertyFeatureId { get; set; } // PropertyFeatureId (Primary key)
        public int PropertyListingId { get; set; } // PropertyListingId (Primary key)
        public bool IsFeatureActive { get; set; } // IsFeatureActive (Primary key)

        public string FeatureName { get; set; }
    }
}
