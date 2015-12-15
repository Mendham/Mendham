namespace Mendham.Equality
{
    public interface IComponentWithComparer
    {
        int GetComponentHashCode();
        bool IsEqualToComponent(object other);
    }
}