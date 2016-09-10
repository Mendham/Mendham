using FluentAssertions;
using Mendham.Testing.AspNetCore.Test.SampleApp;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.AspNetCore.Test
{
    public class BaseServiceProviderFixtureWithStartupTest : Test<ServiceProviderFixture<Startup>>
    {
        public BaseServiceProviderFixtureWithStartupTest(ServiceProviderFixture<Startup> fixture) : base(fixture)
        {
        }

        [Fact]
        public void Services_GetDependency_Dependency1()
        {
            var result = Fixture.Services.GetService<IDependency1>();

            result.Should()
                .NotBeNull("the service binding is configured in startup")
                .And.BeOfType<Dependency1>("that is the type registered in Startup");
        }

        [Fact]
        public async Task Services_TestServiceWithMockedDependency_ValueFromDependency()
        {
            int expectedValue = Dependency1.ExpectedValue;
            var service = Fixture.Services.GetRequiredService<ITestService>();

            var result = await service.GetDependentValueAsync();

            result.Should()
                .Be(expectedValue, "Dependency1.GetValue which is called by GetDependentValueAsync returns {0}", expectedValue);
        }
    }
}
