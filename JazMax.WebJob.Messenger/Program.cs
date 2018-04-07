using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.WebJob.Messenger.BL;
namespace JazMax.WebJob.Messenger
{
    class Program
    {
        static void Main(string[] args)
        {
            DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();

            List<DataAccess.MessengerCoreLog> ListToSent = db.MessengerCoreLogs.Where(x => x.IsSent == false).ToList();

            foreach(var a in ListToSent)
            {
                MessengerService.SendGmail(a.MessageTo, a.MessageSubject, a.IsHtml, a.MessageBody);
                MessengerService.UpdateMessengerToSent(a.MessengerCoreLogId);
            }
        }
    }
}
