using EntityFrameworkCore.BootKit.UnitTest.Tables;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var db = new Database();

            db.BindDbContext<IDbRecord, DbContext4Sqlite>(new DatabaseBind
            {
                MasterConnection = new SqliteConnection($"Data Source={Directory.GetCurrentDirectory()}\\..\\..\\..\\..\\bootkit.db"),
                CreateDbIfNotExist = true,
                AssemblyNames = new string[] { "EntityFrameworkCore.BootKit.UnitTest" }
            });

            int row = db.DbTran(() =>
            {
                AddRecord(db);
                GetRecordsByTableName(db);
                UpdateRecordsByTableName(db);
                PatchRecord(db);
            });

            Assert.IsTrue(row == 1);
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

            int row = db.DbTran(() =>
            {
                AddRecord(db);
                GetRecordsByTableName(db);
                UpdateRecordsByTableName(db);
                PatchRecord(db);
            });

            Assert.IsTrue(row == 1);
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

            db.Table<PizzaOrder>().Add(pizza);

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
            DateTime dt = DateTime.UtcNow;

            var patch = new DbPatchModel
            {
                Table = "PizzaOrder",
                Id = PIZZA_ORDER_ID
            };

            patch.Values.Add("CreatedTime", dt);

            db.Patch<IDbRecord>(patch);

            var po = db.Table<PizzaOrder>().Find(PIZZA_ORDER_ID);
            Assert.IsTrue(po.CreatedTime == dt);
        }
    }
}
