using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.UserAccountView;

namespace JazMax.BusinessLogic.UserAccounts
{
    public class CoreBranchService
    {
        private static JazMax.AzureDataAccess.JazMaxDBProdContext db = new AzureDataAccess.JazMaxDBProdContext();

        public List<CoreBranchView> GetAll()
        {
            return ConvertListModelToView(db.CoreBranches.Where(x => x.IsActive == true).ToList());
        }

        //Get Agents Branches
        public List<CoreBranchView> GetMyBranchs(int provinceId)
        {
            return GetAll().Where(x => x.ProvinceId == provinceId).ToList();
        }

        private static List<CoreBranchView> ConvertListModelToView (List<AzureDataAccess.CoreBranch> model)
        {
            return model.Select(x => new CoreBranchView
            {
                EmailAddress = x.EmailAddress,
                IsActive = x.IsActive,
                StreetAddress = x.StreetAddress,
                BranchId = x.BranchId,
                BranchName = x.BranchName,
                City = x.City,
                CoreTeamLeaderId = x.CoreTeamLeaderId,
                Phone = x.Phone,
                ProvinceId = x.ProvinceId,
                Suburb =x.Suburb
            }).ToList();
        }

        public void CreateNewBranch(CoreBranchView model)
        {
            try
            {
                db.CoreBranches.Add(ConvertViewToModel(model));
                db.SaveChanges();
            }
            catch
            {
                //Error Logging Goes Here 
                //April 2018
            }
        }

        private static AzureDataAccess.CoreBranch ConvertViewToModel(CoreBranchView m)
        {
            AzureDataAccess.CoreBranch a = new AzureDataAccess.CoreBranch();
            a.BranchId = m.BranchId;
            a.BranchName = m.BranchName;
            a.City = m.City;
            a.CoreTeamLeaderId = m.CoreTeamLeaderId;
            a.EmailAddress = m.EmailAddress;
            a.IsActive = m.IsActive;
            a.Phone = m.Phone;
            a.ProvinceId = m.ProvinceId;
            a.StreetAddress = m.StreetAddress;
            a.Suburb = m.Suburb;
            return a;
        }

    }
}
