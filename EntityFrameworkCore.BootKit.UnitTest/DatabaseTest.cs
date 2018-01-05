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
            AddRecord();
            GetRecordsByTableName();
            UpdateRecordsByTableName();
            PatchRecord();
        }

        [TestMethod]
        public void TestSqlServer()
        {
            AddRecord();
            GetRecordsByTableName();
            UpdateRecordsByTableName();
            PatchRecord();
        }

        private Database GetDb()
        {
            var db = new Database();

            db.BindDbContext<IDbRecord, DbContext4Sqlite>(new DatabaseBind
            {
                MasterConnection = new SqliteConnection($"Data Source={Directory.GetCurrentDirectory()}\\..\\..\\..\\..\\bootkit.db"),
                CreateDbIfNotExist = true,
                AssemblyNames = new string[] { "EntityFrameworkCore.BootKit.UnitTest" }
            });

            return db;
        }

        private void GetRecordsByTableName()
        {
            var db = GetDb();

            var table = db.Table("PizzaOrder");
            var pizzaOrder = table.First(x => x.Id == PIZZA_ORDER_ID) as PizzaOrder;
            Assert.IsNotNull(pizzaOrder.Id);
        }

        public static String PIZZA_ORDER_ID = "7974f8d9-9124-4e24-a906-2e5bb3323e01";

        private void AddRecord()
        {
            var db = GetDb();

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

        private void UpdateRecordsByTableName()
        {
            var db = GetDb();

            DateTime dt = DateTime.UtcNow;

            var table = db.Table("PizzaOrder");
            var pizzaOrder = table.First(x => x.Id == PIZZA_ORDER_ID) as PizzaOrder;
            pizzaOrder.CreatedTime = dt;

            var po = db.Table<PizzaOrder>().Find(PIZZA_ORDER_ID);
            Assert.IsTrue(po.CreatedTime == dt);
        }

        private void PatchRecord()
        {
            var db = GetDb();

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
    }
}
