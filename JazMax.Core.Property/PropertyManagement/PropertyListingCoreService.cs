using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.PropertyManagement;
using JazMax.Web.ViewModel.PropertyManagement.CaptureListing;
using System.Data.Entity;
using System.Web.Mvc;

namespace JazMax.Core.Property.PropertyManagement
{
    public class PropertyListingCoreService
    {
        //private static JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext();
        #region GetLists

        public IQueryable<NewListingView> GetPrimaryListing()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                List<NewListingView> view = new List<NewListingView>();

                #region Joins Query
                var query = (from a in db.PropertyListings
                             join b in db.PropertyListingAgents 
                             on a.PropertyListingId equals b.PropertyListingId
                             join c in db.PropertyListingDetails 
                             on a.PropertyListingId equals c.PropertyListingId
                             join d in db.ProprtyListingYoutubeLibraries
                             on a.PropertyListingId equals d.PrfoprtyListingId
                             join e in db.ProprtyListingFeatures 
                             on a.PropertyListingId equals e.PropertyListingId
                             select new
                             {
                                 a.PropertyListingId,
                                 a.BranchId,
                                 a.FriendlyName,
                                 a.ProprtyDesciption,
                                 a.IsListingActive,
                                 a.PropertyListingPricingTypeId,
                                 a.Price,
                                 a.PropertyTypeId,
                                 a.LastUpdate,
                                 a.ListingDate,
                                 a.ProvinceId,
                                 b.AgentId,
                                 b.IsActive,
                                 c.RatesAndTaxes,
                                 c.NumberOfBathRooms,
                                 c.NumberOfBedrooms,
                                 c.NumberOfGarages,
                                 c.NumberOfSquareMeters,
                                 d.YoutubeVideoLink,
                                 e.PropertyFeatureId,
                             }).ToList().AsQueryable();
                #endregion


