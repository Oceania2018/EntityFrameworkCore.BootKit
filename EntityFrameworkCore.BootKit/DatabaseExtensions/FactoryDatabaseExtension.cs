using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;

namespace EntityFrameworkCore.BootKit
{
    public static class FactoryDatabaseExtension
    {
        public static int Patch<TTableInterface>(this Database db, DbPatchModel patch)
        {
            var binding = db.GetBinding(typeof(TTableInterface));

            var assemblies = (string[])AppDomain.CurrentDomain.GetData("Assemblies");
            var permissions = Utility.GetInstanceWithInterface<IRequireDbPermission>(assemblies);

            var result = permissions.Any(x => !x.AllowPatch(patch));

            if (result) return 0;

            if (!String.IsNullOrEmpty(patch.Id))
            {
                var record = db.Table(patch.Table).FirstOrDefault(x => x.Id == patch.Id);
                SetValues(patch, record);

                return 1;
            }
            else
            {
                var records = db.Table(patch.Table).Where(patch.Where, patch.Params).ToList();
                records.ForEach(record => SetValues(patch, record));

                return records.Count;
            }
        }

        private static void SetValues(DbPatchModel patch, DbRecord record)
        {
            patch.Values.Where(x => !patch.IgnoredColumns.Contains(x.Key))
                .ToList()
                .ForEach(x =>
                {
                    try
                    {
                        record.SetValue(x.Key, x.Value);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Set value exception: {x.Key} {x.Value}");
                        throw ex;
                    }
                });
        }

        public static IQueryable<DbRecord> Table(this Database db, string tableName)
        {
            return Table<DbRecord>(db, tableName);
        }

        public static IQueryable<TRecord> Table<TRecord>(this Database db, string tableName)
        {
            var binding = db.GetBinding(tableName);

            var tableType = binding.Entities.First(x => x.Name.ToLower().Equals(tableName.ToLower()));

            if (tableType == null) return null;

            DbContext dc = null;

            if (binding.DbContextMaster != null && binding.DbContextMaster.Database.CurrentTransaction != null)
            {
                dc = db.GetMaster(tableType);
            }
            else
            {
                dc = db.GetReader(tableType);
            }

            var dbSet = (IQueryable<TRecord>)dc.GetType()
                .GetMethod("Set").MakeGenericMethod(tableType)
                .Invoke(dc, null);

            return dbSet;
        }

        public static object Add(this Database db, string table, Object entity)
        {
            var dbSet = db.Table(table);

            var assemblies = (string[])AppDomain.CurrentDomain.GetData("Assemblies");
            var tableType = Utility.GetType(table, assemblies);

            return dbSet.InvokeFunction("Add", new Object[] { entity });
        }

        public static object Remove(this Database db, string table, Object entity)
        {
            var dbSet = db.Table(table);

            var assemblies = (string[])AppDomain.CurrentDomain.GetData("Assemblies");
            var tableType = Utility.GetType(table, assemblies);

            return dbSet.InvokeFunction("Remove", new Object[] { entity });
        }
    }
}
