using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing
{
    /// <summary>
    /// Base for a unit test class that implements IClassFixture and provides access of that fixture to the tests
    /// </summary>
    /// <typeparam name="TFixture">Type of test fixture</typeparam>
    public abstract class BaseUnitTest<TFixture> : IClassFixture<TFixture>
		where TFixture : class, IFixture, new()
	{
		protected TFixture TestFixture { get; private set; }

		public BaseUnitTest(TFixture fixture)
		{
			this.TestFixture = fixture;

            // This constructor is called prior to every test which resets the fixture prior to each test
			TestFixture.ResetFixture();
		}
	}
}
