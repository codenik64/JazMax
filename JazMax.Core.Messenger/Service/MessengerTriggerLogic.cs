using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.DataAccess;
namespace JazMax.Core.Messenger.Service
{
    public class MessengerTriggerLogic
    {
        public List<MessengerTrigger> GetMessages()
        {
            DateTime Today = DateTime.Now.Date;
            using (JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                var q = (from t in db.MessengerTriggers
                         where t.IsCancelled == 0
                         //    && t.SendingDate.Date >= Today
                         && t.HasBeenProcessed == false
                         select t
                        ).ToList();

                return q;
            }
        }

        public void LogicalProcessing()
        {
            var List = GetMessages();
            int CountNumberOfEmails = 0;

            foreach (var m in List)
            {
                if (m.TriggerSetup == "SysUsers")
                {
                    #region Send Bulk Emails To System Users
                    var ListCoreUser = GetUsersForSelection(new CoreUserSearchFilter
                    {
                        BranchId = m.BranchId,
                        CoreUserTypeId = m.CoreUserTypeId,
                        ProvinceId = m.CoreProvinceId
                    });

                    foreach (var user in ListCoreUser)
                    {
                        CountNumberOfEmails++;

                        BusinessLogic.Messenger.JazMaxMail.SendMail(new Web.ViewModel.Messenger.Message
                        {
                            IsBodyHtml = true,
                            MessageBody = m.MessageBody,
                            SendTo = user.Email,
                            Subject = m.MessageSubject,
                            MessengerTriggerId = m.MessengerTriggerId
                        });
                    }
                    #endregion
                }
                else if (m.TriggerSetup == "LeadProspects")
                {
                    #region Send Bulk Email To Prospects
                    var ListProspect = GetLeadProspects(new LeadProspectsSearchFilter
                    {
                        BranchId = m.BranchId,
                        ProvinceId = m.CoreProvinceId
                    });

                    foreach (var Pros in ListProspect)
                    {
                        #region Send Using Template
                        if (m.MessengerTemplateId > 0) //Send Using An Existing Template
                        {
                            if (GetMessageTemplateBody(m.MessengerTemplateId) == null)
                            {
                                //If we can not find a valid email template, break operation back to message body
                                JazMax.BusinessLogic.Messenger.JazMaxMail.SendMail(new Web.ViewModel.Messenger.Message
                                {
                                    IsBodyHtml = true,
                                    MessageBody = m.MessageBody.Replace("#FullName#", Pros.FullName)
                                                   .Replace("#BranchName#", Pros.BranchName)
                                                   .Replace("#PropertyName#", Pros.LeadPropertyItem)
                                                   .Replace("#PropertyPrice#", Pros.PropertyPrice),
                                    SendTo = Pros.EmailAddress,
                                    Subject = m.MessageSubject,
                                    MessengerTriggerId = m.MessengerTriggerId
                                });
                            }
                            else
                            {
                                JazMax.BusinessLogic.Messenger.JazMaxMail.SendMail(new Web.ViewModel.Messenger.Message
                                {
                                    IsBodyHtml = true,
                                    MessageBody = GetMessageTemplateBody(m.MessengerTemplateId).Replace("#FullName#", Pros.FullName)
                                                 .Replace("#BranchName#", Pros.BranchName)
                                                 .Replace("#PropertyName#", Pros.LeadPropertyItem)
                                                 .Replace("#PropertyPrice#", Pros.PropertyPrice),
                                    SendTo = Pros.EmailAddress,
                                    Subject = m.MessageSubject,
                                    MessengerTriggerId = m.MessengerTriggerId
                                });
                            }


                        }
                        #endregion

                        #region Send Using Message Body
                        else
                        {

                            JazMax.BusinessLogic.Messenger.JazMaxMail.SendMail(new Web.ViewModel.Messenger.Message
                            {
                                IsBodyHtml = true,
                                MessageBody = m.MessageBody.Replace("#FullName#", Pros.FullName)
                                                   .Replace("#BranchName#", Pros.BranchName)
                                                   .Replace("#PropertyName#", Pros.LeadPropertyItem)
                                                   .Replace("#PropertyPrice#", Pros.PropertyPrice),
                                SendTo = Pros.EmailAddress,
                                Subject = m.MessageSubject,
                                MessengerTriggerId = m.MessengerTriggerId
                            });

                        }
                        #endregion

                    }
                    #endregion
                }

                #region Mark Messages As Processed and Log
                MarkAsProcessed(m.MessengerTriggerId, CountNumberOfEmails);
                Logging(m.MessengerTriggerId, CountNumberOfEmails);
                #endregion
            }
        }

        public string GetMessageTemplateBody(int MessageTemplateId)
        {
            using (JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                var query = (from t in db.MessengerTemplates
                             where t.MessengerTemplateId == MessageTemplateId
                             select t.TemplateHtml)?.FirstOrDefault();

                if (query != null)
                {
                    return query;
                }
                return null;
            }
        }

        private void MarkAsProcessed(int MessengerTriggerId, int NumberContacts = 0)
        {
            using (JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                MessengerTrigger table = db.MessengerTriggers?.FirstOrDefault(x => x.MessengerTriggerId == MessengerTriggerId);

                if (table != null)
                {
                    table.HasBeenProcessed = true;
                    table.ProcessedDateTime = DateTime.Now;
                    table.NumberOfContacts = NumberContacts;
                }
                db.SaveChanges();
            }
        }

