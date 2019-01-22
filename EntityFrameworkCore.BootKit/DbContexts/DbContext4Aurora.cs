using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public class DbContext4Aurora : DataContext
    {
        public DbContext4Aurora(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4Aurora2 : DataContext
    {
        public DbContext4Aurora2(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbContext4Aurora3 : DataContext
    {
        public DbContext4Aurora3(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
