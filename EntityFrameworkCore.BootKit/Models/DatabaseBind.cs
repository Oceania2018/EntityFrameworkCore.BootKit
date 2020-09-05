using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public class DatabaseBind
    {
        public DbConnection MasterConnection { get; set; }
        public DataContext DbContextMaster { get; set; }

        public int SlaveId { get; set; }
        public List<DbConnection> SlaveConnections { get; set; }
        public DbConnection SlaveConnection => SlaveConnections[SlaveId];
        public DataContext DbContextSlaver { get; set; }

        public Type TableInterface { get; set; }

        public Type DbContextType { get; set; }

        public List<Type> Entities { get; set; }

        public Boolean CreateDbIfNotExist { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
    }
}
