using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.PropertyManagement.CaptureListing
{
    public class NewListingView
    {
        public PropertyListingView PropertyListingView { get; set; }
        public PropertyListingAgentsView PropertyListingAgentsView { get; set; }
        public PropertyListingDetailView PropertyListingDetailView { get; set; }
        public PropertyListingYoutubeView PropertyListingYoutubeView { get; set; }
        public PropertyListingFeatureView PropertyListingFeatureView { get; set; }
        public PropertyImagesView PropertyImagesView { get; set; }

      
    }
}
