using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public enum DatabaseType
    {
        InMemory = 1,
        Sqlite = 2,
        SqlServer = 3,
        MySql = 4,
        Oracle = 5,
        MongoDb = 6,
        PostgreSql = 7
    }
}
