using FluentAssertions;
using Mendham.Testing.Builder.Test.Fixtures;
using Mendham.Testing.Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Builder.Test
{
    public class BuilderQueryServiceTest : BaseUnitTest<BuilderQueryServiceFixture>
    {
        public BuilderQueryServiceTest(BuilderQueryServiceFixture fixture) : base(fixture)
        { }

        [Fact]
        public void GetBuilderTypes_ThisAssembly_NonEmpty()
        {
            var thisAssembly = this.GetType().Assembly;

            Fixture.BuilderAssemblyQueryService.AsMock()
                .Setup(a => a.GetAssembliesWithBuilders(thisAssembly))
                .ReturnItems(thisAssembly, typeof(IBuilder<>).Assembly);

            var sut = Fixture.CreateSut();

            var result = sut.GetBuilderTypes(thisAssembly);

            result.Should()
                .NotBeEmpty("There are types defined in TestObjects");
        }
    }
}
