using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.BootKit
{
    public static class TransactionDatabaseExtension
    {
        public static DatabaseFacade GetDatabaseFacade<TTableInterface>(this Database db)
            => db.GetMaster(typeof(TTableInterface)).Database;
        public static IDbContextTransaction BeginTransaction<TTableInterface>(this Database db)
        {
            var masterDb = db.GetMaster(typeof(TTableInterface)).Database;
            if (masterDb.CurrentTransaction == null)
                return masterDb.BeginTransaction();
            else
                return masterDb.CurrentTransaction;
        }

        public static void RollbackTransaction<TTableInterface>(this Database db)
        {
            var masterDb = db.GetMaster(typeof(TTableInterface)).Database;
            if (masterDb.CurrentTransaction != null)
                masterDb.RollbackTransaction();
        }

        public static void EndTransaction<TTableInterface>(this Database db)
        {
            var masterDb = db.GetMaster(typeof(TTableInterface)).Database;
            // current transaction will be null if it's been rollbacked.
            if (masterDb.CurrentTransaction == null)
                return;
            try
            {
                db.SaveChanges();
                masterDb.CurrentTransaction.Commit();
            }
            catch (Exception ex)
            {
                masterDb.CurrentTransaction.Rollback();
                if (ex.Message.Contains("See the inner exception for details"))
                    throw ex.InnerException;
                else
                    throw ex;
            }
        }

        public static int Transaction<TTableInterface>(this Database db, Action action)
        {
            var masterDb = db.GetMaster(typeof(TTableInterface)).Database;
            int affected = 0;

            if (masterDb.CurrentTransaction == null)
            {
                using (var transaction = masterDb.BeginTransaction())
                {
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
                            throw ex.InnerException;
                        else
                            throw ex;
                    }
                }
            }
            else
            {
                try
                {
                    action();
                    affected = db.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (masterDb.CurrentTransaction != null)
                        masterDb.CurrentTransaction.Rollback();

                    if (ex.Message.Contains("See the inner exception for details"))
                        throw ex.InnerException;
                    else
                        throw ex;
                }
            }

            return affected;
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
                        throw ex.InnerException;
                    else
                        throw ex;
                }

                return result;
            }
        }

        public static async Task<int> TransactionAsync<TTableInterface>(this Database db, Func<Task> func)
        {
            var masterDb = db.GetMaster(typeof(TTableInterface)).Database;
            int affected = 0;
            if (masterDb.CurrentTransaction == null)
            {
                using (var transaction = await masterDb.BeginTransactionAsync())
                {
                    try
                    {
                        await func();
                        affected = await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        if (ex.Message.Contains("See the inner exception for details"))
                            throw ex.InnerException;
                        else throw ex;
                    }
                }
            }
            else
            {
                try
                {
                    await func();
                    affected = await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (masterDb.CurrentTransaction != null)
                        await masterDb.CurrentTransaction.RollbackAsync();
                    else if (ex.Message.Contains("See the inner exception for details"))
                        throw ex.InnerException;
                    else throw ex;
                }
            }
            return affected;
        }

        public static async Task<TResult> TransactionAsync<T, TResult>(this Database db, Func<Task<TResult>> func)
        {
            using (IDbContextTransaction transaction = await db.GetMaster(typeof(T)).Database.BeginTransactionAsync())
            {
                TResult result = default;
                try
                {
                    result = await func();
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    if (ex.Message.Contains("See the inner exception for details"))
                        throw ex.InnerException;
                    else
                        throw ex;
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
            var masterDb = db.GetMaster(typeof(T)).Database;

            if (masterDb.CurrentTransaction == null)
                return masterDb.BeginTransaction();
            else
                return masterDb.CurrentTransaction;
        }

        public static bool IsTransactionOpen<T>(this Database db)
        {
            var masterDb = db.GetMaster(typeof(T)).Database;
            return masterDb.CurrentTransaction != null;
        }
    }
}
