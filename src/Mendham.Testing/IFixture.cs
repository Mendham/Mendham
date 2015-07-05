namespace Mendham.Testing
{
	public interface IFixture
	{
		void ResetFixture();
	}

	public interface IFixture<T> : IFixture
	{
		T CreateSut();
	}
}