        private IQueryable<CoreUserEmailData> GetUsersForSelection(CoreUserSearchFilter filter)
        {
            using (JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                var user = (from t in db.CoreUsers
                            join b in db.CoreAgents
                            on t.CoreUserId equals b.CoreUserId
                            join c in db.CoreBranches
                            on b.CoreBranchId equals c.BranchId
                            where t.IsActive == true
                            select new CoreUserEmailData
                            {
                                BranchId = (int)b.CoreBranchId,
                                CoreUserId = t.CoreUserId,
                                CoreUserTypeId = 4,
                                ProvinceId = (int)c.ProvinceId,
                                Email = t.EmailAddress,
                                Name = t.FirstName + " " + t.LastName
                            }).Union(from t in db.CoreUsers
                                     join b in db.CoreTeamLeaders
                                     on t.CoreUserId equals b.CoreUserId
                                     join c in db.CoreBranches
                                     on b.CoreTeamLeaderId equals c.CoreTeamLeaderId
                                     where t.IsActive == true
                                     select new CoreUserEmailData
                                     {
                                         BranchId = (int)c.BranchId,
                                         CoreUserId = t.CoreUserId,
                                         CoreUserTypeId = 3,
                                         ProvinceId = (int)c.ProvinceId,
                                         Email = t.EmailAddress,
                                         Name = t.FirstName + " " + t.LastName
                                     }).Union(from t in db.CoreUsers
                                              join b in db.CorePas
                                              on t.CoreUserId equals b.CoreUserId
                                              where t.IsActive == true
                                              select new CoreUserEmailData
                                              {
                                                  BranchId = 0,
                                                  CoreUserId = t.CoreUserId,
                                                  CoreUserTypeId = 5,
                                                  ProvinceId = (int)b.ProvinceId,
                                                  Email = t.EmailAddress,
                                                  Name = t.FirstName + " " + t.LastName
                                              }).ToList().AsQueryable();

                var mine = user;

                if (filter.BranchId > 0)
                {
                    mine = mine.Where(x => x.BranchId == filter.BranchId);
                }
                if (filter.CoreUserTypeId > 0)
                {
                    mine = mine.Where(x => x.CoreUserTypeId == filter.CoreUserTypeId);
                }
                if (filter.ProvinceId > 0)
                {
                    mine = mine.Where(x => x.ProvinceId == filter.ProvinceId);
                }
                return mine;
            }
        }

        private IQueryable<LeadProspects> GetLeadProspects(LeadProspectsSearchFilter filter)
        {
            using (JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                var query = (from t in db.Leads
                             join b in db.LeadProspects
                             on t.LeadId equals b.LeadId

                             join c in db.CoreBranches
                             on t.CoreBranchId equals c.BranchId

                             join e in db.PropertyListings
                             on t.PropertyListingId equals e.PropertyListingId
                             
                             select new LeadProspects
                             {
                                 BranchId = t.LeadId,
                                 EmailAddress = b.Email,
                                 FullName = b.FullName,
                                 LeadId = b.LeadId,
                                 ProvinceId = (int)c.ProvinceId,
                                 BranchName = c.BranchName,
                                 LeadPropertyItem = e.FriendlyName,
                                 PropertyPrice = e.Price.ToString()
                             })/*.GroupBy(b => b.EmailAddress).Select(g => g.FirstOrDefault())*/.ToList().AsQueryable();

                var mine = query;

                if (filter.BranchId > 0)
                {
                    mine = mine.Where(x => x.BranchId == filter.BranchId);
                }

                if (filter.ProvinceId > 0)
                {
                    mine = mine.Where(x => x.ProvinceId == filter.ProvinceId);
                }

                return mine;
            }
        }

        private void Logging(int MessengerTriggerId, int NumberOfMails)
        {
            using (JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                MessengerTriggerLog log = new MessengerTriggerLog()
                {
                    DateAdded = DateTime.Now,
                    MessengerTriggerId = MessengerTriggerId,
                    SendToValue = NumberOfMails.ToString()
                };
                db.MessengerTriggerLogs.Add(log);
                db.SaveChanges();
            }
        }

        public class CoreUserEmailData
        {
            public int CoreUserId { get; set; }
            public int BranchId { get; set; }
            public int ProvinceId { get; set; }
            public int CoreUserTypeId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }

        public class CoreUserSearchFilter
        {
            public int ProvinceId { get; set; }
            public int BranchId { get; set; }
            public int CoreUserTypeId { get; set; }
        }

        public class LeadProspects
        {
            public int LeadId { get; set; }
            public int BranchId { get; set; }
            public string FullName { get; set; }
            public string EmailAddress { get; set; }
            public int ProvinceId { get; set; }
            public string BranchName { get; set; }
            public string LeadPropertyItem { get; set; }
            public string PropertyPrice { get; set; }
        }

        public class LeadProspectsSearchFilter
        {
            public int ProvinceId { get; set; }
            public int BranchId { get; set; }
        }
    }
}
