using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.PropertyManagement;
using JazMax.Web.ViewModel.PropertyManagement.CaptureListing;

namespace JazMax.BusinessLogic.PropertyManagement
{
    public class PropertyListingCoreService
    {
        private static JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext();

        private int CaptureListing(PropertyListingView model)
        {
            int ListingId = 0;
            try
            {
                DataAccess.PropertyListing table = new DataAccess.PropertyListing()
                {
                    BranchId = model.BranchId,
                    FriendlyName = model.FriendlyName,
                    IsListingActive = true,
                    IsPriceCash = model.IsPriceCash,
                    IsPricePerAMeter = model.IsPricePerAMeter,
                    IsPricePerAMonth = model.IsPricePerAMonth,
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
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
            return ListingId;
        }

        private void CaptureListingAgents(PropertyListingAgentsView model, int PropetyListingId)
        {
            try
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
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        private void CaptureListingDetail(PropertyListingDetailView model, int PropetyListingId)
        {
            try
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
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        private void CaptureYoutubeLibrary(PropertyListingYoutubeView model, int PropetyListingId)
        {
            try
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
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        private void CapturePropertyListingFeatures(PropertyListingFeatureView model, int PropetyListingId)
        {
            try
            {
                DataAccess.ProprtyListingFeature table = new DataAccess.ProprtyListingFeature()
                {
                    IsFeatureActive = true,
                    PropertyListingId = PropetyListingId,
                    PropertyFeatureId = model.PropertyFeatureId,
                    
                };

                db.ProprtyListingFeatures.Add(table);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public void MainInsert(NewListingView model)
        {
            int Id = CaptureListing(model.PropertyListingView);
            CaptureListingAgents(model.PropertyListingAgentsView, Id);
            CaptureListingDetail(model.PropertyListingDetailView, Id);
            CaptureYoutubeLibrary(model.PropertyListingYoutubeView, Id);
            CapturePropertyListingFeatures(model.PropertyListingFeatureView, Id);

        }

       
    }
}
