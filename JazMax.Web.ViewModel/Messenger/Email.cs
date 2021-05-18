using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.Messenger
{
    public class Email
    {
        public string SendTo { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsBodyHtml { get; set; }
        public bool IsAspUserId { get; set; }
    }

    public class Message
    {
        public string SendTo { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public bool IsBodyHtml { get; set; }
        public int MessengerTriggerId { get; set; }
    }
}
