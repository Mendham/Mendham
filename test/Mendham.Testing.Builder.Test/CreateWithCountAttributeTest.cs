using FluentAssertions;
using Mendham.Testing.Builder.Test.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Builder.Test
{
    public class CreateWithCountAttributeTest
    {
        [Theory, MendhamData]
        public void CreateWithCount_ValueTypeCountSetTo10_TenGuids([CreateWithCount(10)]IEnumerable<Guid> values)
        {
            values.Should()
                .HaveCount(10, "that is the size set in the CreateByCountAttribute");
        }

        [Theory, MendhamData]
        public void CreateWithCount_OthersNotUsing_OnlyImpactWhenApplied(IEnumerable<Guid> beforeVals, 
            [CreateWithCount(13)]IEnumerable<Guid> values, IEnumerable<Guid> afterVals)
        {
            var beforeSutCount = beforeVals.Count();
            var afterSutCount = afterVals.Count();

            var sut = values.Count();

            sut.Should()
                .NotBe(beforeSutCount, "CreateWithCount was not applied to the parameter this count came from")
                .And.NotBe(afterSutCount, "CreateWithCount was not applied to the parameter this count came from");
        }

        [Theory, MendhamData]
        public void CreateWithCount_ClassFromBuilder_TenObjects(
            [CreateWithCount(10)]IEnumerable<ConstrainedInputObject> values)
        {
            values.Should()
                .HaveCount(10, "that is the size set in the CreateByCountAttribute");
        }

        [Theory, MendhamData]
        public void CreateWithCount_ObjectContainingCollection_CollectionCountNotImpacted(
            [CreateWithCount(15)]IEnumerable<ObjectContainingCollection> values)
        {
            var sut = values.First();

            sut.Collection.Should()
                .NotBeEmpty("the anonymous builder should have populated it")
                .And.Match(a => a.Count() != 15, "child collections should not be impacted by CreateWithCount");
        }
    }
}
