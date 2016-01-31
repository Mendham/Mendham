using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mendham;
using System.Text.RegularExpressions;

namespace Mendham.Infrastructure.RelationalDatabase.Mapping
{
    internal abstract class ItemWithSingleFieldMapping<T> : MendhamCollectionConnectionMapping<T>
    {
        private readonly string _tableName;
        private readonly string _columnName;

        private static readonly Regex TableNameRegex = new Regex("^(#|@)[^#@].*", RegexOptions.Compiled);

        public ItemWithSingleFieldMapping(string tableName, string columnName)
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
                return string.Format("CREATE TABLE {0} ({1} {2})",
                    _tableName, _columnName, TSqlType);
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
                return string.Format("INSERT INTO {0} VALUES (@value)", _tableName);
            }
        }

        public override dynamic GetParamForInsert(T item)
        {
            return new { value = item };
        }
    }
}