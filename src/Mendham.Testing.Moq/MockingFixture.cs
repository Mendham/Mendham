namespace Mendham.Testing.Moq
{
    /// <summary>
    /// A base class for creating a <see cref="IFixture"/> where public read/write Mock properties are reset after each run,
    /// unless they are marked with a <see cref="IgnoreFixtureComponentAttribute"/>. Any non interface properties that do not
    /// have a default constructor will result in an exception unless ignored.
    /// </summary>
    public abstract class MockingFixture : IFixture
    {
        void IFixture.ResetFixture()
        {
            this.ResetMockProperties();
        }
    }

    /// <summary>
    /// A base class for creating a <see cref="IFixture{T}"/> where public read/write Mock properties are reset after each run,
    /// unless they are marked with a <see cref="IgnoreFixtureComponentAttribute"/>. Any non interface properties that do not
    /// have a default constructor will result in an exception unless ignored.
    /// </summary>
    public abstract class MockingFixture<T> : Fixture<T>, IFixture<T>
    {
        public sealed override void ResetFixture()
        {
            this.ResetMockProperties();
        }
    }
}
