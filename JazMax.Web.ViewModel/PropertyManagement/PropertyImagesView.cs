using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.PropertyManagement
{
    public class PropertyImagesView
    {
        public int PropertyImagesId { get; set; } // PropertyImagesId (Primary key)
        public int PropertyListingId { get; set; } // PropertyListingId
        public string ImageType { get; set; }
        public byte[] Imagebytes { get; set; }
        public int BlobId { get; set; } // BlobId
        public bool IsActive { get; set; } // IsActive

        public string BlobImagePath { get; set; }
    }
}
