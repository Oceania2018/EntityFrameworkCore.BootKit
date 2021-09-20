using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public class DatabaseSettings
    {
        public string Default { get; set; }
        public DbConnectionSetting DefaultConnection { get; set; }
        public bool EnableSqlLog { get; set; }
        public bool EnableSensitiveDataLogging { get; set; }
    }
}
