using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace EntityFrameworkCore.BootKit;

public static class MySqlContextOptionsExtensions
{
    public static Database GetDefaultPostgre<IDbRecordBinding>(this DefaultDataContextLoader loader, string dbConfigSection)
    {
        var dc = new Database();

        var config = (IConfiguration)AppDomain.CurrentDomain.GetData("Configuration");
        var contentRootPath = AppDomain.CurrentDomain.GetData("ContentRootPath").ToString();

        string db = config.GetSection($"{dbConfigSection}:Default").Value;
        string connectionString = config.GetSection($"{dbConfigSection}:ConnectionStrings")[db];

        if (db.Equals("MySql"))
        {
            dc.BindDbContext<IDbRecordBinding, DbContext4MySql>(new DatabaseBind
            {
                MasterConnection = new MySqlConnection(connectionString),
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

        if (db.Equals("Aurora"))
        {
            dc.BindDbContext<IDbRecordBinding, DbContext4Aurora>(new DatabaseBind
            {
                MasterConnection = new MySqlConnection(connectionString),
                CreateDbIfNotExist = true
            });
        }

        return dc;
    }
}
