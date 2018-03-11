using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.UserAccountView;

namespace JazMax.BusinessLogic.UserAccounts
{
    public class UserGroupService
    {
        private static JazMax.AzureDataAccess.JazMaxDBProdContext db = new AzureDataAccess.JazMaxDBProdContext();

        public Guid? CreateUserRole(string roleName)
        {
            Guid random = Guid.NewGuid();
            try
            {
                AzureDataAccess.AspNetRole r = new AzureDataAccess.AspNetRole();
                r.Id = random.ToString();
                r.Name = roleName;
                db.AspNetRoles.Add(r);
                db.SaveChanges();

                return random;
            }
            catch
            {
                return null;
            }
        }

        public void CreateUserGroup(CoreUserTypeView model)
        {
            try
            {
                model.UserRoleId = CreateUserRole(model.UserTypeName).ToString();
                db.CoreUserTypes.Add(ConvertToModel(model));
                db.SaveChanges();
            }
            catch { }
        }

        private AzureDataAccess.CoreUserType ConvertToModel(CoreUserTypeView m)
        {
            AzureDataAccess.CoreUserType a = new AzureDataAccess.CoreUserType();
            a.IsActive = m.IsActive;
            a.UserRoleId = m.UserRoleId;
            a.UserTypeName = m.UserTypeName;
            return a;
        }

        public List<CoreUserTypeView> GetAll()
        {
            var query = from t in db.CoreUserTypes
                         where t.IsActive == true
                         select new CoreUserTypeView
                         {
                             CoreUserTypeId = t.CoreUserTypeId,
                             IsActive = t.IsActive,
                             UserRoleId = t.UserRoleId,
                             UserTypeName = t.UserTypeName
                         };
            return query.ToList();
               
        }
    }
}
