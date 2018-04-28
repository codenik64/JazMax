using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.PropertyManagement;

namespace JazMax.BusinessLogic.PropertyManagement
{
    public class PropertyFeatureService
    {
        private static JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext();

        public List<PropertyFeatureView> GetAll(bool isActiveAction)
        {
            return (from a in db.PropertyFeatures
                    where a.IsFeatureActive == isActiveAction
                    select new PropertyFeatureView
                    {
                        IsFeatureActive = a.IsFeatureActive,
                        PropertyFeatureId = a.PropertyFeatureId,
                        FeatureName = a.FeatureName
                    }).ToList();
        }

        public void Create(PropertyFeatureView model)
        {
            try
            {
                DataAccess.PropertyFeature table = new DataAccess.PropertyFeature()
                {
                    IsFeatureActive = true,
                    FeatureName = model.FeatureName
                };
                db.PropertyFeatures.Add(table);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public PropertyFeatureView GetById(int? Id)
        {
            PropertyFeatureView model = null;
            try
            {
                if (Id != null)
                {
                    model = (db.PropertyFeatures.Where(x => x.PropertyFeatureId == Id).Select( x => new PropertyFeatureView
                    {
                        FeatureName = x.FeatureName,
                        IsFeatureActive = x.IsFeatureActive,
                        PropertyFeatureId = x.PropertyFeatureId

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

        public void Update(PropertyFeatureView model, int CoreSystemUserId)
        {
            try
            {
                DataAccess.PropertyFeature table = db.PropertyFeatures.FirstOrDefault(x => x.PropertyFeatureId == model.PropertyFeatureId);

                LoadEditLogDetails(table.PropertyFeatureId, CoreSystemUserId);

                ChangeLog.ChangeLogService.LogChange(table.FeatureName, model.FeatureName, "Feature Name");

                if (table != null)
                {
                    table.IsFeatureActive = true;
                    table.FeatureName = model.FeatureName;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public void Activation(bool isAction, int PropertyFeatureId, int UserId)
        {
            try
            {
                DataAccess.PropertyFeature table = db.PropertyFeatures.FirstOrDefault(x => x.PropertyFeatureId == PropertyFeatureId);
                LoadEditLogDetails(table.PropertyFeatureId, UserId);

                if (table != null)
                {
                    if (isAction)
                    {
                        ChangeLog.ChangeLogService.LogChange(
                            ChangeLog.ChangeLogService.GetBoolString(table.IsFeatureActive),
                            ChangeLog.ChangeLogService.GetBoolString(true), "Active Status");

                        table.IsFeatureActive = true;
                    }
                    else
                    {
                        ChangeLog.ChangeLogService.LogChange(
                           ChangeLog.ChangeLogService.GetBoolString(table.IsFeatureActive),
                           ChangeLog.ChangeLogService.GetBoolString(false), "Active Status");

                        table.IsFeatureActive = false;
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
            ChangeLog.ChangeLogService.tableName = "PropertyFeature";
            ChangeLog.ChangeLogService.tableKey = PrimaryKey;
            ChangeLog.ChangeLogService.LoggedInUserId = UserId;
        }
    }
}
