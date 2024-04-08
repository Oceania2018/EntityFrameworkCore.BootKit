using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFrameworkCore.BootKit;

public static class MongoDbQueryExtension
{
    public static IMongoQueryable<TSource> Queryable<TSource>(this IMongoCollection<TSource> source)
    {
        return source.AsQueryable();
    }

    public static TSource FirstOrDefault<TSource>(this IMongoCollection<TSource> source, Expression<Func<TSource, bool>> filter = null)
    {
        return filter == null ? source.AsQueryable().FirstOrDefault() : source.AsQueryable().FirstOrDefault(filter);
    }

    public static IMongoQueryable<TSource> Where<TSource>(this IMongoCollection<TSource> source, Expression<Func<TSource, bool>> filter)
    {
        return source.AsQueryable().Where(filter);
    }

    public static UpdateResult UpdateOne<TDocument, TField>(this IMongoCollection<TDocument> source, Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value)
    {
        return source.UpdateOne(filter, Builders<TDocument>.Update.Set(field, value));
    }

    public static DeleteResult DeleteOne<TDocument, TField>(this IMongoCollection<TDocument> source, Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value)
    {
        return source.DeleteMany(filter);
    }
}
