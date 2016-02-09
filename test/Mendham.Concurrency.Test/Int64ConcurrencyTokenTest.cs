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
    public class Int64ConcurrencyTokenTest
    {
        [Theory, MendhamData]
        public void Value_FromLong_Equal(long value)
        {
            var token = new Int64ConcurrencyToken(value);

            var result = token.Value;

            result.Should().Be(value);
        }

        [Theory, MendhamData]
        public void Bytes_FromBytes_EqualBytes([WithCount(8)]byte[] bytes)
        {
            var token = new Int64ConcurrencyToken(bytes);

            var result = token.Bytes;

            result.Should().ContainInOrder(bytes);
        }

        [Fact]
        public void ConstructorBytes_Null_ThrowsArgumentNullException()
        {
            byte[] bytes = null;

            Action act = () => new Int64ConcurrencyToken(bytes);

            act.ShouldThrow<ArgumentNullException>("value cannot be null");
        }

        [Theory, MendhamData]
        public void ConstructorBytes_7Bytes_ThrowsArgumentException([WithCount(7)]byte[] bytes)
        {
            Action act = () => new Int64ConcurrencyToken(bytes);

            act.ShouldThrow<ArgumentException>("value must be 8 bytes");
        }

        [Fact]
        public void ConstructorBytes_AllZero_ThrowsArgumentException()
        {
            byte[] bytes = new byte[8];

            Action act = () => new Int64ConcurrencyToken(bytes);

            act.ShouldThrow<ArgumentException>("value cannot be all zeros");
        }

        [Theory, MendhamData]
        public void EqualsIConcurrencyToken_SameReference_True([WithCount(8)]byte[] bytes)
        {
            IConcurrencyToken token = new Int64ConcurrencyToken(bytes);

            var result = token.Equals(token);

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void EqualsIConcurrencyToken_SameBytes_True([WithCount(8)]byte[] bytes)
        {
            IConcurrencyToken token1 = new Int64ConcurrencyToken(bytes);
            IConcurrencyToken token2 = new Int64ConcurrencyToken(bytes);

            var result = token1.Equals(token2);

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void EqualsIConcurrencyToken_DifferentTypes_False([WithCount(8)]byte[] bytes)
        {
            IConcurrencyToken token1 = new Int64ConcurrencyToken(bytes);
            IConcurrencyToken token2 = new AltConcurrencyToken();

            var result = token1.Equals(token2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsIConcurrencyToken_DifferentBytes_False([WithCount(8)]byte[] bytes1, [WithCount(8)]byte[] bytes2)
        {
            IConcurrencyToken token1 = new Int64ConcurrencyToken(bytes1);
            IConcurrencyToken token2 = new Int64ConcurrencyToken(bytes2);

            var result = token1.Equals(token2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsIConcurrencyToken_SameBytesDifferentOrder_False([WithCount(8)]byte[] bytes)
        {
            var reorderedBytes = bytes
                .OrderBy(a => Guid.NewGuid())
                .ToArray();

            IConcurrencyToken token1 = new Int64ConcurrencyToken(bytes);
            IConcurrencyToken token2 = new Int64ConcurrencyToken(reorderedBytes);

            var result = token1.Equals(token2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsObject_SameReference_True([WithCount(8)]byte[] bytes)
        {
            IConcurrencyToken token = new Int64ConcurrencyToken(bytes);

            var result = token.Equals(token as object);

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void EqualsObject_SameBytes_True([WithCount(8)]byte[] bytes)
        {
            IConcurrencyToken token1 = new Int64ConcurrencyToken(bytes);
            object token2 = new Int64ConcurrencyToken(bytes);

            var result = token1.Equals(token2);

            result.Should().BeTrue();
        }

        [Theory, MendhamData]
        public void EqualsObject_DifferentBytes_False([WithCount(8)]byte[] bytes1, [WithCount(8)]byte[] bytes2)
        {
            IConcurrencyToken token1 = new Int64ConcurrencyToken(bytes1);
            object token2 = new Int64ConcurrencyToken(bytes2);

            var result = token1.Equals(token2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsObject_DifferentTypes_False([WithCount(8)]byte[] bytes)
        {
            IConcurrencyToken token1 = new Int64ConcurrencyToken(bytes);
            object token2 = new AltConcurrencyToken();

            var result = token1.Equals(token2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void EqualsObject_SameBytesDifferentOrder_False([WithCount(8)]byte[] bytes)
        {
            var reorderedBytes = bytes
                .OrderBy(a => Guid.NewGuid())
                .ToArray();

            IConcurrencyToken token1 = new Int64ConcurrencyToken(bytes);
            object token2 = new Int64ConcurrencyToken(reorderedBytes);

            var result = token1.Equals(token2);

            result.Should().BeFalse();
        }

        [Theory, MendhamData]
        public void GetHashCode_SameReference_Equal([WithCount(8)]byte[] bytes)
        {
            var token = new Int64ConcurrencyToken(bytes);

            var expected = token.GetHashCode();
            var result = token.GetHashCode();

            result.Should().Be(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_SameBytes_Equal([WithCount(8)]byte[] bytes)
        {
            var token1 = new Int64ConcurrencyToken(bytes);
            var token2 = new Int64ConcurrencyToken(bytes);

            var expected = token1.GetHashCode();
            var result = token2.GetHashCode();

            result.Should().Be(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_DifferentBytes_NotEqual([WithCount(8)]byte[] bytes1, [WithCount(8)]byte[] bytes2)
        {
            var token1 = new Int64ConcurrencyToken(bytes1);
            var token2 = new Int64ConcurrencyToken(bytes2);

            var expected = token1.GetHashCode();
            var result = token2.GetHashCode();

            result.Should().NotBe(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_DifferentTypes_NotEqual([WithCount(8)]byte[] bytes)
        {
            var token1 = new Int64ConcurrencyToken(bytes);
            var token2 = new AltConcurrencyToken();

            var expected = token1.GetHashCode();
            var result = token2.GetHashCode();

            result.Should().NotBe(expected);
        }

        [Theory, MendhamData]
        public void GetHashCode_SameBytesDifferentOrder_NotEqual([WithCount(8)]byte[] bytes)
        {
            var reorderedBytes = bytes
                .OrderBy(a => Guid.NewGuid())
                .ToArray();

            var token1 = new Int64ConcurrencyToken(bytes);
            var token2 = new Int64ConcurrencyToken(reorderedBytes);

            var expected = token1.GetHashCode();
            var result = token2.GetHashCode();

            result.Should().NotBe(expected);
        }
    }
}
