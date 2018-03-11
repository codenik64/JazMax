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
        private static JazMax.AzureDataAccess.JazMaxDBProdContext db = new AzureDataAccess.JazMaxDBProdContext();

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
                AzureDataAccess.CoreUser a = new AzureDataAccess.CoreUser();
                a.CellPhone = model.CellPhone;
                a.CreatedBy = model.CreatedBy;
                a.CreatedDate = DateTime.Now;
                a.EmailAddress = model.EmailAddress;
                a.FirstName = model.FirstName;
                a.GenderId = model.GenderId;
                a.IDNumber = model.IDNumber;
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
                    AzureDataAccess.CorePA z = new AzureDataAccess.CorePA();
                    z.CoreUserId = userId;
                    z.ProvinceId = model.CapturePAView.provinceId;
                    z.IsActive = true;
                    db.CorePAs.Add(z);
                    db.SaveChanges();

                    updateProvinceToAssigned(model.CapturePAView.provinceId);
                }

                //TeamLeader
                else if(model.CoreUserTypeId == 3)
                {
                    AzureDataAccess.CoreTeamLeader y = new AzureDataAccess.CoreTeamLeader();
                    y.CoreProvinceId = model.CaptureTeamLeader.provinceId;
                    y.CoreUserId = userId;
                    y.IsActive = true;
                    db.CoreTeamLeaders.Add(y);
                    db.SaveChanges();
                }
            }
            catch
            {
                userId = -1;
            }

            return userId;
        }
        #endregion

        #region Update Priovince After Assigned
        public void updateProvinceToAssigned(int proId)
        {
            AzureDataAccess.CoreProvince d = db.CoreProvinces.FirstOrDefault(x => x.ProvinceId == proId);
            d.IsAssigned = true;
            db.SaveChanges();
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
            AzureDataAccess.AspNetUserRole r = new AzureDataAccess.AspNetUserRole();
            r.RoleId = roleId;
            r.UserId = userId;
            db.AspNetUserRoles.Add(r);
            db.SaveChanges();
        }

        public void AddUserToType(int coreUserId, int coreUserTypeId)
        {
            AzureDataAccess.CoreUserInType a = new AzureDataAccess.CoreUserInType();
            a.CoreUserId = coreUserId;
            a.CoreUserTypeId = coreUserTypeId;
            a.IsUserTypeActive = true;
            db.CoreUserInTypes.Add(a);
            db.SaveChanges();
        }

        #endregion

        #region Model Helpers Because Why Not!
        public List<CoreUserView> ConvertListModelToView(List<AzureDataAccess.CoreUser> modol)
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
                IDNumber = x.IDNumber,
                LastName = x.LastName,
                LastUpdatedDate = x.LastUpdatedDate,
                MiddleName = x.MiddleName,
                PhoneNumber = x.PhoneNumber,
            }).ToList();
        }
        #endregion
    }
}
