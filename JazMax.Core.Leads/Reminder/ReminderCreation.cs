using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Core.Leads.Reminder
{
    public class ReminderCreation
    {
        public static void CaptureLeadReminder(LeadReminder reminder)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                JazMax.DataAccess.LeadReminder act = new DataAccess.LeadReminder()
                {
                    CoreUserId = reminder.CoreUserId,
                    DateCreated = DateTime.Now,
                    Description = reminder.Description,
                    LeadId = reminder.LeadId,
                    ReminderDate = reminder.ReminderDate,
                    WebJobCompleted =false,
                    WebJobCompletedDate = null
                };
                db.LeadReminders.Add(act);
                db.SaveChanges();

                JazMax.Core.Leads.Activity.ActivityCreation.CaptureLeadActivity(new Core.Leads.Activity.LeadActivity
                {
                    CoreUserId = reminder.CoreUserId,
                    LeadActivityId = 5,
                    LeadId = reminder.LeadId,
                    IsSystem = false,
                    Description = "Reminder added for " + reminder.ReminderDate.ToShortDateString()
                });

            }
        }
    }

    public class LeadReminder
    {
        public int LeadId { get; set; }
        public string Description { get; set; }
        public DateTime ReminderDate { get; set; }
        public int CoreUserId { get; set; }
    }
}
