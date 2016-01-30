using Mendham.Infrastructure.Connection.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.Connection.Test.Helpers
{
    public class CompositeId : IEquatable<CompositeId>
    {
        public Guid GuidVal { get; set; }
        public int IntVal { get; set; }

        public bool Equals(CompositeId other)
        {
            return other != null
                && this.GuidVal == other.GuidVal
                && this.IntVal == other.IntVal;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as CompositeId);
        }

        public override int GetHashCode()
        {
            return this.GuidVal.GetHashCode()
                + this.IntVal.GetHashCode();
        }
    }

    public class CompositeIdMapping : SetMapping<CompositeId>
    {
        public override string CreateTableSql
        {
            get
            {
                return @"CREATE TABLE #TestCompositeIdSet (
                        GuidVal UNIQUEIDENTIFIER, 
                        IntVal INT
                    )";
            }
        }

        public override string InsertItemSql
        {
            get
            {
                return @"INSERT INTO #TestCompositeIdSet (GuidVal, IntVal)
                        VALUES (@guidVal, @intVal)
                    ";
            }
        }

        public override string TableName
        {
            get
            {
                return "#TestCompositeIdSet";
            }
        }

        public override dynamic GetParamForInsert(CompositeId item)
        {
            return new { guidVal = item.GuidVal, intVal = item.IntVal };
        }
    }
}