                foreach (var item in query)
                {
                    #region Property Listing View
                    PropertyListingView list = new PropertyListingView()
                    {
                        PropertyListingId = item.PropertyListingId,
                        BranchId = item.BranchId,
                        FriendlyName = item.FriendlyName,
                        IsListingActive = item.IsListingActive,
                        PropertyListingPricingTypeId = item.PropertyListingPricingTypeId,
                        Price = item.Price,
                        PropertyTypeId = item.PropertyTypeId,
                        LastUpdate = item.LastUpdate,
                        ListingDate = item.ListingDate,
                        ProvinceId = item.ProvinceId,
                        ProprtyDesciption = item.ProprtyDesciption
                    };
                    #endregion

                    #region Agent View
                    PropertyListingAgentsView agent = new PropertyListingAgentsView()
                    {
                        AgentId = item.AgentId,
                        IsActive = item.IsActive,
                    };
                    #endregion

                    #region Details View
                    PropertyListingDetailView details = new PropertyListingDetailView()
                    {
                        RatesAndTaxes = item.RatesAndTaxes,
                        NumberOfBathRooms = item.NumberOfBathRooms,
                        NumberOfBedrooms = item.NumberOfBedrooms,
                        NumberOfGarages = item.NumberOfGarages,
                        NumberOfSquareMeters = item.NumberOfSquareMeters,
                    };
                    #endregion

                    #region Youtube View
                    PropertyListingYoutubeView youtube = new PropertyListingYoutubeView()
                    {
                        YoutubeVideoLink = item.YoutubeVideoLink,
                    };

                    #endregion

                    #region Features View
                    PropertyListingFeatureView feature = new PropertyListingFeatureView()
                    {
                        PropertyFeatureId = item.PropertyFeatureId,
                    };
                    #endregion

                    #region NewList
                    NewListingView lists = new NewListingView()
                    {
                        PropertyListingView = list,
                        PropertyListingAgentsView = agent,
                        PropertyListingDetailView = details,
                        PropertyListingYoutubeView = youtube,
                        PropertyListingFeatureView = feature,
                    };
                    #endregion


                    view.Add(lists);
                }
                return view.AsQueryable();
            }

        }

        public IQueryable<NewListingView> GetPrimaryListingOK()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                List<NewListingView> view = new List<NewListingView>();

                #region Joins Query
                var query = (from a in db.PropertyListings
                             join b in db.PropertyListingDetails
                             on a.PropertyListingId equals b.PropertyListingId
                            
                             select new
                             {
                                 a.PropertyListingId,
                                 a.FriendlyName,
                                 b.RatesAndTaxes,
                                 b.NumberOfBathRooms,
                                 b.NumberOfBedrooms,
                                 b.NumberOfGarages,
                                 b.NumberOfSquareMeters,
                             
                             }).ToList().AsQueryable();
                #endregion


                foreach (var item in query)
                {
                    #region Property Listing View
                    PropertyListingView list = new PropertyListingView()
                    {
                        PropertyListingId = item.PropertyListingId,
                        FriendlyName = item.FriendlyName,
                       
                    };
                    #endregion
                    #region Details View
                    PropertyListingDetailView details = new PropertyListingDetailView()
                    {
                        RatesAndTaxes = item.RatesAndTaxes,
                        NumberOfBathRooms = item.NumberOfBathRooms,
                        NumberOfBedrooms = item.NumberOfBedrooms,
                        NumberOfGarages = item.NumberOfGarages,
                        NumberOfSquareMeters = item.NumberOfSquareMeters,
                    };
                    #endregion

                 

                    #region NewList
                    NewListingView lists = new NewListingView()
                    {
                        PropertyListingView = list,
                        PropertyListingDetailView = details,
                      
                    };
                    #endregion


                    view.Add(lists);
                }
                return view.AsQueryable();
            }

        }

        public IQueryable<PropertyView> GetPrimaryListingNew()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var query = (from t in db.PropertyListings
                             join b in db.PropertyTypes
                             on t.PropertyTypeId equals b.PropertyTypeId
                             join c in db.CoreBranches
                             on t.BranchId equals c.BranchId
                             join d in db.CoreProvinces
                             on t.ProvinceId equals d.ProvinceId
                             join e in db.PropertyListingPricingTypes
                             on t.PropertyListingPricingTypeId equals e.PropertyListingPricingTypeId
                             select new PropertyView
                             {
                                 BranchId = t.BranchId,
                                 BranchName = c.BranchName,
                                 FriendlyName = t.FriendlyName,
                                 IsListingActive = t.IsListingActive,
                                 LastUpdate = t.LastUpdate,
                                 ListingDate = t.ListingDate,
                                 Price = t.Price,
                                 PropertyListingId = t.PropertyListingId,
                                 PropertyListingPricingTypeId = t.PropertyListingPricingTypeId,
                                 PropertyPriceTypeName = e.TypeName,
                                 PropertyTypeId = t.PropertyTypeId,
                                 PropertyTypeName = b.TypeName,
                                 ProprtyDesciption = t.ProprtyDesciption,
                                 ProvinceId = t.ProvinceId,
                                 ProvinceName = d.ProvinceName,
                                 PropertyListingAgentsView = (from n in db.PropertyListingAgents
                                                              join m in db.CoreAgents
                                                              on n.AgentId equals m.CoreAgentId
                                                              join o in db.CoreUsers
                                                              on m.CoreUserId equals o.CoreUserId
                                                              where n.PropertyListingId == t.PropertyListingId
                                                              && n.AgentId != 0
                                                              select new PropertyListingAgentsView
                                                              {
                                                                  AgentId = n.AgentId,
                                                                  AgentName = o.FirstName,
                                                                  IsActive =n.IsActive

                                                              }).ToList(),
                                 PropertyImagesView = (from q in db.PropertyImages
                                                       join y in db.BlobCoreStorages
                                                       on q.BlobId equals y.BlobId
                                                       where q.PropertyListingId == t.PropertyListingId
                                                       select new PropertyImagesView
                                                       {
                                                           BlobImagePath = y.BlobUrl

                                                       }).ToList(),
                                 PropertyListingFeatureView = (from r in db.ProprtyListingFeatures
                                                               join v in db.PropertyFeatures
                                                               on r.PropertyFeatureId equals v.PropertyFeatureId
                                                               where r.PropertyListingId == t.PropertyListingId
                                                               select new PropertyListingFeatureView
                                                               {
                                                                   FeatureName = v.FeatureName
                                                               }).ToList(),


                             }).ToList().AsQueryable();
                return query;
            }
        }
        #endregion

        #region FIndbyIdz

        public NewListingView FindPrimaryById(int id)
        {
            var query = GetPrimaryListing().FirstOrDefault(x => x.PropertyListingView.PropertyListingId == id);
            return query;
        }

        #endregion

        #region Capture Listings

        private int CaptureListing(PropertyListingView model)
        {

            int ListingId = 0;
            try
            {
                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {
                    DataAccess.PropertyListing table = new DataAccess.PropertyListing()
                    {
                        BranchId = model.BranchId,
                        FriendlyName = model.FriendlyName,
                        IsListingActive = true,
                        PropertyListingPricingTypeId = model.PropertyListingPricingTypeId,
                        LastUpdate = DateTime.Now,
                        ListingDate = DateTime.Now,
                        Price = model.Price,
                        PropertyTypeId = model.PropertyTypeId,
                        ProprtyDesciption = model.ProprtyDesciption,
                        ProvinceId = model.ProvinceId
                    };
                    db.PropertyListings.Add(table);
                    db.SaveChanges();
                    ListingId = table.PropertyListingId;
                }
            }

            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }
            return ListingId;
        }

        private void CaptureListingAgents(PropertyListingAgentsView model, int PropetyListingId)
        {
            try
            {
                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {
                    DataAccess.PropertyListingAgent table = new DataAccess.PropertyListingAgent()
                    {
                        PropertyListingId = PropetyListingId,
                        IsActive = true,
                        AgentId = model.AgentId
                    };
                    db.PropertyListingAgents.Add(table);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }


        }

        private void CaptureListingDetail(PropertyListingDetailView model, int PropetyListingId)
        {
            try
            {
                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {
                    DataAccess.PropertyListingDetail table = new DataAccess.PropertyListingDetail()
                    {
                        RatesAndTaxes = model.RatesAndTaxes,
                        NumberOfBathRooms = model.NumberOfBathRooms,
                        NumberOfBedrooms = model.NumberOfBedrooms,
                        NumberOfGarages = model.NumberOfGarages,
                        NumberOfSquareMeters = model.NumberOfSquareMeters,
                        PropertyListingId = PropetyListingId,
                    };
                    db.PropertyListingDetails.Add(table);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        private void CaptureYoutubeLibrary(PropertyListingYoutubeView model, int PropetyListingId)
        {
            try
            {
                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {
                    DataAccess.ProprtyListingYoutubeLibrary table = new DataAccess.ProprtyListingYoutubeLibrary()
                    {
                        IsVideoActive = true,
                        PrfoprtyListingId = PropetyListingId,
                        YoutubeVideoLink = model.YoutubeVideoLink
                    };
                    db.ProprtyListingYoutubeLibraries.Add(table);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        private void CapturePropertyListingFeatures(PropertyListingFeatureView model, int PropetyListingId , FormCollection collection)
        {
            try
            {
               
                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {
                    string[] ids = collection["PropertyListingFeatureView.PropertyFeatureId"].Split(new char[] { ',' });
                    foreach (var item in ids)
                    {

                        DataAccess.ProprtyListingFeature table = new DataAccess.ProprtyListingFeature()
                        {
                            IsFeatureActive = true,
                            PropertyListingId = PropetyListingId,
                            PropertyFeatureId = int.Parse(item),

                        };

                        db.ProprtyListingFeatures.Add(table);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public void CapturePropertyImages(PropertyImagesView model, int PropertyListingId)
        {
            try
            {
                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {
                    DataAccess.PropertyImage table = new DataAccess.PropertyImage()
                    {
                        PropertyImagesId = model.PropertyImagesId,
                        PropertyListingId = PropertyListingId,
                        BlobId = model.BlobId,
                        IsActive = model.IsActive
                    };

                    db.PropertyImages.Add(table);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public int MainInsert(Web.ViewModel.PropertyManagement.CaptureListing.NewListingView model, FormCollection collection)
        {
            int Id = CaptureListing(model.PropertyListingView);
            CaptureListingAgents(model.PropertyListingAgentsView, Id);
            CaptureListingDetail(model.PropertyListingDetailView, Id);
            CaptureYoutubeLibrary(model.PropertyListingYoutubeView, Id);
            CapturePropertyListingFeatures(model.PropertyListingFeatureView, Id, collection);
            return Id;

        }
        #endregion

        #region Update Listings
        public int UpdatePropertyListing(PropertyListingView model, int PropertyListingId)
        {
            int Lid = 0;
            try
            {

                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {
                    var listing = db.PropertyListings.FirstOrDefault(x => x.PropertyListingId == PropertyListingId);
                    listing.PropertyListingId = PropertyListingId;
                    listing.PropertyTypeId = model.PropertyTypeId;
                    listing.BranchId = model.BranchId;
                    listing.ProvinceId = model.ProvinceId;
                    listing.FriendlyName = model.FriendlyName;
                    listing.Price = model.Price;
                    listing.PropertyListingPricingTypeId = model.PropertyListingPricingTypeId;
                    listing.ProprtyDesciption = model.ProprtyDesciption;
                    listing.IsListingActive = model.IsListingActive;

                    db.SaveChanges();
                    Lid = listing.PropertyListingId;

                }

            }

            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }
            return Lid;
        }

        public void UpdatePropertyAgents(PropertyListingAgentsView model, int PropertyListingId)
        {
            try
            {
                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {

                    var agents = db.PropertyListingAgents.FirstOrDefault(x => x.AgentId == model.AgentId);
                    agents.PropertyListingId = PropertyListingId;
                    agents.IsActive = model.IsActive;
                    agents.AgentId = model.AgentId;

                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public void UpdateListingDetail(PropertyListingDetailView model, int PropertyListingId)
        {
            try
            {
                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {

                    var Details = db.PropertyListingDetails.FirstOrDefault(x => x.PropertyListingDetailId == model.PropertyListingDetailId);
                    Details.RatesAndTaxes = model.RatesAndTaxes;
                    Details.NumberOfBathRooms = model.NumberOfBathRooms;
                    Details.NumberOfBedrooms = model.NumberOfBedrooms;
                    Details.NumberOfGarages = model.NumberOfGarages;
                    Details.NumberOfSquareMeters = model.NumberOfSquareMeters;
                    Details.PropertyListingId = PropertyListingId;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public void UpdateYoutubeLibrary(PropertyListingYoutubeView model, int PropertyListingId)
        {
            try
            {
                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {

                    var youtube = db.ProprtyListingYoutubeLibraries.FirstOrDefault(x => x.ProprtyListingYoutubeLibraryId == model.ProprtyListingYoutubeLibraryId);
                    youtube.IsVideoActive = model.IsVideoActive;
                    youtube.PrfoprtyListingId = PropertyListingId;
                    youtube.YoutubeVideoLink = model.YoutubeVideoLink;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public void UpdatePropertyFeatures(PropertyListingFeatureView model, int PropertyListingId)
        {
            try
            {
                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {

                    var features = db.ProprtyListingFeatures.FirstOrDefault(x => x.PropertyFeatureId == model.PropertyFeatureId);
                    features.IsFeatureActive = model.IsFeatureActive;
                    features.PropertyListingId = PropertyListingId;
                    features.PropertyFeatureId = model.PropertyFeatureId;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public void MainUpdate(Web.ViewModel.PropertyManagement.CaptureListing.NewListingView model, int id)
        {
            int Id = UpdatePropertyListing(model.PropertyListingView, id);
            UpdatePropertyAgents(model.PropertyListingAgentsView, Id);
            UpdateListingDetail(model.PropertyListingDetailView, Id);
            UpdateYoutubeLibrary(model.PropertyListingYoutubeView, Id);
            UpdatePropertyFeatures(model.PropertyListingFeatureView, Id);

        }
        #endregion

        #region GetAgentInformation

        public List<PropertyListingAgentsView>GetAgents(Web.ViewModel.PropertyManagement.CaptureListing.NewListingView model , int PropertyId)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
               
                var query = (from a in db.PropertyListingAgents
                             join b in db.CoreAgents on a.AgentId equals b.CoreAgentId
                             join c in db.CoreUsers on b.CoreUserId equals c.CoreUserId
                             where a.PropertyListingId == PropertyId
                             select new PropertyListingAgentsView
                             {
                                 PropertyListingAgentsId = a.PropertyListingAgentsId,
                                 AgentId = a.AgentId,
                                 PropertyListingId = PropertyId,
                                 IsActive = a.IsActive,
                                 AgentName = c.FirstName + "   " + c.LastName,
                                 Contact = c.CellPhone,
                                 Address = "Unknown",
                                 Email = c.EmailAddress,

                             }).ToList();

                return query;
                             
                             
            }
           
        }
        #endregion

        #region Get Feature Overview

        public List<PropertyListingFeatureView> GetFeatureOverview(Web.ViewModel.PropertyManagement.CaptureListing.NewListingView model, int PropertyId)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {

                var query = (from a in db.ProprtyListingFeatures
                             join b in db.PropertyFeatures
                             on a.PropertyFeatureId equals b.PropertyFeatureId
                             where a.PropertyListingId == PropertyId
                             select new PropertyListingFeatureView
                             {
                                 ProprtyListingFeaturesId = a.ProprtyListingFeaturesId,
                                 PropertyListingId = PropertyId,
                                 PropertyFeatureId = a.PropertyFeatureId,
                                 FeatureName = b.FeatureName,
                                 IsFeatureActive = b.IsFeatureActive,
                             }).ToList();

                return query;


            }

        }
        #endregion

        #region Get Property Images Overview

        public List<PropertyImagesView> GetPropertyImages(Web.ViewModel.PropertyManagement.CaptureListing.NewListingView model, int PropertyId)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {

                var query = (from a in db.BlobCoreStorages
                             join b in db.PropertyImages
                             on a.BlobId equals b.BlobId
                             where b.PropertyListingId == PropertyId
                             select new PropertyImagesView
                             {
                               BlobId = a.BlobId,
                               BlobImagePath = a.BlobUrl,
                               PropertyListingId = PropertyId
                             }).ToList();

                return query;


            }

        }
        #endregion




    }
}
