using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.BusinessLogic.ChangeLog
{
    public static class ChangeLogService
    {
        public static string tableName { get; set; }
        public static int tableKey { get; set; }
        public static int LoggedInUserId { get; set; }

        private static JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext();

        public static void LogChange(string beforeValue, string afterValue, string tableColumn)
        {
            try
            {
                if (beforeValue.ToLower() != afterValue.ToLower())
                {
                    db.SpSaveEditLog(tableName, tableColumn, tableKey, beforeValue, afterValue, LoggedInUserId, null);
                }
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public static List<JazMax.Web.ViewModel.ChangeLog.EditLogView> GetEditLog(string tableName, int Id)
        {
            var query = (from t in db.CoreUsers
                         join b in db.SystemEditLogs
                         on t.CoreUserId equals b.CoreUserId
                         where b.TableName == tableName && b.TablePrimaryKey == Id
                         select new Web.ViewModel.ChangeLog.EditLogView
                         {
                             ChangeDate = (DateTime)b.ChangeDate,
                             CoreUserId = (int)b.CoreUserId,
                             Name = t.FirstName + " " + t.LastName,
                             TableColumn = b.TableColumn,
                             TableKey = (int)b.TablePrimaryKey,
                             TableName = b.TableName,
                             ValueAfter = b.ValueAfter,
                             ValueBefore = b.ValueBefore
                         }).OrderByDescending(x => x.ChangeDate).ToList();
            return query;
        }

        public static string GetBoolString(bool value)
        {
            if (value)
            {
                return "Active";
            }
            return "Deactivated";
        }

    }
}
