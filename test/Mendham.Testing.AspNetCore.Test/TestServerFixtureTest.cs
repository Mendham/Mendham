using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.AspNetCore.Test
{
    public class TestServerFixtureTest : UnitTest<TestServerFixtureTestFixture>
    {
        public TestServerFixtureTest(TestServerFixtureTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Client_CallTestPath_Status200()
        {
            var response = await Fixture.Client.GetAsync("test");

            response.Should()
                .HaveStatusCode(HttpStatusCode.OK, "that is the value on the test path");
        }

        [Fact]
        public async Task Client_CallBase_Status404()
        {
            var response = await Fixture.Client.GetAsync("");

            response.Should()
                .HaveStatusCode(HttpStatusCode.NotFound, "the path is not mapped to anything");
        }
    }

    public class TestServerFixtureTestFixture : TestServerFixture
    {
        protected override IWebHostBuilder GetWebHostBuilder()
        {
            return new WebHostBuilder()
                .Configure(app =>
                {
                    app.Map("/test",
                        testApp => testApp.Run(ctx =>
                        {
                            ctx.Response.StatusCode = 200;
                            return Task.FromResult(0);
                        }));
                });
        }
    }
}
