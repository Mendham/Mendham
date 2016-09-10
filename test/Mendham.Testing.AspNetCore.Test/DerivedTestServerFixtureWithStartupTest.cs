using FluentAssertions;
using Mendham.Infrastructure.Http;
using Mendham.Testing.AspNetCore.Test.SampleApp;
using Mendham.Testing.Moq;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.AspNetCore.Test
{
    public class DerivedTestServerFixtureWithStartupTest : UnitTest<DerivedTestServerFixtureStartupFixture>
    {
        public DerivedTestServerFixtureWithStartupTest(DerivedTestServerFixtureStartupFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Client_GetWithServiceMocked_MockedValueReturns()
        {
            int expectedResult = 117;

            Fixture.TestService.AsMock()
                .Setup(a => a.GetValue())
                .ReturnsAsync(expectedResult);

            var result = await Fixture.Client.GetAsync("");

            result.Should()
                .HaveStatusCode(HttpStatusCode.OK)
                .And.Content.BeString(expectedResult.ToString());
        }

        [Fact]
        public async Task Client_PostWithServiceMocked_OKAndServiceCalled()
        {
            string value = "test string";

            Fixture.TestService.AsMock()
                .Setup(a => a.TakeAction(value))
                .ReturnsAsync(true)
                .Verifiable("did not call action with value");

            var result = await Fixture.Client.PostAsync("", JsonContent.FromObject(value));

            result.Should()
                .HaveStatusCode(HttpStatusCode.NoContent);
            Fixture.TestService.AsMock()
                .Verify();
        }
    }

    public class DerivedTestServerFixtureStartupFixture : TestServerFixture<Startup>
    {
        public ITestService TestService { get; set; }

        protected override void ServiceConfiguration(IServiceCollection services)
        {
            services.AddTransient(a => TestService);
        }

        public override void ResetFixture()
        {
            this.ResetMockProperties();
        }
    }
}
