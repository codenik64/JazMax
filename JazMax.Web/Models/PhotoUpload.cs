using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JazMax.Web.Models
{
    public class PhotoUpload
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Please select file.")]
        public HttpPostedFileBase FileUpload { get; set; }
    }
}