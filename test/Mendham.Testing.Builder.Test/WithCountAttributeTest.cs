using FluentAssertions;
using Mendham.Testing.Builder.Test.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Builder.Test
{
    public class WithCountAttributeTest
    {
        [Theory, MendhamData]
        public void WithCount_ValueTypeCountSetTo10_TenGuids([WithCount(10)]IEnumerable<Guid> values)
        {
            values.Should()
                .HaveCount(10, "that is the size set in the CreateByCountAttribute");
        }

        [Theory, MendhamData]
        public void WithCount_OthersNotUsing_OnlyImpactWhenApplied(IEnumerable<Guid> beforeVals,
            [WithCount(13)]IEnumerable<Guid> values, IEnumerable<Guid> afterVals)
        {
            var beforeSutCount = beforeVals.Count();
            var afterSutCount = afterVals.Count();

            var sut = values.Count();

            sut.Should()
                .NotBe(beforeSutCount, "WithCount was not applied to the parameter this count came from")
                .And.NotBe(afterSutCount, "WithCount was not applied to the parameter this count came from");
        }

        [Theory, MendhamData]
        public void WithCount_ClassFromBuilder_TenObjects(
            [WithCount(10)]IEnumerable<ConstrainedInputObject> values)
        {
            values.Should()
                .HaveCount(10, "that is the size set in the CreateByCountAttribute");
        }

        [Theory, MendhamData]
        public void WithCount_ClassThatDoesNotImplementIBuilder_CollectionCountNotImpacted(
            [WithCount(15)]IEnumerable<ClassThatDoesNotImplementIBuilder> values)
        {
            var sut = values.First();

            sut.Collection.Should()
                .NotBeEmpty("the anonymous builder should have populated it")
                .And.Match(a => a.Count() != 15, "child collections should not be impacted by WithCount");
        }
    }
}
