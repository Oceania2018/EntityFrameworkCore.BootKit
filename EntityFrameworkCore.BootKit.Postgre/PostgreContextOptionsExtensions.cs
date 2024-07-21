using EntityFrameworkCore.BootKit.DbContexts;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace EntityFrameworkCore.BootKit;

public static class PostgreContextOptionsExtensions
{
    public static Database GetDefaultPostgre<IDbRecordBinding>(this DefaultDataContextLoader loader, string dbConfigSection)
    {
        var dc = new Database();

        var config = (IConfiguration)AppDomain.CurrentDomain.GetData("Configuration");
        var contentRootPath = AppDomain.CurrentDomain.GetData("ContentRootPath").ToString();

        string db = config.GetSection($"{dbConfigSection}:Default").Value;
        string connectionString = config.GetSection($"{dbConfigSection}:ConnectionStrings")[db];

        if (db.Equals("Redshift"))
        {
            dc.BindDbContext<IDbRecordBinding, DbContext4PostgreSql>(new DatabaseBind
            {
                MasterConnection = new NpgsqlConnection("Server=*.us-east-1.redshift.amazonaws.com; Port=5439;User ID=;Password=;Database=;Server Compatibility Mode=Redshift;SSL Mode=Require;Trust Server Certificate=True;Use SSL Stream=True"),
                CreateDbIfNotExist = true
            });
        }

        return dc;
    }

    public static Database GetDefaultRedshift<IDbRecordBinding>(this DefaultDataContextLoader loader, string dbConfigSection)
    {
        var dc = new Database();

        var config = (IConfiguration)AppDomain.CurrentDomain.GetData("Configuration");
        var contentRootPath = AppDomain.CurrentDomain.GetData("ContentRootPath").ToString();

        string db = config.GetSection($"{dbConfigSection}:Default").Value;
        string connectionString = config.GetSection($"{dbConfigSection}:ConnectionStrings")[db];

        if (db.Equals("Redshift"))
        {
            dc.BindDbContext<IDbRecordBinding, DbContext4PostgreSql>(new DatabaseBind
            {
                MasterConnection = new NpgsqlConnection("Server=*.us-east-1.redshift.amazonaws.com; Port=5439;User ID=;Password=;Database=;Server Compatibility Mode=Redshift;SSL Mode=Require;Trust Server Certificate=True;Use SSL Stream=True"),
                CreateDbIfNotExist = true
            });
        }

        return dc;
    }
}
