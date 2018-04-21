using JazMax.BusinessLogic.WebJob;
using JazMax.Win.Messenger.BL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Win.Messenger
{
    public class MessengerCoreService
    {
        public static void DoWork()
        {
            Debug.WriteLine("JazMax Messenger Service V2.0 has started");
            DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();
            List<DataAccess.MessengerCoreLog> ListToSent = db.MessengerCoreLogs.Where(x => x.IsSent == false).ToList();
            foreach (var a in ListToSent)
            {
                MessengerService.SendGmail(a.MessageTo, a.MessageSubject, a.IsHtml, a.MessageBody);
                MessengerService.UpdateMessengerToSent(a.MessengerCoreLogId);
            }
        }

        public static void OnStop()
        {
            Debug.WriteLine("JazMax Messenger Service V2.0 has stopped");
        }
    }
}
