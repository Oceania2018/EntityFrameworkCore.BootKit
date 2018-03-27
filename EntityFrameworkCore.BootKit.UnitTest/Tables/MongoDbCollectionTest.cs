using System;
using System.Collections.Generic;
using System.Text;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace EntityFrameworkCore.BootKit.UnitTest.Tables
{
    [BsonIgnoreExtraElements]
    public class MongoDbCollectionTest : IDbRecord
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
    }
}
