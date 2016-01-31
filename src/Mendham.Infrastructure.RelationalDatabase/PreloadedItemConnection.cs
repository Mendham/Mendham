using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

#if DOTNET5_4
using IDbConnection = global::System.Data.Common.DbConnection;
using IDbTransaction = global::System.Data.Common.DbTransaction;
using IDbCommand = global::System.Data.Common.DbCommand;
#endif

namespace Mendham.Infrastructure.RelationalDatabase
{
    public class PreloadedItemConnection<T> : IDbConnection, IDisposable
    {
        private readonly IDbConnection _conn;
        private readonly IEnumerable<T> _items;
        private readonly IItemLoaderMapping<T> _mapping;

        public PreloadedItemConnection(IDbConnection connection, IEnumerable<T> items, IItemLoaderMapping<T> mapping)
        {
            _conn  = connection.VerifyArgumentNotNull(nameof(connection))
                .VerifyArgumentMeetsCriteria(a => a.State == ConnectionState.Closed, nameof(connection),
                    "Connection must not be closed before wrapping");

            _mapping = mapping.VerifyArgumentNotDefaultValue(nameof(mapping));

            _items = items.VerifyArgumentMeetsCriteria(a => a.All(mapping.ItemIsValidPredicate), nameof(items),
                mapping.InvalidSetErrorMessage);
        }

#if DOTNET5_4
        public override async Task OpenAsync(CancellationToken cancellationToken)
        {
            await _conn.OpenAsync(cancellationToken);
            await _conn.LoadDataAsync(_items, _mapping);
        }

        public override void Open()
        {
            _conn.Open();
            _conn.LoadData(_items, _mapping);
        }

        public override void Close()
        {
            _conn.DropDataAsync(_mapping);
            _conn.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                DropData();
            }

            _conn.Dispose();
        }
#else
        public async Task OpenAsync()
        {
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

            await _conn.LoadDataAsync(_items, _mapping);
        }

        public void Open()
        {
            _conn.Open();
            _conn.LoadData(_items, _mapping);
        }

        public void Close()
        {
            _conn.DropDataAsync(_mapping);
            _conn.Close();
        }

        public void Dispose()
        {
            DropData();
            _conn.Dispose();
        }

#endif

        public async Task CloseAsync()
        {
            var dropTask = _conn.DropDataAsync(_mapping);
            EvaluateDataDrop(await dropTask);
            _conn.Close();
        }

        private void DropData()
        {
            var dropped = _conn.DropData(_mapping);
            EvaluateDataDrop(dropped);
        }

        private void EvaluateDataDrop(bool successful)
        {
            if (!successful && _conn.State != ConnectionState.Closed)
            {
                var error = $"Attempted to dispose connection in an invalid open state ({_conn.State}).";
                throw new InvalidOperationException(error);
            }
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
            DropData();
            _conn.ChangeDatabase(databaseName);
            _conn.LoadData(_items, _mapping);
        }

        protected override IDbCommand CreateDbCommand()
        {
            return _conn.CreateCommand();
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

        void IDbConnection.ChangeDatabase(string databaseName)
        {
            DropData();
            _conn.ChangeDatabase(databaseName);
            _conn.LoadData(_items, _mapping);
        }

        IDbCommand IDbConnection.CreateCommand()
        {
            return _conn.CreateCommand();
        }
#endif 
        #endregion
    }
}