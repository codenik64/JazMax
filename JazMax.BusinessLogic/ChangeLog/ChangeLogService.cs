using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.BusinessLogic.ChangeLog
{
    public static class ChangeLogService
    {
        private static JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext();

        //public static void LogChange(string tableName, int tableKey, string beforeValue, string afterValue, int coreUserId, string comment)
        //{
        //    if (beforeValue.ToLower() != afterValue.ToLower())
        //    {
        //        DataAccess.SystemEditLog table = new DataAccess.SystemEditLog()
        //        {
        //            ChangeDate = DateTime.Now,
        //            Comment = comment,
        //            CoreUserId = coreUserId,
        //            TableName = tableName,
        //            TablePrimaryKey = tableKey,
        //            ValueBefore = beforeValue,
        //            ValueAfter = afterValue
        //        };
        //        db.SystemEditLogs.Add(table);
        //        db.SaveChanges();
        //    }
        //}

        public static void LogChange(int tableKey, string beforeValue, string afterValue, int coreUserId, string tableName, string tableColumn)
        {
            try
            {
                if (beforeValue.ToLower() != afterValue.ToLower())
                {
                    //DataAccess.SystemEditLog aa = new DataAccess.SystemEditLog();
                    //aa.ChangeDate = DateTime.Now;
                    //aa.Comment = "None";
                    //aa.CoreUserId = coreUserId;
                    //aa.TableColumn = tableColumn;
                    //aa.TableName = tableName;
                    //aa.TablePrimaryKey = tableKey;
                    //aa.ValueAfter = afterValue;
                    //aa.ValueBefore = beforeValue;
                    //db.Configuration.ValidateOnSaveEnabled = false;
                    //db.SystemEditLogs.Add(aa);
                    //db.SaveChanges();
                    db.SpSaveEditLog(tableName, tableColumn, tableKey, beforeValue, afterValue, coreUserId, null);
                   
                }   
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }

 

    }
}
