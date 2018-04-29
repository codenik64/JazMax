using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.PropertyManagement
{
    public class PropertyListingDetailView
    {
        public int PropertyListingDetailId { get; set; } // PropertyListingDetailId (Primary key)
        public int PropertyListingId { get; set; } // PropertyListingId (Primary key)
        public decimal? NumberOfBathRooms { get; set; } // NumberOfBathRooms
        public decimal? NumberOfBedrooms { get; set; } // NumberOfBedrooms
        public int? NumberOfGarages { get; set; } // NumberOfGarages
        public int? NumberOfSquareMeters { get; set; } // NumberOfSquareMeters
        public decimal? RatesAndTaxes { get; set; } // RatesAndTaxes
    }
}
