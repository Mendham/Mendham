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
		public void VerifyArgumentNotNull_NullString_Throws(string msg)
		{
			String str = null;

			Action act = () => str.VerifyArgumentNotNull(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNull_Int_Passes(int? obj, string msg)
		{
			var result = obj.VerifyArgumentNotNull(msg);

			result.Should().Be(obj);
		}

		[Theory]
		[AutoData]
		public void VerifyArguementNotDefault_NullInt_Throws(string msg)
		{
			int? val = null;

			Action act = () => val.VerifyArgumentNotDefaultValue(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArguementNotDefault_DefaultInt_Throws(string msg)
		{
			int val = default(int);

			Action act = () => val.VerifyArgumentNotDefaultValue(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArguementNotDefault_DefaultString_Throws(string msg)
		{
			string val = default(String);

			Action act = () => val.VerifyArgumentNotDefaultValue(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArguementNotDefault_String_Passes(string val, string msg)
		{
			var result = val.VerifyArgumentNotDefaultValue(msg);

			result.Should().Be(val);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrEmpty_NullCollection_Throws(string msg)
		{
			List<int> val = null;

			Action act = () => val.VerifyArgumentNotNullOrEmpty(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrEmpty_EmptyCollection_Throws(string msg)
		{
			List<int> val = new List<int>();

			Action act = () => val.VerifyArgumentNotNullOrEmpty(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrEmpty_NonEmptyCollection_Passes(List<int> vals, string msg)
		{
			var result = vals.VerifyArgumentNotNullOrEmpty(msg);

			result.Should().ContainInOrder(vals);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrEmpty_NullString_Throws(string msg)
		{
			string val = null;

			Action act = () => val.VerifyArgumentNotNullOrEmpty(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrEmpty_EmptyString_Throws(string msg)
		{
			string val = String.Empty;

			Action act = () => val.VerifyArgumentNotNullOrEmpty(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrEmpty_NonEmptyString_Passes(string val, string msg)
		{
			var result = val.VerifyArgumentNotNullOrEmpty(msg);

			result.Should().Be(val);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrWhiteSpace_NullString_Throws(string msg)
		{
			string val = null;

			Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrWhiteSpac_EmptyString_Throws(string msg)
		{
			string val = String.Empty;

			Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrWhiteSpace_WhiteSpaceString_Throws(string msg)
		{
			string val = "   ";

			Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentNotNullOrWhiteSpace_ValidString_Passes(string val, string msg)
		{
			var result = val.VerifyArgumentNotNullOrWhiteSpace(msg);

			result.Should().Be(val);
		}

		[Theory]
		[InlineAutoData("test", 1, 5, false)]
		[InlineAutoData("test", null, 5, false)]
		[InlineAutoData("test", 1, null, false)]
		[InlineAutoData(" test  ", 1, 5, true)]
		public void VerifyArgumentLength_ValidCase_Passes(string val, int? minimum, int? maximum, bool trimFirst, string msg)
		{
			var result = val.VerifyArgumentLength(minimum, maximum, msg, trimFirst);

			result.Should().Be(val);
		}

		[Theory]
		[InlineAutoData("test", 5, 10, false)]
		[InlineAutoData("test", null, 3, false)]
		[InlineAutoData("test", 5, null, false)]
		[InlineAutoData(" test  ", 5, 10, true)]
		public void VerifyArgumentLength_InvalidCase_Throws(string val, int? minimum, int? maximum, bool trimFirst, string msg)
		{
			Action act = () => val.VerifyArgumentLength(minimum, maximum, msg, trimFirst);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentMeetsCriteria_ValidCriteria_Passes(int val, string msg)
		{
			var result = val.VerifyArgumentMeetsCriteria(a => true, msg);

			result.Should().Be(val);
		}

		[Theory]
		[AutoData]
		public void VerifyArgumentMeetsCriteria_InvalidCriteria_Throws(string val, string msg)
		{
			Action act = () => val.VerifyArgumentMeetsCriteria(a => false, msg);

			act.ShouldThrow<ArgumentException>()
				.WithMessage(msg);
		}
	}
}
