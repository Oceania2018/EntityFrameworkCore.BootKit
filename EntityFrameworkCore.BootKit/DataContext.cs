using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public class DataContext : DbContextWithTriggers
    {
        public String ConnectionString = "";
        public List<Type> EntityTypes { get; set; }

        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // http://www.learnentityframeworkcore.com/
            // modelBuilder.Entity<TaxonomyTermEntity>().HasIndex(x => x.Name);
            // don't need this code.
            //modelBuilder.Entity<Bundle>().ForSqlServerToTable("Bundles");
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            EntityTypes.ForEach(type =>
            {
                var type1 = modelBuilder.Model.FindEntityType(type);
                if(type1 == null)
                {
                    modelBuilder.Model.AddEntityType(type);
                }
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
