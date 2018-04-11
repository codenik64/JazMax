using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.WebJob.Messenger.BL;
using JazMax.BusinessLogic.WebJob;
namespace JazMax.WebJob.Messenger
{
    class Program
    {
        static void Main(string[] args)
        {
            DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();
            int? JobId = WebJobService.GetWebJobId("JazMaxMesseneger");
            JazMax.Web.ViewModel.WebJob.JobModel m = new Web.ViewModel.WebJob.JobModel()
            {
                JobId = (int)JobId,
                StartDate = DateTime.Now
            };
            List<DataAccess.MessengerCoreLog> ListToSent = db.MessengerCoreLogs.Where(x => x.IsSent == false).ToList();
            int numberOfMailsSent = 0;
            foreach(var a in ListToSent)
            {
                MessengerService.SendGmail(a.MessageTo, a.MessageSubject, a.IsHtml, a.MessageBody);
                MessengerService.UpdateMessengerToSent(a.MessengerCoreLogId);
                numberOfMailsSent++;
            }

            m.RecordsAffected = Convert.ToString(numberOfMailsSent);
            m.EndDate = DateTime.Now;

            WebJobService.LogWebJob(m);
        }
    }
}
