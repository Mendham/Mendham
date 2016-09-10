using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing
{
    /// <summary>
    /// Base for a test class that implements IClassFixture and provides access of that fixture to the tests
    /// </summary>
    /// <typeparam name="TFixture">Type of test fixture</typeparam>
    public abstract class Test<TFixture> : IClassFixture<TFixture>
		where TFixture : class, IFixture, new()
	{
		protected TFixture Fixture { get; private set; }

		public Test(TFixture fixture)
		{
			Fixture = fixture;

            // This constructor is called prior to every test which resets the fixture prior to each test
			Fixture.ResetFixture();
		}
	}
}
