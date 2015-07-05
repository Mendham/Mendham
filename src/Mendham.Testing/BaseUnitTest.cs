using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing
{
    public abstract class BaseUnitTest<TFixture> : IClassFixture<TFixture>
		where TFixture : class, IFixture, new()
	{
		protected TFixture _fixture;

		public BaseUnitTest(TFixture fixture)
		{
			this._fixture = fixture;
			_fixture.ResetFixture();
		}
	}
}
