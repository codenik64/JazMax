using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JazMax.Core.SystemHelpers
{
    public class JazMaxDropDownList
    {
        static JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();
        public static IEnumerable<SelectListItem> GetUserTypes()
        {
            List<SelectListItem> a = new List<SelectListItem>
            {
                new SelectListItem { Text = "-All User Groups-", Value = "-1".ToString() }
            };
            
                var bob = db.CoreUserTypes.Where(x => x.IsActive == true).Select(x => new SelectListItem
                {
                    Text = x.UserTypeName,
                    Value = x.CoreUserTypeId.ToString(),

                });
                a.AddRange(bob);
                return a;
            
        }

        public static IEnumerable<SelectListItem> GetAllProvince()
        {
            List<SelectListItem> a = new List<SelectListItem>
            {
                new SelectListItem { Text = "-All Provinces-", Value = "-1".ToString() }
            };
           
                var bob = db.CoreProvinces.Where(x => x.IsActive == true).Select(x => new SelectListItem
                {
                    Text = x.ProvinceName,
                    Value = x.ProvinceId.ToString(),

                });
                a.AddRange(bob);
                return a;
            
        }

        public static IEnumerable<SelectListItem> GetMessengerTemplates(int CoreBranchId)
        {
            List<SelectListItem> a = new List<SelectListItem>
            {
                new SelectListItem { Text = "-Select-", Value = "-1".ToString() },
                 new SelectListItem { Text = "-Create New Message-", Value = "-2".ToString() }
            };
         
                var bob = db.MessengerTemplates.Where(x => x.IsActive == true && x.CoreBranchId == CoreBranchId).Select(x => new SelectListItem
                {
                    Text = x.TemplateName,
                    Value = x.MessengerTemplateId.ToString(),

                });
                a.AddRange(bob);
                return a;
            
        }

        public static IEnumerable<SelectListItem> GetMessengerTemplates()
        {
            List<SelectListItem> a = new List<SelectListItem>
            {
                new SelectListItem { Text = "-Select-", Value = "-1".ToString() },
                 new SelectListItem { Text = "Create New Message", Value = "-2".ToString() }
            };
           
                var bob = db.MessengerTemplates.Where(x => x.IsActive == true).Select(x => new SelectListItem
                {
                    Text = x.TemplateName,
                    Value = x.MessengerTemplateId.ToString(),

                });
                a.AddRange(bob);
                return a;
            
        }

        public static IEnumerable<SelectListItem> GetAllBranches()
        {
           
                List<SelectListItem> a = new List<SelectListItem>
            {
                new SelectListItem { Text = "-All Branches-", Value = "-1".ToString() }
            };
                var bob = db.CoreBranches.Where(x => x.IsActive == true).Select(x => new SelectListItem
                {
                    Text = x.BranchName,
                    Value = x.BranchId.ToString(),

                });
                a.AddRange(bob);
                return a;
            
        }

        public static IEnumerable<SelectListItem> GetProvinceNotAssigned()
        {
             return db.CoreProvinces.Where(x => x.IsActive == true && x.IsAssigned == false).Select(x => new SelectListItem
                {
                    Text = x.ProvinceName,
                    Value = x.ProvinceId.ToString(),
                });
            
        }

        public static IEnumerable<SelectListItem> GetAllTeamLeaders()
        {
          
                var q = from a in db.CoreUsers
                        join b in db.CoreTeamLeaders
                        on a.CoreUserId equals b.CoreUserId
                        select new SelectListItem
                        {
                            Text = a.FirstName + " " + a.LastName,
                            Value = b.CoreTeamLeaderId.ToString()
                        };
                return q;
            
        }

        public static IEnumerable<SelectListItem> GetAllBranchesBasedOnProvince(int proId)
        {
            
                var q = from a in db.CoreBranches
                        where a.ProvinceId == proId
                        select new SelectListItem
                        {
                            Text = a.BranchName,
                            Value = a.BranchId.ToString()
                        };
                return q;
            
        }

        public static IEnumerable<SelectListItem> GetAllTeamLeadersBasedOnPAProvince(int proId)
        {
            
                var q = from a in db.CoreUsers
                        join b in db.CoreTeamLeaders
                        on a.CoreUserId equals b.CoreUserId
                        where b.CoreProvinceId == proId
                        select new SelectListItem
                        {
                            Text = a.FirstName + " " + a.LastName,
                            Value = b.CoreTeamLeaderId.ToString()
                        };
                return q;
           
        }

        public static IEnumerable<SelectListItem> GetAllTeamLeadersThatAreNotAssigned(int proId)
        {
            
                List<int?> teamLeaderId = db.CoreBranches.Where(x => x.ProvinceId == proId).Select(x => x.CoreTeamLeaderId).ToList();

                var q = from a in db.CoreUsers
                        join b in db.CoreTeamLeaders
                        on a.CoreUserId equals b.CoreUserId
                        where b.CoreProvinceId == proId &&
                        !teamLeaderId.Contains(b.CoreTeamLeaderId)
                        select new SelectListItem
                        {
                            Text = a.FirstName + " " + a.LastName,
                            Value = b.CoreTeamLeaderId.ToString()
                        };

                return q.ToList();
            

        }

        public static IEnumerable<SelectListItem> GetAllPropertyFeatures()
        {
            
                var q = from a in db.PropertyFeatures
                        where a.IsFeatureActive == true
                        select new SelectListItem
                        {
                            Text = a.FeatureName,
                            Value = a.PropertyFeatureId.ToString()
                        };
                return q;
            
        }

        public static IEnumerable<SelectListItem> GetAllPropertyTypes()
        {
           
                var q = from a in db.PropertyTypes
                        where a.IsActive == true
                        select new SelectListItem
                        {
                            Text = a.TypeName,
                            Value = a.PropertyTypeId.ToString()
                        };
                return q;
            
        }

        public static IEnumerable<SelectListItem> GetAgentsForBranch(int BranchId)
        {
            
                List<SelectListItem> au = new List<SelectListItem>
            {
                new SelectListItem { Text = "-Select-", Value = "-1".ToString() }
            };
                var bob = from a in db.CoreUsers
                          join b in db.CoreAgents
                          on a.CoreUserId equals b.CoreUserId
                          join c in db.CoreBranches
                          on b.CoreBranchId equals c.BranchId
                          where c.BranchId == BranchId
                          select new SelectListItem
                          {
                              Text = a.FirstName + " " + a.LastName,
                              Value = b.CoreAgentId.ToString()
                          };
                au.AddRange(bob);
                return au;
            
        }

        public static IEnumerable<SelectListItem> GetAllAgents()
        {
           
                List<SelectListItem> au = new List<SelectListItem>
            {
                new SelectListItem { Text = "-Select-", Value = "-1".ToString() }
            };
                var bob = from a in db.CoreUsers
                          join b in db.CoreAgents
                          on a.CoreUserId equals b.CoreUserId
                          join c in db.CoreBranches
                          on b.CoreBranchId equals c.BranchId
                          select new SelectListItem
                          {
                              Text = a.FirstName + " " + a.LastName,
                              Value = b.CoreAgentId.ToString()
                          };
                au.AddRange(bob);
                return au;
            
        }

        public static IEnumerable<SelectListItem> GetAllAgentsByCoreUserId()
        {
           
                List<SelectListItem> au = new List<SelectListItem>
            {
                new SelectListItem { Text = "-Select-", Value = "-1".ToString() }
            };
                var bob = from a in db.CoreUsers
                          join b in db.CoreAgents
                          on a.CoreUserId equals b.CoreUserId
                          join c in db.CoreBranches
                          on b.CoreBranchId equals c.BranchId
                          select new SelectListItem
                          {
                              Text = a.FirstName + " " + a.LastName,
                              Value = b.CoreUserId.ToString()
                          };
                au.AddRange(bob);
                return au;
            
        }

        public static IEnumerable<SelectListItem> GetAllPropertyPriceTypes()
        {
           
                return (from a in db.PropertyListingPricingTypes
                        where a.IsActive == true
                        select new SelectListItem
                        {
                            Text = a.TypeName,
                            Value = a.PropertyListingPricingTypeId.ToString()
                        });
            
        }

        public static IEnumerable<SelectListItem> GetAllPropertyListings()
        {
            
                return (from a in db.PropertyListings
                        where a.IsListingActive == true
                        select new SelectListItem
                        {
                            Text = a.FriendlyName,
                            Value = a.PropertyListingId.ToString()
                        });
            
        }

        public static IEnumerable<SelectListItem> GetAllPropertyListingsInMyBranch(int BranchId)
        {
            
                return (from a in db.PropertyListings
                        where a.IsListingActive == true
                        && a.BranchId == BranchId
                        select new SelectListItem
                        {
                            Text = a.FriendlyName,
                            Value = a.PropertyListingId.ToString()
                        });
            
        }

        public static IEnumerable<SelectListItem> GetAllPropertyListingsAssignedToMe(int CoreUserId)
        {
            
                return (from a in db.PropertyListings
                        where a.IsListingActive == true
                        join b in db.PropertyListingAgents
                        on a.PropertyListingId equals b.PropertyListingId
                        join c in db.CoreAgents
                        on b.AgentId equals c.CoreAgentId
                        where c.CoreUserId == CoreUserId
                        select new SelectListItem
                        {
                            Text = a.FriendlyName,
                            Value = a.PropertyListingId.ToString()
                        });
            
        }

        public static IEnumerable<SelectListItem> GetLeadActivityTypes()
        {
           
                return (from t in db.LeadActivities
                        where t.IsActive == true && t.IsSystem == false
                        select new SelectListItem
                        {
                            Text = t.ActivityName,
                            Value = t.LeadActivityId.ToString()
                        });
            
        }

        public static IEnumerable<SelectListItem> GetLeadTypes()
        {
           
                List<SelectListItem> a = new List<SelectListItem>
            {
                new SelectListItem { Text = "-Select-", Value = "-1".ToString() }
            };
                var bob = (from t in db.LeadTypes
                           where t.IsActive == true
                           select new SelectListItem
                           {
                               Text = t.TypeName,
                               Value = t.LeadTypeId.ToString()
                           });
                a.AddRange(bob);
                return a;
            
        }

        public static IEnumerable<SelectListItem> GetLeadStatusTypes()
        {
            
                List<SelectListItem> a = new List<SelectListItem>
            {
                new SelectListItem { Text = "-Select-", Value = "-1".ToString() }
            };
                var bob = (from t in db.LeadStatus
                           where t.IsActive == true
                           select new SelectListItem
                           {
                               Text = t.StatusName,
                               Value = t.LeadStatusId.ToString()
                           });
                a.AddRange(bob);
                return a;
            
        }

        public static IEnumerable<SelectListItem> GetLeadSources()
        {
           
                return (from t in db.LeadSources
                        where t.IsActive == true
                        select new SelectListItem
                        {
                            Text = t.SourceName,
                            Value = t.SourceName.ToString()
                        });
            
        }

        public static IEnumerable<SelectListItem> GetGender()
        {

            List<SelectListItem> listItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Male",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "Female",
                    Value = "2",
                }
            };

            return listItems;
        }

        public IQueryable<SelectListItem> GetAllFileTypes()
        {

            var bob = db.CoreFileTypes.Where(x => x.IsActive == true).Select(x => new SelectListItem
            {
                Text = x.TypeName,
                Value = x.CoreFileTypeId.ToString(),

            });

            return bob.AsQueryable();

        }

        public IQueryable<SelectListItem> GetAllDocumentTypes()
        {

            var bob = db.CoreDocumentTypes.Where(x => x.IsActive == true).Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = x.CoreFileCategoryId.ToString(),

            });

            return bob.AsQueryable();

        }

        public IQueryable<SelectListItem> GetAllUserEmails()
        {

            var bob = db.CoreUsers.ToList().Where(x => x.EmailAddress != JazMaxIdentityHelper.UserName).Select(x => new SelectListItem
            {
                Text = x.EmailAddress,
                Value = x.EmailAddress.ToString(),

            });

            return bob.AsQueryable();

        }

        public IQueryable<SelectListItem> GetAllDocs()
        {

            var bob = db.CoreFileUploads.ToList().Select(x => new SelectListItem
            {
                Text = x.FileNames,
                Value = x.FileUploadId.ToString(),

            });

            return bob.AsQueryable();

        }

    }
}
