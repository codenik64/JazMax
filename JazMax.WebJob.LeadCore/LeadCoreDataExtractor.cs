using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.DataAccess;
namespace JazMax.WebJob.LeadCore
{
    public class LeadCoreDataExtractor
    {
        private List<LeadRawData> GetRawData()
        {
            using (JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                var query = (from t in db.LeadRawDatas
                             where t.ServiceCompleted == false
                             orderby t.DateCreated ascending
                             select t)?.ToList();

                return query;
            }
        }

        private void ProcessLeadData(LeadRawData data)
        {
            using (JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                #region Linked Lead Check
                int BranchId = GetPropertyListing(data.PropertyListingId).BranchId;
                var LinkedLeadData = GetLeadsWithSameProspect(data.FullName, data.ContactNumber, data.Email, BranchId);
                #endregion

                #region Create New Lead
                int LeadID = 0;
                Lead lead = new Lead()
                {
                    IsCompleted = false,
                    DateCreated = DateTime.Now,
                    LeadStatusId = 1,
                    LeadSourceId = FindSource(data.SourceName),
                    PropertyListingId = data.PropertyListingId,
                    LeadTypeId = data.IsManualCapture ? (int)LeadType.NewManualLead : (int)LeadType.NewLead,
                    HasLinkedLead = LinkedLeadData != null ? true : false,
                    ServiceCompleted = false,
                    CoreBranchId = BranchId,
                   
                };
                db.Leads.Add(lead);
                db.SaveChanges();
                LeadID = lead.LeadId;

                JazMax.Core.Leads.Activity.ActivityCreation.CaptureLeadActivity(new Core.Leads.Activity.LeadActivity
                {
                    CoreUserId = -999,
                    LeadActivityId = 1,
                    LeadId = LeadID,
                    IsSystem = true,
                    Description = "New Lead Has Been Created on " + DateTime.Now.ToLongDateString()
                });

                Console.WriteLine("Lead has been inserted. LeadID - " + LeadID.ToString());
                #endregion

                #region Prospect
                LeadProspect prop = new LeadProspect()
                {
                    FullName = data.FullName,
                    LeadId = LeadID,
                    ContactNumber = data.ContactNumber,
                    Comments = data.Comments,
                    Email = data.Email
                };
                db.LeadProspects.Add(prop);
                db.SaveChanges();
                Console.WriteLine("Lead Prospect Has Been Inserted " + prop.LeadProspectId);
                #endregion

                #region Assign Agents To Lead
                if (data.IsManualCapture)
                {
                    int? GetAgentId = GetAgentIdFromCoreUserId((int)data.CoreUserId);

                    //FOUND AN AGENT
                    if (GetAgentId != null)
                    {
                        LeadAgent agent = new LeadAgent()
                        {
                            LeadId = LeadID,
                            IsActive = true,
                            AgentId = (int)GetAgentId
                        };
                        db.LeadAgents.Add(agent);
                        db.SaveChanges();
                        Console.WriteLine("Manual Agent Added " + agent.LeadAgentsId);

                        //Add Lead Activity For Manual Lead
                        JazMax.Core.Leads.Activity.ActivityCreation.CaptureLeadActivity(new Core.Leads.Activity.LeadActivity
                        {
                            CoreUserId = (int)data.CoreUserId,
                            LeadActivityId = 2,
                            LeadId = LeadID,
                            IsSystem = true,
                            Description = "Manual Lead Has Been Assigned To Agent : " + GetAgentName((int)data.CoreUserId),
                        });
                    }
                    else
                    {
                        foreach (var PropAgent in AgentsForListing(data.PropertyListingId))
                        {
                            LeadAgent agent = new LeadAgent()
                            {
                                LeadId = LeadID,
                                AgentId = PropAgent.AgentId,
                                IsActive = true
                            };
                            db.LeadAgents.Add(agent);
                            db.SaveChanges();

                            int cuid = GetCoreUserIdFromAgentId(PropAgent.AgentId);
                            //Add Lead Activity For Lead
                            JazMax.Core.Leads.Activity.ActivityCreation.CaptureLeadActivity(new Core.Leads.Activity.LeadActivity
                            {
                                CoreUserId = cuid,
                                LeadActivityId = 2,
                                LeadId = LeadID,
                                IsSystem = true,
                                Description = "Lead Has Been Assigned To Agent : " + GetAgentName(cuid),
                            });

                            Console.WriteLine("Agents have been added to the lead " + agent.LeadAgentsId);
                        }
                    }
                  
                }
                else
                {
                    foreach (var PropAgent in AgentsForListing(data.PropertyListingId))
                    {
                        LeadAgent agent = new LeadAgent()
                        {
                            LeadId = LeadID,
                            AgentId = PropAgent.AgentId,
                            IsActive = true
                        };
                        db.LeadAgents.Add(agent);
                        db.SaveChanges();

                        int cuid = GetCoreUserIdFromAgentId(PropAgent.AgentId);
                        //Add Lead Activity For Lead
                        JazMax.Core.Leads.Activity.ActivityCreation.CaptureLeadActivity(new Core.Leads.Activity.LeadActivity
                        {
                            CoreUserId = cuid,
                            LeadActivityId = 2,
                            LeadId = LeadID,
                            IsSystem = true,
                            Description = "Lead Has Been Assigned To Agent : " + GetAgentName(cuid),
                        });

                        Console.WriteLine("Agents have been added to the lead " + agent.LeadAgentsId);
                    }
                }
                #endregion

                #region Perform Linked Leads Magic
                if (LinkedLeadData != null)
                {
                    var check = db.LeadWithSameProspects.FirstOrDefault(x => x.LeadId == LinkedLeadData.LeadID);

                    if (check != null)
                    {
                        LeadWithSameProspectLink link = new LeadWithSameProspectLink()
                        {
                            LeadWithSameProspectId = check.LeadWithSameProspectId,
                            LinkedLeadId = LeadID
                        };
                        db.LeadWithSameProspectLinks.Add(link);
                        db.SaveChanges();

                        JazMax.Core.Leads.Activity.ActivityCreation.CaptureLeadActivity(new Core.Leads.Activity.LeadActivity
                        {
                            CoreUserId = -999,
                            LeadActivityId = 3,
                            LeadId = LeadID,
                            IsSystem = true,
                            Description = "Lead Has Been Linked To " + LeadID,
                        });
                    }
                    else
                    {
                        LeadWithSameProspect same = new LeadWithSameProspect()
                        {
                            LeadId = LinkedLeadData.LeadID
                        };
                        db.LeadWithSameProspects.Add(same);
                        db.SaveChanges();

                        LeadWithSameProspectLink link = new LeadWithSameProspectLink()
                        {
                            LeadWithSameProspectId = same.LeadWithSameProspectId,
                            LinkedLeadId = LeadID
                        };
                        db.LeadWithSameProspectLinks.Add(link);
                        db.SaveChanges();

                        JazMax.Core.Leads.Activity.ActivityCreation.CaptureLeadActivity(new Core.Leads.Activity.LeadActivity
                        {
                            CoreUserId = -999,
                            LeadActivityId = 3,
                            LeadId = LeadID,
                            IsSystem = true,
                            Description = "Lead Has Been Linked To " + LinkedLeadData.LeadID,
                        });
                    }
                }
                #endregion

                #region Mark Raw Data As Processed And Log New Lead
                var record = db.LeadRawDatas.FirstOrDefault(x => x.LeadRawDataId == data.LeadRawDataId);
                record.ServiceCompleted = true;
                record.ServiceCompletedDate = DateTime.Now;
                db.SaveChanges();

                LeadRawDataLog log = new LeadRawDataLog()
                {
                    LeadId = LeadID,
                    LeadRawDataId = data.LeadRawDataId,
                    DateCreated = DateTime.Now,
                    Message = "New Lead Created"
                };
                db.LeadRawDataLogs.Add(log);
                db.SaveChanges();
                #endregion
            }
        }

