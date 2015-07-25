using Mendham.Infrastructure.Dapper.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.Dapper
{
    public class DefaultConnectionWithSetMapping
    {
        private const string DEFAULT_TABLE_NAME = "#Items";
        private const string DEFAULT_COLUMN_NAME = "Value";

        public IConnectionWithSetMapping<T> Get<T>(string tableName = DEFAULT_TABLE_NAME, string columnName = DEFAULT_COLUMN_NAME)
        {
            if (typeof(T) == typeof(int))
                return new IntSetMapping(tableName, columnName) as IConnectionWithSetMapping<T>;
            if (typeof(T) == typeof(string))
                return new StringSetMapping(tableName, columnName) as IConnectionWithSetMapping<T>;

            throw new InvalidOperationException(string.Format("Mapping for type '{0}' is not defined by default.", typeof(T).FullName));
        }
    }
}