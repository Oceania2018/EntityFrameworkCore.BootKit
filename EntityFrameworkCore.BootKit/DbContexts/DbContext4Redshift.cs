using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.BootKit.DbContexts
{
    public class DbContext4Redshift : DataContext
    {
        public DbContext4Redshift(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options, serviceProvider) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SetLog(optionsBuilder);
            optionsBuilder.UseNpgsql(ConnectionString, 
                x => x.UseNetTopologySuite().EnableRetryOnFailure());
            base.OnConfiguring(optionsBuilder);
        }
    }
}
