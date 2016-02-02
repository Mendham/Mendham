using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.RelationalDatabase.SqlServer.BuiltInMapping
{
    public class UniqueIdentifierMapping : TSqlSingleFieldMapping<Guid>
    {
        private const string GUID_TYPE = "UNIQUEIDENTIFIER";

        public UniqueIdentifierMapping(string tableName, string columnName) : base(tableName, columnName)
        { }

        protected override string TSqlType
        {
            get { return GUID_TYPE; }
        }
    }
}