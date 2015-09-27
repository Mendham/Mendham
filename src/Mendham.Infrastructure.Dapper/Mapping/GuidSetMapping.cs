﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.Dapper.Mapping
{
    internal class GuidSetMapping : ItemWithSingleFieldMapping<Guid>
    {
        private const string GUID_TYPE = "UNIQUEIDENTIFIER";

        public GuidSetMapping(string tableName, string columnName)
            : base(tableName, columnName)
        { }

        protected override string TSqlType
        {
            get { return GUID_TYPE; }
        }
    }
}