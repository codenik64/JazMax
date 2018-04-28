using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.PropertyManagement;

namespace JazMax.BusinessLogic.PropertyManagement
{
    public class PropertyTypeService
    {
        private static JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext();

        public List<PropertyTypeView> GetAll(bool isActiveAction)
        {
            return (from a in db.PropertyTypes
                    where a.IsActive == isActiveAction
                    select new PropertyTypeView
                    {
                        isActive = a.IsActive,
                        PropertyTypeId = a.PropertyTypeId,
                        TypeName = a.TypeName
                    }).ToList();
        }

        public PropertyTypeView GetById(int? Id)
        {
            PropertyTypeView model = null;
            try
            {
                if (Id != null)
                {
                    model = (db.PropertyTypes.Where(x => x.PropertyTypeId == Id).Select(x => new PropertyTypeView
                    {
                        isActive = x.IsActive,
                        PropertyTypeId = x.PropertyTypeId,
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

        public void Create(PropertyTypeView model)
        {
            try
            {
                DataAccess.PropertyType table = new DataAccess.PropertyType()
                {
                    IsActive = true,
                    TypeName = model.TypeName
                };
                db.PropertyTypes.Add(table);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public void Update(PropertyTypeView model, int CoreSystemUserId)
        {
            try
            {
                DataAccess.PropertyType table = db.PropertyTypes.FirstOrDefault(x => x.PropertyTypeId == model.PropertyTypeId);

                LoadEditLogDetails(table.PropertyTypeId, CoreSystemUserId);

                ChangeLog.ChangeLogService.LogChange(table.TypeName, model.TypeName, "Property Type");

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

        public void Activation(bool isAction, int PropertyTypeId, int UserId)
        {
            try
            {
                DataAccess.PropertyType table = db.PropertyTypes.FirstOrDefault(x => x.PropertyTypeId == PropertyTypeId);
                LoadEditLogDetails(table.PropertyTypeId, UserId);

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
            ChangeLog.ChangeLogService.tableName = "PropertyType";
            ChangeLog.ChangeLogService.tableKey = PrimaryKey;
            ChangeLog.ChangeLogService.LoggedInUserId = UserId;
        }
    }
}