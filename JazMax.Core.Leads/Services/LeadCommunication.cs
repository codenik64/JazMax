using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Core.Leads.Services
{
    public class LeadCommunication
    {
        private List<int> GetLeads()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var leads = db.Leads.Where(x => x.ServiceCompleted == false).Select(x => x.LeadId).ToList();

                return leads;
            }
        }

        public void DoWork()
        {
            foreach (var a in GetLeads())
            {
                ProcessLeadCommunication(a);
            }
        }

        private void ProcessLeadCommunication(int LeadId)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var lead = db.Leads?.FirstOrDefault(x => x.LeadId == LeadId);

                if (lead != null)
                {
                    var TeamLeader = (from t in db.CoreTeamLeaders
                                      join b in db.CoreBranches
                                      on t.CoreTeamLeaderId equals b.CoreTeamLeaderId
                                      where b.BranchId == lead.CoreBranchId
                                      join e in db.CoreUsers
                                      on t.CoreUserId equals e.CoreUserId
                                      select e)?.FirstOrDefault();

                    if (TeamLeader != null)
                    {

                        //Send TeamLeader An Email
                        JazMax.BusinessLogic.Messenger.JazMaxMail.SendMail(new Web.ViewModel.Messenger.Email
                        {
                            IsAspUserId = false,
                            IsBodyHtml = true,
                            Message = "A new lead has been created in your branch. LeadID: " + lead.LeadId,
                            SendTo = TeamLeader.EmailAddress,
                            Subject = "New Lead In Your Branch (LeadID: " + LeadId + ")"
                        });

                    }
                    var AgentList = (from t in db.CoreAgents
                                     join b in db.CoreUsers
                                     on t.CoreUserId equals b.CoreUserId
                                     join e in db.LeadAgents
                                     on t.CoreAgentId equals e.AgentId
                                     where e.LeadId == lead.LeadId
                                     select b)?.ToList();

                    //Send Agents An Email

                    if (AgentList != null)
                    {

                        foreach (var agent in AgentList)
                        {
                            JazMax.BusinessLogic.Messenger.JazMaxMail.SendMail(new Web.ViewModel.Messenger.Email
                            {
                                IsAspUserId = false,
                                IsBodyHtml = true,
                                Message = "A new lead has been created and assigned to you. LeadID: " + lead.LeadId,
                                SendTo = agent.EmailAddress,
                                Subject = "New Lead Assigned To You (LeadID: " + LeadId + ")"
                            });
                        }
                    }

                    var Customer = (from t in db.LeadProspects
                                    where t.LeadId == lead.LeadId
                                    select t)?.FirstOrDefault();

                    //Send Customer an Email

                    if (Customer != null)
                    {
                        JazMax.BusinessLogic.Messenger.JazMaxMail.SendMail(new Web.ViewModel.Messenger.Email
                        {
                            IsAspUserId = false,
                            IsBodyHtml = true,
                            Message = "Thank you for your enquiry an agent will be in contact with you shortly. Please use " + lead.LeadId + " as your reference number",
                            SendTo = Customer.Email,
                            Subject = "Thank you for your enquiry"
                        });
                    }

                    lead.ServiceCompleted = true;
                    db.SaveChanges();
                }
            }
        }
    }
}
