EntityFrameworkCore.BootKit
===========================
EntityFrameworkCore Boot Kit (EFBK) is a quick start database connect library for using .NET EntityFrameworkCore.

Features:
*********
* Inherits from EntityFrameworkCore Triggers to enable entries update notfication.
* Support mulitple databases like MySql, SQL Server, Sqlite, PostgreSql, MongoDb, Amazon Redshift and InMemory.
* Support dynamic linq to query and update database.
* Support read/write seperated mode. Randomly choose multiple slaves.
* Multiple database with distributed transaction supported, and MySQL multiple databases/tables sharding supported.
* Tracking entry change history.
* Built-in DbFactory with access control list (ACL) hook.

Get started
***********
How to install
::

  Install-Package EntityFrameworkCore.BootKit


How to use

1. Define entity

:: 

  public class PizzaOrder : DbRecord, IDbRecord
  {
	[MaxLength(32)]
	public String OrderNumber { get; set; } 

	[MaxLength(64)]
	public String CustomerName { get; set; }

	[Required]
	public DateTime CreatedTime { get; set; }

	[ForeignKey("OrderId")]
	public List<PizzaType> PizzaTypes { get; set; }
  }

2. Init data context

::

  var db = new Database();
  AppDomain.CurrentDomain.SetData("Assemblies", new string[] { "EntityFrameworkCore.BootKit.UnitTest" });

  // bind as much data context as you can
  db.BindDbContext<IDbRecord, DbContext4Sqlite>(new DatabaseBind
  {
	MasterConnection = new SqliteConnection($"Data Source={Directory.GetCurrentDirectory()}\\..\\..\\..\\..\\bootkit.db"),
	CreateDbIfNotExist = true
  });

  db.BindDbContext<IDbRecord, DbContext4PostgreSql>(new DatabaseBind
  {
      MasterConnection = new NpgsqlConnection("Server=; Port=5439;User ID=;Password=;Database=;SSL Mode=Require;Trust Server Certificate=True;Use SSL Stream=True"),
  });

3. Retrieve record

:: 

  var order = db.Table<PizzaOrder>().Include(x => x.PizzaTypes).FirstOrDefault();

4. Retrieve record by table name

::

  var table = db.Table("PizzaOrder");
  var pizzaOrder = table.First() as PizzaOrder;

5. Update record in transaction

::

  int row = db.DbTran(() =>
  {
    var po = db.Table<PizzaOrder>().Find(PIZZA_ORDER_ID);
    po.CreatedTime = DateTime.UtcNow
  });

6. Update record in Patch function

:: 

  int row = db.DbTran(() =>
  {
	var patch = new DbPatchModel
	{
		Table = "PizzaOrder",
		Id = PIZZA_ORDER_ID
	};

	patch.Values.Add("CreatedTime", dt);
	db.Patch<IDbRecord>(patch);
  });

7. Implement IRequireDbPermission to interupt update
8. View raw sql

::
 
  string sql = table.ToSql();


9. Added MongoDb support

::

  db.BindDbContext<IDbRecord, DbContext4MongoDb>(new DatabaseBind
  {
	MasterConnection = new MongoDbConnection("mongodb://user:password@localhost:27017/db"),
  });
  var collection = db.Collection<MongoDbCollectionTest>().FirstOrDefault();

10. Support Amazon Redshift

::

  db.BindDbContext<IDbRecord, DbContext4Redshift>(new DatabaseBind
  {
      string connString = "Server=*.us-east-1.redshift.amazonaws.com; Port=5439;User ID=;Password=;Database=;Server Compatibility Mode=Redshift;SSL Mode=Require;Trust Server Certificate=True;Use SSL Stream=True";
      MasterConnection = new NpgsqlConnection(connString),
  });


Documentation
*************
https://entityframeworkcorebootkit.readthedocs.io
