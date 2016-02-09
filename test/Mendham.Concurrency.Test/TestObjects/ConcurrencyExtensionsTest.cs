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
    }
}
