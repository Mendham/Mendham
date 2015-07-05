namespace Mendham.Testing
{
	public abstract class BaseFixture<T> : IFixture<T>
	{
		protected Ploeh.AutoFixture.IFixture _dataFixture;

		public BaseFixture()
		{
			this._dataFixture = new Ploeh.AutoFixture.Fixture();
		}

		public abstract T CreateSut();

		public virtual void ResetFixture() { }

	}
}
