using System.Text.RegularExpressions;

namespace Mendham.Infrastructure.RelationalDatabase.SqlServer.BuiltInMapping
{
    public abstract class TSqlSingleFieldMapping<T> : SqlServerItemLoaderMapping<T>
    {
        private readonly string _tableName;
        private readonly string _columnName;

        private static readonly Regex TableNameRegex = new Regex("^(#|@)[^#@].*", RegexOptions.Compiled);

        internal TSqlSingleFieldMapping(string tableName, string columnName)
        {
            tableName.VerifyArgumentNotNullOrWhiteSpace(nameof(tableName), "Table name is required")
                .VerifyArgumentMeetsCriteria(a => TableNameRegex.IsMatch(a),
                nameof(tableName),
                "Table name must be a valid temporary table name");
            columnName.VerifyArgumentNotNullOrWhiteSpace(nameof(columnName), "Column name is required");

            this._tableName = tableName;
            this._columnName = columnName;
        }

        protected abstract string TSqlType { get; }

        public sealed override string CreateTableSql
        {
            get
            {
                return $"CREATE TABLE {_tableName} ({_columnName} {TSqlType})";
            }
        }

        public sealed override string TableName
        {
            get
            {
                return _tableName;
            }
        }

        public sealed override string InsertItemSql
        {
            get
            {
                return $"INSERT INTO {_tableName} VALUES (@value)";
            }
        }

        public override dynamic GetParamForInsert(T item)
        {
            return new { value = item };
        }
    }
}