using FluentAssertions;
using Mendham.Testing.Builder.Test.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Builder.Test
{
    public class BuilderExtensionTest
    {
        [Theory]
        [MendhamData]
        public void BuildMultiple_FactoryFunc_MultipleObjectsWithValue(int commonVal, int count)
        {
            Func<IBuilder<BasicObject>> factory = () => new BasicObjectBuilder()
                .WithIntVal(commonVal);

            var result = factory.BuildMultiple(count);

            result.Should().HaveCount(count, "that was the count passed")
                .And.OnlyContain(a => a.IntVal == commonVal, "they were all set with that value")
                .And.Subject.Select(a => a.StringVal).Should()
                    .OnlyHaveUniqueItems("the factory creates a new value each run");
        }

        [Theory]
        [MendhamData]
        public void BuildMultiple_BuilderGetFactoryMethod_MultipleObjectsWithValue(int commonVal, int count)
        {
            var result = BasicObjectFullBuilder
                .GetFactory(b => b.WithIntVal(commonVal))
                .BuildMultiple(count);

            result.Should().HaveCount(count, "that was the count passed")
                .And.OnlyContain(a => a.IntVal == commonVal, "they were all set with that value")
                .And.Subject.Select(a => a.StringVal).Should()
                    .OnlyHaveUniqueItems("the factory creates a new value each run");
        }
    }
}
