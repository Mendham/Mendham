using Dapper;
using Mendham.Infrastructure.Dapper.Mapping;
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
    public class ConnectionWithSet : IDisposable
    {
        private readonly IDbConnection _conn;
        private IConnectionWithSetMapping _mapping;

        private const string DEFAULT_TABLE_NAME = "#Items";
        private const string DEFAULT_COLUMN_NAME = "Value";

        public ConnectionWithSet(Func<IDbConnection> connectionFactory)
            : this(connectionFactory())
        { }

        public ConnectionWithSet(IDbConnection connection)
        {
            connection.VerifyArgumentNotNull(nameof(connection))
                .VerifyArgumentMeetsCriteria(nameof(connection), 
                    a => a.State == ConnectionState.Closed,  "Connection must not be closed before wrapping");

            this._conn = connection;
        }

        public async Task<ConnectionWithSet> OpenAsync<T>(IEnumerable<T> set, IConnectionWithSetMapping<T> mapping)
        {
            mapping.VerifyArgumentNotDefaultValue(nameof(mapping));
            set.VerifyArgumentMeetsCriteria(nameof(set), a => 
                a.All(mapping.ItemIsValidPredicate), mapping.InvalidSetErrorMessage);

            // Validate connection is in a valid state to be opened
            if (_conn.State != ConnectionState.Closed)
            {
                throw new AttemptedToOpenNonClosedConnectionWithSetException(_conn.State);
            }

            _mapping = mapping;

            await OpenConnectionAsync();
            await _conn.ExecuteAsync(mapping.CreateTableSql);

            foreach (var item in set)
                await SqlMapper.ExecuteAsync(_conn, mapping.InsertItemSql, mapping.GetParamForInsert(item));

            return this;
        }

        public Task<ConnectionWithSet> OpenAsync(IEnumerable<int> set, string intTableName = DEFAULT_TABLE_NAME, 
            string intColName = DEFAULT_COLUMN_NAME)
        {
            return OpenAsync(set, new IntSetMapping(intTableName, intColName));
        }

        public Task<ConnectionWithSet> OpenAsync(IEnumerable<Guid> set, string guidTableName = DEFAULT_TABLE_NAME,
            string guidColName = DEFAULT_COLUMN_NAME)
        {
            return OpenAsync(set, new GuidSetMapping(guidTableName, guidColName));
        }

        public Task<ConnectionWithSet> OpenAsync(IEnumerable<string> set, string stringTableName = DEFAULT_TABLE_NAME,
            string stringColName = DEFAULT_COLUMN_NAME)
        {
            return OpenAsync(set, new StringSetMapping(stringTableName, stringColName));
        }

        private async Task OpenConnectionAsync()
        {
            try
            {
#if DOTNET5_4
                await _conn.OpenAsync();
#else
                DbConnection dbConnection = _conn as DbConnection;

                if (dbConnection != default(DbConnection))
                {
                    await dbConnection.OpenAsync();
                }
                else
                {
                    // No known async opening support, use non async open
                    _conn.Open();
                }
#endif
            }
            catch (Exception ex)
            {
                throw new FailureToOpenConnectionWithSetException(ex);
            }
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