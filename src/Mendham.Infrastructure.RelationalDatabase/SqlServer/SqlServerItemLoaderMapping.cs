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
