using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using JazMax.BusinessLogic.AuditLog;
namespace JazMax.WebJob.Messenger.BL
{
    public class MessengerService
    {
        private static JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext();

        public static void SendGmail(string To, string Subject, bool IsBodyHtml, string Message)
        {
            string from = GetModel().FromAddress;
            using (MailMessage mail = new MailMessage(from, To))
            {
                mail.Subject = Subject;
                mail.Body = Message;
                mail.IsBodyHtml = IsBodyHtml;
                SmtpClient smtp = new SmtpClient()
                {
                    Host = GetModel().GmailHost,
                    EnableSsl = true
                };
                NetworkCredential networkCredential = new NetworkCredential(from, GetModel().GmailPassword);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
                smtp.Port = GetModel().GmailPort;
                try
                {
                    smtp.Send(mail);
                }
                catch (Exception e)
                {
                    ErrorLog.LogError(db, e, 0);
                }
            }
        }

        public static void UpdateMessengerToSent(int Id)
        {

            JazMax.DataAccess.MessengerCoreLog a = db.MessengerCoreLogs.FirstOrDefault(x => x.MessengerCoreLogId == Id);
            try
            {
                a.IsSent = true;
                a.DateSent = DateTime.Now;
                a.MessengerResponse = "Sent";
            }
            catch (Exception e)
            {
                a.MessengerResponse = e.Message;
                ErrorLog.LogError(db, e, 0);
            }
            db.SaveChanges();
        }

        private static GmailModel GetModel()
        {
            var a = db.SystemSettingsDatas.FirstOrDefault(x => x.SettingName == "GmailAddress").SettingValueA;
            var b = db.SystemSettingsDatas.FirstOrDefault(x => x.SettingName == "GmailPassword").SettingValueA;
            var c = db.SystemSettingsDatas.FirstOrDefault(x => x.SettingName == "GmailHost").SettingValueA;
            var d = db.SystemSettingsDatas.FirstOrDefault(x => x.SettingName == "GmailPort").SettingValueA;

            GmailModel model = new GmailModel()
            {
                FromAddress = a,
                GmailPassword = b,
                GmailHost = c,
                GmailPort = Convert.ToInt32(d)
            };
            return model;
        }
    }
    public class GmailModel
    {
        public string FromAddress { get; set; }
        public string GmailPassword { get; set; }
        public string GmailHost { get; set; }
        public int GmailPort { get; set; }
    }
}


