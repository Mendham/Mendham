using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.RelationalDatabase.SqlServer.BuiltInMapping
{
    public class IntMapping : TSqlSingleFieldMapping<int>
    {
        private const string INT_TYPE = "INT";

        public IntMapping(string tableName, string columnName) : base(tableName, columnName)
        { }

        protected override string TSqlType
        {
            get { return INT_TYPE; }
        }
    }
}
