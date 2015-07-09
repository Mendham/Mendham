using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mendham;
using FluentAssertions;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace Mendham.Test.BaseExtensions
{
	public class ObjectExtensionTest
	{
		[Theory]
		[AutoData]
		public void AsSingleItemEnumerable_Int_SingleItemCollection(int val)
		{
			var result = val.AsSingleItemEnumerable();

			result.Should().BeAssignableTo<IEnumerable<int>>();
			result.Should().Equal(val);
			result.Should().HaveCount(1);
		}
	}
}