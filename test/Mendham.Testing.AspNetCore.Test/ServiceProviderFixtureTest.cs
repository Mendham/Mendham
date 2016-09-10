using FluentAssertions;
using Mendham.Testing.AspNetCore.Test.SampleApp;
using Mendham.Testing.Moq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.AspNetCore.Test
{
    public class ServiceProviderFixtureTest : Test<ServiceProviderFixtureTestFixture>
    {
        public ServiceProviderFixtureTest(ServiceProviderFixtureTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Services_GetMockedDependency_Dependency()
        {
            var result = Fixture.Services.GetService<IDependency1>();

            result.Should()
                .NotBeNull("the service binding is configured")
                .And.Be(Fixture.Dependency1, "that is the value it is set to be");
        }

        [Fact]
        public async Task Services_TestServiceWithMockedDependency_ValueFromDependency()
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

    public class ServiceProviderFixtureTestFixture : ServiceProviderFixture
    {
        public IDependency1 Dependency1 { get; set; }

        protected override void ServiceConfiguration(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ITestService, TestService>();
            serviceCollection.AddTransient(a => Dependency1);
        }

        public override void ResetFixture()
        {
            this.ResetMockProperties();
        }
    }
}

