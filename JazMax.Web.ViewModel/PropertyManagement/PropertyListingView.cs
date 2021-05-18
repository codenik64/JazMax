using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.PropertyManagement
{
    public class PropertyListingView
    {
        public int PropertyListingId { get; set; } // PropertyListingId (Primary key)
        public int PropertyTypeId { get; set; } // PropertyTypeId (Primary key)
        public int BranchId { get; set; } // BranchId (Primary key)
        public int ProvinceId { get; set; } // ProvinceId (Primary key)
        public string FriendlyName { get; set; } // FriendlyName (Primary key) (length: 255)
        public decimal Price { get; set; } // Price (Primary key)
        public System.DateTime ListingDate { get; set; } // ListingDate (Primary key)
        public System.DateTime LastUpdate { get; set; } // LastUpdate (Primary key)
        public string ProprtyDesciption { get; set; } // ProprtyDesciption (Primary key)
        public bool IsListingActive { get; set; } // IsListingActive (Primary key)
        public int PropertyListingPricingTypeId { get; set; }

        public string BranchName { get; set; }
        public string ProvinceName { get; set; }
        public string PropertyPriceTypeName { get; set; }
        public string PropertyTypeName { get; set; }
    }
}
