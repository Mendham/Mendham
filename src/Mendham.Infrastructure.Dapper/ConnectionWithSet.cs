using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

#if DOTNET5_4
using IDbConnection = global::System.Data.Common.DbConnection;
using IDbTransaction = global::System.Data.Common.DbTransaction;
#endif

namespace Mendham.Infrastructure.Dapper
{
    public class ConnectionWithSet<T> : IDisposable
    {
        private readonly IDbConnection _conn;
        private readonly IConnectionWithSetMapping<T> _mapping;

        public ConnectionWithSet(Func<IDbConnection> connectionFactory, IConnectionWithSetMapping<T> mapping)
            : this(connectionFactory(), mapping)
        { }

        public ConnectionWithSet(IDbConnection connection, IConnectionWithSetMapping<T> mapping)
        {
            connection.VerifyArgumentNotNull(nameof(connection))
                .VerifyArgumentMeetsCriteria(nameof(connection), a => a.State == ConnectionState.Closed, "Connection must not be closed before wrapping");
            mapping.VerifyArgumentNotDefaultValue(nameof(mapping));

            this._conn = connection;
            this._mapping = mapping;
        }

        public async Task<ConnectionWithSet<T>> OpenAsync(IEnumerable<T> set)
        {
            set.VerifyArgumentMeetsCriteria(nameof(set), a => 
                a.All(_mapping.ItemIsValidPredicate), _mapping.InvalidSetErrorMessage);

            await OpenConnectionAsync();
            await _conn.ExecuteAsync(_mapping.CreateTableSql);

            foreach (var item in set)
                await SqlMapper.ExecuteAsync(_conn, _mapping.InsertItemSql, _mapping.GetParamForInsert(item));

            return this;
        }

        private Task OpenConnectionAsync()
        {
#if DOTNET5_4
            return _conn.OpenAsync();
#else
            DbConnection dbConnection = _conn as DbConnection;

            if (dbConnection != default(DbConnection))
                return dbConnection.OpenAsync();
            else
            {
                // No known async opening support, use non async open
                _conn.Open();
                return Task.FromResult(0);
            }
#endif
        }

        public Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.QueryAsync<TResult>(_conn, sql, param, transaction, commandTimeout, commandType);
        }

        public Task<IEnumerable<dynamic>> QueryAsync(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.QueryAsync<dynamic>(_conn, sql, param, transaction, commandTimeout, commandType);
        }

        public Task<TResult> ExecuteScalarAsync<TResult>(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.ExecuteScalarAsync<TResult>(_conn, sql, param, transaction, commandTimeout, commandType);
        }

        public Task<dynamic> ExecuteScalarAsync(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.ExecuteScalarAsync<dynamic>(_conn, sql, param, transaction, commandTimeout, commandType);
        }

        public void Dispose()
        {
            if (_conn != null)
            {
                if (_conn.State == ConnectionState.Open)
                {
                    var dropSql = string.Format("IF OBJECT_ID('tempdb..{0}') IS NOT NULL DROP TABLE {0}", _mapping.TableName);
                    _conn.Execute(dropSql);
                }
                else if (_conn.State != ConnectionState.Closed)
                {
                    var error = string.Format("Attempted to dispose connection in an invalid open state ({0}).", _conn.State);
                    throw new InvalidOperationException(error);
                }
            }
        }
    }
}