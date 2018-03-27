# EntityFrameworkCore.BootKit
EntityFrameworkCore Boot Kit (EFBK) is a quick start lib for using EntityFrameworkCore.

## Features:

* Inherits from EntityFrameworkCore Triggers to enable entries update notfication.
* Support mulitple databases like MySql, SQL Server, Sqlite, InMemory.
* Support dynamic linq to query and update database.
* Support read/write seperate mode. Randomly choose multiple slaves.
* Multiple database with distributed transaction supported, and MySQL multiple databases/tables sharding supported.
* Tracking entry change history.
* Built-in DbFactory with access control list (ACL) hook.
* Support MongoDb in LINQ.

## Get started
### How to install
````sh
Install-Package EntityFrameworkCore.BootKit
````

#### How to use
* Define entity

````cs
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
````
* Init data context

````cs
var db = new Database();

db.BindDbContext<IDbRecord, DbContext4Sqlite>(new DatabaseBind
{
	MasterConnection = new SqliteConnection($"Data Source=		{Directory.GetCurrentDirectory()}\\..\\..\\..\\..\\bootkit.db"),
	CreateDbIfNotExist = true,
	AssemblyNames = new string[] { "EntityFrameworkCore.BootKit.UnitTest" }
});
````
* Retrieve record

````cs
var order = db.Table<PizzaOrder>().Include(x => x.PizzaTypes).FirstOrDefault();

````
* Retrieve record by table name

````cs
var table = db.Table("PizzaOrder");
var pizzaOrder = table.First() as PizzaOrder;
````
* Update record in transaction

````cs
int row = db.DbTran(() =>
{
	var po = db.Table<PizzaOrder>().Find(PIZZA_ORDER_ID);
    po.CreatedTime = DateTime.UtcNow
});
````
* Update record in Patch function

````cs
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
````
* Implement IRequireDbPermission to interupt update
* View raw sql

````cs
	string sql = table.ToSql();
````

* Added MongoDb support

````cs
db.BindDbContext<IDbRecord, DbContext4MongoDb>(new DatabaseBind
{
	MasterConnection = new MongoDbConnection("mongodb://user:password@localhost:27017/db"),
});
var collection = db.Collection<MongoDbCollectionTest>().FirstOrDefault();
````