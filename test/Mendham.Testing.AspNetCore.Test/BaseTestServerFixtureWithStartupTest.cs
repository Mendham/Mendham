using FluentAssertions;
using Mendham.Infrastructure.Http;
using Mendham.Testing.AspNetCore.Test.SampleApp;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.AspNetCore.Test
{
    public class BaseTestServerFixtureWithStartupTest : UnitTest<TestServerFixture<Startup>>
    {
        public BaseTestServerFixtureWithStartupTest(TestServerFixture<Startup> fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Client_GetValue_ValueSetByService()
        {
            var result = await Fixture.Client.GetAsync("");

            result.Should()
                .HaveStatusCode(HttpStatusCode.OK)
                .And.Content.BeString(TestService.DefaultGetValue.ToString(), "that is the value that comes from the server");
        }

        [Theory]
        [InlineData(TestService.StringForTrueAction, true)]
        [InlineData("badstring", false)]
        public async Task Client_PostConditionalTrueValue_ResultFromService(string valueToPost, bool expected)
        {
            var content = JsonContent.FromObject(valueToPost);

            var result = await Fixture.Client.PostAsync("conditional", content);

            result.Should()
                .HaveStatusCode(HttpStatusCode.OK)
                .And.Content.HaveJsonBe(expected, "that is the value that the server returns based on the input");
        }
    }
}
