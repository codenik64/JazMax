using System;
using System.Linq;
using JazMax.BusinessLogic.UserAccounts;
using JazMax.BusinessLogic.AuditLog;

namespace JazMax.BusinessLogic.Messenger
{
    public class JazMaxMail
    {
        private static JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext();

        public static string SendTo { get; set; }
        public static string Subject { get; set; }
        public static string Message { get; set; }
        public static bool IsBodyHtml { get; set; }
        public static bool IsAspUserId { get; set; }

        public static void SendSingleMail()
        {
            string ToAddress = SendTo;
            if (IsAspUserId == true)
            {
                ToAddress = CoreUserService.GetAspUserEmailById(db, SendTo);
            }
            SaveMessenger(ToAddress, Message, Subject, IsBodyHtml);
        }

        private static void SaveMessenger(string toAddress, string messageBody, string messageSubject, bool isBodyHtml)
        {
            try
            {
                var a = db.SystemSettingsDatas.FirstOrDefault(x => x.SettingName == "GmailAddress").SettingValueA ?? "ashveebee@gmail.com";
                JazMax.DataAccess.MessengerCoreLog log = new JazMax.DataAccess.MessengerCoreLog()
                {
                    DateCreated = DateTime.Now,
                    IsHtml = isBodyHtml,
                    MessageBody = messageBody,
                    MessageFrom = a,
                    MessageTo = toAddress,
                    IsSent = false,
                    MessenegerTypeId = -1,
                    MessageSubject = Subject
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
