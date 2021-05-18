using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.PropertyManagement;

namespace JazMax.Core.Property.PropertyManagement
{
    public class PropertyTypeService
    {
        public List<PropertyTypeView> GetAll(bool isActiveAction)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
            {
                return (from a in db.PropertyTypes
                        where a.IsActive == isActiveAction
                        select new PropertyTypeView
                        {
                            isActive = a.IsActive,
                            PropertyTypeId = a.PropertyTypeId,
                            TypeName = a.TypeName
                        })?.ToList();
            }
        }

        public PropertyTypeView GetById(int? Id)
        {
            PropertyTypeView model = null;
            try
            {
                if (Id != null)
                {
                    using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                    {
                        model = (db.PropertyTypes.Where(x => x.PropertyTypeId == Id).Select(x => new PropertyTypeView
                        {
                            isActive = x.IsActive,
                            PropertyTypeId = x.PropertyTypeId,
                            TypeName = x.TypeName

                        }))?.FirstOrDefault();
                        return model;
                    }
                }
            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }
            return model;
        }

        public void Create(PropertyTypeView model)
        {
            try
            {
                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {
                    DataAccess.PropertyType table = new DataAccess.PropertyType()
                    {
                        IsActive = true,
                        TypeName = model.TypeName
                    };
                    db.PropertyTypes.Add(table);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public void Update(PropertyTypeView model, int CoreSystemUserId)
        {
            try
            {
                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {
                    DataAccess.PropertyType table = db.PropertyTypes.FirstOrDefault(x => x.PropertyTypeId == model.PropertyTypeId);

                    LoadEditLogDetails(table.PropertyTypeId, CoreSystemUserId);

                    JazMax.BusinessLogic.ChangeLog.ChangeLogService.LogChange(table.TypeName, model.TypeName, "Property Type");

                    if (table != null)
                    {
                        table.IsActive = true;
                        table.TypeName = model.TypeName;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public void Activation(bool isAction, int PropertyTypeId, int UserId)
        {
            try
            {
                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {
                    DataAccess.PropertyType table = db.PropertyTypes.FirstOrDefault(x => x.PropertyTypeId == PropertyTypeId);
                    LoadEditLogDetails(table.PropertyTypeId, UserId);

                    if (table != null)
                    {
                        if (isAction)
                        {
                            JazMax.BusinessLogic.ChangeLog.ChangeLogService.LogChange(
                                 JazMax.BusinessLogic.ChangeLog.ChangeLogService.GetBoolString(table.IsActive),
                                 JazMax.BusinessLogic.ChangeLog.ChangeLogService.GetBoolString(true), "Active Status");

                            table.IsActive = true;
                        }
                        else
                        {
                            JazMax.BusinessLogic.ChangeLog.ChangeLogService.LogChange(
                                JazMax.BusinessLogic.ChangeLog.ChangeLogService.GetBoolString(table.IsActive),
                                JazMax.BusinessLogic.ChangeLog.ChangeLogService.GetBoolString(false), "Active Status");

                            table.IsActive = false;
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        private void LoadEditLogDetails(int PrimaryKey, int UserId)
        {
            JazMax.BusinessLogic.ChangeLog.ChangeLogService.tableName = "PropertyType";
            JazMax.BusinessLogic.ChangeLog.ChangeLogService.tableKey = PrimaryKey;
            JazMax.BusinessLogic.ChangeLog.ChangeLogService.LoggedInUserId = UserId;
        }
    }
}
