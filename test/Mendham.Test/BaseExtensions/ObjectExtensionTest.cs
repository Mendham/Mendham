using FluentAssertions;
using Mendham;
using Mendham.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Test.BaseExtensions
{
	public class ObjectExtensionTest
	{
		[Theory]
		[MendhamData]
		public void AsSingleItemEnumerable_Int_SingleItemEnumerable(int val)
		{
			var result = val.AsSingleItemEnumerable();

            result.Should()
                .BeAssignableTo<IEnumerable<int>>()
                .And.Contain(val)
                .And.HaveCount(1);
		}

        [Theory, MendhamData]
        public void AsSingleItemList_Int_SingleItemList(int val)
        {
            var result = val.AsSingleItemList();

            result.Should()
                .BeAssignableTo<List<int>>()
                .And.Contain(val)
                .And.HaveCount(1);
        }
    }
}