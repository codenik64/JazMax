using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.BusinessLogic.AuditLog
{
    public class ErrorLog
    {
        private static JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext();
        private static DateTime Current = DateTime.Now;

        //Hits the DB connection string to log an error
        //This should be used in the Controllers
        public static void LogError(Exception e, int coreUserId = 0)
        {
            JazMax.DataAccess.SystemErrorLog a = new DataAccess.SystemErrorLog()
            {
                CoreUserId = coreUserId,
                SystemErrorMessage = e.Message.ToString(),
                Source = e.Source.ToString(),
                StackTrace = e.StackTrace.ToString(),
                ErrorDateTime = (DateTime)DateTime.Now
            };
            db.SystemErrorLogs.Add(a);
            db.SaveChanges();
        }

        public static void LogError(ErrorMessage model)
        {
            JazMax.DataAccess.SystemErrorLog a = new DataAccess.SystemErrorLog()
            {
                CoreUserId = model.CoreUserId,
                SystemErrorMessage = model.Message,
                Source = model.Source,
                StackTrace = model.StackTrace,
                ErrorDateTime = (DateTime)DateTime.Now
            };
            db.SystemErrorLogs.Add(a);
            db.SaveChanges();
        }

        //Uses the current open DB connection to log error
        //This should be used in the Business Logic
        public static void LogError(JazMax.DataAccess.JazMaxDBProdContext dbcontext, Exception e, int coreUserId = 0)
        {

            JazMax.DataAccess.SystemErrorLog a = new JazMax.DataAccess.SystemErrorLog()
            {
                CoreUserId = 0,
                SystemErrorMessage = e.Message.ToString() != null ? e.Message.ToString() : "Bad",
                ErrorDateTime = DateTime.Now
            };

            dbcontext.SystemErrorLogs.Add(a);
            dbcontext.SaveChanges();
        }

        public class ErrorMessage
        {
            public string Message { get; set; }
            public string Source { get; set; }
            public string StackTrace { get; set; }
            public int CoreUserId { get; set; }
        }
    }
}
