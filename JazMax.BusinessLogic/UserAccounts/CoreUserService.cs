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

    

        //LEGACY CODE
        public List<CoreUserView> GetAll()
        {
            var query = from t in db.CoreUsers
                        where t.IsActive == true
                        select t;

            return ConvertListModelToView(query.ToList());
        }


        public int GetCount()
        {
            var count = 0;
            var query = (from t in db.CoreUserInTypes
                        
                        select t).FirstOrDefault();

            if (query.CoreUserTypeId == 2)
            {
                var ok = GetAll().Where(x => x.CoreUserTypeId == 2).Count();
                count = ok;
            }
            else if (query.CoreUserTypeId == 3)
            {
                var ok = GetAll().Where(x => x.CoreUserTypeId == 3).Count();
                count = ok;
            }
            else if (query.CoreUserTypeId == 4)
            {
                var ok = GetAll().Where(x => x.CoreUserTypeId == 4).Count();
                count = ok;
            }

            else if (query.CoreUserTypeId == 5)
            {
                var ok = GetAll().Where(x => x.CoreUserTypeId == 5).Count();
                count = ok;
            }


            return count;
        }
        

        #region Get All CoreUsers
        public IQueryable<CoreUserDetails> GetAllSystemUsers(List<bool> isActiveList)
        {
            IQueryable<CoreUserDetails> coreUserList = (from a in db.CoreUsers
                                                        join b in db.CoreUserInTypes
                                                        on a.CoreUserId equals b.CoreUserId
                                                        join c in db.CoreUserTypes
                                                        on b.CoreUserTypeId equals c.CoreUserTypeId
                                                        where a.IsActive == isActiveList.Contains((bool)a.IsActive)
                                                        select new CoreUserDetails
                                                        {
                                                            CellPhone = a.CellPhone,
                                                            EmailAddress = a.EmailAddress,
                                                            FirstName = a.FirstName,
                                                            GenderId = a.GenderId == 1 ? "Male" : "Female",
                                                            IDNumber = a.IdNumber,
                                                            LastName = a.LastName,
                                                            MiddleName = a.MiddleName,
                                                            PhoneNumber = a.PhoneNumber,
                                                            UserType = c.UserTypeName,
                                                            CoreUserId = a.CoreUserId
                                                        }).AsQueryable();

            return coreUserList;
        }
        #endregion    

        #region Gets Core User Details    
        public CoreUserDetails GetUserDetails(int coreUserID)
        {

            CoreUserDetails coreUserList = (from a in db.CoreUsers
                                            join b in db.CoreUserInTypes
                                            on a.CoreUserId equals b.CoreUserId
                                            join c in db.CoreUserTypes
                                            on b.CoreUserTypeId equals c.CoreUserTypeId
                                            where a.CoreUserId == coreUserID
                                            select new CoreUserDetails
                                            {
                                                CellPhone = a.CellPhone,
                                                EmailAddress = a.EmailAddress,
                                                FirstName = a.FirstName,
                                                GenderId = a.GenderId == 1 ? "Male" : "Female",
                                                IDNumber = a.IdNumber,
                                                LastName = a.LastName,
                                                MiddleName = a.MiddleName,
                                                PhoneNumber = a.PhoneNumber,
                                                UserType = c.UserTypeName,
                                                CoreUserId = a.CoreUserId,
                                                CoreUserTypeId = b.CoreUserTypeId,
                                                isActive = (bool)a.IsActive
                                            }).AsQueryable().FirstOrDefault();

            //Gets The Branch Details For The User
            coreUserList.UserBranchDetails = GetBranchDetailsForUser(coreUserList.CoreUserId, coreUserList.CoreUserTypeId);

            //Get Provbince Details For user
            coreUserList.UserProvinceDetails = GetProvineDetailsForUser(coreUserList.CoreUserId, coreUserList.CoreUserTypeId);

            //Gets Edit Log For User <List>
            //Swicthed off for now
            //coreUserList.UserEditLog = GetUserEditLog(coreUserList.CoreUserId);

            return coreUserList;
        }

        public static UserBranchDetails GetBranchDetailsForUser(int CoreUserId, int coreUserType)
        {
            UserBranchDetails m = new UserBranchDetails();
            //AGENT 
            if (coreUserType == 4)
            {
                int GetAgentBranchId = 0;
                try
                {
                    GetAgentBranchId = (int)db.CoreAgents.FirstOrDefault(x => x.CoreUserId == CoreUserId).CoreBranchId;
                }
                catch
                {
                    GetAgentBranchId = 0;
                }

                //The agent has a branch assigned to them
                if (GetAgentBranchId > 0)
                {
                    CoreBranchView model = CoreBranchService.DetailsNew(db, GetAgentBranchId).CoreBranchView;
                    m.BranchName = model.BranchName;
                    m.BranchId = model.BranchId;
                    m.Province = model.ProvinceName;
                    m.TeamLeader = model.TeamLeaderName;
                    m.HasResult = true;
                }
                //if they do not, dont break the front end
                else
                {
                    m.HasResult = false;
                }
            }
            //TeamLeader
            else if (coreUserType == 3)
            {
                int GetTeamLeaderBranch = 0;
                int GetBranchId = 0;

                try
                {
                    GetTeamLeaderBranch = db.CoreTeamLeaders.FirstOrDefault(x => x.CoreUserId == CoreUserId).CoreTeamLeaderId;
                }
                catch
                {
                    GetTeamLeaderBranch = 0;
                }

                if (GetTeamLeaderBranch != 0)
                {
                    GetBranchId = db.CoreBranches.FirstOrDefault(x => x.CoreTeamLeaderId == GetTeamLeaderBranch).BranchId;
                    CoreBranchView model = CoreBranchService.DetailsNew(db, GetBranchId).CoreBranchView;
                    m.BranchName = model.BranchName;
                    m.BranchId = model.BranchId;
                    m.Province = model.ProvinceName;
                    m.TeamLeader = model.TeamLeaderName;
                    m.HasResult = true;
                }
                else
                {
                    m.BranchName = "No Branch Assigned To Teamleader";
                    m.HasResult = false;
                }
            }
            return m;

        }

        public static UserProvinceDetails GetProvineDetailsForUser(int ProvinceId, int UserTypeId)
        {
            if (UserTypeId == 4)
            {
                UserProvinceDetails b = new UserProvinceDetails();

                var model = CoreProvinceService.GetProvinceDetails(ProvinceId);
                b.Pronvince = model.ProvinceName;
                b.PAInfo = model.PAName;

                return b;
            }
            return null;

        }

        public static List<JazMax.Web.ViewModel.ChangeLog.EditLogView> GetUserEditLog(int coreUserId)
        {
            return ChangeLog.ChangeLogService.GetEditLog("CoreUser", coreUserId);
        }
        #endregion

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

                    UpdateProvinceToAssigned(model.CapturePAView.provinceId);
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
        public void UpdateProvinceToAssigned(int proId)
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

        #region Update CoreUser Details
        public void UpdateCoreUser(int coreUserId, CoreUserDetails model, int CoreSystemUserId)
        {
            if (coreUserId > 0)
            {
                var user = db.CoreUsers.FirstOrDefault(x => x.CoreUserId == coreUserId);
                if (user != null)
                {
                    ChangeLog.ChangeLogService.tableName = "CoreUser";
                    ChangeLog.ChangeLogService.tableKey = user.CoreUserId;
                    ChangeLog.ChangeLogService.LoggedInUserId = CoreSystemUserId;

                    #region Edit Logging
                    ChangeLog.ChangeLogService.LogChange(user.FirstName, model.FirstName, "First Name");
                    ChangeLog.ChangeLogService.LogChange(user.LastName, model.LastName, "Last Name");
                    ChangeLog.ChangeLogService.LogChange(user.MiddleName, model.MiddleName, "Middle Name");
                    ChangeLog.ChangeLogService.LogChange(user.PhoneNumber, model.PhoneNumber, "Phone Number");
                    ChangeLog.ChangeLogService.LogChange(user.IdNumber, model.IDNumber, "ID Number");
                    #endregion

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.LastUpdatedDate = DateTime.Now;
                    user.MiddleName = model.MiddleName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.CellPhone = model.CellPhone;
                    user.IdNumber = model.IDNumber;
                    db.SaveChanges();
                }
            }
        }
        #endregion

        public static void DeactiveCoreUser(int coreUserId, int LoggedInUserId, bool isActiveAction)
        {
            try
            {
                var user = db.CoreUsers.FirstOrDefault(x => x.CoreUserId == coreUserId);

                ChangeLog.ChangeLogService.tableName = "CoreUser";
                ChangeLog.ChangeLogService.tableKey = user.CoreUserId;
                ChangeLog.ChangeLogService.LoggedInUserId = LoggedInUserId;

                if (user != null)

                    if (isActiveAction)
                    {
                        ChangeLog.ChangeLogService.LogChange(ChangeLog.ChangeLogService.GetBoolString((bool)user.IsActive), ChangeLog.ChangeLogService.GetBoolString(false), "Account Status");

                        user.IsActive = false;
                    }
                    else
                    {
                        ChangeLog.ChangeLogService.LogChange(ChangeLog.ChangeLogService.GetBoolString((bool)user.IsActive),ChangeLog.ChangeLogService.GetBoolString(true), "Account Status");
                        user.IsActive = true;
                    }

                db.SaveChanges();
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        #region Move Agent
        public static void MoveAgent(int CoreUserId, int LoggedInUserId, int BranchId)
        {
            var Agent = db.CoreAgents.FirstOrDefault(x => x.CoreUserId == CoreUserId);

            if(Agent != null)
            {
                ChangeLog.ChangeLogService.tableName = "CoreUser";
                ChangeLog.ChangeLogService.tableKey = CoreUserId;
                ChangeLog.ChangeLogService.LoggedInUserId = LoggedInUserId;

                ChangeLog.ChangeLogService.LogChange(
                           GetBranchNamne((int)Agent.CoreBranchId),
                            GetBranchNamne(BranchId), "Branch");
                Agent.CoreBranchId = BranchId;
            }
            db.SaveChanges();
        }

        private static string GetBranchNamne(int BranchId)
        {
            return db.CoreBranches.Where(x => x.BranchId == BranchId).FirstOrDefault().BranchName;
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
