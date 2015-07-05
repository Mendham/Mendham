namespace Mendham.Testing
{
	public abstract class BaseFixture<T> : IFixture<T>
	{
		public BaseFixture()
		{ }

		public abstract T CreateSut();

		public virtual void ResetFixture() { }

	}
}
