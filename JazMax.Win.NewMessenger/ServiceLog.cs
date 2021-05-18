using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using JazMax.Win.NewMessenger.Core;

namespace JazMax.Win.NewMessenger
{
    public static class ServiceLog
    {
        public static void CoreLog(string Message)
        {
            StreamWriter write = null;
            try
            {
                write = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\MessengerService.txt", true);
                write.WriteLine(DateTime.Now.ToLongDateString() + ":" + Message);
                write.Flush();
                write.Close();
            }
            catch (Exception)
            {
               

            }
        }

        public static void CoreLog(Exception ex)
        {
            StreamWriter write = null;
            try
            {
                write = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\MessengerService.txt", true);
                write.WriteLine(DateTime.Now.ToLongDateString() + ":" + ex.Source.ToString().Trim() + "," + ex.Message.ToString().Trim());
                write.Flush();
                write.Close();
            }
            catch (Exception )
            {
              
            }
        }

        public static void DoWork()
        {
            Debug.WriteLine("JazMax Messenger Service V2.0 has started");
            DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();
            List<DataAccess.MessengerCoreLog> ListToSent = db.MessengerCoreLogs.Where(x => x.IsSent == false).ToList();
            foreach (var a in ListToSent)
            {
                Messenger.SendGmail(a.MessageTo, a.MessageSubject, a.IsHtml, a.MessageBody);
                Messenger.UpdateMessengerToSent(a.MessengerCoreLogId);
            }
        }
    }
}
