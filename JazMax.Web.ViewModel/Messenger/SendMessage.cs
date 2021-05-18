using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.Messenger
{
    public class SendMessage
    {
        public int MessengerTriggerId { get; set; }
        public int SendTo { get; set; }
        public int MessageType { get; set; }
        [Display(Name = "Sending Date")]
        public DateTime SendDate { get; set; }
        [Display(Name = "Message Template")]
        public int TemplateId { get; set; }
        [Display(Name = "Message Body")]
        public string MessageBody { get; set; }
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        public string TriggerText { get; set; }
        public int CoreUserId { get; set; }
        public bool HasBeenProcessed { get; set; }
        public bool ProcessdDateTime { get; set; }
        public int NumberOfContacts { get; set; }
        public bool IsCancelled { get; set; }
        [Display(Name = "Province")]
        public int ProvinceId { get; set; }
        [Display(Name = "User Groups")]
        public int CoreUserTypeId { get; set; }
        [Display(Name = "Subject")]
        public string MessageSubject { get; set; }
    }
}
