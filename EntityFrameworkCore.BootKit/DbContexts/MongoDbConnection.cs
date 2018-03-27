using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public sealed class MongoDbConnection : DbConnection, ICloneable
    {
        public override string ConnectionString { get; set; }

        public override string Database => throw new NotImplementedException();

        public override string DataSource => throw new NotImplementedException();

        public override string ServerVersion => throw new NotImplementedException();

        public override ConnectionState State => throw new NotImplementedException();

        public MongoDbConnection()
        {

        }

        public MongoDbConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        protected override DbCommand CreateDbCommand()
        {
            throw new NotImplementedException();
        }

        public override void Open()
        {
            throw new NotImplementedException();
        }
    }
}
