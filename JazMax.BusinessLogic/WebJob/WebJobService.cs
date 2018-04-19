using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.BusinessLogic.WebJob
{
    public class WebJobService
    {
        private static DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();

        public static int? GetWebJobId(string JobName)
        {
            try
            {
                return db.AzureWebJobs.FirstOrDefault(x => x.WebJobName == JobName).AzureWebJobId;
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 1);
                return null;
            }
        }

        //2018-03-30 -------- Checking Azure Web Job Logging
        //Catch all records and update. Does not log (See Exception Log ID : 58)

        //2018-04-11 ----- This seems to be working but why?
        //I am 99.9999999999% sure this works.

        public static void LogWebJob(JazMax.Web.ViewModel.WebJob.JobModel model)
        {
            try
            {
                DataAccess.AzureWebJobLog log = new DataAccess.AzureWebJobLog()
                {
                    AzureWebJobId = Convert.ToString(model.JobId),
                    RecordsAffected = model.RecordsAffected,
                    StartDateTime = model.StartDate,
                    EndDateTime = model.EndDate
                };
                db.AzureWebJobLogs.Add(log);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 1);
            }
        }
    }
}
