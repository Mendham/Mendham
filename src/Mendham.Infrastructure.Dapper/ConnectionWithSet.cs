using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

#if DNXCORE50
using IDbTransaction = global::System.Data.Common.DbTransaction;
#endif

namespace Mendham.Infrastructure.Dapper
{
    public class ConnectionWithSet<T> : IDisposable
    {
        private readonly SqlConnection _conn;
        private readonly IConnectionWithSetMapping<T> _mapping;

        public ConnectionWithSet(Func<SqlConnection> connectionFactory, IConnectionWithSetMapping<T> mapping)
            : this(connectionFactory(), mapping)
        { }

        public ConnectionWithSet(SqlConnection connection, IConnectionWithSetMapping<T> mapping)
        {
            connection.VerifyArgumentNotNull("Connection is required")
                .VerifyArgumentMeetsCriteria(a => a.State == ConnectionState.Closed, "Connection must not be closed before wrapping");
            mapping.VerifyArgumentNotDefaultValue("Mapping is required");

            this._conn = connection;
            this._mapping = mapping;
        }

        public async Task<ConnectionWithSet<T>> OpenAsync(IEnumerable<T> set)
        {
            set.VerifyArgumentMeetsCriteria(a => 
                a.All(_mapping.ItemIsValidPredicate), _mapping.InvalidSetErrorMessage);

            await _conn.OpenAsync();
            await _conn.ExecuteAsync(_mapping.CreateTableSql);

            foreach (var item in set)
                await SqlMapper.ExecuteAsync(_conn, _mapping.InsertItemSql, _mapping.GetParamForInsert(item));

            return this;
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
                    var dropSql = String.Format("IF OBJECT_ID('tempdb..{0}') IS NOT NULL DROP TABLE {0}", _mapping.TableName);
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