using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public class DbContext4SqlServer : DataContext
    {
        public DbContext4SqlServer(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseSqlServer(ConnectionString,
                x =>
                {
                    x.UseNetTopologySuite();
                    if (enableRetryOnFailure)
                    {
                        x.EnableRetryOnFailure();
                    }
                });
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4SqlServer2 : DataContext
    {
        public DbContext4SqlServer2(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseSqlServer(ConnectionString,
                x =>
                {
                    x.UseNetTopologySuite();
                    if (enableRetryOnFailure)
                    {
                        x.EnableRetryOnFailure();
                    }
                });
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4SqlServer3 : DataContext
    {
        public DbContext4SqlServer3(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseSqlServer(ConnectionString,
                x =>
                {
                    x.UseNetTopologySuite();
                    if (enableRetryOnFailure)
                    {
                        x.EnableRetryOnFailure();
                    }
                });
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4SqlServer4 : DataContext
    {
        public DbContext4SqlServer4(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseSqlServer(ConnectionString,
                x =>
                {
                    x.UseNetTopologySuite();
                    if (enableRetryOnFailure)
                    {
                        x.EnableRetryOnFailure();
                    }
                });
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4SqlServer5 : DataContext
    {
        public DbContext4SqlServer5(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseSqlServer(ConnectionString,
                x =>
                {
                    x.UseNetTopologySuite();
                    if (enableRetryOnFailure)
                    {
                        x.EnableRetryOnFailure();
                    }
                });
            base.OnConfiguring(optionsBuilder);
        }
    }
}
