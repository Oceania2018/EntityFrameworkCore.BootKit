using EntityFrameworkCore.BootKit.UnitTest.Tables;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            var db = new Database();

            db.BindDbContext<IDbRecord, DbContext4Sqlite>(new DatabaseBind
            {
                MasterConnection = new SqliteConnection($"Data Source={Directory.GetCurrentDirectory()}\\..\\..\\bootkit.db"),
                CreateDbIfNotExist = true,
                AssemblyNames = new string[] { "EntityFrameworkCore.BootKit.UnitTest" }
            });

            AddRecord(db);
            GetRecordsByTableName(db);
        }

        [TestMethod]
        public void TestSqlServer()
        {
            var db = new Database();

            db.BindDbContext<IDbRecord, DbContext4SqlServer>(new DatabaseBind
            {
                MasterConnection = new SqlConnection($"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=bootkit;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"),
                CreateDbIfNotExist = true,
                AssemblyNames = new string[] { "EntityFrameworkCore.BootKit.UnitTest" }
            });

            AddRecord(db);
            GetRecordsByTableName(db);
        }

        private void GetRecordsByTableName(Database db)
        {
            var table = db.Table("PizzaOrder");
            var pizzaOrder = table.First() as PizzaOrder;
            Assert.IsNotNull(pizzaOrder.Id);
        }

        private void AddRecord(Database db)
        {
            db.DbTran(delegate {
                db.Table<PizzaOrder>().Add(new PizzaOrder
                {
                    OrderNumber = new Random().Next(1000).ToString(),
                    CustomerName = "Haiping Chen",
                    CreatedTime = DateTime.UtcNow
                });
            });

            var order = db.Table<PizzaOrder>().FirstOrDefault();

            Assert.IsNotNull(order.Id);
        }
    }
}
