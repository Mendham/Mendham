using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.RelationalDatabase.SqlServer.BuiltInMapping
{
    public class NVarchar100Mapping : TSqlSingleFieldMapping<string>
    {
        private const int MAX_STRING_LENGTH = 100;
        private readonly string STRING_TYPE = $"NVARCHAR({MAX_STRING_LENGTH})";

        public NVarchar100Mapping(string tableName, string columnName) : base(tableName, columnName)
        { }

        protected override string TSqlType
        {
            get { return STRING_TYPE; }
        }

        public override bool ItemIsValidPredicate(string item)
        {
            return base.ItemIsValidPredicate(item) && item.Length <= MAX_STRING_LENGTH;
        }

        public override string InvalidSetErrorMessage
        {
            get
            {
                return $"One or more string in set is either null or exceeds the max length of {MAX_STRING_LENGTH}.";
            }
        }
    }
}