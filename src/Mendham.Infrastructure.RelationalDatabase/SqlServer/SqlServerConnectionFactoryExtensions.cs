using Mendham.Infrastructure.RelationalDatabase.SqlServer.BuiltInMapping;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var mapping = new GuidMapping(guidTableName, guidColName);

            return connectionFactory.GetPreloadedItemConnection(items, mapping);
        }

        public static Task<PreloadedItemConnection<Guid>> GetOpenPreloadedItemConnectionAsync(
            this IConnectionFactory connectionFactory, IEnumerable<Guid> items,
            string guidTableName = DEFAULT_TABLE_NAME, string guidColName = DEFAULT_COLUMN_NAME)
        {
            var mapping = new GuidMapping(guidTableName, guidColName);

            return connectionFactory.GetOpenPreloadedItemConnectionAsync(items, mapping);
        }

        public static PreloadedItemConnection<Guid> GetOpenPreloadedItemConnection(
            this IConnectionFactory connectionFactory, IEnumerable<Guid> items,
            string guidTableName = DEFAULT_TABLE_NAME, string guidColName = DEFAULT_COLUMN_NAME)
        {
            var mapping = new GuidMapping(guidTableName, guidColName);

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
    }
}
