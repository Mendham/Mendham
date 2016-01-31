using FluentAssertions;
using Mendham.Testing.Test.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Test
{
    public class OrderAgnosticComparerTest
    {
        [Theory, MendhamData]
        public void Equals_SameList_True([WithCount(20)]List<int> firstSet)
        {
            var secondSet = firstSet;

            var sut = OrderAgnosticComparer<int>.Default;

            var result = sut.Equals(firstSet, secondSet);

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void Equals_TwoIdenetialIntSets_True([WithCount(20)]List<int> firstSet)
        {
            var secondSet = firstSet.ToList();

            var sut = OrderAgnosticComparer<int>.Default;

            var result = sut.Equals(firstSet, secondSet);

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void Equals_SameValuesDifferentOrder_True([WithCount(20)]List<int> firstSet)
        {
            var secondSet = firstSet
                .OrderBy(a => Guid.NewGuid())
                .ToList();

            var sut = OrderAgnosticComparer<int>.Default;

            var result = sut.Equals(firstSet, secondSet);

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void Equals_SameLengthDifferentValues_False([WithCount(20)]List<int> firstSet, 
            [WithCount(20)]List<int> secondSet)
        {
            var sut = OrderAgnosticComparer<int>.Default;

            var result = sut.Equals(firstSet, secondSet);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void Equals_DifferentLengthDifferentValues_False([WithCount(20)]List<int> firstSet,
            [WithCount(19)]List<int> secondSet)
        {
            var sut = OrderAgnosticComparer<int>.Default;

            var result = sut.Equals(firstSet, secondSet);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void Equals_OneSetSubsetOfOther_False([WithCount(20)]List<int> firstSet)
        {
            var secondSet = firstSet
                .Take(15)
                .ToList();

            var sut = OrderAgnosticComparer<int>.Default;

            var result = sut.Equals(firstSet, secondSet);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void Equals_EquatableObjectSameValuesDifferentOrder_True(
            [WithCount(20)]List<BasicEquatableObject> firstSet)
        {
            var secondSet = firstSet
                .OrderBy(a => Guid.NewGuid())
                .ToList();

            var sut = OrderAgnosticComparer<BasicEquatableObject>.Default;

            var result = sut.Equals(firstSet, secondSet);

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void Equals_EquatableObjectSameLengthDifferentValues_False(
            [WithCount(20)]List<BasicEquatableObject> firstSet,
            [WithCount(20)]List<BasicEquatableObject> secondSet)
        {
            var sut = OrderAgnosticComparer<BasicEquatableObject>.Default;

            var result = sut.Equals(firstSet, secondSet);

            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_DifferentValuesSameSumAndLength_False()
        {
            var firstSet = new int[] { 1, 2, 3, 6 };
            var secondSet = new int[] { 1, 2, 4, 5 };

            var sut = OrderAgnosticComparer<int>.Default;

            var result = sut.Equals(firstSet, secondSet);

            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_DifferentValuesAndLengthSameSum_False()
        {
            var firstSet = new int[] { 1, 2, 3, 6 };
            var secondSet = new int[] { 5, 7 };

            var sut = OrderAgnosticComparer<int>.Default;

            var result = sut.Equals(firstSet, secondSet);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void GetHashCode_TwoIdenetialIntSets_Equal([WithCount(20)]List<int> firstSet)
        {
            var secondSet = firstSet.ToList();

            var sut = OrderAgnosticComparer<int>.Default;

            var expected = sut.GetHashCode(firstSet);
            var result = sut.GetHashCode(secondSet);

            result.Should().Be(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_SameValuesDifferentOrder_Equal([WithCount(20)]List<int> firstSet)
        {
            var secondSet = firstSet
                .OrderBy(a => Guid.NewGuid())
                .ToList();

            var sut = OrderAgnosticComparer<int>.Default;

            var expected = sut.GetHashCode(firstSet);
            var result = sut.GetHashCode(secondSet);

            result.Should().Be(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_SameLengthDifferentValues_NotEqual([WithCount(20)]List<int> firstSet,
            [WithCount(20)]List<int> secondSet)
        {
            var sut = OrderAgnosticComparer<int>.Default;

            var expected = sut.GetHashCode(firstSet);
            var result = sut.GetHashCode(secondSet);

            result.Should().NotBe(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_DifferentLengthDifferentValues_NotEqual([WithCount(20)]List<int> firstSet,
            [WithCount(19)]List<int> secondSet)
        {
            var sut = OrderAgnosticComparer<int>.Default;

            var expected = sut.GetHashCode(firstSet);
            var result = sut.GetHashCode(secondSet);

            result.Should().NotBe(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_OneSetSubsetOfOther_NotEqual([WithCount(20)]List<int> firstSet)
        {
            var secondSet = firstSet
                .Take(15)
                .ToList();

            var sut = OrderAgnosticComparer<int>.Default;

            var expected = sut.GetHashCode(firstSet);
            var result = sut.GetHashCode(secondSet);

            result.Should().NotBe(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_EquatableObjectSameValuesDifferentOrder_Equal(
            [WithCount(20)]List<BasicEquatableObject> firstSet)
        {
            var secondSet = firstSet
                .OrderBy(a => Guid.NewGuid())
                .ToList();

            var sut = OrderAgnosticComparer<BasicEquatableObject>.Default;

            var expected = sut.GetHashCode(firstSet);
            var result = sut.GetHashCode(secondSet);

            result.Should().Be(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_EquatableObjectSameLengthDifferentValues_NotEqual(
            [WithCount(20)]List<BasicEquatableObject> firstSet,
            [WithCount(20)]List<BasicEquatableObject> secondSet)
        {
            var sut = OrderAgnosticComparer<BasicEquatableObject>.Default;

            var expected = sut.GetHashCode(firstSet);
            var result = sut.GetHashCode(secondSet);

            result.Should().NotBe(expected);
        }

        [Fact]
        public void GetHashCode_DifferentValuesSameSumAndLength_NotEqual()
        {
            var firstSet = new int[] { 1, 2, 3, 6 };
            var secondSet = new int[] { 1, 2, 4, 5 };

            var sut = OrderAgnosticComparer<int>.Default;

            var expected = sut.GetHashCode(firstSet);
            var result = sut.GetHashCode(secondSet);

            result.Should().NotBe(expected);
        }

        [Fact]
        public void GetHashCode_DifferentValuesAndLengthSameSum_NotEqual()
        {
            var firstSet = new int[] { 1, 2, 3, 6 };
            var secondSet = new int[] { 5, 7 };

            var sut = OrderAgnosticComparer<int>.Default;

            var expected = sut.GetHashCode(firstSet);
            var result = sut.GetHashCode(secondSet);

            result.Should().NotBe(expected);
        }
    }
}
