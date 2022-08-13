using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public class DbContext4Aurora : DataContext
    {
        public DbContext4Aurora(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString),
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

    public class DbContext4Aurora2 : DataContext
    {
        public DbContext4Aurora2(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString),
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

    public class DbContext4Aurora3 : DataContext
    {
        public DbContext4Aurora3(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString),
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
