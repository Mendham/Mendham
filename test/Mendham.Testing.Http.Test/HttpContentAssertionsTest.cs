using FluentAssertions;
using FluentAssertions.Json;
using Mendham.Infrastructure.Http;
using Mendham.Testing.Http.Test.TestObjects;
using Mendham.Testing.Moq;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Mendham.Testing.Http.Test
{
    public class HttpContentAssertionsTest
    {
        [Theory]
        [MendhamData]
        public void HaveMediaType_MatchesAssertions_DoesNotThrow(string strContent)
        {
            string expectedMediaType = "text/plain";
            HttpContent content = new StringContent(strContent, Encoding.UTF8, expectedMediaType);

            Action act = () => content.Should().HaveMediaType(expectedMediaType);

            act.ShouldNotThrow();
        }

        [Theory]
        [MendhamData]
        public void HaveMediaType_DoesNotAssertions_Throws(string strContent)
        {
            string expectedMediaType = "text/plain";
            HttpContent content = new StringContent(strContent, Encoding.UTF8, "text/html");

            Action act = () => content.Should()
                .HaveMediaType(expectedMediaType, "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage("Expected media type \"text/plain\" because we want to test the failure message, but found \"text/html\".");
        }

        [Fact]
        public void HaveMediaType_NullContent_Throws()
        {
            string expectedMediaType = "text/plain";
            HttpContent content = null;

            Action act = () => content.Should()
                .HaveMediaType(expectedMediaType, "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage("Expected media type \"text/plain\" because we want to test the failure message, but HttpContent was null.");
        }

        [Theory]
        [MendhamData]
        public void HaveJsonMediaType_ContentIsJson_DoesNotThrow(BasicObject obj)
        {
            HttpContent content = JsonContent.FromObject(obj);

            Action act = () => content.Should().HaveJsonMediaType();

            act.ShouldNotThrow();
        }

        [Theory]
        [MendhamData]
        public void HaveJsonMediaType_ContentIsNotJson_Throws(string strContent)
        {
            HttpContent content = new StringContent(strContent, Encoding.UTF8, "text/html");

            Action act = () => content.Should()
                .HaveJsonMediaType("we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage("Expected media type \"application/json\" because we want to test the failure message, but found \"text/html\".");
        }

        [Theory]
        [MendhamData]
        public void BeString_StringEqual_DoesNotThrow(string strContent)
        {
            HttpContent content = new StringContent(strContent);

            Action act = () => content.Should().BeString(strContent);

            act.ShouldNotThrow();
        }

        [Theory]
        [MendhamData]
        public void BeString_StringNotEqual_Throws(string strContent, string incorrectExpectation)
        {
            HttpContent content = new StringContent(strContent);

            Action act = () => content.Should()
                .BeString(incorrectExpectation, "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage($"Expected string \"{incorrectExpectation}\" because we want to test the failure message, but found \"{strContent}\".");
        }

        [Fact]
        public void BeString_NullContent_Throws()
        {
            HttpContent content = null;

            Action act = () => content.Should()
                .BeString("abc", "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage("Expected string \"abc\" because we want to test the failure message, but HttpContent was null.");
        }

        [Theory]
        [MendhamData]
        public async Task BeString_ContainsByteArray_Throws(byte[] bytes, string incorrectExpectation)
        {
            HttpContent content = new ByteArrayContent(bytes);
            var bytesAsString = await content.ReadAsStringAsync();

            Action act = () => content.Should()
                .BeString(incorrectExpectation, "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage($"Expected string \"{incorrectExpectation}\" because we want to test the failure message, but found \"{bytesAsString}\".");
        }

        [Theory]
        [MendhamData]
        public void HaveStringMatch_StringMatches_DoesNotThrow(string strContent)
        {
            HttpContent content = new StringContent(strContent);

            Action act = () => content.Should().HaveStringMatch(a => a.Equals(strContent));

            act.ShouldNotThrow();
        }

        [Theory]
        [MendhamData]
        public void HaveStringMatch_StringDoesNotMatch_Throws(string strContent, string incorrectExpectation)
        {
            HttpContent content = new StringContent(strContent);

            Action act = () => content.Should()
                .HaveStringMatch(a => a == "abc", "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage($"Expected string to match (a == \"abc\") because we want to test the failure message, but \"{strContent}\" does not.");
        }

        [Fact]
        public void HaveStringMatch_NullContent_Throws()
        {
            HttpContent content = null;

            Action act = () => content.Should()
                .HaveStringMatch(a => a == "abc", "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage("Expected string to match (a == \"abc\") because we want to test the failure message, but HttpContent was null.");
        }

        [Theory]
        [MendhamData]
        public async Task HaveStringMatch_ContainsByteArray_Throws(byte[] bytes, string incorrectExpectation)
        {
            HttpContent content = new ByteArrayContent(bytes);
            var bytesAsString = await content.ReadAsStringAsync();

            Action act = () => content.Should()
                .HaveStringMatch(a => a == "abc", "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage($"Expected string to match (a == \"abc\") because we want to test the failure message, but \"{bytesAsString}\" does not.");
        }

        [Theory]
        [MendhamData]
        public void HaveJsonBe_AllFieldsMatch_DoesNotThrow(BasicObject obj)
        {
            HttpContent content = JsonContent.FromObject(obj);
            JToken token = JToken.FromObject(obj);

            Action act = () => content.Should()
                .HaveJsonBe(token);

            act.ShouldNotThrow();
        }

        [Theory]
        [MendhamData]
        public void HaveJsonBe_AllFieldsDoNotMatch_Throws(BasicObject actualObject, BasicObject expectedObject)
        {
            HttpContent content = JsonContent.FromObject(actualObject);
            JToken expectedToken = JToken.FromObject(expectedObject);

            JTokenFormatter _formatter = new JTokenFormatter();

            var expectedMessage = $"Expected json content to be {_formatter.ToString(expectedToken)} " +
                                  "because we want to test the failure message, " +
                                  $"but found {_formatter.ToString(JToken.FromObject(actualObject))}.";

            Action act = () => content.Should()
                .HaveJsonBe(expectedToken, "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage(expectedMessage);
        }

        [Theory]
        [MendhamData]
        public void HaveJsonEquivalentTo_AllFieldsMatch_DoesNotThrow(BasicObject obj)
        {
            HttpContent content = JsonContent.FromObject(obj);

            Action act = () => content.Should()
                .HaveJsonEquivalentTo(obj);

            act.ShouldNotThrow();
        }

        [Theory]
        [MendhamData]
        public void HaveJsonEquivalentTo_AllFieldsMatch_Throws(BasicObject actualObject, BasicObject expectedObject)
        {
            HttpContent content = JsonContent.FromObject(actualObject);

            var expectedMessage = $"Expected json content to be equivalent to {expectedObject} " +
                                  "because we want to test the failure message, " +
                                  $"but found {actualObject}.";

            Action act = () => content.Should()
                .HaveJsonEquivalentTo(expectedObject, "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage(expectedMessage);
        }

        [Theory]
        [MendhamData]
        public void HaveJsonEquivalentToWithEquality_EqualityTrue_DoesNotThrow(BasicObject actualObject, BasicObject expectedObject)
        {
            HttpContent content = JsonContent.FromObject(actualObject);

            var equalityComparer = Mock.Of<IEqualityComparer<BasicObject>>();
            equalityComparer.AsMock()
                .Setup(a => a.Equals(It.Is<BasicObject>(o => o.Equals(actualObject)), expectedObject))
                .Returns(true)
                .Verifiable();

            Action act = () => content.Should()
                .HaveJsonEquivalentTo(expectedObject, equalityComparer);

            act.ShouldNotThrow();
            equalityComparer.AsMock()
                .Verify();
        }

        [Theory]
        [MendhamData]
        public void HaveJsonEquivalentToWithEquality_EqualityFalse_Throws(BasicObject actualObject, BasicObject expectedObject)
        {
            HttpContent content = JsonContent.FromObject(actualObject);

            var equalityComparer = Mock.Of<IEqualityComparer<BasicObject>>();
            equalityComparer.AsMock()
                .Setup(a => a.Equals(It.Is<BasicObject>(o => o.Equals(actualObject)), expectedObject))
                .Returns(false)
                .Verifiable();

            var expectedMessage = $"Expected json content to be equivalent to {expectedObject} " +
                                  "because we want to test the failure message, " +
                                  $"but found {actualObject}.";

            Action act = () => content.Should()
                .HaveJsonEquivalentTo(expectedObject, equalityComparer, "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage(expectedMessage);
            equalityComparer.AsMock()
                .Verify();
        }

        [Theory]
        [MendhamData]
        public void HaveJsonMatchT_PredicateMatches_DoesNotThrow(BasicObject contentObject)
        {
            string expectedValue1 = contentObject.Value1;

            HttpContent content = JsonContent.FromObject(contentObject);

            Action act = () => content.Should()
                .HaveJsonMatch<BasicObject>(a => a.Value1 == expectedValue1);

            act.ShouldNotThrow();
        }

        [Theory]
        [MendhamData]
        public void HaveJsonMatchT_PredicateDoesNotMatch_Throws(BasicObject contentObject)
        {
            HttpContent content = JsonContent.FromObject(contentObject);

            Action act = () => content.Should()
                .HaveJsonMatch<BasicObject>(a => a.Value1 == "wrong value", "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage("Expected content to match (a.Value1 == \"wrong value\") because we want to test the failure message");
        }

        [Fact]
        public void HaveJsonMatchT_HttpContentNull_Throws()
        {
            HttpContent content = null;

            Action act = () => content.Should()
                .HaveJsonMatch<BasicObject>(a => a.Value1 == "wrong value", "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage("Expected content to match (a.Value1 == \"wrong value\") because we want to test the failure message, but HttpContent was null.");
        }

        [Fact]
        public void HaveJsonMatchT_NotJsonContent_Throws()
        {
            HttpContent content = new StringContent("abc");

            Action act = () => content.Should()
                .HaveJsonMatch<BasicObject>(a => a.Value1 == "wrong value", "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage("Expected content to match (a.Value1 == \"wrong value\") because we want to test the failure message, but media type \"text/plain\" is invalid for json content.");
        }

        [Theory]
        [MendhamData]
        public void HaveJsonMatch_PredicateMatches_DoesNotThrow(BasicObject contentObject)
        {
            string expectedValue1 = contentObject.Value1;

            HttpContent content = JsonContent.FromObject(contentObject);

            Action act = () => content.Should()
                .HaveJsonMatch(a => a["Value1"].ToString() == expectedValue1);

            act.ShouldNotThrow();
        }

        [Theory]
        [MendhamData]
        public void HaveJsonMatch_PredicateDoesNotMatch_Throws(BasicObject contentObject)
        {
            HttpContent content = JsonContent.FromObject(contentObject);

            Action act = () => content.Should()
                .HaveJsonMatch(a => a["Value1"].ToString() == "wrong value", "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage("Expected content to match (a.get_Item(\"Value1\").ToString() == \"wrong value\") because we want to test the failure message");
        }

        [Fact]
        public void HaveJsonMatch_HttpContentNull_Throws()
        {
            HttpContent content = null;

            Action act = () => content.Should()
                .HaveJsonMatch(a => a["Value1"].ToString() == "wrong value", "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage("Expected content to match (a.get_Item(\"Value1\").ToString() == \"wrong value\") because we want to test the failure message, but HttpContent was null.");
        }

        [Fact]
        public void HaveJsonMatch_NotJsonContent_Throws()
        {
            HttpContent content = new StringContent("abc");

            Action act = () => content.Should()
                .HaveJsonMatch(a => a["Value1"].ToString() == "wrong value", "we want to test the failure {0}", "message");

            act.ShouldThrow<XunitException>()
                .WithMessage("Expected content to match (a.get_Item(\"Value1\").ToString() == \"wrong value\") because we want to test the failure message, but media type \"text/plain\" is invalid for json content.");
        }

    }
}
