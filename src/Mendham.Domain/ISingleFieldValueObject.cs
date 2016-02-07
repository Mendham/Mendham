namespace Mendham.Domain
{
    public interface ISingleFieldValueObject<T>
    {
        T Value { get; }
    }
}