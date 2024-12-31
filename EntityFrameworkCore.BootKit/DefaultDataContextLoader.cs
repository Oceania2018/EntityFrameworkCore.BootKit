using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace EntityFrameworkCore.BootKit;

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
        else if (db.Equals("InMemory"))
        {
            dc.BindDbContext<IDbRecordBinding, DbContext4Memory>(new DatabaseBind
            {
            });
        }

        return dc;
    }
}
