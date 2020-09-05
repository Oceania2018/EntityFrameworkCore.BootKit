using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public class DbContext4MySql : DataContext
    {
        public DbContext4MySql(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseMySql(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4MySql2 : DataContext
    {
        public DbContext4MySql2(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseMySql(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4MySql3 : DataContext
    {
        public DbContext4MySql3(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseMySql(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4MySql4 : DataContext
    {
        public DbContext4MySql4(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseMySql(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4MySql5 : DataContext
    {
        public DbContext4MySql5(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseMySql(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
