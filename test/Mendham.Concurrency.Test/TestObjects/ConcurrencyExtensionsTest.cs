using FluentAssertions;
using Mendham.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Concurrency.Test.TestObjects
{
    public class ConcurrencyExtensionsTest
    {
        [Fact]
        public void VerifyConcurrencyTokenIsApplied_WithToken_Object()
        {
            var sut = ObjectWithConcurrencyToken.WithToken();

            var result = sut.VerifyConcurrencyTokenIsApplied();

            result.Should().Be(sut);
        }

        [Theory, MendhamData]
        public void VerifyConcurrencyTokenIsApplied_WithTokenWithMessage_Object(string msg)
        {
            var sut = ObjectWithConcurrencyToken.WithToken();

            var result = sut.VerifyConcurrencyTokenIsApplied(msg);

            result.Should().Be(sut);
        }

        [Fact]
        public void VerifyConcurrencyTokenIsApplied_WithoutToken_ThrowsConcurrencyTokenAlreadyAppliedException()
        {
            var sut = ObjectWithConcurrencyToken.WithoutToken();

            Action act = () => sut.VerifyConcurrencyTokenIsApplied();

            act.ShouldThrow<ConcurrencyTokenAlreadyAppliedException>()
                .Where(a => a.ExistingToken == sut.GetToken());
        }

        [Theory, MendhamData]
        public void VerifyConcurrencyTokenIsApplied_WithoutTokenWithMessage_ThrowsConcurrencyTokenAlreadyAppliedException(string msg)
        {
            var sut = ObjectWithConcurrencyToken.WithoutToken();

            Action act = () => sut.VerifyConcurrencyTokenIsApplied(msg);

            act.ShouldThrow<ConcurrencyTokenAlreadyAppliedException>()
                .Where(a => a.ExistingToken == sut.GetToken())
                .Where(a => a.Message.Contains(msg));
        }

        [Fact]
        public void VerifyConcurrencyTokenIsNotApplied_WithoutToken_Object()
        {
            var sut = ObjectWithConcurrencyToken.WithoutToken();

            var result = sut.VerifyConcurrencyTokenIsNotApplied();

            result.Should().Be(sut);
        }

        [Theory, MendhamData]
        public void VerifyConcurrencyTokenIsNotApplied_WithoutTokenWithMessage_Object(string msg)
        {
            var sut = ObjectWithConcurrencyToken.WithoutToken();

            var result = sut.VerifyConcurrencyTokenIsNotApplied(msg);

            result.Should().Be(sut);
        }

        [Fact]
        public void VerifyConcurrencyTokenIsNotApplied_WithToken_ThrowsConcurrencyTokenNotAppliedException()
        {
            var sut = ObjectWithConcurrencyToken.WithToken();

            Action act = () => sut.VerifyConcurrencyTokenIsNotApplied();

            act.ShouldThrow<ConcurrencyTokenNotAppliedException>();
        }

        [Theory, MendhamData]
        public void VerifyConcurrencyTokenIsNotApplied_WithTokenWithMessage_ThrowsConcurrencyTokenNotAppliedException(string msg)
        {
            var sut = ObjectWithConcurrencyToken.WithToken();

            Action act = () => sut.VerifyConcurrencyTokenIsNotApplied(msg);

            act.ShouldThrow<ConcurrencyTokenNotAppliedException>()
                .Where(a => a.Message.Contains(msg));
        }

        [Theory, MendhamData]
        public void ValidateConcurrencyToken_SameToken_Object(Int64ConcurrencyToken token)
        {
            var sut = ObjectWithConcurrencyToken.WithToken(token);

            var result = sut.ValidateConcurrencyToken(token);

            result.Should().Be(sut);
        }

        [Theory, MendhamData]
        public void ValidateConcurrencyToken_SameTokenWithMessage_Object(Int64ConcurrencyToken token, string msg)
        {
            var sut = ObjectWithConcurrencyToken.WithToken(token);

            var result = sut.ValidateConcurrencyToken(token, msg);

            result.Should().Be(sut);
        }

        [Theory, MendhamData]
        public void ValidateConcurrencyToken_DifferentTokens_ThrowsInvaildConcurrencyTokenException(Int64ConcurrencyToken token1, Int64ConcurrencyToken token2)
        {
            var sut = ObjectWithConcurrencyToken.WithToken(token1);

            Action act = () => sut.ValidateConcurrencyToken(token2);

            act.ShouldThrow<InvaildConcurrencyTokenException>()
                .Where(a => a.Expected == token1)
                .Where(a => a.Actual == token2);
        }

        [Theory, MendhamData]
        public void ValidateConcurrencyToken_DifferentTokensWithMessage_ThrowsInvaildConcurrencyTokenException(Int64ConcurrencyToken token1, Int64ConcurrencyToken token2, string msg)
        {
            var sut = ObjectWithConcurrencyToken.WithToken(token1);

            Action act = () => sut.ValidateConcurrencyToken(token2, msg);

            act.ShouldThrow<InvaildConcurrencyTokenException>()
                .Where(a => a.Expected == token1)
                .Where(a => a.Actual == token2)
                .Where(a => a.Message.Contains(msg));
        }

        [Theory, MendhamData]
        public void SetConcurrencyToken_ObjectWithoutToken_Object(Int64ConcurrencyToken newToken)
        {
            var sut = ObjectWithConcurrencyToken.WithoutToken();

            var result = sut.SetConcurrencyToken(newToken);

            result.Should().Be(sut)
                .And.Match(a => (a as IHasConcurrencyToken).Token == newToken);
        }

        [Theory, MendhamData]
        public void SetConcurrencyToken_ObjectWithToken_Object(Int64ConcurrencyToken newToken)
        {
            var sut = ObjectWithConcurrencyToken.WithToken();

            var result = sut.SetConcurrencyToken(newToken);

            result.Should().Be(sut)
                .And.Match(a => (a as IHasConcurrencyToken).Token == newToken);
        }

        [Fact]
        public void SetConcurrencyToken_SetNullToken_ThrowsArgumentNullException()
        {
            IConcurrencyToken newToken = default(IConcurrencyToken);
            var sut = ObjectWithConcurrencyToken.WithoutToken();

            Action act = () => sut.SetConcurrencyToken(newToken);

            act.ShouldThrow<ArgumentNullException>("cannot set a null token to a IHasConcurrencyToken");
        }
    }
}
