using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace EntityFrameworkCore.BootKit
{
    public class DataContext : DbContext
    {
        public string ConnectionString = "";
        public List<Type> EntityTypes { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        protected bool enableRetryOnFailure => dbSettings.EnableRetryOnFailure;
        protected bool useCamelCase => dbSettings.UseCamelCase;


        private static DatabaseSettings _dbSettings;
        protected DatabaseSettings dbSettings
        {
            get
            {
                if (_dbSettings == null)
                {
                    if (ServiceProvider == null)
                    {
                        throw new Exception($"ServiceProvider is not initialized.");
                    }
                    _dbSettings = (DatabaseSettings)ServiceProvider.GetService(typeof(DatabaseSettings));
                }
                return _dbSettings;
            }
        }

        public DataContext(DbContextOptions options, IServiceProvider serviceProvider)
            : base(options)
        {
            ServiceProvider = serviceProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityTypes.ForEach(type =>
            {
                var type1 = modelBuilder.Model.FindEntityType(type);
                if(type1 == null)
                {
                    modelBuilder.Model.AddEntityType(type);
                }

                if (type.GetCustomAttributes(typeof(HasNoKeyAttribute)).Any())
                    modelBuilder.Entity(type).HasNoKey();
            });

            base.OnModelCreating(modelBuilder);
        }

        protected void SetLog(DbContextOptionsBuilder optionsBuilder)
        {
            if (ServiceProvider == null)
                return;

            if (dbSettings.EnableSqlLog)
                optionsBuilder.UseLoggerFactory((ILoggerFactory)ServiceProvider.GetService(typeof(ILoggerFactory)));
            if (dbSettings.EnableSensitiveDataLogging)
                optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
