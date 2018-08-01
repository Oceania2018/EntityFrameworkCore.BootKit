using EntityFrameworkCore.BootKit.UnitTest.Tables;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace EntityFrameworkCore.BootKit.UnitTest
{
    [TestClass]
    public class DatabaseTest
    {
        [TestMethod]
        public void TestSqlite()
        {
            AddRecord(GetDb(DatabaseType.Sqlite));
            AddRecordByTableName(GetDb(DatabaseType.Sqlite));
            GetRecordsByTableName(GetDb(DatabaseType.Sqlite));
            UpdateRecordsByTableName(GetDb(DatabaseType.Sqlite));
            PatchRecord(GetDb(DatabaseType.Sqlite));
        }

        [TestMethod]
        public void TestSqlServer()
        {
            AddRecord(GetDb(DatabaseType.SqlServer));
            AddRecordByTableName(GetDb(DatabaseType.SqlServer));
            GetRecordsByTableName(GetDb(DatabaseType.SqlServer));
            UpdateRecordsByTableName(GetDb(DatabaseType.SqlServer));
            PatchRecord(GetDb(DatabaseType.SqlServer));
        }

        [TestMethod]
        public void TestMySql()
        {
            AddRecord(GetDb(DatabaseType.MySql));
            AddRecordByTableName(GetDb(DatabaseType.MySql));
            GetRecordsByTableName(GetDb(DatabaseType.MySql));
            UpdateRecordsByTableName(GetDb(DatabaseType.MySql));
            PatchRecord(GetDb(DatabaseType.MySql));
        }

        [TestMethod]
        public void TestPostgreSql()
        {
            AddRecord(GetDb(DatabaseType.PostgreSql));
            AddRecordByTableName(GetDb(DatabaseType.PostgreSql));
            GetRecordsByTableName(GetDb(DatabaseType.PostgreSql));
            UpdateRecordsByTableName(GetDb(DatabaseType.PostgreSql));
            PatchRecord(GetDb(DatabaseType.PostgreSql));
        }

        [TestMethod]
        public void TestMongoDb()
        {
            var db = GetDb(DatabaseType.MongoDb);
            var collection = db.Collection<MongoDbCollection>().FirstOrDefault();
        }

        private Database GetDb(DatabaseType databaseType)
        {
            var db = new Database();
            Database.Assemblies = new string[] { "EntityFrameworkCore.BootKit.UnitTest" };

            if (databaseType == DatabaseType.Sqlite)
            {
                db.BindDbContext<IDbRecord, DbContext4Sqlite>(new DatabaseBind
                {
                    MasterConnection = new SqliteConnection($"Data Source={Directory.GetCurrentDirectory()}\\..\\..\\..\\..\\Bootkit.db"),
                    CreateDbIfNotExist = true,
                });
            }
            else if (databaseType == DatabaseType.SqlServer)
            {
                db.BindDbContext<IDbRecord, DbContext4SqlServer>(new DatabaseBind
                {
                    MasterConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bootkit;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"),
                    CreateDbIfNotExist = true
                });
            }
            else if (databaseType == DatabaseType.MySql)
            {
                db.BindDbContext<IDbRecord, DbContext4MySql>(new DatabaseBind
                {
                    MasterConnection = new MySqlConnection("Data Source=;port=3306;Initial Catalog=;user id=;password=;CharSet=utf8;Allow User Variables=True;"),
                    CreateDbIfNotExist = false
                });
            }
            else if (databaseType == DatabaseType.MongoDb)
            {
                db.BindDbContext<INoSqlDbRecord, DbContext4MongoDb>(new DatabaseBind
                {
                    MasterConnection = new MongoDbConnection("mongodb://user:password@localhost:27017/db"),
                });
            }
            else if (databaseType == DatabaseType.PostgreSql)
            {
                db.BindDbContext<INoSqlDbRecord, DbContext4MongoDb>(new DatabaseBind
                {
                    MasterConnection = new MongoDbConnection("Server=; Port=5439;User ID=;Password=;Database="),
                });
            }

            return db;
        }

        private void GetRecordsByTableName(Database db)
        {
            var table = db.Table("PizzaOrder");
            var pizzaOrder = table.First(x => x.Id == PIZZA_ORDER_ID) as PizzaOrder;
            Assert.IsNotNull(pizzaOrder.Id);
        }

        public static String PIZZA_ORDER_ID = "7974f8d9-9124-4e24-a906-2e5bb3323e01";

        private void AddRecord(Database db)
        {
            var pizza = new PizzaOrder
            {
                Id = PIZZA_ORDER_ID,
                OrderNumber = new Random().Next(1000).ToString(),
                CustomerName = "Haiping Chen",
                CreatedTime = DateTime.UtcNow,
                PizzaTypes = new List<PizzaType> {
                    new PizzaType { Name = "Pizza Type 1", Amount = 10.99M },
                    new PizzaType { Name = "Pizza Type 2", Amount = 9.9M }
                }
            };

            if (db.Table<PizzaOrder>().Any(x => x.Id == PIZZA_ORDER_ID)) return;

            db.DbTran(() => db.Table<PizzaOrder>().Add(pizza));

            var order = db.Table<PizzaOrder>().Include(x => x.PizzaTypes).FirstOrDefault(x => x.Id == PIZZA_ORDER_ID);

            Assert.IsNotNull(order.Id);
            Assert.IsTrue(order.PizzaTypes.Count == 2);
        }

        private void UpdateRecordsByTableName(Database db)
        {
            DateTime dt = DateTime.UtcNow;

            var table = db.Table("PizzaOrder");
            var pizzaOrder = table.First(x => x.Id == PIZZA_ORDER_ID) as PizzaOrder;
            pizzaOrder.CreatedTime = dt;

            var po = db.Table<PizzaOrder>().Find(PIZZA_ORDER_ID);
            Assert.IsTrue(po.CreatedTime == dt);
        }

        private void PatchRecord(Database db)
        {
            DateTime dt = DateTime.UtcNow.AddMinutes(-5);

            var patch = new DbPatchModel
            {
                Table = "PizzaOrder",
                Id = PIZZA_ORDER_ID
            };

            patch.Values.Add("CreatedTime", dt);

            int row = db.DbTran(() => db.Patch<IDbRecord>(patch));
            
            var po = db.Table<PizzaOrder>().Find(PIZZA_ORDER_ID);
            Assert.IsTrue(po.CreatedTime.ToString() == dt.ToString());
        }

        private void AddRecordByTableName(Database db)
        {
            var entity = new PizzaType {
                Name = "PIZZA" + DateTime.UtcNow.Ticks,
                OrderId = PIZZA_ORDER_ID,
                Amount = new Random().Next(10000)
            };

            db.Add("PizzaType", entity);

            Assert.IsNotNull(entity.Id);
        }
    }
}
