﻿using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

#if DOTNET5_4
using IDbConnection = global::System.Data.Common.DbConnection;
using IDbTransaction = global::System.Data.Common.DbTransaction;
using IDbCommand = global::System.Data.Common.DbCommand;
#endif

namespace Mendham.Infrastructure.RelationalDatabase
{
    public static class ItemLoaderExtensions
    {
        public async static Task<IItemLoaderMapping<T>> LoadDataAsync<T>(this IDbConnection connection, 
            IEnumerable<T> items, IItemLoaderMapping<T> mapping)
        {
            await connection.ExecuteAsync(mapping.CreateTableSql);

            foreach (var item in items)
            {
                await SqlMapper.ExecuteAsync(connection, mapping.InsertItemSql, mapping.GetParamForInsert(item));
            }

            return mapping;
        }

        public static IItemLoaderMapping<T> LoadData<T>(this IDbConnection connection, IEnumerable<T> items,
            IItemLoaderMapping<T> mapping)
        {
            connection.Execute(mapping.CreateTableSql);

            foreach (var item in items)
            {
                SqlMapper.Execute(connection, mapping.InsertItemSql, mapping.GetParamForInsert(item));
            }

            return mapping;
        }

        public static Task<bool> DropDataAsync<T>(this IDbConnection connection, IItemLoaderMapping<T> mapping)
        {
            if (connection.State != ConnectionState.Open)
            {
                return Task.FromResult(false);
            }

            return connection.ExecuteScalarAsync<bool>(mapping.DropTableSql);
        }

        public static bool DropData<T>(this IDbConnection connection, IItemLoaderMapping<T> mapping)
        {
            if (connection.State != ConnectionState.Open)
            {
                return false;
            }

            return connection.ExecuteScalar<bool>(mapping.DropTableSql);
        }
    }
}
