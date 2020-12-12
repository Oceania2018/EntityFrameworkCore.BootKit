using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public class DbContext4Sqlite : DataContext
    {
        public DbContext4Sqlite(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseSqlite(ConnectionString,
                x => x.UseNetTopologySuite());
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4Sqlite2 : DataContext
    {
        public DbContext4Sqlite2(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseSqlite(ConnectionString,
                x => x.UseNetTopologySuite());
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4Sqlite3 : DataContext
    {
        public DbContext4Sqlite3(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseSqlite(ConnectionString,
                x => x.UseNetTopologySuite());
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4Sqlite4 : DataContext
    {
        public DbContext4Sqlite4(DbContextOptions options, IServiceProvider serviceProvider)
             : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseSqlite(ConnectionString,
                x => x.UseNetTopologySuite());
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4Sqlite5 : DataContext
    {
        public DbContext4Sqlite5(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseSqlite(ConnectionString,
                x => x.UseNetTopologySuite());
            base.OnConfiguring(optionsBuilder);
        }
    }
}
