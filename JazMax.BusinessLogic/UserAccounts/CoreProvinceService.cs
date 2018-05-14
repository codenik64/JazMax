using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.UserAccountView;

namespace JazMax.BusinessLogic.UserAccounts
{
    public class CoreProvinceService
    {
        private static JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();

        public List<CoreProvinceView> GetAll()
        {
            var query = from t in db.CoreProvinces
                        select t;

            return ConvertListToView(query.ToList());
        }

        public static CoreProvinceView GetProvinceDetails(int ProvinceId)
        {
            var query = (from a in db.CoreProvinces
                         join b in db.CorePas
                         on a.ProvinceId equals b.CorePaId
                         join c in db.CoreUsers
                         on b.CoreUserId equals c.CoreUserId
                         select new CoreProvinceView
                         {
                             PAName = c.FirstName + " " + c.LastName,
                             ProvinceId = a.ProvinceId,
                             ProvinceName = a.ProvinceName,
                             IsActive = a.IsActive
                         }).FirstOrDefault();
            return query;
        }


        public static CoreProvinceView GetProvinceDetailsNew(int ProvinceId)
        {
            var query = (from a in db.CoreProvinces
                         select new CoreProvinceView
                         {
                             ProvinceId = a.ProvinceId,
                             ProvinceName = a.ProvinceName,
                             IsActive = a.IsActive
                         }).FirstOrDefault();
            return query;
        }

        public void Create(CoreProvinceView view)
        {
            try
            {
                db.CoreProvinces.Add(ConvertToView(view));
                db.SaveChanges();
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(db, e, 0);
            }
        }

        public CoreProvinceView FindById(int Id)
        {
            var query = db.CoreProvinces.FirstOrDefault(x => x.ProvinceId == Id);

            return ConvertModelToView(query);
        }

        public static void Update(string ProvinceName, int LoggedInCoreUserId, int ProvinceId)
        {
            try
            {
                if(ProvinceId > 0)
                {
                    var table = db.CoreProvinces.FirstOrDefault(x => x.ProvinceId == ProvinceId);
                    PrepareLogging(ProvinceId, LoggedInCoreUserId);
                    ChangeLog.ChangeLogService.LogChange(table.ProvinceName, ProvinceName, "Province Name");

                    if (table != null)
                    {
                        table.ProvinceName = ProvinceName;
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public static void DeactiveCoreProvince(int ProvinceId, int LoggedInUserId, bool isActiveAction)
        {
            try
            {
                var user = db.CoreProvinces.FirstOrDefault(x => x.ProvinceId == ProvinceId);

                PrepareLogging(ProvinceId, LoggedInUserId);

                if (user != null)

                    if (isActiveAction)
                    {
                        ChangeLog.ChangeLogService.LogChange(ChangeLog.ChangeLogService.GetBoolString((bool)user.IsActive), ChangeLog.ChangeLogService.GetBoolString(false), "Status");

                        user.IsActive = false;
                    }
                    else
                    {
                        ChangeLog.ChangeLogService.LogChange(ChangeLog.ChangeLogService.GetBoolString((bool)user.IsActive), ChangeLog.ChangeLogService.GetBoolString(true), "Status");
                        user.IsActive = true;
                    }

                db.SaveChanges();
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }



        private static void PrepareLogging(int PriamryKey, int LoggedInUserId)
        {
            ChangeLog.ChangeLogService.tableName = "CoreProvince";
            ChangeLog.ChangeLogService.tableKey = PriamryKey;
            ChangeLog.ChangeLogService.LoggedInUserId = LoggedInUserId;
        }

        #region Helpers
        private List<CoreProvinceView> ConvertListToView(List<DataAccess.CoreProvince> model)
        {
            return model.Select(t => new CoreProvinceView
            {
                IsActive = t.IsActive,
                IsAssigned = t.IsAssigned,
                ProvinceId = t.ProvinceId,
                ProvinceName = t.ProvinceName

            }).ToList();
        }

        private CoreProvinceView ConvertModelToView(DataAccess.CoreProvince v)
        {
            CoreProvinceView model = new CoreProvinceView()
            {
                IsActive = v.IsActive,
                IsAssigned = v.IsAssigned,
                ProvinceId = v.ProvinceId,
                ProvinceName = v.ProvinceName
            };
            return model;
        }

        private DataAccess.CoreProvince ConvertToView(CoreProvinceView v)
        {
            DataAccess.CoreProvince model = new DataAccess.CoreProvince()
            {
                IsActive = v.IsActive,
                IsAssigned = v.IsAssigned,
                ProvinceId = v.ProvinceId,
                ProvinceName = v.ProvinceName
            };
            return model;
        }
        #endregion
    }
}
