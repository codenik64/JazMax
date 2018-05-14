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
        private static JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();

        public Guid? CreateUserRole(string roleName)
        {
            Guid random = Guid.NewGuid();
            try
            {
                DataAccess.AspNetRole r = new DataAccess.AspNetRole()
                {
                    Id = random.ToString(),
                    Name = roleName
                };
                db.AspNetRoles.Add(r);
                db.SaveChanges();

                return random;
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
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
            catch(Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        private DataAccess.CoreUserType ConvertToModel(CoreUserTypeView m)
        {
            DataAccess.CoreUserType a = new DataAccess.CoreUserType()
            {
                IsActive = m.IsActive,
                UserRoleId = m.UserRoleId,
                UserTypeName = m.UserTypeName
            };
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
