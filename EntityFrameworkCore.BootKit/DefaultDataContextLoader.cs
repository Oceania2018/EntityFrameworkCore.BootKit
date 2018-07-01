using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
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

            string db = Database.Configuration.GetSection($"{dbConfigSection}:Default").Value;
            string connectionString = Database.Configuration.GetSection($"{dbConfigSection}:ConnectionStrings")[db];

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
                connectionString = connectionString.Replace($"|DataDirectory|", Database.ContentRootPath + $"{Path.DirectorySeparatorChar}App_Data{Path.DirectorySeparatorChar}");
                Console.WriteLine(connectionString);
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
