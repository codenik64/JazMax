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
