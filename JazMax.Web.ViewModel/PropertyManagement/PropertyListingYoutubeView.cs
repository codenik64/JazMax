using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.PropertyManagement
{
    public class PropertyListingYoutubeView
    {
        public int ProprtyListingYoutubeLibraryId { get; set; } // ProprtyListingYoutubeLibraryId (Primary key)
        public int PrfoprtyListingId { get; set; } // PrfoprtyListingId (Primary key)
        public string YoutubeVideoLink { get; set; } // YoutubeVideoLink (Primary key)
        public bool IsVideoActive { get; set; } // IsVideoActive (Primary key)
    }
}
