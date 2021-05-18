using JazMax.Web.ViewModel.Leads;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.WebJob.LeadReminder
{
    public class LeadReminderCore
    {
        public void DoWork()
        {
            string body = string.Empty;
            //using (StreamReader reader = new StreamReader(@"C:\Users\NikhilChetty\documents\visual studio 2017\Projects\JazMax\JazMax.WebJob.LeadReminder\Basic.html"))
            //{
            //    body = reader.ReadToEnd();
            //}
         
            DateTime TomorrowsDate = DateTime.Now.AddDays(+1).Date;
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                JazMax.Core.Leads.Logic.LeadHelper o = new Core.Leads.Logic.LeadHelper();
                var LeadReminderList = (from t in db.LeadReminders
                                join b in db.CoreUsers
                                on t.CoreUserId equals b.CoreUserId
                                where t.WebJobCompleted == false
                                select new LeadReminderList
                                {
                                    LeadId = t.LeadId,
                                    ReminderDate = t.ReminderDate,
                                    User = b.FirstName + " " + b.LastName,
                                    EmailAddress = b.EmailAddress,
                                    LeadReminderId = t.LeadReminderId
                                }
                               ).ToList();

                List<LeadReminderMailList> List = new List<LeadReminderMailList>();

                var DueTomorrow = LeadReminderList.Where(x => x.ReminderDate.Date >= TomorrowsDate && x.ReminderDate.Date <= TomorrowsDate).Select(x => new LeadReminderMailList
                {
                    EmailAddress = x.EmailAddress,
                    LeadCore = x.LeadCore,
                    LeadId = x.LeadId,
                    ReminderDate = x.ReminderDate,
                    Type = ReminderType.Tomorrow,
                    User = x.User,
                    LeadReminderId = x.LeadReminderId
                }).ToList();

                var DueToday = LeadReminderList.Where(x => x.ReminderDate.Date >= DateTime.Today.Date && x.ReminderDate.Date <= DateTime.Today.Date).Select(x => new LeadReminderMailList
                {
                    EmailAddress = x.EmailAddress,
                    LeadCore = x.LeadCore,
                    LeadId = x.LeadId,
                    ReminderDate = x.ReminderDate,
                    Type = ReminderType.Today,
                    User = x.User,
                    LeadReminderId = x.LeadReminderId
                }).ToList();

                List.AddRange(DueToday);
                List.AddRange(DueTomorrow);

                foreach (var send in List.Where(x =>x.Type == ReminderType.Today).ToList())
                {
                    body = body.Replace("{ UserName}", send.User);
                    body = body.Replace("{Title}", "Lead Reminder - Due Today: LeadID: " + send.LeadId);
                    JazMax.BusinessLogic.Messenger.JazMaxMail.SendMail(new Web.ViewModel.Messenger.Email
                    {
                        IsAspUserId = false,
                        IsBodyHtml = true,
                        Message = "Lead Reminder due today for LeadID " + send.LeadId,
                        SendTo = send.EmailAddress,
                        Subject = "Lead Reminder Due Today"
                    });
                    Update(send.LeadReminderId);
                }

                foreach (var send in List.Where(x => x.Type == ReminderType.Tomorrow).ToList())
                {
                    JazMax.BusinessLogic.Messenger.JazMaxMail.SendMail(new Web.ViewModel.Messenger.Email
                    {
                        IsAspUserId = false,
                        IsBodyHtml = true,
                        Message = "Lead Reminder due tomorrow for LeadID " + send.LeadId,
                        SendTo = send.EmailAddress,
                        Subject = "Lead Reminder Due Tomorrow",
                  
                    });
                    Update(send.LeadReminderId);
                }
            }


        }

        public void Update(int LeadReminderId)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                JazMax.DataAccess.LeadReminder o = db.LeadReminders.Where(x => x.LeadReminderId == LeadReminderId).FirstOrDefault();
                o.WebJobCompleted = true;
                o.WebJobCompletedDate = DateTime.Now;
                db.SaveChanges();
            }
        }
    }
    public class LeadReminderList
    {
        public int LeadReminderId { get; set; }
        public int LeadId { get; set; }
        public string User { get; set; }
        public DateTime ReminderDate { get; set; }
        public JazMax.Web.ViewModel.Leads.LeadCore LeadCore { get; set; }
        public string EmailAddress { get; set; }
    }

    public class LeadReminderMailList
    {
        public int LeadReminderId { get; set; }
        public int LeadId { get; set; }
        public string User { get; set; }
        public DateTime ReminderDate { get; set; }
        public JazMax.Web.ViewModel.Leads.LeadCore LeadCore { get; set; }
        public string EmailAddress { get; set; }
        public ReminderType Type { get; set; }
    }
    public enum ReminderType
    {
        Today,
        Tomorrow
    }
}
