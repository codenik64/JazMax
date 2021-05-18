using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JazMax.Core.Leads.Creation;
namespace JazMax.API.Leads.Controllers
{
    public class SubmitLeadController : ApiController
    {
        public HttpResponseMessage Submit(string fullName, string contactNumber, string Email, string comments, string source, int PropertyId)
        {
            try
            {
                LeadItem model = new LeadItem()
                {
                    Comments = comments,
                    ContactNumber = contactNumber,
                    CoreUserId = null,
                    Email = Email,
                    FullName = fullName,
                    PropertyListingID = PropertyId,
                    IsManual = false,
                    Source = source
                };
                LeadCreation.CaptureLead(model);
                return Request.CreateResponse(HttpStatusCode.OK, "Lead Saved");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, e.Message.ToString());
            }
        }
    }
}
