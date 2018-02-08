using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EntityFrameworkCore.BootKit
{
    public class DbContext4MongoDb : DataContext
    {
        public DbContext4MongoDb(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMongoDb(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public IMongoCollection<TEntity> Set<TEntity>(string name = "")
        {
            return GetDatabase().GetCollection<TEntity>(String.IsNullOrEmpty(name) ? typeof(TEntity).Name : name);
        }

        public IMongoDatabase GetDatabase()
        {
            string databaseName = ConnectionString.Split('/').Last();

            MongoClientSettings settings = new MongoClientSettings();
            settings.ConnectTimeout = new TimeSpan(0, 0, 0, 30, 0);
            settings.ConnectionMode = ConnectionMode.Direct;

            string server = new Regex("@.+/").Match(ConnectionString).Value.Substring(1);
            string host = server.Split(':').First();
            string userName = new Regex("//[^@]+").Match(ConnectionString).Value.Split(":").First().Substring(2);
            string password = new Regex("//[^@]+").Match(ConnectionString).Value.Split(":").Last();

            MongoCredential credential = MongoCredential.CreateCredential(databaseName, userName, password);
            settings.Credential = credential;

            settings.Server = new MongoServerAddress(host);

            MongoClient client = new MongoClient(settings);

            IMongoDatabase database = client.GetDatabase(databaseName);

            return database;
        }


    }
}
