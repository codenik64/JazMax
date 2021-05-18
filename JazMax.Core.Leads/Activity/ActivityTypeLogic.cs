using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.Leads.ActivityType;

namespace JazMax.Core.Leads.Activity
{
    public class ActivityTypeLogic
    {
        #region Get All
        public List<LeadActivityType> GetAll(bool isActiveAction)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
            {
                return (from a in db.LeadActivities
                        where a.IsActive == isActiveAction
                        select new LeadActivityType
                        {
                            ActivityName = a.ActivityName,
                            IsSystemActivity = a.IsSystem,
                        }).ToList();
            }
        }
        #endregion

        #region Create
        public void Create(LeadActivityType model)
        {
            try
            {
                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {
                    DataAccess.LeadActivity table = new DataAccess.LeadActivity()
                    {
                        IsActive = true,
                        ActivityName = model.ActivityName,
                        IsSystem = model.IsSystemActivity

                    };
                    db.LeadActivities.Add(table);
                    db.SaveChanges();
                }

            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }
        }
        #endregion

        #region Get By Id
        public LeadActivityType GetById(int? Id)
        {
            LeadActivityType model = null;
            try
            {
                if (Id != null)
                {
                    using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                    {
                        model = (db.LeadActivities.Where(x => x.LeadActivityId == Id).Select(x => new LeadActivityType
                        {
                            ActivityName = x.ActivityName,
                            IsSystemActivity = x.IsSystem,

                        })).FirstOrDefault();
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
        #endregion

        #region Update
        public void Update(LeadActivityType model, int CoreSystemUserId)
        {
            try
            {
                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {
                    DataAccess.LeadActivity table = db.LeadActivities.FirstOrDefault(x => x.LeadActivityId == model.LeadActivityId);

                    LoadEditLogDetails(table.LeadActivityId, CoreSystemUserId);

                    JazMax.BusinessLogic.ChangeLog.ChangeLogService.LogChange(table.ActivityName, model.ActivityName, "Activity Name");

                    if (table != null)
                    {
                        table.IsActive = true;
                        table.ActivityName = model.ActivityName;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
            }
        }
        #endregion

        #region Activation
        public void Activation(bool isAction, int LeadActivityId, int UserId)
        {
            try
            {
                using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
                {
                    DataAccess.LeadActivity table = db.LeadActivities.FirstOrDefault(x => x.LeadActivityId == LeadActivityId);
                    LoadEditLogDetails(table.LeadActivityId, UserId);

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
        #endregion

        #region Logging
        private void LoadEditLogDetails(int PrimaryKey, int UserId)
        {
            JazMax.BusinessLogic.ChangeLog.ChangeLogService.tableName = "LeadActivity";
            JazMax.BusinessLogic.ChangeLog.ChangeLogService.tableKey = PrimaryKey;
            JazMax.BusinessLogic.ChangeLog.ChangeLogService.LoggedInUserId = UserId;
        }
        #endregion
    }
}
