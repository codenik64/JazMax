using System;
using System.Linq;
using JazMax.BusinessLogic.UserAccounts;
using JazMax.BusinessLogic.AuditLog;
using JazMax.Web.ViewModel.Messenger;

namespace JazMax.BusinessLogic.Messenger
{
    public class JazMaxMail
    {
        private static JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext();

        public static void SendMail(Email model)
        {
            string ToAddress = model.SendTo;
            if (model.IsAspUserId == true)
            {
                ToAddress = CoreUserService.GetAspUserEmailById(db, model.SendTo);
            }
            SaveMessenger(ToAddress, model.Message, model.Subject, model.IsBodyHtml);
        }

        public static void SendMail(Message model)
        {
            string ToAddress = model.SendTo;
            SaveMessenger(ToAddress, model.MessageBody, model.Subject, model.IsBodyHtml, model.MessengerTriggerId);
        }

        private static void SaveMessenger(string toAddress, string messageBody, string messageSubject, bool isBodyHtml)
        {
            try
            {
                var a = db.SystemSettingsDatas.FirstOrDefault(x => x.SettingName == "GmailAddress").SettingValueA ?? "nikhilchetty64@gmail.com";
                JazMax.DataAccess.MessengerCoreLog log = new JazMax.DataAccess.MessengerCoreLog()
                {
                    DateCreated = DateTime.Now,
                    IsHtml = isBodyHtml,
                    MessageBody = messageBody,
                    MessageFrom = a,
                    MessageTo = toAddress,
                    IsSent = false,
                    MessenegerTypeId = -1,
                    MessageSubject = messageSubject,
                    MessengerTriggerId = 0
                };
                db.MessengerCoreLogs.Add(log);
                db.SaveChanges();

            }
            catch (Exception e)
            {
               ErrorLog.LogError(e, 0);
            }
           
        }

        private static void SaveMessenger(string toAddress, string messageBody, string messageSubject, bool isBodyHtml, int MessengerTriggerId)
        {
            try
            {
                var a = db.SystemSettingsDatas.FirstOrDefault(x => x.SettingName == "GmailAddress").SettingValueA ?? "nikhilchetty64@gmail.com";
                JazMax.DataAccess.MessengerCoreLog log = new JazMax.DataAccess.MessengerCoreLog()
                {
                    DateCreated = DateTime.Now,
                    IsHtml = isBodyHtml,
                    MessageBody = messageBody,
                    MessageFrom = a,
                    MessageTo = toAddress,
                    IsSent = false,
                    MessenegerTypeId = -1,
                    MessageSubject = messageSubject,
                    MessengerTriggerId = MessengerTriggerId
                };
                db.MessengerCoreLogs.Add(log);
                db.SaveChanges();

            }
            catch (Exception e)
            {
                ErrorLog.LogError(e, 0);
            }

        }




    }
}
