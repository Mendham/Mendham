using Mendham.Infrastructure.RelationalDatabase.SqlServer.BuiltInMapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

#if DOTNET5_4
using IDbConnection = global::System.Data.Common.DbConnection;
#endif

namespace Mendham.Infrastructure.RelationalDatabase.SqlServer
{
    public static class SqlServerConnectionFactoryExtensions
    {
        private const string DEFAULT_TABLE_NAME = "#Items";
        private const string DEFAULT_COLUMN_NAME = "Value";
        private const int MAX_STRING_LENGTH = 10;

        public static PreloadedItemConnection<int> GetPreloadedItemConnection(
            this IConnectionFactory connectionFactory, IEnumerable<int> items, 
            string intTableName = DEFAULT_TABLE_NAME, string intColName = DEFAULT_COLUMN_NAME)
        {
            var mapping = new IntMapping(intTableName, intColName);

            return connectionFactory.GetPreloadedItemConnection(items, mapping);
        }

        public static Task<PreloadedItemConnection<int>> GetOpenPreloadedItemConnectionAsync(
            this IConnectionFactory connectionFactory, IEnumerable<int> items, 
            string intTableName = DEFAULT_TABLE_NAME, string intColName = DEFAULT_COLUMN_NAME)
        {
            var mapping = new IntMapping(intTableName, intColName);

            return connectionFactory.GetOpenPreloadedItemConnectionAsync(items, mapping);
        }

        public static PreloadedItemConnection<int> GetOpenPreloadedItemConnection(
            this IConnectionFactory connectionFactory, IEnumerable<int> items, 
            string intTableName = DEFAULT_TABLE_NAME, string intColName = DEFAULT_COLUMN_NAME)
        {
            var mapping = new IntMapping(intTableName, intColName);

            return connectionFactory.GetOpenPreloadedItemConnection(items, mapping);
        }

        public static PreloadedItemConnection<Guid> GetPreloadedItemConnection(
            this IConnectionFactory connectionFactory, IEnumerable<Guid> items,
            string guidTableName = DEFAULT_TABLE_NAME, string guidColName = DEFAULT_COLUMN_NAME)
        {
            var mapping = new UniqueIdentifierMapping(guidTableName, guidColName);

            return connectionFactory.GetPreloadedItemConnection(items, mapping);
        }

        public static Task<PreloadedItemConnection<Guid>> GetOpenPreloadedItemConnectionAsync(
            this IConnectionFactory connectionFactory, IEnumerable<Guid> items,
            string guidTableName = DEFAULT_TABLE_NAME, string guidColName = DEFAULT_COLUMN_NAME)
        {
            var mapping = new UniqueIdentifierMapping(guidTableName, guidColName);

            return connectionFactory.GetOpenPreloadedItemConnectionAsync(items, mapping);
        }

        public static PreloadedItemConnection<Guid> GetOpenPreloadedItemConnection(
            this IConnectionFactory connectionFactory, IEnumerable<Guid> items,
            string guidTableName = DEFAULT_TABLE_NAME, string guidColName = DEFAULT_COLUMN_NAME)
        {
            var mapping = new UniqueIdentifierMapping(guidTableName, guidColName);

            return connectionFactory.GetOpenPreloadedItemConnection(items, mapping);
        }

        public static PreloadedItemConnection<string> GetPreloadedItemConnection(
            this IConnectionFactory connectionFactory, IEnumerable<string> items,
            string stringTableName = DEFAULT_TABLE_NAME, string stringColName = DEFAULT_COLUMN_NAME,
            int maxStringLength = MAX_STRING_LENGTH)
        {
            var mapping = new NVarcharMapping(stringTableName, stringColName, maxStringLength);

            return connectionFactory.GetPreloadedItemConnection(items, mapping);
        }

