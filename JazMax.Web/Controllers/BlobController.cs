using JazMax.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JazMax.Web.Controllers
{
    public class BlobController : Controller
    {
        public ActionResult Upload()
        {
            ViewBag.Message = "Upload page.";

            return View();
        }

        [HttpPost]
        public ActionResult Upload(PhotoUpload photo)
        {
            if (ModelState.IsValid)
            {
                if (photo.FileUpload != null && photo.FileUpload.ContentLength > 0)
                {
                   JazMax.Core.Blob.BlobStorageService.UploadToBlob("testimage", "test", photo.FileUpload);
                    //ViewBag.Message = url.ToString();
                }
            }

            return RedirectToAction("Index");

        }
    }
}