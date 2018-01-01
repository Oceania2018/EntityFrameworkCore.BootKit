using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/in-memory
    /// </summary>
    public class DbContext4Memory : DataContext
    {
        public DbContext4Memory(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