        private List<PropertyListingAgent> AgentsForListing(int PropID)
        {
            using (JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                var list = db.PropertyListingAgents.Where(x => x.PropertyListingId == PropID).ToList();
                return list;
            }
        }

        private PropertyListing GetPropertyListing(int PropID)
        {
            using (JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                var query = db.PropertyListings.Where(x => x.PropertyListingId == PropID)?.FirstOrDefault();
                return query;
            }
        }

        private int FindSource(string SourceName)
        {
            using (JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                var source = db.LeadSources.FirstOrDefault(x => x.SourceName.ToLower() == SourceName.ToLower())?.LeadSourceId;

                if (source == null)
                {
                    return -999;
                }

                return (int)source;
            }
        }

        public LinkedLeads GetLeadsWithSameProspect(string FullName, string ContactNumber, string Email, int BranchID)
        {
            using (JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                var leads = (from t in db.Leads
                             join b in db.LeadProspects
                             on t.LeadId equals b.LeadId
                             join c in db.PropertyListings
                             on t.PropertyListingId equals c.PropertyListingId
                             where
                             b.FullName.ToLower() == FullName.ToLower()
                             || b.ContactNumber == ContactNumber
                             || b.Email == Email
                             &&
                             c.BranchId == BranchID
                             select new LinkedLeads
                             {
                                 BranchID = c.BranchId,
                                 LeadID = t.LeadId
                             })?.FirstOrDefault();
                return leads;
            }
        }

        private int? GetAgentIdFromCoreUserId(int CoreUserId)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                var query = (from t in db.CoreAgents
                             where t.CoreUserId == CoreUserId
                             select t.CoreAgentId)?.FirstOrDefault();


                if(query == 0)
                {
                    return null;
                }
                return query;
            }
        }

        private int GetCoreUserIdFromAgentId(int AgentId)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                var query = (from t in db.CoreAgents
                             where t.CoreAgentId == AgentId
                             select t.CoreUserId)?.FirstOrDefault();


                if (query == 0)
                {
                    return -999;
                }
                return (int)query;
            }
        }

        public void DoWork()
        {
            var list = GetRawData();
            foreach (var lead in list)
            {
                ProcessLeadData(lead);
            }
        }

        public class LinkedLeads
        {
            public int LeadID { get; set; }
            public int BranchID { get; set; }
        }

        public enum LeadType
        {
            NewManualLead = 1,
            NewLead = 2
        }

        public string GetAgentName(int CoreUserID)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                var query = (from t in db.CoreUsers
                             where t.CoreUserId == CoreUserID
                             select t.FirstName + " " + t.LastName)?.FirstOrDefault();


                if (query == null)
                {
                    return "Unknown";
                }
                return query;
            }
        }
    }
}
