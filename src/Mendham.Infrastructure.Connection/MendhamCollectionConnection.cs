using Dapper;
using Mendham.Infrastructure.Connection.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

#if DOTNET5_4
using IDbConnection = global::System.Data.Common.DbConnection;
using IDbTransaction = global::System.Data.Common.DbTransaction;
using IDbCommand = global::System.Data.Common.DbCommand;
#endif

namespace Mendham.Infrastructure.Connection
{
    public class MendhamCollectionConnection : IDbConnection, IDisposable
    {
        private readonly IDbConnection _conn;

        private const string DEFAULT_TABLE_NAME = "#Items";
        private const string DEFAULT_COLUMN_NAME = "Value";

        private string _setTableName;

        public MendhamCollectionConnection(Func<IDbConnection> connectionFactory)
            : this(connectionFactory())
        { }

        public MendhamCollectionConnection(IDbConnection connection)
        {
            connection.VerifyArgumentNotNull(nameof(connection))
                .VerifyArgumentMeetsCriteria(a => a.State == ConnectionState.Closed, nameof(connection),
                    "Connection must not be closed before wrapping");

            this._conn = connection;
        }

        public async Task<MendhamCollectionConnection> OpenAsync<T>(IEnumerable<T> set, IMendhamCollectionConnectionMapping<T> mapping)
        {
            mapping.VerifyArgumentNotDefaultValue(nameof(mapping));
            set.VerifyArgumentMeetsCriteria(a => a.All(mapping.ItemIsValidPredicate), nameof(set), 
                mapping.InvalidSetErrorMessage);

            // Validate connection is in a valid state to be opened
            if (_conn.State != ConnectionState.Closed)
            {
                throw new AttemptedToOpenNonClosedConnectionWithSetException(_conn.State);
            }

            _setTableName = mapping.TableName;

            await OpenConnectionAsync();
            await _conn.ExecuteAsync(mapping.CreateTableSql);

            foreach (var item in set)
                await SqlMapper.ExecuteAsync(_conn, mapping.InsertItemSql, mapping.GetParamForInsert(item));

            return this;
        }

        public Task<MendhamCollectionConnection> OpenAsync(IEnumerable<int> set, string intTableName = DEFAULT_TABLE_NAME, 
            string intColName = DEFAULT_COLUMN_NAME)
        {
            return OpenAsync(set, new IntSetMapping(intTableName, intColName));
        }

        public Task<MendhamCollectionConnection> OpenAsync(IEnumerable<Guid> set, string guidTableName = DEFAULT_TABLE_NAME,
            string guidColName = DEFAULT_COLUMN_NAME)
        {
            return OpenAsync(set, new GuidSetMapping(guidTableName, guidColName));
        }

        public Task<MendhamCollectionConnection> OpenAsync(IEnumerable<string> set, string stringTableName = DEFAULT_TABLE_NAME,
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

#if DOTNET5_4
        protected override void Dispose(bool disposing)
        {
            DisposeConnection(disposing);
        }
#else
        public void Dispose()
        {
            DisposeConnection();
        }
#endif
        private void DisposeConnection(bool disposing = false)
        {
            if (_conn != null && !disposing)
            {
                var dropped = DropSetTable();

                if (!dropped && _conn.State != ConnectionState.Closed)
                {
                    var error = $"Attempted to dispose connection in an invalid open state ({_conn.State}).";
                    throw new InvalidOperationException(error);
                }
            }
        }

        private bool DropSetTable()
        {
            if (_conn.State == ConnectionState.Open)
            {
                if (_setTableName != null)
                {
                    var dropSql = $"IF OBJECT_ID('tempdb..{_setTableName}') IS NOT NULL DROP TABLE {_setTableName}";
                    _conn.Execute(dropSql);
                    _setTableName = null;
                }

                return true;
            }

            return false;
        }

        #region IDBConnection Connection Wrapper

#if DOTNET5_4
        public override string ConnectionString
        {
            get
            {
                return _conn.ConnectionString;
            }

            set
            {
                _conn.ConnectionString = value;
            }
        }

        public override string Database
        {
            get
            {
                return _conn.Database;
            }
        }

        public override string DataSource
        {
            get
            {
                return _conn.DataSource;
            }
        }

        public override string ServerVersion
        {
            get
            {
                return _conn.ServerVersion;
            }
        }

        public override ConnectionState State
        {
            get
            {
                return _conn.State;
            }
        }

        protected override IDbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return _conn.BeginTransaction(isolationLevel);
        }

        public override void ChangeDatabase(string databaseName)
        {
            DropSetTable();
            _conn.ChangeDatabase(databaseName);
        }

        public override void Close()
        {
            DropSetTable();
            _conn.Close();
        }

        protected override IDbCommand CreateDbCommand()
        {
            return _conn.CreateCommand();
        }

        public override void Open()
        {
            _conn.Open();
        }

#else
        string IDbConnection.ConnectionString
        {
            get
            {
                return _conn.ConnectionString;
            }

            set
            {
                _conn.ConnectionString = value;
            }
        }

        public int ConnectionTimeout
        {
            get
            {
                return _conn.ConnectionTimeout;
            }
        }

        public string Database
        {
            get
            {
                return _conn.Database;
            }
        }

        public ConnectionState State
        {
            get
            {
                return _conn.State;
            }
        }

        IDbTransaction IDbConnection.BeginTransaction()
        {
            return _conn.BeginTransaction();
        }

        IDbTransaction IDbConnection.BeginTransaction(IsolationLevel il)
        {
            return _conn.BeginTransaction(il);
        }

        void IDbConnection.Close()
        {
            DropSetTable();
            _conn.Close();
        }

        void IDbConnection.ChangeDatabase(string databaseName)
        {
            DropSetTable();
            _conn.ChangeDatabase(databaseName);
        }

        IDbCommand IDbConnection.CreateCommand()
        {
            return _conn.CreateCommand();
        }

        void IDbConnection.Open()
        {
            try
            {
                _conn.Open();
            }
            catch (Exception ex)
            {
                throw new FailureToOpenConnectionWithSetException(ex);
            }
        }
#endif 
        #endregion
    }
}