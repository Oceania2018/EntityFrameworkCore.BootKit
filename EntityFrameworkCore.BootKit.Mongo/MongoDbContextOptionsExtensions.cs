using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkCore.BootKit;

public static class MongoDbContextOptionsExtensions
{
    public static DbContextOptionsBuilder UseMongoDb(this DbContextOptionsBuilder optionsBuilder, string connectionString)
    {
        return optionsBuilder;
    }

    public static IMongoCollection<T> Collection<T>(this Database database, string name = "") where T : class
    {
        Type entityType = typeof(T);

        DatabaseBind binding = database.DbContextBinds.First(x => x.GetType().Equals(typeof(DatabaseBind)));
        var db = database.GetMaster(entityType);
        if (string.IsNullOrEmpty(name))
        {
            // Default collection name
            name = typeof(T).Name;

            // Check if the class has TableAttribute
            var attributes = typeof(T).GetCustomAttributesData()
                .FirstOrDefault(x => x.AttributeType == typeof(TableAttribute));
            if (attributes != null)
            {
                var arguments = attributes.ConstructorArguments;
                if (arguments.Count > 0)
                {
                    name = attributes.ConstructorArguments[0].Value.ToString();
                }
            }
        }
        return (db as DbContext4MongoDb).Set<T>(name);
    }
}
