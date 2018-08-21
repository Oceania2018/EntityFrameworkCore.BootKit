using EntityFrameworkCore.BootKit.DbContexts;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public class DefaultDataContextLoader
    {
        /// <summary>
        /// Get data contexts implemented IDbRecord
        /// </summary>
        /// <returns></returns>
        public Database GetDefaultDc()
        {
            return GetDefaultDc<IDbRecord>("Database");
        }

        public Database GetDefaultDc<IDbRecordBinding>(string dbConfigSection)
        {
            var dc = new Database();

            var config = (IConfiguration)AppDomain.CurrentDomain.GetData("Configuration");
            var contentRootPath = AppDomain.CurrentDomain.GetData("ContentRootPath").ToString();

            string db = config.GetSection($"{dbConfigSection}:Default").Value;
            string connectionString = config.GetSection($"{dbConfigSection}:ConnectionStrings")[db];

            if (db.Equals("SqlServer"))
            {
                dc.BindDbContext<IDbRecordBinding, DbContext4SqlServer>(new DatabaseBind
                {
                    MasterConnection = new SqlConnection(connectionString),
                    CreateDbIfNotExist = true
                });
            }
            else if (db.Equals("Sqlite"))
            {
                connectionString = connectionString.Replace($"|DataDirectory|", Path.Combine(contentRootPath, "App_Data") + Path.DirectorySeparatorChar.ToString());
                dc.BindDbContext<IDbRecordBinding, DbContext4Sqlite>(new DatabaseBind
                {
                    MasterConnection = new SqliteConnection(connectionString),
                    SlaveConnections = new List<System.Data.Common.DbConnection> {
                        new SqliteConnection(connectionString)
                    },
                    CreateDbIfNotExist = true
                });
            }
            else if (db.Equals("MySql"))
            {
                dc.BindDbContext<IDbRecordBinding, DbContext4MySql>(new DatabaseBind
                {
                    MasterConnection = new MySqlConnection(connectionString),
                    CreateDbIfNotExist = true
                });
            }
            else if (db.Equals("PostgreSql"))
            {
                dc.BindDbContext<IDbRecordBinding, DbContext4PostgreSql>(new DatabaseBind
                {
                    MasterConnection = new NpgsqlConnection("Server=; Port=5439;User ID=;Password=;Database=;SSL Mode=Require;Trust Server Certificate=True;Use SSL Stream=True"),
                    CreateDbIfNotExist = true
                });
            }
            else if (db.Equals("Redshift"))
            {
                dc.BindDbContext<IDbRecordBinding, DbContext4PostgreSql>(new DatabaseBind
                {
                    MasterConnection = new NpgsqlConnection("Server=*.us-east-1.redshift.amazonaws.com; Port=5439;User ID=;Password=;Database=;Server Compatibility Mode=Redshift;SSL Mode=Require;Trust Server Certificate=True;Use SSL Stream=True"),
                    CreateDbIfNotExist = true
                });
            }
            else if (db.Equals("InMemory"))
            {
                dc.BindDbContext<IDbRecordBinding, DbContext4Memory>(new DatabaseBind
                {
                    CreateDbIfNotExist = true
                });
            }

            return dc;
        }

        public Database GetDefaultDc2<IDbRecordBinding>(string dbConfigSection)
        {
            var dc = new Database();

            var config = (IConfiguration)AppDomain.CurrentDomain.GetData("Assemblies");
            var contentRootPath = AppDomain.CurrentDomain.GetData("ContentRootPath").ToString();
            string db = config.GetSection($"{dbConfigSection}:Default").Value;
            string connectionString = config.GetSection($"{dbConfigSection}:ConnectionStrings")[db];

            if (db.Equals("SqlServer"))
            {
                dc.BindDbContext<IDbRecordBinding, DbContext4SqlServer2>(new DatabaseBind
                {
                    MasterConnection = new SqlConnection(connectionString),
                    CreateDbIfNotExist = true
                });
            }
            else if (db.Equals("Sqlite"))
            {
                connectionString = connectionString.Replace($"|DataDirectory|", Path.Combine(contentRootPath, "App_Data", Path.DirectorySeparatorChar.ToString()));
                Console.WriteLine(connectionString);
                dc.BindDbContext<IDbRecordBinding, DbContext4Sqlite2>(new DatabaseBind
                {
                    MasterConnection = new SqliteConnection(connectionString),
                    SlaveConnections = new List<System.Data.Common.DbConnection> {
                        new SqliteConnection(connectionString)
                    },
                    CreateDbIfNotExist = true
                });
            }
            else if (db.Equals("MySql"))
            {
                dc.BindDbContext<IDbRecordBinding, DbContext4MySql2>(new DatabaseBind
                {
                    MasterConnection = new MySqlConnection(connectionString),
                    CreateDbIfNotExist = true
                });
            }
            else if (db.Equals("InMemory"))
            {
                dc.BindDbContext<IDbRecordBinding, DbContext4Memory>(new DatabaseBind
                {
                });
            }

            return dc;
        }
    }
}
