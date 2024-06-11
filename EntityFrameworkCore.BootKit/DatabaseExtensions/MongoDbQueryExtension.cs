using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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

    public static UpdateResult UpdateOne<TDocument, TField>(this IMongoCollection<TDocument> source, 
        Expression<Func<TDocument, bool>> filter, 
        Expression<Func<TDocument, TField>> field, 
        TField value)
    {
        return source.UpdateOne(filter, Builders<TDocument>.Update.Set(field, value), options: new UpdateOptions
        {
            IsUpsert = true
        });
    }

    public static UpdateResult UpdateOne<TDocument, TField1, TField2>(this IMongoCollection<TDocument> source,
        Expression<Func<TDocument, bool>> filter,
        (Expression<Func<TDocument, TField1>>, TField1) kv1,
        (Expression<Func<TDocument, TField2>>, TField2) kv2)
    {
        var update = Builders<TDocument>.Update
            .Set(kv1.Item1, kv1.Item2)
            .Set(kv2.Item1, kv2.Item2);

        return source.UpdateOne(filter, update, options: new UpdateOptions
        {
            IsUpsert = true
        });
    }

    public static UpdateResult UpdateOne<TDocument, TField1, TField2, TField3>(this IMongoCollection<TDocument> source,
        Expression<Func<TDocument, bool>> filter,
        (Expression<Func<TDocument, TField1>>, TField1) kv1,
        (Expression<Func<TDocument, TField2>>, TField2) kv2,
        (Expression<Func<TDocument, TField3>>, TField3) kv3)
    {
        var update = Builders<TDocument>.Update
            .Set(kv1.Item1, kv1.Item2)
            .Set(kv2.Item1, kv2.Item2)
            .Set(kv3.Item1, kv3.Item2);

        return source.UpdateOne(filter, update, options: new UpdateOptions
        {
            IsUpsert = true
        });
    }

    public static UpdateResult UpdateMany<TDocument, TField>(this IMongoCollection<TDocument> source, 
        Expression<Func<TDocument, bool>> filter, 
        Expression<Func<TDocument, TField>> field, 
        TField value)
    {
        return source.UpdateMany(filter, Builders<TDocument>.Update.Set(field, value), options: new UpdateOptions
        {
            IsUpsert = true
        });
    }

    public static DeleteResult DeleteOne<TDocument, TField>(this IMongoCollection<TDocument> source, 
        Expression<Func<TDocument, bool>> filter)
    {
        return source.DeleteOne(filter);
    }

    public static DeleteResult DeleteMany<TDocument, TField>(this IMongoCollection<TDocument> source, 
        Expression<Func<TDocument, bool>> filter)
    {
        return source.DeleteMany(filter);
    }
}
