using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public class DbConnectionSetting
    {
        public DbConnectionSetting()
        {
            Slavers = new string[0];
        }

        public string Master { get; set; }
        public string[] Slavers { get; set; }
    }
}
