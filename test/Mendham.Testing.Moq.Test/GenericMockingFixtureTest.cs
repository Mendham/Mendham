using FluentAssertions;
using Mendham.Testing.Moq.Test.TestObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Moq.Test
{
    public class GenericMockingFixtureTest
    {
        [Fact]
        public void ResetFixture_FirstRun_CreatesMoqs()
        {
            var sut = new TestServiceMockingFixture();

            sut.ResetFixture();

            sut.Dependency1.Should()
                .NotBeNull();
            sut.Dependency2.Should()
                .NotBeNull();
        }

        [Fact]
        public void ResetFixture_SecondRun_NewObject()
        {
            var sut = new TestServiceMockingFixture();

            sut.ResetFixture();

            var originalObject = sut.Dependency1;

            sut.ResetFixture();

            sut.Dependency1.Should()
                .NotBeNull()
                .And.NotBeSameAs(originalObject);
        }

        [Fact]
        public void ResetFixture_IgnoreDependency_IsNull()
        {
            var sut = new TestServiceMockingFixture();

            sut.ResetFixture();

            sut.IgnoredDependency.Should()
                .BeNull();
        }

        [Fact]
        public void ResetFixture_DependencyPrivateSetter_IsNull()
        {
            var sut = new TestServiceMockingFixture();

            sut.ResetFixture();

            sut.IgnoredDependency.Should()
                .BeNull();
        }

        [Fact]
        public void CreateSut_WiredDependency_HasValue()
        {
            int expectedResult = 17;
            var sut = new TestServiceMockingFixture();
            sut.ResetFixture();

            sut.Dependency1.AsMock()
                .Setup(a => a.GetValue())
                .Returns(expectedResult);

            var result = sut.CreateSut().Dependency1.GetValue();

            result.Should()
                .Be(expectedResult);
        }
    }
}
