using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Core.Leads.Creation
{
    public class LeadCreation
    {
        
        public static void CaptureLead(LeadItem item)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                try
                {
                    JazMax.DataAccess.LeadRawData raw = new DataAccess.LeadRawData()
                    {
                        FullName = item.FullName,
                        ContactNumber = item.ContactNumber,
                        Email = item.Email,
                        Comments = item.Comments,
                        SourceName = item.Source,
                        PropertyListingId = item.PropertyListingID,
                        DateCreated = DateTime.Now,
                        ServiceCompleted = false,
                        ServiceCompletedDate = null,
                        CoreUserId = item.CoreUserId ?? -999,
                        IsManualCapture = item.IsManual,
                       
                    };
                    db.LeadRawDatas.Add(raw);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
                }
            }
        }
    }

    public class LeadItem
    {
        [Required]
        [Display(Name = "Prospect Name")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Prospect Contact Number")]
        [StringLength(10, MinimumLength = 10)]
        public string ContactNumber { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [Required]
        public string Comments { get; set; }
        [Required]
        public string Source { get; set; }
        [Required]
        public int PropertyListingID { get; set; }
        public bool IsManual { get; set; }
        public int? CoreUserId { get; set; }
    }
}
