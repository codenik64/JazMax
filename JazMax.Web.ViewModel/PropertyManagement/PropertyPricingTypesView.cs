using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.PropertyManagement
{
    public class PropertyPricingTypesView
    {
        public int PropertyListingPricingTypeId { get; set; } // PropertyListingPricingTypeId (Primary key)
        public string TypeName { get; set; } // TypeName (Primary key) (length: 200)
        public bool IsActive { get; set; } // IsActive (Primary key)
    }
}
