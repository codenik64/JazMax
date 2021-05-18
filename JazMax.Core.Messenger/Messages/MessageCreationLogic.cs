using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.DataAccess;
using JazMax.Web.ViewModel.Messenger;
namespace JazMax.Core.Messenger.Messages
{
    public static class MessageCreationLogic
    {
        public static void CreateTrigger(SendMessage model)
        {
            using (JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                MessengerTrigger t = new MessengerTrigger()
                {
                    BranchId = model.BranchId,
                    CoreUserId = model.CoreUserId,
                    CreatedDate = DateTime.Now,
                    HasBeenProcessed = false,
                    IsCancelled = 0,
                    MessengerComTypeId = 1,
                    CoreProvinceId = model.ProvinceId,
                    CoreUserTypeId = model.CoreUserTypeId,
                    MessageBody = model.MessageBody,
                    MessengerTemplateId = 1,
                    NumberOfContacts = null,
                    ProcessedDateTime = null,
                    SendingDate = model.SendDate,
                    SendingTo = model.SendTo,
                    TriggerSetup = model.TriggerText,
                    MessageSubject = model.MessageSubject
                };
                db.MessengerTriggers.Add(t);
                db.SaveChanges();
            }
        }
    }
}
