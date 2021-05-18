using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.Web.ViewModel.PropertyManagement;
using JazMax.Web.ViewModel.PropertyManagement.CaptureListing;
using JazMax.Core.Property.PropertyManagement;
using JazMax.BusinessLogic.UserAccounts;
using JazMax.Core.SystemHelpers;
using System.Net;

namespace JazMax.Web.Areas.Property.Controllers
{
    public class PropertyListingController : Controller
    {
        #region Declarations
        private static PropertyListingCoreService o = new PropertyListingCoreService();
        private static AgentService s = new AgentService();
        private static JazMaxIdentityHelper _helper = new JazMaxIdentityHelper();
        #endregion

        #region Index View
        public ActionResult Index()
        {
            return View(o.GetPrimaryListingOK());
        }
        #endregion

        #region Insert Property
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NewListingView list , FormCollection collection)
        {
           int ProptyId = o.MainInsert(list, collection);

            for (int i = 0; i < Request.Files.Count ; i++)
            {
                var file = Request.Files[i];

                if (ModelState.IsValid)
                {
                    if (file!= null && file.ContentLength > 0)
                    {
                        int BlobIds = JazMax.Core.Blob.BlobStorageService.UploadToBlob("testimge", "test", file);

                        PropertyImagesView table = new PropertyImagesView()
                        {
                            BlobId = BlobIds,
                            PropertyListingId = ProptyId,

                    
                        };

                        o.CapturePropertyImages(table, ProptyId);
                     
                    }
                }
             
                
               

            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Cascades
        public ActionResult GetAgentForBranch(int Id)
        {
            return Json(s.GetMyAgentInBranch(Id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBranchForProvince(int Id)
        {
            return Json(_helper.GetBranchesBasedOnProvince(Id), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Partial Property Views

        public PartialViewResult GetAgents( NewListingView List , int id)
        {
            var query = o.GetAgents(List , id);
            return PartialView("_PropertyAgents",query);
        }


        public PartialViewResult GetFeatures(NewListingView List, int id)
        {
            var query = o.GetFeatureOverview(List, id);
            return PartialView("_PropertyFeatures", query);
        }

        public PartialViewResult GetImages(NewListingView List, int id)
        {
            var query = o.GetPropertyImages(List, id);
            return PartialView("_PropertyImages", query);
        }


        #endregion

        #region Update for Just PropertyListingView

        public ActionResult UpdateListings(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var query = o.FindPrimaryById(id);
            return View(query);
        }

        [HttpPost]
        public ActionResult UpdateListings(NewListingView list, int id)
        {
            o.MainUpdate(list, id);
            return RedirectToAction("Index");
        }
        #endregion

        #region Details
        public ActionResult PropertyDetails(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var query = o.FindPrimaryById(id);
            return View(query);
        }
        #endregion
        public ActionResult Tab()
        {
            return View();
        }
        public PartialViewResult GalleryImage()
        {
           
            return PartialView("Gallery");
        }

        #region Gallery of Images
        public ActionResult Gallery()
        {
            return View();
        }
        #endregion
    }
}