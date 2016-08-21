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
        [Theory, MendhamData]
        public void BuildMultiple_FactoryFunc_MultipleObjectsWithValue(int commonVal)
        {
            int count = 17;

            Func<IBuilder<BasicObject>> factory = () => new BasicObjectBuilder()
                .WithIntVal(commonVal);

            var result = factory.BuildMultiple(count);

            result.Should().HaveCount(count, "that was the count passed")
                .And.OnlyContain(a => a.IntVal == commonVal, "they were all set with that value")
                .And.Subject.Select(a => a.StringVal).Should()
                    .OnlyHaveUniqueItems("the factory creates a new value each run");
        }

        [Theory, MendhamData]
        public void BuildMultipleRandomCount_FactoryFunc_MultipleObjectsWithValue(int commonVal)
        {
            Func<IBuilder<BasicObject>> factory = () => new BasicObjectBuilder()
                .WithIntVal(commonVal);

            var result = factory.BuildMultiple();

            result.Should().OnlyContain(a => a.IntVal == commonVal, "they were all set with that value")
                .And.Subject.Select(a => a.StringVal).Should()
                    .OnlyHaveUniqueItems("the factory creates a new value each run");

            result.Count().Should()
                .BeInRange(3, 10, "Build Multiple returns between 4 and 9 items");
        }

        [Theory, MendhamData]
        public void BuildMultiple_BuilderGetFactoryMethod_MultipleObjectsWithValue(int commonVal)
        {
            int count = 19;

            var result = BasicObjectFullBuilder
                .GetFactory(b => b.WithIntVal(commonVal))
                .BuildMultiple(count);

            result.Should().HaveCount(count, "that was the count passed")
                .And.OnlyContain(a => a.IntVal == commonVal, "they were all set with that value")
                .And.Subject.Select(a => a.StringVal).Should()
                    .OnlyHaveUniqueItems("the factory creates a new value each run");
        }

        [Theory, MendhamData]
        public void BuildMultipleRandomCount_BuilderGetFactoryMethod_MultipleObjectsWithValue(int commonVal)
        {
            var result = BasicObjectFullBuilder
                .GetFactory(b => b.WithIntVal(commonVal))
                .BuildMultiple();

            result.Should().OnlyContain(a => a.IntVal == commonVal, "they were all set with that value")
                .And.Subject.Select(a => a.StringVal).Should()
                    .OnlyHaveUniqueItems("the factory creates a new value each run");
            result.Count().Should()
                .BeInRange(3, 10, "Build Multiple returns between 4 and 9 items");
        }

        [Fact]
        public void BuildMultiple_BuilderGetDefaultFactoryMethod_MultipleObjects()
        {
            int count = 23;

            var result = BasicObjectFullBuilder
                .GetFactory()
                .BuildMultiple(count);

            result.Should().HaveCount(count, "that was the count passed")
                .And.OnlyHaveUniqueItems("the factory creates different values for each");
        }

        [Fact]
        public void BuildMultipleRandomCount_BuilderGetDefaultFactoryMethod_MultipleObjects()
        {
            var result = BasicObjectFullBuilder
                .GetFactory()
                .BuildMultiple();

            result.Should().OnlyHaveUniqueItems("the factory creates different values for each");
            result.Count().Should()
                .BeInRange(3, 10, "Build Multiple returns between 4 and 9 items");
        }

        [Fact]
        public void BuildMultipleRandomCount_MultipleRuns_AlwaysReturnsValueBetween3and10Items()
        {
            var results = Enumerable.Range(0, 20)
                .Select(a => BasicObjectFullBuilder.GetFactory().BuildMultiple())
                .Select(a => a.Count())
                .ToList();

            results.Should()
                .OnlyContain(a => a >= 3 && a <= 10, "each run of Build Multiple returns between 4 and 9 items");
        }
    }
}
