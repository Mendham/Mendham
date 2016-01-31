using Mendham.Infrastructure.RelationalDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.RelationalDatabase.SqlServer
{
    public abstract class SqlServerItemLoaderMapping<T> : ItemLoaderMapping<T>
    {
        public sealed override string DropTableSql
        {
            get
            {
                return $@"
                    IF OBJECT_ID('tempdb..{TableName}') IS NOT NULL 
                    BEGIN 
                        DROP TABLE {TableName};
                        SELECT 1;
                    END
                        SELECT 0;";
            }
        }
    }
}
