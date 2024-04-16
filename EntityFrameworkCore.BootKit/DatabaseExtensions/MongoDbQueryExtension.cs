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
        return filter == null ? source.Queryable().FirstOrDefault() : source.Queryable().FirstOrDefault(filter);
    }

    public static TSource LastOrDefault<TSource>(this IMongoCollection<TSource> source, Expression<Func<TSource, bool>> filter = null)
    {
        throw new NotSupportedException("MongoDB does not support LastOrDefault() method.");
        // return filter == null ? source.Queryable().LastOrDefault() : source.Queryable().LastOrDefault(filter);
    }

    public static IOrderedMongoQueryable<TSource> OrderByDescending<TSource, TKey>(this IMongoCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector)
    {
        return source.Queryable().OrderByDescending(keySelector);
    }

    public static IMongoQueryable<TSource> Where<TSource>(this IMongoCollection<TSource> source, Expression<Func<TSource, bool>> filter)
    {
        return source.Queryable().Where(filter);
    }

    public static UpdateResult UpdateOne<TDocument, TField>(this IMongoCollection<TDocument> source, Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value)
    {
        return source.UpdateOne(filter, Builders<TDocument>.Update.Set(field, value));
    }

    public static UpdateResult UpdateMany<TDocument, TField>(this IMongoCollection<TDocument> source, Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value)
    {
        return source.UpdateMany(filter, Builders<TDocument>.Update.Set(field, value));
    }

    public static DeleteResult DeleteOne<TDocument, TField>(this IMongoCollection<TDocument> source, Expression<Func<TDocument, bool>> filter)
    {
        return source.DeleteOne(filter);
    }

    public static DeleteResult DeleteMany<TDocument, TField>(this IMongoCollection<TDocument> source, Expression<Func<TDocument, bool>> filter)
    {
        return source.DeleteMany(filter);
    }
}
