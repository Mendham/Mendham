using FluentAssertions;
using Mendham.Equality;
using Mendham.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Test.Equality
{
    public class ComponentWithComparerTest
    {
        [Theory]
        [MendhamData]
        public void IsEqualToComponent_SameComponentWithComparerReference_True(string componentValue)
        {
            var sut = new ComponentWithComparer<string>(componentValue, EqualityComparer<string>.Default);
            var other = sut;

            var result = sut.IsEqualToComponent(other);

            result.Should()
                .BeTrue("they are the same reference");
        }

        [Theory]
        [MendhamData]
        public void IsEqualToComponent_SameComponent_True(string componentValue)
        {
            var altComponentValue = componentValue;

            var sut = new ComponentWithComparer<string>(componentValue, EqualityComparer<string>.Default);
            var other = new ComponentWithComparer<string>(altComponentValue, EqualityComparer<string>.Default);

            var result = sut.IsEqualToComponent(other);

            result.Should()
                .BeTrue("they contain an equal component");
        }

        [Theory]
        [MendhamData]
        public void IsEqualToComponent_DifferentComponent_False(string componentValue, string altComponentValue)
        {
            var sut = new ComponentWithComparer<string>(componentValue, EqualityComparer<string>.Default);
            var other = new ComponentWithComparer<string>(altComponentValue, EqualityComparer<string>.Default);

            var result = sut.IsEqualToComponent(other);

            result.Should()
                .BeFalse("they have different components");
        }

        [Theory]
        [MendhamData]
        public void IsEqualToComponent_EquatableComponent_True(string componentValue)
        {
            var componentValue1 = componentValue.ToUpper();
            var componentValue2 = componentValue.ToLower();

            var sut = new ComponentWithComparer<string>(componentValue1, StringComparer.OrdinalIgnoreCase);
            var other = new ComponentWithComparer<string>(componentValue2, StringComparer.OrdinalIgnoreCase);

            var result = sut.IsEqualToComponent(other);

            result.Should()
                .BeTrue("the components are equatable");
        }

        [Theory]
        [MendhamData]
        public void IsEqualToComponent_DifferentCompareresBothEqual_True(string componentValue1, string componentValue2)
        {
            var comparerMock1 = new Mock<IEqualityComparer<string>>();
            comparerMock1.Setup(a => a.Equals(componentValue1, componentValue2))
                .Returns(true);
            var comparerMock2 = new Mock<IEqualityComparer<string>>();
            comparerMock2.Setup(a => a.Equals(componentValue1, componentValue2))
                .Returns(true);

            var sut = new ComponentWithComparer<string>(componentValue1, comparerMock1.Object);
            var other = new ComponentWithComparer<string>(componentValue2, comparerMock2.Object);

            var result = sut.IsEqualToComponent(other);

            result.Should()
                .BeTrue("both comparers evaluate to true");
        }

        [Theory]
        [MendhamData]
        public void IsEqualToComponent_DifferentCompareresFirstNotEqual_False(string componentValue1, string componentValue2)
        {
            var comparerMock1 = new Mock<IEqualityComparer<string>>();
            comparerMock1.Setup(a => a.Equals(componentValue1, componentValue2))
                .Returns(false);
            var comparerMock2 = new Mock<IEqualityComparer<string>>();
            comparerMock2.Setup(a => a.Equals(componentValue1, componentValue2))
                .Returns(true);

            var sut = new ComponentWithComparer<string>(componentValue1, comparerMock1.Object);
            var other = new ComponentWithComparer<string>(componentValue2, comparerMock2.Object);

            var result = sut.IsEqualToComponent(other);

            result.Should()
                .BeFalse("the first comparer evalutes to false");
        }

        [Theory]
        [MendhamData]
        public void IsEqualToComponent_DifferentCompareresSecondNotEqual_False(string componentValue1, string componentValue2)
        {
            var comparerMock1 = new Mock<IEqualityComparer<string>>();
            comparerMock1.Setup(a => a.Equals(componentValue1, componentValue2))
                .Returns(true);
            var comparerMock2 = new Mock<IEqualityComparer<string>>();
            comparerMock2.Setup(a => a.Equals(componentValue1, componentValue2))
                .Returns(false);

            var sut = new ComponentWithComparer<string>(componentValue1, comparerMock1.Object);
            var other = new ComponentWithComparer<string>(componentValue2, comparerMock2.Object);

            var result = sut.IsEqualToComponent(other);

            result.Should()
                .BeFalse("the second comparer evalutes to false");
        }

        [Theory]
        [MendhamData]
        public void GetComponentHashCode_SameComponentWithComparerReference_Equal(string componentValue)
        {
            var expectedComponent = new ComponentWithComparer<string>(componentValue, EqualityComparer<string>.Default);
            var sut = expectedComponent;

            var expected = expectedComponent.GetComponentHashCode();
            var result = sut.GetComponentHashCode();

            result.Should()
                .Be(expected, "they are the same reference");
        }

        [Theory]
        [MendhamData]
        public void GetComponentHashCode_SameComponent_Equal(string componentValue)
        {
            var altComponentValue = componentValue;

            var expectedComponent = new ComponentWithComparer<string>(componentValue, EqualityComparer<string>.Default);
            var sut = new ComponentWithComparer<string>(altComponentValue, EqualityComparer<string>.Default);

            var expected = expectedComponent.GetComponentHashCode();
            var result = sut.GetComponentHashCode();

            result.Should()
                .Be(expected, "they contain an equal component");
        }

        [Theory]
        [MendhamData]
        public void GetComponentHashCode_DifferentComponent_NotEqual(string componentValue, string altComponentValue)
        {
            var expectedComponent = new ComponentWithComparer<string>(componentValue, EqualityComparer<string>.Default);
            var sut = new ComponentWithComparer<string>(altComponentValue, EqualityComparer<string>.Default);

            var expected = expectedComponent.GetComponentHashCode();
            var result = sut.GetComponentHashCode();

            result.Should()
                .NotBe(expected, "they have different components");
        }

        [Theory]
        [MendhamData]
        public void GetComponentHashCode_EquatableComponent_Equal(string componentValue)
        {
            var componentValue1 = componentValue.ToUpper();
            var componentValue2 = componentValue.ToLower();

            var expectedComponent = new ComponentWithComparer<string>(componentValue1, StringComparer.OrdinalIgnoreCase);
            var sut = new ComponentWithComparer<string>(componentValue2, StringComparer.OrdinalIgnoreCase);

            var expected = expectedComponent.GetComponentHashCode();
            var result = sut.GetComponentHashCode();

            result.Should()
                .Be(expected, "the components are equatable");
        }

        [Theory]
        [MendhamData]
        public void GetComponentHashCode_ResultFromComparer_ExpectedHashCode(string componentValue, int comparerHashResult)
        {
            var comparerMock = new Mock<IEqualityComparer<string>>();
            comparerMock.Setup(a => a.GetHashCode(componentValue))
                .Returns(comparerHashResult);

            var sut = new ComponentWithComparer<string>(componentValue, comparerMock.Object);

            var result = sut.GetComponentHashCode();

            result.Should()
                .Be(comparerHashResult, "that is fixed result from comparer");
        }
    }
}
