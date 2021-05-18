using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Clients;

namespace JazMax.Core.Twilio
{
    public class TwilioCoreProcessor
    {

        public void CallProcessing()
        {
            string AccountSid = "ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            string AuthToken = "your_auth_token";
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            
        }
    }
}
