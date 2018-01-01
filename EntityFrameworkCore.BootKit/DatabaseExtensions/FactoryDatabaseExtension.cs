using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public static class FactoryDatabaseExtension
    {
        public static int Patch<TTableInterface>(this Database db, DbPatchModel patch)
        {
            var binding = db.GetBinding(typeof(TTableInterface));

            var permissions = Utility.GetInstanceWithInterface<IRequireDbPermission>(binding.AssemblyNames);

            var result = permissions.Any(x => !x.AllowPatch(patch));

            if (result) return 0;

            var record = db.Table(patch.Table).FirstOrDefault(x => x.Id == patch.Id);

            patch.Values.Where(x => !patch.IgnoredColumns.Contains(x.Key))
                .ToList()
                .ForEach(x =>
                {
                    record.SetValue(x.Key, x.Value);
                });

            return 1;
        }

        public static IQueryable<DbRecord> Table(this Database db, string tableName)
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

            var dbSet = (IQueryable<DbRecord>)dc.GetType()
                .GetMethod("Set").MakeGenericMethod(tableType)
                .Invoke(dc, null);

            return dbSet;
        }
    }
}
