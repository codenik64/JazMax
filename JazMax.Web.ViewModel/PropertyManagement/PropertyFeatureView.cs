using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.PropertyManagement
{
    public class PropertyFeatureView
    {
        public int PropertyFeatureId { get; set; } // PropertyFeatureId (Primary key)
        public string FeatureName { get; set; } // FeatureName (Primary key) (length: 255)
        public bool IsFeatureActive { get; set; } // IsFeatureActive (Primary key)
    }
}
