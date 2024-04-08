using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace EntityFrameworkCore.BootKit.UnitTest.Tables
{
    [BsonIgnoreExtraElements]
    public class MongoDbCollection : INoSqlDbRecord
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public interface INoSqlDbRecord
    {

    }
}
