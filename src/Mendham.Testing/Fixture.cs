namespace Mendham.Testing
{
    /// <summary>
    /// A base class for creating a fixture for unit testing 
    /// </summary>
	public abstract class Fixture<T> : IFixture<T>
	{
		public Fixture()
		{ }

        /// <summary>
        /// Create the system under test
        /// </summary>
		public abstract T CreateSut();

        /// <summary>
        /// Prepares fixture for new test to be run
        /// </summary>
		public virtual void ResetFixture() { }
	}
}
