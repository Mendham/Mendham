namespace Mendham.Infrastructure.Connection
{
    public abstract class MendhamCollectionConnectionMapping<T> : IMendhamCollectionConnectionMapping<T>
    {
        public virtual bool ItemIsValidPredicate(T item)
        {
            return !Equals(item, default(T));
        }

        public virtual string InvalidSetErrorMessage
        {
            get
            {
                return "One or more items in set are equal to the default value of the type";
            }
        }

        public abstract string CreateTableSql { get; }
        public abstract string TableName { get; }
        public abstract string InsertItemSql { get; }

        public abstract dynamic GetParamForInsert(T item);
    }
}
