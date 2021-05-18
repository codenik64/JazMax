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
        private static JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();


        public List<CoreBranchView> GetAll()
        {
            return ConvertListModelToView(db.CoreBranches.Where(x => x.IsActive == true).ToList());
        }

        //Get Agents Branches Based on ProvineID
        public List<CoreBranchView> GetMyBranchs(int provinceId)
        {
            return GetAll().Where(x => x.ProvinceId == provinceId).ToList();
        }

        private static List<CoreBranchView> ConvertListModelToView (List<DataAccess.CoreBranch> model)
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
                Suburb =x.Suburb,

            }).ToList();
        }

        public void CreateNewBranch(CoreBranchView model)
        {
            try
            {
                db.CoreBranches.Add(ConvertViewToModel(model));
                db.SaveChanges();
            }
            catch(Exception e)
            {
                AuditLog.ErrorLog.LogError(db, e, 0);
            }
        }

        private static DataAccess.CoreBranch ConvertViewToModel(CoreBranchView m)
        {
            DataAccess.CoreBranch a = new DataAccess.CoreBranch()
            {
                BranchId = m.BranchId,
                BranchName = m.BranchName,
                City = m.City,
                CoreTeamLeaderId = m.CoreTeamLeaderId,
                EmailAddress = m.EmailAddress,
                IsActive = m.IsActive,
                Phone = m.Phone,
                ProvinceId = m.ProvinceId,
                StreetAddress = m.StreetAddress,
                Suburb = m.Suburb
            };
            return a;
        }

        public BranchDetailsView Details(int branchId)
        {
            AgentService bb = new AgentService();

            var query = from a in db.CoreBranches
                        join b in db.CoreTeamLeaders
                        on a.CoreTeamLeaderId equals b.CoreTeamLeaderId
                        join c in db.CoreUsers
                        on b.CoreUserId equals c.CoreUserId
                        join d in db.CoreProvinces
                        on a.ProvinceId equals d.ProvinceId
                        where a.BranchId == branchId
                        select new CoreBranchView
                        {
                            BranchId = a.BranchId,
                            EmailAddress = a.EmailAddress,
                            IsActive = a.IsActive,
                            StreetAddress = a.StreetAddress,
                            BranchName = a.BranchName,
                            City = a.City,
                            CoreTeamLeaderId = a.CoreTeamLeaderId,
                            Phone = a.Phone,
                            ProvinceId = a.ProvinceId,
                            ProvinceName = d.ProvinceName,
                            Suburb = a.Suburb,
                            TeamLeaderName = c.FirstName + " " + c.LastName
                        };

            
            List<AgentDetailsView> y = bb.GetMyAgentInBranch(branchId);
            BranchDetailsView bru = new BranchDetailsView()
            {
                AgentDetailsView = y,
                CoreBranchView = query.FirstOrDefault()
            };
            return bru;
        }

        public List<CoreBranchView> GetAllBranches()
        {

            var query = from a in db.CoreBranches
                        join b in db.CoreTeamLeaders
                        on a.CoreTeamLeaderId equals b.CoreTeamLeaderId
                        join c in db.CoreUsers
                        on b.CoreUserId equals c.CoreUserId
                        join d in db.CoreProvinces
                        on a.ProvinceId equals d.ProvinceId
                        select new CoreBranchView
                        {
                            BranchId = a.BranchId,
                            EmailAddress = a.EmailAddress,
                            IsActive = a.IsActive,
                            StreetAddress = a.StreetAddress,
                            BranchName = a.BranchName,
                            City = a.City,
                            CoreTeamLeaderId = a.CoreTeamLeaderId,
                            Phone = a.Phone,
                            ProvinceId = a.ProvinceId,
                            ProvinceName = d.ProvinceName,
                            Suburb = a.Suburb,
                            TeamLeaderName = c.FirstName + "" + c.LastName
                        };
           
            return query.ToList();
        }


        public static BranchDetailsView DetailsNew(JazMax.DataAccess.JazMaxDBProdContext dbcon, int branchId)
        {
            AgentService bb = new AgentService();

            var query = from a in dbcon.CoreBranches
                        join b in dbcon.CoreTeamLeaders
                        on a.CoreTeamLeaderId equals b.CoreTeamLeaderId
                        join c in dbcon.CoreUsers
                        on b.CoreUserId equals c.CoreUserId
                        join d in dbcon.CoreProvinces
                        on a.ProvinceId equals d.ProvinceId
                        where a.BranchId == branchId
                        select new CoreBranchView
                        {
                            BranchId = a.BranchId,
                            EmailAddress = a.EmailAddress,
                            IsActive = a.IsActive,
                            StreetAddress = a.StreetAddress,
                            BranchName = a.BranchName,
                            City = a.City,
                            CoreTeamLeaderId = a.CoreTeamLeaderId,
                            Phone = a.Phone,
                            ProvinceId = a.ProvinceId,
                            ProvinceName = d.ProvinceName,
                            Suburb = a.Suburb,
                            TeamLeaderName = c.FirstName + " " + c.LastName
                        };


            List<AgentDetailsView> y = bb.GetMyAgentInBranch(branchId);
            BranchDetailsView bru = new BranchDetailsView()
            {
                AgentDetailsView = y,
                CoreBranchView = query.FirstOrDefault()
            };
            return bru;
        }



        public bool Update(CoreBranchView model, int CoreSystemUserId)
        {
            try
            {
            

                var data = (from a in db.CoreBranches
                            where model.BranchId == a.BranchId
                            select a).FirstOrDefault();


                ChangeLog.ChangeLogService.tableName = "CoreBranch";
                ChangeLog.ChangeLogService.tableKey = data.BranchId;
                ChangeLog.ChangeLogService.LoggedInUserId = CoreSystemUserId;

                if (model.CoreTeamLeaderId == null)
                {
                    data.CoreTeamLeaderId = data.CoreTeamLeaderId;
                }
                else
                {
                    ChangeLog.ChangeLogService.LogChange(data.CoreTeamLeaderId.ToString(), model.CoreTeamLeaderId.ToString(), "CoreTeamLeaderId");
                    data.CoreTeamLeaderId = model.CoreTeamLeaderId;

                }

                #region Edit Log
                ChangeLog.ChangeLogService.LogChange(data.BranchName, model.BranchName, "BranchName");
                ChangeLog.ChangeLogService.LogChange(data.City, model.City, "City");
                ChangeLog.ChangeLogService.LogChange(data.EmailAddress, model.EmailAddress, "EmailAddress");
                ChangeLog.ChangeLogService.LogChange(data.IsActive.ToString(), model.IsActive.ToString(), "IsActive");
                ChangeLog.ChangeLogService.LogChange(data.Phone, model.Phone, "Phone");
                ChangeLog.ChangeLogService.LogChange(data.ProvinceId.ToString(), model.ProvinceId.ToString(), "ProvinceId");
                ChangeLog.ChangeLogService.LogChange(data.StreetAddress, model.StreetAddress, "StreetAddress");
                #endregion

                data.BranchName = model.BranchName;
                data.City = model.City;
                data.EmailAddress = model.EmailAddress;
                data.IsActive = model.IsActive;
                data.Phone = model.Phone;
                data.ProvinceId = model.ProvinceId;
                data.StreetAddress = model.StreetAddress;
                data.Suburb = model.Suburb;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(db, e, 0);
                return false;
            }
        }

    }
}
