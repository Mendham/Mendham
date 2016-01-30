using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.Connection.Mapping
{
    internal class IntSetMapping : ItemWithSingleFieldMapping<int>
    {
        private const string INT_TYPE = "INT";

        public IntSetMapping(string tableName, string columnName)
            : base(tableName, columnName)
        { }

        protected override string TSqlType
        {
            get { return INT_TYPE; }
        }
    }
}
