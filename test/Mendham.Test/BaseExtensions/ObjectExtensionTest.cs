using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBeall.Mendham;
using FluentAssertions;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace CBeall.Mendham.Test.BaseExtensions
{
	public class ObjectExtensionTest
	{
		[Theory]
		[AutoData]
		public void SingleIntAsCollection(int val)
		{
			var result = val.AsSingleItemEnumerable();

			result.Should().BeAssignableTo<IEnumerable<int>>();
			result.Should().Equal(val);
			result.Should().HaveCount(1);
		}
	}
}