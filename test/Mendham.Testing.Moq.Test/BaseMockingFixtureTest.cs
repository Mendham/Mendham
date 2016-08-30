using FluentAssertions;
using Mendham.Testing.Moq.Test.TestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Moq.Test
{
    public class BaseMockingFixtureTest
    {
        [Fact]
        public void ResetFixture_FirstRun_CreatesMoqs()
        {
            var sut = new NonCreationMockingFixture();
            var sutFixture = sut as IFixture;

            sutFixture.ResetFixture();

            sut.Dependency1.Should()
                .NotBeNull();
            sut.Dependency2.Should()
                .NotBeNull();
        }

        [Fact]
        public void ResetFixture_SecondRun_NewObject()
        {
            var sut = new NonCreationMockingFixture();
            var sutFixture = sut as IFixture;

            sutFixture.ResetFixture();

            var originalObject = sut.Dependency1;

            sutFixture.ResetFixture();

            sut.Dependency1.Should()
                .NotBeNull()
                .And.NotBeSameAs(originalObject);
        }

        [Fact]
        public void ResetFixture_IgnoreDependency_IsNull()
        {
            var sut = new NonCreationMockingFixture();
            var sutFixture = sut as IFixture;

            sutFixture.ResetFixture();

            sut.IgnoredDependency.Should()
                .BeNull();
        }

        [Fact]
        public void ResetFixture_DependencyPrivateSetter_IsNull()
        {
            var sut = new NonCreationMockingFixture();
            var sutFixture = sut as IFixture;

            sutFixture.ResetFixture();

            sut.IgnoredDependency.Should()
                .BeNull();
        }
    }
}
