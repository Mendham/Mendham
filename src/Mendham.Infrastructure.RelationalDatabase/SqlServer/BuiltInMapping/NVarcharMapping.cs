using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.RelationalDatabase.SqlServer.BuiltInMapping
{
    public class NVarcharMapping : TSqlSingleFieldMapping<string>
    {
        private readonly int _maxLength;
        private readonly string _stringType;

        public NVarcharMapping(string tableName, string columnName, int maxLength) : base(tableName, columnName)
        {
            _maxLength = maxLength
                .VerifyArgumentRange(1, null, nameof(maxLength), "Max length was not within the valid range for an NVARCHAR");

            _stringType = $"NVARCHAR({_maxLength})";
        }

        protected override string TSqlType
        {
            get { return _stringType; }
        }

        public override bool ItemIsValidPredicate(string item)
        {
            return base.ItemIsValidPredicate(item) && item.Length <= _maxLength;
        }

        public override string InvalidSetErrorMessage
        {
            get
            {
                return $"One or more string in set is either null or exceeds the max length of {_maxLength}.";
            }
        }
    }
}