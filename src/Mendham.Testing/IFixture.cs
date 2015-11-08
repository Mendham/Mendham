namespace Mendham.Testing
{
    /// <summary>
    /// A fixture for unit testing 
    /// </summary>
	public interface IFixture
	{
        /// <summary>
        /// Prepares fixture for new test to be run
        /// </summary>
		void ResetFixture();
	}

    /// <summary>
    /// A fixture for unit testing a specific type
    /// </summary>
    /// <typeparam name="T">Type of System Under Test</typeparam>
	public interface IFixture<T> : IFixture
	{
        /// <summary>
        /// Create the system under test
        /// </summary>
		T CreateSut();
	}
}
