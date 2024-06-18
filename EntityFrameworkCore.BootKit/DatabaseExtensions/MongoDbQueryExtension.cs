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
        return source.UpdateOne(filter, Builders<TDocument>.Update.Set(field, value));
    }

    public static UpdateResult UpdateOne<TDocument>(this IMongoCollection<TDocument> source,
        Expression<Func<TDocument, bool>> filter,
        (Expression<Func<TDocument, object>>, object)[] kvs)
    {
        var updateDefinitionBuilder = Builders<TDocument>.Update;
        var definitions = new List<UpdateDefinition<TDocument>>();
        foreach (var pair in kvs)
        {
            definitions.Add(updateDefinitionBuilder.Set(pair.Item1, pair.Item2));
        }

        var updateFields = updateDefinitionBuilder.Combine(definitions);

        return source.UpdateOne(filter, updateFields);
    }

    public static UpdateResult UpsetOne<TDocument, TField>(this IMongoCollection<TDocument> source,
        Expression<Func<TDocument, bool>> filter,
        Expression<Func<TDocument, TField>> field,
        TField value)
        {
            return source.UpdateOne(filter, Builders<TDocument>.Update.Set(field, value), options: new UpdateOptions
            {
                IsUpsert = true
            });
        }

    public static UpdateResult UpsertOne<TDocument, TField1, TField2>(this IMongoCollection<TDocument> source,
        Expression<Func<TDocument, bool>> filter,
        (Expression<Func<TDocument, TField1>>, TField1)[] kvs)
    {
        var updateDefinitionBuilder = Builders<TDocument>.Update;
        var definitions = new List<UpdateDefinition<TDocument>>();
        foreach (var pair in kvs)
        {
            definitions.Add(updateDefinitionBuilder.Set(pair.Item1, pair.Item2));
        }

        var updateFields = updateDefinitionBuilder.Combine(definitions);

        return source.UpdateOne(filter, updateFields, options: new UpdateOptions
        {
            IsUpsert = true
        });
    }

    public static UpdateResult UpdateMany<TDocument, TField>(this IMongoCollection<TDocument> source, 
        Expression<Func<TDocument, bool>> filter, 
        Expression<Func<TDocument, TField>> field, 
        TField value)
    {
        return source.UpdateMany(filter, Builders<TDocument>.Update.Set(field, value));
    }

    public static UpdateResult UpsertMany<TDocument, TField>(this IMongoCollection<TDocument> source,
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
