using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.UserAccountView;

namespace JazMax.BusinessLogic.UserAccounts
{
    public class CoreUserService
    {
        private static JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();

        public List<CoreUserView> GetAll()
        {
            var query = from t in db.CoreUsers
                        where t.IsActive == true
                        select t;

            return ConvertListModelToView(query.ToList());
        }

        #region Create A New Core System User 
        public int? CreateNewCoreUser(CoreUserView model)
        {
            int? userId = null;

            try
            {
                DataAccess.CoreUser a = new DataAccess.CoreUser();
                a.CellPhone = model.CellPhone;
                a.CreatedBy = model.CreatedBy;
                a.CreatedDate = DateTime.Now;
                a.EmailAddress = model.EmailAddress;
                a.FirstName = model.FirstName;
                a.GenderId = model.GenderId;
                a.IdNumber = model.IDNumber;
                a.IsActive = true;
                a.LastName = model.LastName;
                a.LastUpdatedDate = DateTime.Now;
                a.MiddleName = model.MiddleName;
                a.PhoneNumber = model.PhoneNumber;
                db.CoreUsers.Add(a);
                db.SaveChanges();
                userId = a.CoreUserId;
                AddUserToType(a.CoreUserId, model.CoreUserTypeId);

                //PA
                if(model.CoreUserTypeId == 5)
                {
                    DataAccess.CorePa z = new DataAccess.CorePa()
                    {
                        CoreUserId = userId,
                        ProvinceId = model.CapturePAView.provinceId,
                        IsActive = true
                    };
                    db.CorePas.Add(z);
                    db.SaveChanges();

                    updateProvinceToAssigned(model.CapturePAView.provinceId);
                }

                //TeamLeader
                else if(model.CoreUserTypeId == 3)
                {
                    DataAccess.CoreTeamLeader y = new DataAccess.CoreTeamLeader()
                    {
                        CoreProvinceId = model.CaptureTeamLeader.provinceId,
                        CoreUserId = userId,
                        IsActive = true
                    };
                    db.CoreTeamLeaders.Add(y);
                    db.SaveChanges();
                }

                //Agent
                else if (model.CoreUserTypeId == 4)
                {
                    DataAccess.CoreAgent p = new DataAccess.CoreAgent()
                    {
                        CoreBranchId = model.CaptureAgent.BranchId,
                        CoreUserId = userId,
                        IsActive = true
                    };
                    db.CoreAgents.Add(p);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(db, e, 0);
                userId = -1;
            }

            return userId;
        }
        #endregion

        #region Update Priovince After Assigned
        public void updateProvinceToAssigned(int proId)
        {
            try
            {
                DataAccess.CoreProvince d = db.CoreProvinces.FirstOrDefault(x => x.ProvinceId == proId);
                d.IsAssigned = true;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(db, e, 0);
            }
        }
        #endregion

        #region JazMax Custom Identity Helpers
        public string GetRoleGUID(int typeId)
        {
            var query = from a in db.CoreUserTypes
                        where a.CoreUserTypeId == typeId
                        select a;

            return query.FirstOrDefault().UserRoleId.ToString();
        }
        public string GetAspUserGUID(string userName)
        {
            var q = from a in db.AspNetUsers
                    where a.UserName == userName
                    select a;

            return q.FirstOrDefault().Id.ToString();
        }

        public void AddUserToAspUserRole(string userId, string roleId)
        {
            try
            {
                DataAccess.AspNetUserRole r = new DataAccess.AspNetUserRole()
                {
                    RoleId = roleId,
                    UserId = userId
                };
                db.AspNetUserRoles.Add(r);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(db, e, 0);
            }
        }

        public void AddUserToType(int coreUserId, int coreUserTypeId)
        {
            try
            {
                DataAccess.CoreUserInType a = new DataAccess.CoreUserInType()
                {
                    CoreUserId = coreUserId,
                    CoreUserTypeId = coreUserTypeId,
                    IsUserTypeActive = true
                };
                db.CoreUserInTypes.Add(a);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(db, e, 0);
            }
        }

        //UPDATE
        //Use external DB Context - Avoids multiple hits 
        public static string GetAspUserEmailById(DataAccess.JazMaxDBProdContext dbcon, string UserId)
        {
            var a = dbcon.AspNetUsers.FirstOrDefault(x => x.Id == UserId).Email;
            return a;
        }

        #endregion

        #region Model Helpers Because Why Not!
        public List<CoreUserView> ConvertListModelToView(List<DataAccess.CoreUser> modol)
        {
            return modol.Select(x => new CoreUserView
            {
                EmailAddress = x.EmailAddress,
                IsActive = x.IsActive,
                CellPhone = x.CellPhone,
                CoreUserId = x.CoreUserId,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                FirstName = x.FirstName,
                GenderId = x.GenderId,
                IDNumber = x.IdNumber,
                LastName = x.LastName,
                LastUpdatedDate = x.LastUpdatedDate,
                MiddleName = x.MiddleName,
                PhoneNumber = x.PhoneNumber,
            }).ToList();
        }
        #endregion
    }
}
