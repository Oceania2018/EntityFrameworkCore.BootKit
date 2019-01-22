using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public class DbContext4MySql : DataContext
    {
        public DbContext4MySql(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4MySql2 : DataContext
    {
        public DbContext4MySql2(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4MySql3 : DataContext
    {
        public DbContext4MySql3(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4MySql4 : DataContext
    {
        public DbContext4MySql4(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4MySql5 : DataContext
    {
        public DbContext4MySql5(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
