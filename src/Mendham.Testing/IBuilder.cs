namespace Mendham.Testing
{
    /// <summary>
    /// A builder to create an object needed for testing
    /// </summary>
    /// <typeparam name="T">Type to be built by the builder</typeparam>
	public interface IBuilder<T>
	{
		T Build();
	}
}
