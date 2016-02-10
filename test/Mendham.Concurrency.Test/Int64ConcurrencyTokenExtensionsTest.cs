using FluentAssertions;
using Mendham.Concurrency.Test.TestObjects;
using Mendham.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Concurrency.Test
{
    public class Int64ConcurrencyTokenExtensionsTest
    {
        [Theory, MendhamData]
        public void ValidateInt64ConcurrencyToken_SameToken_Object([WithCount(8)]byte[] tokenBytes)
        {
            var sut = ObjectWithConcurrencyToken.WithTokenBytes(tokenBytes);

            var result = sut.ValidateInt64ConcurrencyToken(tokenBytes);

            result.Should().Be(sut);
        }

        [Theory, MendhamData]
        public void ValidateInt64ConcurrencyToken_SameTokenWithMessage_Object([WithCount(8)]byte[] tokenBytes, string msg)
        {
            var sut = ObjectWithConcurrencyToken.WithTokenBytes(tokenBytes);

            var result = sut.ValidateInt64ConcurrencyToken(tokenBytes, msg);

            result.Should().Be(sut);
        }

        [Theory, MendhamData]
        public void ValidateInt64ConcurrencyToken_DifferentTokens_ThrowsInvaildConcurrencyTokenException([WithCount(8)]byte[] tokenBytes1, [WithCount(8)]byte[] tokenBytes2)
        {
            var sut = ObjectWithConcurrencyToken.WithTokenBytes(tokenBytes1);

            Action act = () => sut.ValidateInt64ConcurrencyToken(tokenBytes2);

            act.ShouldThrow<InvaildConcurrencyTokenException>()
                .Where(a => (a.Expected as Int64ConcurrencyToken).Bytes.SequenceEqual(tokenBytes1))
                .Where(a => (a.Actual as Int64ConcurrencyToken).Bytes.SequenceEqual(tokenBytes2));
        }

        [Theory, MendhamData]
        public void ValidateInt64ConcurrencyToken_DifferentTokensWithMessage_ThrowsInvaildConcurrencyTokenException([WithCount(8)]byte[] tokenBytes1, [WithCount(8)]byte[] tokenBytes2, string msg)
        {
            var sut = ObjectWithConcurrencyToken.WithTokenBytes(tokenBytes1);

            Action act = () => sut.ValidateInt64ConcurrencyToken(tokenBytes2, msg);

            act.ShouldThrow<InvaildConcurrencyTokenException>()
                .Where(a => (a.Expected as Int64ConcurrencyToken).Bytes.SequenceEqual(tokenBytes1))
                .Where(a => (a.Actual as Int64ConcurrencyToken).Bytes.SequenceEqual(tokenBytes2))
                .Where(a => a.Message.Contains(msg));
        }

        [Theory, MendhamData]
        public void SetInt64ConcurrencyToken_ObjectWithoutToken_Object([WithCount(8)]byte[] newTokenBytes)
        {
            var sut = ObjectWithConcurrencyToken.WithoutToken();

            var result = sut.SetInt64ConcurrencyToken(newTokenBytes);

            result.Should().Be(sut)
                .And.Match(a => ((a as IHasConcurrencyToken).Token as Int64ConcurrencyToken).Bytes.SequenceEqual(newTokenBytes));
        }

        [Theory, MendhamData]
        public void SetInt64ConcurrencyToken_ObjectWithToken_Object([WithCount(8)]byte[] newTokenBytes)
        {
            var sut = ObjectWithConcurrencyToken.WithToken();

            var result = sut.SetInt64ConcurrencyToken(newTokenBytes);

            result.Should().Be(sut)
                .And.Match(a => ((a as IHasConcurrencyToken).Token as Int64ConcurrencyToken).Bytes.SequenceEqual(newTokenBytes));
        }

        [Fact]
        public void SetInt64ConcurrencyToken_SetNullToken_ThrowsArgumentNullException()
        {
            byte[] newTokenBytes = null;
            var sut = ObjectWithConcurrencyToken.WithoutToken();

            Action act = () => sut.SetInt64ConcurrencyToken(newTokenBytes);

            act.ShouldThrow<ArgumentNullException>("cannot set a null token to a IHasConcurrencyToken");
        }
    }
}
