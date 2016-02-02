using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.ObjectModel;
using Mendham.Infrastructure.RelationalDatabase.Exceptions;

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
        private readonly ReadOnlyCollection<T> _items;
        private readonly IItemLoaderMapping<T> _mapping;

        private bool _preLoadedTableExists;

        public PreloadedItemConnection(IDbConnection connection, IEnumerable<T> items, IItemLoaderMapping<T> mapping)
        {
            _conn  = connection.VerifyArgumentNotNull(nameof(connection))
                .VerifyArgumentMeetsCriteria(a => a.State == ConnectionState.Closed, nameof(connection),
                    "Connection must not be closed before wrapping");

            _mapping = mapping.VerifyArgumentNotDefaultValue(nameof(mapping));

            items.VerifyArgumentNotNull(nameof(items))
                .VerifyArgumentMeetsCriteria(a => a.All(mapping.ItemIsValidPredicate), 
                    a => AttemptedToLoadInvalidItemException.BuildException(items, mapping));

            _items = new ReadOnlyCollection<T>(items.ToList());

            _preLoadedTableExists = false;
        }

        public IEnumerable<T> Items
        {
            get { return _items; }
        }

#if DOTNET5_4
        public override async Task OpenAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _conn.OpenAsync(cancellationToken);
                await _conn.LoadDataAsync(_items, _mapping);
                _preLoadedTableExists = true;
            }
            catch (Exception ex)
            {
                throw new FailedToOpenPreloadedItemsConnectionException(ex);
            }
        }

        public override void Open()
        {
            try
            {
                _conn.Open();
                _conn.LoadData(_items, _mapping);
                _preLoadedTableExists = true;
            }
            catch (Exception ex)
            {
                throw new FailedToOpenPreloadedItemsConnectionException(ex);
            }
        }

        public override void Close()
        {
            var dropped =_conn.DropData(_mapping);
            EvaluateDataDrop(dropped);
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
            try
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
                _preLoadedTableExists = true;
            }
            catch (Exception ex)
            {
                throw new FailedToOpenPreloadedItemsConnectionException(ex);
            }
        }

        public void Open()
        {
            try
            {
                _conn.Open();
                _conn.LoadData(_items, _mapping);
                _preLoadedTableExists = true;
            }
            catch (Exception ex)
            {
                throw new FailedToOpenPreloadedItemsConnectionException(ex);
            }
        }

        public void Close()
        {
            var dropped =_conn.DropData(_mapping);
            EvaluateDataDrop(dropped);
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
            if (successful)
            {
                _preLoadedTableExists = false;
            }

            if (_preLoadedTableExists)
            {
                // Prevents from rethrowing on an dispose
                _preLoadedTableExists = false;

                throw new FailedToDropPreloadedDataException(_conn.State);
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
            if (_conn.State == ConnectionState.Open)
            {
                DropData();
            }

            _conn.ChangeDatabase(databaseName);
            _conn.LoadData(_items, _mapping);
            _preLoadedTableExists = true;
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
            if (_conn.State == ConnectionState.Open)
            {
                DropData();
            }

            _conn.ChangeDatabase(databaseName);
            _conn.LoadData(_items, _mapping);
            _preLoadedTableExists = true;
        }

        IDbCommand IDbConnection.CreateCommand()
        {
            return _conn.CreateCommand();
        }
#endif 
        #endregion
    }
}