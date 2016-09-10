using FluentAssertions;
using Mendham.Testing.AspNetCore.Test.SampleApp;
using Mendham.Testing.Moq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.AspNetCore.Test
{
    public class DerivedServiceProviderFixtureWithStartupTest : UnitTest<DerivedServiceProviderFixtureWithStartupFixture>
    {
        public DerivedServiceProviderFixtureWithStartupTest(DerivedServiceProviderFixtureWithStartupFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Services_GetMockedDependency_Dependency()
        {
            var result = Fixture.Services.GetService<IDependency1>();

            result.Should()
                .NotBeNull("IDependecy1 was setup by the fixture")
                .And.Be(Fixture.Dependency1, "this is the mocked value set by the fixture");
        }

        [Fact]
        public async Task Services_TestServiceWithMockedDependency_ValueFromDependenc()
        {
            int expectedValue = 17;
            var service = Fixture.Services.GetRequiredService<ITestService>();

            Fixture.Dependency1.AsMock()
                .Setup(a => a.GetValue())
                .Returns(expectedValue);

            var result = await service.GetDependentValueAsync();

            result.Should()
                .Be(expectedValue, "the mock of IDependency1 returns {0}", expectedValue);
        }
    }

    public class DerivedServiceProviderFixtureWithStartupFixture : TestServerFixture<Startup>
    {
        public IDependency1 Dependency1 { get; set; }

        protected override void ServiceConfiguration(IServiceCollection services)
        {
            services.AddTransient(a => Dependency1);
        }

        public override void ResetFixture()
        {
            this.ResetMockProperties();
        }
    }
}
