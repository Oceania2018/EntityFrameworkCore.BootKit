using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public static class TransactionDatabaseExtension
    {
        public static int Transaction<TTableInterface>(this Database db, Action action)
        {
            using (IDbContextTransaction transaction = db.GetMaster(typeof(TTableInterface)).Database.BeginTransaction())
            {
                int affected = 0;
                try
                {
                    action();
                    affected = db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    if (ex.Message.Contains("See the inner exception for details"))
                    {
                        throw ex.InnerException;
                    }
                    else
                    {
                        throw ex;
                    }
                }

                return affected;
            }
        }

        public static TResult Transaction<T, TResult>(this Database db, Func<TResult> action)
        {
            using (IDbContextTransaction transaction = db.GetMaster(typeof(T)).Database.BeginTransaction())
            {
                TResult result = default(TResult);
                try
                {
                    result = action();
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    if (ex.Message.Contains("See the inner exception for details"))
                    {
                        throw ex.InnerException;
                    }
                    else
                    {
                        throw ex;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Shortcut for IDbRecord Transaction
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static int DbTran(this Database db, Action action)
        {
            return db.Transaction<IDbRecord>(action);
        }

        public static IDbContextTransaction GetDbContextTransaction<T>(this Database db)
        {
            return db.GetMaster(typeof(T)).Database.BeginTransaction();
        }
    }
}
