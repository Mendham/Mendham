using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.RelationalDatabase
{
    public static class ConnectionFactoryExtensions
    {
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
    }
}
