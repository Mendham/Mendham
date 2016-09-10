using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using Xunit;
using Xunit.Sdk;

namespace Mendham.Testing.Http.Test
{
    public class HttpResponseMessageAssestionsTest
    {
        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.Created)]
        [InlineData(HttpStatusCode.Accepted)]
        [InlineData(HttpStatusCode.NoContent)]
        public void HaveSuccessStatusCode_200Statuses_DoesNotThrow(HttpStatusCode statusCode)
        {
            var httpReponseMessage = new HttpResponseMessage(statusCode);

            Action act = () => httpReponseMessage.Should().HaveSuccessStatusCode();

            act.ShouldNotThrow();
        }

        [Theory]
        [InlineData(HttpStatusCode.Redirect)]
        [InlineData(HttpStatusCode.TemporaryRedirect)]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.MethodNotAllowed)]
        [InlineData(HttpStatusCode.Conflict)]
        [InlineData(HttpStatusCode.Gone)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.NotImplemented)]
        public void HaveSuccessStatusCode_NonSuccessStatuses_Throws(HttpStatusCode statusCode)
        {
            var httpReponseMessage = new HttpResponseMessage(statusCode);

            Action act = () => httpReponseMessage.Should()
                .HaveSuccessStatusCode("we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage($"Expected a success status code because we want to test the failure message, but found \"{statusCode.ToString()}\".");
        }

        [Fact]
        public void HaveSuccessStatusCode_NullHttpResponseMessage_Throws()
        {
            HttpResponseMessage httpReponseMessage = null;

            Action act = () => httpReponseMessage.Should()
                .HaveSuccessStatusCode("we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage("Expected a success status code because we want to test the failure message, but the HttpResponseMessage was null.");
        }

        [Fact]
        public void HaveStatusCode_MatchesStatusCode_DoesNotThrow()
        {
            var httpReponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            Action act = () => httpReponseMessage.Should()
                .HaveStatusCode(HttpStatusCode.OK);

            act.ShouldNotThrow();
        }

        [Fact]
        public void HaveSuccessStatusCode_DoesNotMatchStatusCode_Throws()
        {
            var httpReponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            Action act = () => httpReponseMessage.Should()
                .HaveStatusCode(HttpStatusCode.OK, "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage("Expected status code \"OK\" because we want to test the failure message, but found \"BadRequest\".");
        }

        [Fact]
        public void HaveStatusCode_NullHttpResponseMessage_Throws()
        {
            HttpResponseMessage httpReponseMessage = null;

            Action act = () => httpReponseMessage.Should()
                .HaveStatusCode(HttpStatusCode.OK, "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage("Expected status code \"OK\" because we want to test the failure message, but the HttpResponseMessage was null.");
        }
    }
}
