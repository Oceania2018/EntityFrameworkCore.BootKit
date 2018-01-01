using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Remotion.Linq.Parsing.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public static class DataContextExtension
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();
        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");
        private static readonly PropertyInfo NodeTypeProviderField = QueryCompilerTypeInfo.DeclaredProperties.Single(x => x.Name == "NodeTypeProvider");
        private static readonly MethodInfo CreateQueryParserMethod = QueryCompilerTypeInfo.DeclaredMethods.First(x => x.Name == "CreateQueryParser");
        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");
        private static readonly PropertyInfo DatabaseDependenciesProperty = typeof(Microsoft.EntityFrameworkCore.Storage.Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        /// <summary>
        /// Retrieve trace string for IQueryable
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ToSql<T>(this IQueryable<T> query)
        {
            var str = query.ToString();
            var provider = query.Provider;
            var fields = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields;
            var properties = typeof(EntityQueryProvider).GetTypeInfo().DeclaredProperties;
            try
            {
                var queryCompiler = QueryCompilerField.GetValue(query.Provider);
                var nodeTypeProvider = (INodeTypeProvider)NodeTypeProviderField.GetValue(queryCompiler);
                var parser = (IQueryParser)CreateQueryParserMethod.Invoke(queryCompiler, new object[] { nodeTypeProvider });
                var queryModel = parser.GetParsedQuery(query.Expression);
                var database = DataBaseField.GetValue(queryCompiler);
                var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesProperty.GetValue(database);
                var queryCompilationContextFactory = databaseDependencies.QueryCompilationContextFactory;
                var queryCompilationContext = queryCompilationContextFactory.Create(false);
                var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
                modelVisitor.CreateQueryExecutor<T>(queryModel);
                var sql = modelVisitor.Queries.First().ToString();

                return sql;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }
    }
}
