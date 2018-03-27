using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public static class MongoDbContextOptionsExtensions
    {
        public static DbContextOptionsBuilder UseMongoDb(this DbContextOptionsBuilder optionsBuilder, string connectionString)
        {
            return optionsBuilder;
        }
    }
}
