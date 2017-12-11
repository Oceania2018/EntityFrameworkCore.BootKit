using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public class Database
    {
        internal List<DatabaseBind> DbContextBinds;

        public Database()
        {
            DbContextBinds = new List<DatabaseBind>();
        }

        private List<Type> GetAllEntityTypes(DatabaseBind bind)
        {
            return Utility.GetClassesWithInterface(bind.TableInterface, bind.AssemblyNames);
        }


        public void BindDbContext(DatabaseBind bind)
        {
            bind.Entities = GetAllEntityTypes(bind).ToList();

            DbContextBinds.Add(bind);
        }

        public void BindDbContext<TTableInterface, TDbContextType>(DatabaseBind bind)
        {
            bind.TableInterface = typeof(TTableInterface);
            bind.DbContextType = typeof(TDbContextType);

            bind.Entities = GetAllEntityTypes(bind).ToList();

            DbContextBinds.Add(bind);

            if (bind.CreateDbIfNotExist)
            {
                GetMaster(bind.TableInterface).Database.EnsureCreated();
            }
        }

        private DataContext GetMaster(Type entityType)
        {
            DatabaseBind binding = DbContextBinds.First(x => (x.TableInterface != null && x.TableInterface.Equals(entityType)) || (x.Entities != null && x.Entities.Contains(entityType)));

            if (binding.DbContextMaster == null)
            {
                DbContextOptions options = new DbContextOptions<DataContext>();
                DataContext dbContext = Activator.CreateInstance(binding.DbContextType, options) as DataContext;
                dbContext.ConnectionString = binding.MasterConnection.ConnectionString;
                dbContext.EntityTypes = binding.Entities;
                binding.DbContextMaster = dbContext;
            }

            return binding.DbContextMaster;
        }

        private DataContext GetReader(Type entityType)
        {
            DatabaseBind binding = DbContextBinds.First(x => x.Entities.Contains(entityType) || x.Entities.Contains(entityType));

            if (binding.DbContextSlavers == null)
            {
                binding.DbContextSlavers = new List<DataContext>();

                DbContextOptions options = new DbContextOptions<DataContext>();

                DataContext dbContext = Activator.CreateInstance(binding.DbContextType, options) as DataContext;
                dbContext.ConnectionString = (binding.SlaveConnections == null || binding.SlaveConnections.Count == 0) ? binding.MasterConnection.ConnectionString : binding.SlaveConnections.First().ConnectionString;
                dbContext.EntityTypes = binding.Entities;
                binding.DbContextSlavers.Add(dbContext);
            }

            return binding.DbContextSlavers.First();
        }

        /*public IMongoCollection<T> Collection<T>(string collection) where T : class
        {
            EfDbBinding4MongoDb binding = DbContextBinds.First(x => x.GetType().Equals(typeof(EfDbBinding4MongoDb))) as EfDbBinding4MongoDb;
            if (binding.DbContext == null)
            {
                binding.DbContext = new MongoDbContext(binding.ConnectionString);
            }

            return binding.DbContext.Database.GetCollection<T>(collection);
        }*/

        public Object Find(Type type, params string[] keys)
        {
            DatabaseBind binding = DbContextBinds.First(x => x.Entities.Contains(type));
            if (binding.DbContextMaster == null || binding.DbContextMaster.Database.CurrentTransaction == null)
            {
                return GetReader(type).Find(type, keys);
            }
            else
            {
                return GetMaster(type).Find(type, keys);
            }
        }

        public DbSet<T> Table<T>() where T : class
        {
            Type entityType = typeof(T);

            DatabaseBind binding = DbContextBinds.First(x => x.Entities.Contains(entityType));
            if (binding.DbContextMaster == null || binding.DbContextMaster.Database.CurrentTransaction == null)
            {
                return GetReader(entityType).Set<T>();
            }
            else
            {
                return GetMaster(entityType).Set<T>();
            }
        }

        /*public DbSet Table(string tableName)
        {
            DatabaseBind binding = DbContextBinds.FirstOrDefault(x => x.EntityTypeList != null && x.EntityTypeList.Select(entity => entity.Name.ToLower()).Contains(tableName.ToLower()));
            if (binding == null) return null;

            Type tableType = binding.EntityTypeList.First(x => x.Name.ToLower().Equals(tableName.ToLower()));

            if (tableType == null) return null;

            if (binding.DbContextMaster != null && binding.DbContextMaster.Database.CurrentTransaction != null)
            {
               gett
                return GetMaster(tableType).Set().(tableType);
            }
            else
            {
                return GetReader(tableType).Set(tableType);
            }
        }*/

        public int ExecuteSqlCommand<T>(string sql, params object[] parameterms)
        {
            var db = GetMaster(typeof(T)).Database;
            return db.ExecuteSqlCommand(sql, parameterms);
        }

        public int ExecuteSqlCommand<T>(string sql, IEnumerable<object> parameterms)
        {
            var db = GetMaster(typeof(T)).Database;
            return db.ExecuteSqlCommand(sql, parameterms);
        }

        public int SaveChanges()
        {
            DatabaseBind binding = DbContextBinds.Where(x => x.DbContextType != null).First(x => x.DbContextMaster != null && x.DbContextMaster.Database.CurrentTransaction != null);
            return binding.DbContextMaster.SaveChanges();
        }

        public int Transaction<T>(Action action)
        {
            using (IDbContextTransaction transaction = GetMaster(typeof(T)).Database.BeginTransaction())
            {
                int affected = 0;
                try
                {
                    action();
                    affected = SaveChanges();
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

        public TResult Transaction<T, TResult>(Func<TResult> action)
        {
            using (IDbContextTransaction transaction = GetMaster(typeof(T)).Database.BeginTransaction())
            {
                TResult result = default(TResult);
                try
                {
                    result = action();
                    SaveChanges();
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

        public IDbContextTransaction GetDbContextTransaction<T>()
        {
            return GetMaster(typeof(T)).Database.BeginTransaction();
        }
    }
}