        public static Task<PreloadedItemConnection<string>> GetOpenPreloadedItemConnectionAsync(
            this IConnectionFactory connectionFactory, IEnumerable<string> items,
            string stringTableName = DEFAULT_TABLE_NAME, string stringColName = DEFAULT_COLUMN_NAME,
            int maxStringLength = MAX_STRING_LENGTH)
        {
            var mapping = new NVarcharMapping(stringTableName, stringColName, maxStringLength);

            return connectionFactory.GetOpenPreloadedItemConnectionAsync(items, mapping);
        }

        public static PreloadedItemConnection<string> GetOpenPreloadedItemConnection(
            this IConnectionFactory connectionFactory, IEnumerable<string> items,
            string stringTableName = DEFAULT_TABLE_NAME, string stringColName = DEFAULT_COLUMN_NAME,
            int maxStringLength = MAX_STRING_LENGTH)
        {
            var mapping = new NVarcharMapping(stringTableName, stringColName, maxStringLength);

            return connectionFactory.GetOpenPreloadedItemConnection(items, mapping);
        }

        public async static Task<T> ExecuteWithPreloadedItemsAsync<T>(this IConnectionFactory connectionFactory,
            IEnumerable<int> items, Func<IDbConnection, Task<T>> action, string intTableName = DEFAULT_TABLE_NAME, 
            string intColName = DEFAULT_COLUMN_NAME)
        {
            var connTask = connectionFactory.GetOpenPreloadedItemConnectionAsync(items, intTableName, intColName);

            using (var conn = await connTask)
            {
                return await action(conn);
            }
        }

        public static T ExecuteWithPreloadedItems<T>(this IConnectionFactory connectionFactory, 
            IEnumerable<int> items, Func<IDbConnection, T> action, string intTableName = DEFAULT_TABLE_NAME,
            string intColName = DEFAULT_COLUMN_NAME)
        {
            using (var conn = connectionFactory.GetOpenPreloadedItemConnection(items, intTableName, intColName))
            {
                return action(conn);
            }
        }

        public async static Task<T> ExecuteWithPreloadedItemsAsync<T>(this IConnectionFactory connectionFactory,
            IEnumerable<Guid> items, Func<IDbConnection, Task<T>> action, string guidTableName = DEFAULT_TABLE_NAME,
            string guidColName = DEFAULT_COLUMN_NAME)
        {
            var connTask = connectionFactory.GetOpenPreloadedItemConnectionAsync(items, guidTableName, guidColName);

            using (var conn = await connTask)
            {
                return await action(conn);
            }
        }

        public static T ExecuteWithPreloadedItems<T>(this IConnectionFactory connectionFactory,
            IEnumerable<Guid> items, Func<IDbConnection, T> action, string guidTableName = DEFAULT_TABLE_NAME,
            string guidColName = DEFAULT_COLUMN_NAME)
        {
            using (var conn = connectionFactory.GetOpenPreloadedItemConnection(items, guidTableName, guidColName))
            {
                return action(conn);
            }
        }

        public async static Task<T> ExecuteWithPreloadedItemsAsync<T>(this IConnectionFactory connectionFactory,
            IEnumerable<string> items, Func<IDbConnection, Task<T>> action, string stringTableName = DEFAULT_TABLE_NAME,
            string stringColName = DEFAULT_COLUMN_NAME, int maxStringLength = MAX_STRING_LENGTH)
        {
            var connTask = connectionFactory.GetOpenPreloadedItemConnectionAsync(items, stringTableName, stringColName, maxStringLength);

            using (var conn = await connTask)
            {
                return await action(conn);
            }
        }

        public static T ExecuteWithPreloadedItems<T>(this IConnectionFactory connectionFactory,
            IEnumerable<string> items, Func<IDbConnection, T> action, string stringTableName = DEFAULT_TABLE_NAME,
            string stringColName = DEFAULT_COLUMN_NAME, int maxStringLength = MAX_STRING_LENGTH)
        {
            using (var conn = connectionFactory.GetOpenPreloadedItemConnection(items, stringTableName, stringColName, maxStringLength))
            {
                return action(conn);
            }
        }
    }
}
