using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.PropertyManagement;

namespace JazMax.BusinessLogic.PropertyManagement
{
    public class PropertyPriceService
    {
        private static JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext();

        public List<PropertyPricingTypesView> GetAll(bool isActiveAction)
        {
            return (from a in db.PropertyListingPricingTypes
                    where a.IsActive == isActiveAction
                    select new PropertyPricingTypesView
                    {
                        IsActive = a.IsActive,
                        PropertyListingPricingTypeId = a.PropertyListingPricingTypeId,
                        TypeName = a.TypeName
                    }).ToList();
        }

        public PropertyPricingTypesView GetById(int? Id)
        {
            PropertyPricingTypesView model = null;
            try
            {
                if (Id != null)
                {
                    model = (db.PropertyListingPricingTypes.Where(x => x.PropertyListingPricingTypeId == Id).Select(x => new PropertyPricingTypesView
                    {
                        IsActive = x.IsActive,
                        PropertyListingPricingTypeId = x.PropertyListingPricingTypeId,
                        TypeName = x.TypeName

                    })).FirstOrDefault();
                    return model;
                }
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
            return model;
        }

        public void Create(PropertyPricingTypesView model)
        {
            try
            {
                DataAccess.PropertyListingPricingType table = new DataAccess.PropertyListingPricingType()
                {
                    IsActive = true,
                    TypeName = model.TypeName
                };
                db.PropertyListingPricingTypes.Add(table);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public void Update(PropertyPricingTypesView model, int CoreSystemUserId)
        {
            try
            {
                DataAccess.PropertyListingPricingType table = db.PropertyListingPricingTypes.FirstOrDefault(x => x.PropertyListingPricingTypeId == model.PropertyListingPricingTypeId);

                LoadEditLogDetails(table.PropertyListingPricingTypeId, CoreSystemUserId);

                ChangeLog.ChangeLogService.LogChange(table.TypeName, model.TypeName, "Property Price Type");

                if (table != null)
                {
                    table.IsActive = true;
                    table.TypeName = model.TypeName;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public void Activation(bool isAction, int PropertyListingPricingTypeId, int UserId)
        {
            try
            {
                DataAccess.PropertyListingPricingType table = db.PropertyListingPricingTypes.FirstOrDefault(x => x.PropertyListingPricingTypeId == PropertyListingPricingTypeId);
                LoadEditLogDetails(table.PropertyListingPricingTypeId, UserId);

                if (table != null)
                {
                    if (isAction)
                    {
                        ChangeLog.ChangeLogService.LogChange(
                            ChangeLog.ChangeLogService.GetBoolString(table.IsActive),
                            ChangeLog.ChangeLogService.GetBoolString(true), "Active Status");

                        table.IsActive = true;
                    }
                    else
                    {
                        ChangeLog.ChangeLogService.LogChange(
                           ChangeLog.ChangeLogService.GetBoolString(table.IsActive),
                           ChangeLog.ChangeLogService.GetBoolString(false), "Active Status");

                        table.IsActive = false;
                    }
                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        private void LoadEditLogDetails(int PrimaryKey, int UserId)
        {
            ChangeLog.ChangeLogService.tableName = "PricingType";
            ChangeLog.ChangeLogService.tableKey = PrimaryKey;
            ChangeLog.ChangeLogService.LoggedInUserId = UserId;
        }
    }
}
