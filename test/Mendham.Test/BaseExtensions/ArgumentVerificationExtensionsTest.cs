using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace Mendham.Test.BaseExtensions
{
    public class ArgumentVerificationExtensionsTest
    {
		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullFalse(String msg)
		{
			String str = null;

			Action act = () => str.VerifyArgumentNotNull(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullTrue(int? obj, String msg)
		{
			var result = obj.VerifyArgumentNotNull(msg);

			result.Should().Be(obj);
		}

		[Theory]
		[AutoData]
		public void VerifyArguementNotDefaultFalseInt(String msg)
		{
			int val = default(int);

			Action act = () => val.VerifyArgumentNotDefaultValue(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArguementNotDefaultFalseString(String msg)
		{
			String val = default(String);

			Action act = () => val.VerifyArgumentNotDefaultValue(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArguementNotDefaultTrueString(String val, String msg)
		{
			var result = val.VerifyArgumentNotDefaultValue(msg);

			result.Should().Be(val);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrEmptyFalseNullCollection(String msg)
		{
			List<int> val = null;

			Action act = () => val.VerifyArgumentNotNullOrEmpty(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrEmptyFalseEmptyCollection(String msg)
		{
			List<int> val = new List<int>();

			Action act = () => val.VerifyArgumentNotNullOrEmpty(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrEmptyTrueCollection(List<int> vals, String msg)
		{
			var result = vals.VerifyArgumentNotNullOrEmpty(msg);

			result.Should().ContainInOrder(vals);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrEmptyFalseNullString(String msg)
		{
			String val = null;

			Action act = () => val.VerifyArgumentNotNullOrEmpty(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrEmptyFalseEmptyString(String msg)
		{
			String val = String.Empty;

			Action act = () => val.VerifyArgumentNotNullOrEmpty(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrEmptyTrueString(String val, String msg)
		{
			var result = val.VerifyArgumentNotNullOrEmpty(msg);

			result.Should().Be(val);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrWhiteSpaceFalseNullString(String msg)
		{
			String val = null;

			Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrWhiteSpaceFalseEmptyString(String msg)
		{
			String val = String.Empty;

			Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrWhiteSpaceFalseWhiteSpaceString(String msg)
		{
			String val = "   ";

			Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrWhiteSpaceTrueString(String val, String msg)
		{
			var result = val.VerifyArgumentNotNullOrWhiteSpace(msg);

			result.Should().Be(val);
		}

		[Theory]
		[InlineAutoData("test", 1, 5, false)]
		[InlineAutoData("test", null, 5, false)]
		[InlineAutoData("test", 1, null, false)]
		[InlineAutoData(" test  ", 1, 5, true)]
		public void VerifyArgumentLengthTrue(String val, int? minimum, int? maximum, bool trimFirst, String msg)
		{
			var result = val.VerifyArgumentLength(minimum, maximum, msg, trimFirst);

			result.Should().Be(val);
		}

		[Theory]
		[InlineAutoData("test", 5, 10, false)]
		[InlineAutoData("test", null, 3, false)]
		[InlineAutoData("test", 5, null, false)]
		[InlineAutoData(" test  ", 5, 10, true)]
		public void VerifyArgumentLengthFalse(String val, int? minimum, int? maximum, bool trimFirst, String msg)
		{
			Action act = () => val.VerifyArgumentLength(minimum, maximum, msg, trimFirst);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentMeetsCriteriaTrue(int val, String msg)
		{
			var result = val.VerifyArgumentMeetsCriteria(a => true, msg);

			result.Should().Be(val);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentMeetsCriteriaFalse(String val, String msg)
		{
			Action act = () => val.VerifyArgumentMeetsCriteria(a => false, msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}
	}
}
