using FluentAssertions;
using Xunit;

namespace Mendham.Testing.Domain.Test
{
    public class TimesRaisedTest
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 3)]
        [InlineData(5, 5)]
        [InlineData(5, 6)]
        public void TimesRaiseAtLeast_Valid_True(int atLeast, int actualTimesRaised)
        {
            var sut = TimesRaised.AtLeast(atLeast);

            var result = sut.Validate(actualTimesRaised);

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(5, 0)]
        [InlineData(5, 1)]
        [InlineData(5, 4)]
        public void TimesRaiseAtLeast_Invalid_False(int atLeast, int actualTimesRaised)
        {
            var sut = TimesRaised.AtLeast(atLeast);

            var result = sut.Validate(actualTimesRaised);

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public void TimesRaiseAtLeastOnce_Valid_True(int actualTimesRaised)
        {
            var sut = TimesRaised.AtLeastOnce;

            var result = sut.Validate(actualTimesRaised);

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        public void TimesRaiseAtLeastOnce_Invalid_False(int actualTimesRaised)
        {
            var sut = TimesRaised.AtLeastOnce;

            var result = sut.Validate(actualTimesRaised);

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, 1)]
        [InlineData(5, 2)]
        [InlineData(5, 5)]
        public void TimesRaiseAtMost_Valid_True(int atMost, int actualTimesRaised)
        {
            var sut = TimesRaised.AtMost(atMost);

            var result = sut.Validate(actualTimesRaised);

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        [InlineData(5, 6)]
        [InlineData(5, 10)]
        public void TimesRaiseAtMost_Invalid_False(int atMost, int actualTimesRaised)
        {
            var sut = TimesRaised.AtMost(atMost);

            var result = sut.Validate(actualTimesRaised);

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void TimesRaiseAtMostOnce_Valid_True(int actualTimesRaised)
        {
            var sut = TimesRaised.AtMostOnce;

            var result = sut.Validate(actualTimesRaised);

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        public void TimesRaiseAtMostOnce_Invalid_False(int actualTimesRaised)
        {
            var sut = TimesRaised.AtMostOnce;

            var result = sut.Validate(actualTimesRaised);

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(0, 1, 1)]
        [InlineData(0, 3, 0)]
        [InlineData(0, 3, 1)]
        [InlineData(0, 3, 3)]
        [InlineData(1, 1, 1)]
        [InlineData(1, 3, 1)]
        [InlineData(1, 3, 3)]
        [InlineData(4, 8, 4)]
        [InlineData(4, 8, 6)]
        [InlineData(4, 8, 8)]

        public void TimesRaiseBetween_Valid_True(int atLeast, int atMost, int actualTimesRaised)
        {
            var sut = TimesRaised.Between(atLeast, atMost);

            var result = sut.Validate(actualTimesRaised);

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(0, 0, 1)]
        [InlineData(0, 1, 2)]
        [InlineData(1, 1, 0)]
        [InlineData(1, 1, 2)]
        [InlineData(1, 3, 0)]
        [InlineData(1, 3, 4)]
        [InlineData(4, 8, 0)]
        [InlineData(4, 8, 3)]
        [InlineData(4, 8, 10)]
        public void TimesRaiseBetween_Invalid_False(int atLeast, int atMost, int actualTimesRaised)
        {
            var sut = TimesRaised.Between(atLeast, atMost);

            var result = sut.Validate(actualTimesRaised);

            result.Should().BeFalse();
        }


        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(3, 3)]
        public void TimesRaiseExactly_Valid_True(int exactly, int actualTimesRaised)
        {
            var sut = TimesRaised.Exactly(exactly);

            var result = sut.Validate(actualTimesRaised);

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, 2)]
        [InlineData(5, 0)]
        [InlineData(5, 1)]
        [InlineData(5, 6)]
        public void TimesRaiseExactly_Invalid_False(int exactly, int actualTimesRaised)
        {
            var sut = TimesRaised.Exactly(exactly);

            var result = sut.Validate(actualTimesRaised);

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(1)]
        public void TimesRaiseOnce_Valid_True(int actualTimesRaised)
        {
            var sut = TimesRaised.Once;

            var result = sut.Validate(actualTimesRaised);

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(10)]
        public void TimesRaiseOnce_Invalid_False(int actualTimesRaised)
        {
            var sut = TimesRaised.Once;

            var result = sut.Validate(actualTimesRaised);

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(0)]
        public void TimesRaiseNever_Valid_True(int actualTimesRaised)
        {
            var sut = TimesRaised.Never;

            var result = sut.Validate(actualTimesRaised);

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        public void TimesRaiseNever_Invalid_False(int actualTimesRaised)
        {
            var sut = TimesRaised.Never;

            var result = sut.Validate(actualTimesRaised);

            result.Should().BeFalse();
        }

        [Fact]
        public void TimesRaiseAtLeastOnceEquals_AtLeast1_True()
        {
            var sut = TimesRaised.AtLeastOnce;

            var result = sut == TimesRaised.AtLeast(1);

            result.Should().BeTrue();
        }

        [Fact]
        public void TimesRaiseOnceEquals_Exactly1_True()
        {
            var sut = TimesRaised.Once;

            var result = sut == TimesRaised.Exactly(1);

            result.Should().BeTrue();
        }

        [Fact]
        public void TimesRaiseOnceEquals_Never_False()
        {
            var sut = TimesRaised.Once;

            var result = sut == TimesRaised.Never;

            result.Should().BeFalse();
        }

        [Fact]
        public void TimesRaiseOnceEquals_AtLeastOnce_False()
        {
            var sut = TimesRaised.Once;

            var result = sut == TimesRaised.AtLeastOnce;

            result.Should().BeFalse();
        }
    }
}
