using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

#if NETSTANDARD1_3
using IDbConnection = System.Data.Common.DbConnection;
#endif

namespace Mendham.Infrastructure.RelationalDatabase
{
    public static class ConnectionFactoryExtensions
    {
        public async static Task<IDbConnection> GetOpenConnectionAsync(this IConnectionFactory connectionFactory)
        {
            var conn = connectionFactory.GetConnection();

            var dbConnection = conn as DbConnection;

            if (dbConnection == default(DbConnection))
            {
                conn.Open();
            }
            else
            {
                await dbConnection.OpenAsync();
            }

            return conn;
        }

        public static IDbConnection GetOpenConnection(this IConnectionFactory connectionFactory)
        {
            var conn = connectionFactory.GetConnection();

            conn.Open();

            return conn;
        }

        public static PreloadedItemConnection<T> GetPreloadedItemConnection<T>(
            this IConnectionFactory connectionFactory, IEnumerable<T> items, IItemLoaderMapping<T> mapping)
        {
            connectionFactory.VerifyArgumentNotDefaultValue(nameof(connectionFactory));

            return new PreloadedItemConnection<T>(connectionFactory.GetConnection(), items, mapping);
        }

        public async static Task<PreloadedItemConnection<T>> GetOpenPreloadedItemConnectionAsync<T>(
            this IConnectionFactory connectionFactory, IEnumerable<T> items, IItemLoaderMapping<T> mapping)
        {
            var conn = connectionFactory.GetPreloadedItemConnection(items, mapping);

            await conn.OpenAsync();

            return conn;
        }

        public static PreloadedItemConnection<T> GetOpenPreloadedItemConnection<T>(
            this IConnectionFactory connectionFactory, IEnumerable<T> items, IItemLoaderMapping<T> mapping)
        {
            var conn = connectionFactory.GetPreloadedItemConnection(items, mapping);

            conn.Open();

            return conn;
        }

        public async static Task<T> ExecuteAsync<T>(this IConnectionFactory connectionFactory, 
            Func<IDbConnection, Task<T>> action)
        {
            using (var conn = await connectionFactory.GetOpenConnectionAsync())
            {
                return await action(conn);
            }
        }

        public static T Execute<T>(this IConnectionFactory connectionFactory, Func<IDbConnection, T> action)
        {
            using (var conn = connectionFactory.GetOpenConnection())
            {
                return action(conn);
            }
        }

        public async static Task<T> ExecuteWithPreloadedItemsAsync<T>(this IConnectionFactory connectionFactory, 
            IEnumerable<T> items, IItemLoaderMapping<T> mapping, Func<IDbConnection, Task<T>> action)
        {
            using (var conn = await connectionFactory.GetOpenPreloadedItemConnectionAsync(items, mapping))
            {
                return await action(conn);
            }
        }

        public static T ExecuteWithPreloadedItems<T>(this IConnectionFactory connectionFactory,
            IEnumerable<T> items, IItemLoaderMapping<T> mapping, Func<IDbConnection, T> action)
        {
            using (var conn = connectionFactory.GetOpenPreloadedItemConnection(items, mapping))
            {
                return action(conn);
            }
        }
    }
}
