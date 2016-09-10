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
    public class ServerFixtureWithStartupTest : UnitTest<SampleAppFixture>
    {
        public ServerFixtureWithStartupTest(SampleAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task CallGetAction_WithServiceMocked_MockedValueReturns()
        {
            int expectedResult = 117;

            Fixture.TestService.AsMock()
                .Setup(a => a.GetValue())
                .ReturnsAsync(expectedResult);

            var result = await Fixture.Client.GetAsync("test");

            result.Should()
                .HaveStatusCode(HttpStatusCode.OK)
                .And.Content.BeString(expectedResult.ToString());
        }

        [Fact]
        public async Task CallPostNoAction_Exists_NoContent()
        {
            var result = await Fixture.Client.PostAsync("test/noaction", new StringContent(""));

            result.Should()
                .HaveStatusCode(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task CallPostActionWithJson_WithServiceMocked_NoContentAndServiceCalled()
        {
            string value = "abc";

            Fixture.TestService.AsMock()
                .Setup(a => a.TakeAction(value))
                .ReturnsTask()
                .Verifiable("did not call action with value");

            var result = await Fixture.Client.PostAsync("test", JsonContent.FromObject(value));

            result.Should()
                .HaveStatusCode(HttpStatusCode.NoContent);
            Fixture.TestService.AsMock()
                .Verify();
        }
    }

    public class SampleAppFixture : ServerFixture<Startup>
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
