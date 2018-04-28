using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.PropertyManagement;

namespace JazMax.BusinessLogic.PropertyManagement
{
    public class PropertyListingCoreService
    {
        private static JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext();

        public int CaptureListing(PropertyListingView model)
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
    }
}
